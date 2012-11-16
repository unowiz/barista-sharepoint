﻿namespace Barista.SharePoint.Services
{
  using Barista.Bundles;
  using Barista.Library;
  using Barista.SharePoint.Bundles;
  using Barista.SharePoint.Library;
  using Jurassic;
  using Jurassic.Library;
  using Microsoft.SharePoint;
  using Microsoft.SharePoint.Administration;
  using Microsoft.SharePoint.Utilities;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.InteropServices;
  using System.ServiceModel;
  using System.Web;

  [Guid("9B4C0B5C-8A42-401A-9ACB-42EA6246E960")]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
  internal sealed class BaristaServiceApplication : SPIisWebServiceApplication, IBaristaServiceApplication
  {
    [Persisted]
    private int m_settings;
    public int Settings
    {
      get { return m_settings; }
      set { m_settings = value; }
    }

    public BaristaServiceApplication()
      : base() { }

    private BaristaServiceApplication(string name, BaristaService service, SPIisWebServiceApplicationPool appPool)
      : base(name, service, appPool) { }

    public static BaristaServiceApplication Create(string name, BaristaService service, SPIisWebServiceApplicationPool appPool)
    {
      #region validation
      if (name == null) throw new ArgumentNullException("name");
      if (service == null) throw new ArgumentNullException("service");
      if (appPool == null) throw new ArgumentNullException("appPool");
      #endregion

      // create the service application
      BaristaServiceApplication serviceApplication = new BaristaServiceApplication(name, service, appPool);
      serviceApplication.Update();

      // register the supported endpoints
      serviceApplication.AddServiceEndpoint("http", SPIisWebServiceBindingType.Http);
      serviceApplication.AddServiceEndpoint("https", SPIisWebServiceBindingType.Https, "secure");

      return serviceApplication;
    }

    #region service application details
    protected override string DefaultEndpointName
    {
      get { return "http"; }
    }

    public override string TypeName
    {
      get { return "Barista Service Application"; }
    }

    protected override string InstallPath
    {
      get { return SPUtility.GetGenericSetupPath(@"WebServices\Barista"); }
    }

    protected override string VirtualPath
    {
      get { return "Barista.svc"; }
    }

    public override Guid ApplicationClassId
    {
      get { return new Guid("9B4C0B5C-8A42-401A-9ACB-42EA6246E960"); }
    }

    public override Version ApplicationVersion
    {
      get { return new Version("1.0.0.0"); }
    }
    #endregion

    #region Service Application UI

    public override SPAdministrationLink ManageLink
    {
      get
      { return new SPAdministrationLink("/_admin/BaristaService/Manage.aspx"); }
    }

    public override SPAdministrationLink PropertiesLink
    {
      get
      { return new SPAdministrationLink("/_admin/BaristaService/Manage.aspx"); }
    }

    #endregion

    #region IBaristaServiceApplication implementation
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    public BrewResponse Eval(BrewRequest request)
    {
      if (request == null)
        throw new ArgumentNullException("request");

      var response = new BrewResponse();

      BaristaContext.Current = new BaristaContext(request, response);

      WebBundle webBundle = new WebBundle();
      var engine = GetScriptEngine(webBundle);

      object result = null;
      try
      {
        result = engine.Evaluate(request.Code);

        var isRaw = false;

        //If the web instance has been initialized on the web bundle, use the value set via script, otherwise use defaults.
        if (webBundle.WebInstance == null || webBundle.WebInstance.Response.AutoDetectContentType)
        {
          response.ContentType = BrewResponse.AutoDetectContentTypeFromResult(result);
        }

        if (webBundle.WebInstance != null)
        {
          isRaw = webBundle.WebInstance.Response.IsRaw;
        }

        var stringified = JSONObject.Stringify(engine, result, null, null);
        response.SetContentsFromResultObject(engine, result, isRaw);
      }
      catch (Exception ex)
      {
        BaristaDiagnosticsService.Local.LogException(ex, BaristaDiagnosticCategory.Runtime, "An error occured while evaluating script: ");
        throw;
      }
      finally
      {
        //Cleanup
        engine = null;

        if (BaristaContext.Current != null)
          BaristaContext.Current.Dispose();

        BaristaContext.Current = null;
      }

      return response;
    }

    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    public void Exec(BrewRequest request)
    {
      if (request == null)
        throw new ArgumentNullException("request");

      var response = new BrewResponse();

      BaristaContext.Current = new BaristaContext(request, response);

      WebBundle webBundle = new WebBundle();
      var engine = GetScriptEngine(webBundle);

      try
      {
        engine.Execute(request.Code);
      }
      catch (Exception ex)
      {
        BaristaDiagnosticsService.Local.LogException(ex, BaristaDiagnosticCategory.Runtime, "An error occured while executing script: ");
        throw;
      }
      finally
      {
        //Cleanup
        engine = null;
        BaristaContext.Current.Dispose();
        BaristaContext.Current = null;
      }
    }

    /// <summary>
    /// Returns a new instance of a script engine object with all runtime objects available.
    /// </summary>
    /// <returns></returns>
    private ScriptEngine GetScriptEngine(WebBundle webBundle)
    {
      var engine = new Jurassic.ScriptEngine();

      if (BaristaContext.Current.Request.ForceStrict)
      {
        engine.ForceStrictMode = true;
      }

      var console = new FirebugConsole(engine);
      console.Output = new BaristaConsoleOutput(engine);

      //Register Bundles.
      Common common = new Common();
      common.RegisterBundle(webBundle);
      common.RegisterBundle(new MustacheBundle());
      common.RegisterBundle(new LinqBundle());
      common.RegisterBundle(new JsonDataBundle());
      common.RegisterBundle(new SharePointBundle());
      common.RegisterBundle(new ActiveDirectoryBundle());
      common.RegisterBundle(new DocumentBundle());
      common.RegisterBundle(new K2Bundle());
      common.RegisterBundle(new UtilityBundle());
      common.RegisterBundle(new UlsLogBundle());
      common.RegisterBundle(new DocumentStoreBundle());
      common.RegisterBundle(new SimpleInheritanceBundle());
      common.RegisterBundle(new SqlDataBundle());
      common.RegisterBundle(new StateMachineBundle());

      //Global Types

      //engine.SetGlobalValue("file", new FileSystemInstance(engine));

      engine.SetGlobalValue("Guid", new GuidConstructor(engine));
      engine.SetGlobalValue("Uri", new UriConstructor(engine));
      engine.SetGlobalValue("Deferred", new DeferredConstructor(engine));
      engine.SetGlobalValue("Base64EncodedByteArrayInstance", new Base64EncodedByteArrayConstructor(engine));

      engine.SetGlobalValue("console", console);

      //Functions
      engine.SetGlobalFunction("help", new Func<object, object>(obj => Help.GenerateHelpJsonForObject(engine, obj)));
      engine.SetGlobalFunction("require", new Func<string, object>(obj => common.Require(engine, obj)));
      engine.SetGlobalFunction("listBundles", new Func<object>(() => common.List(engine)));

      engine.SetGlobalFunction("delay", new Action<int>((millisecondsTimeout) => { System.Threading.Thread.Sleep(millisecondsTimeout); }));
      engine.SetGlobalFunction("waitAll", new Action<object, object>((deferreds, timeout) => { DeferredInstance.WaitAll(deferreds, timeout); }));

      engine.SetGlobalFunction("include", new Action<string>((scriptUrl) =>
      {
        bool isHiveFile;
        string code;
        if (SPHelper.TryGetSPFileAsString(scriptUrl, out code, out isHiveFile))
        {
          code = SPHelper.ReplaceTokens(code);
          engine.Execute(code);
          return;
        }

        throw new JavaScriptException(engine, "Error", "Could not locate the specified script file:  " + scriptUrl);
      }));

      engine.SetGlobalFunction("replaceJsonReferences", new Func<object, object>((o) =>
      {
        return ReplaceJsonReferences(o);
      }));
      return engine;
    }

    private object ReplaceJsonReferences(object o)
    {
      var dictionary = new Dictionary<string,ObjectInstance>();
      return ReplaceJsonReferences(o, dictionary);
    }

    private object ReplaceJsonReferences(object o, Dictionary<string, ObjectInstance> dictionary)
    {
      if (o is ArrayInstance)
      {
        var array = o as ArrayInstance;
        for (int i = 0; i < array.ElementValues.Count(); i++)
        {
          
          array[i] = ReplaceJsonReferences(array[i], dictionary);
        }
      }
      else if (o is ObjectInstance)
      {
        var obj = o as ObjectInstance;
        var properties = obj.Properties.ToList();

        //If there's only one property named "$ref" and it's value is a key that exists in the dictionary, return the value.
        if (properties.Count == 1 && properties[0].Name == "$ref" && dictionary.ContainsKey((string)properties[0].Value))
          return dictionary[(string)properties[0].Value];

        var idProperty = properties.Where(p => p.Name == "$id").FirstOrDefault();
        if (idProperty != null && dictionary.ContainsKey((string)idProperty.Value) == false)
        {
          var str = JSONObject.Stringify(obj.Engine, obj, null, null);
          var clone = JSONObject.Parse(obj.Engine, str, null) as ObjectInstance;

          if (clone.HasProperty("$id"))
            clone.Delete("$id", false);

          dictionary.Add((string)idProperty.Value, clone);
        }

        foreach (var property in properties)
        {
          obj.SetPropertyValue(property.Name, ReplaceJsonReferences(property.Value, dictionary), false);
        }
      }
      return o;
    }
    #endregion
  }
}
