﻿namespace Barista
{
  using Barista.Extensions;
  using System;
  using System.Collections.Generic;
  using System.Net;
  using System.Runtime.Serialization;
  using System.ServiceModel.Web;
  using System.Text;
  using System.Linq;
  using System.Web;

  [DataContract(Namespace = Constants.ServiceNamespace)]
  [Serializable]
  public sealed class BrewRequest : IExtensibleDataObject
  {
    public BrewRequest()
    {
      this.CodePath = String.Empty;
      this.Content = new byte[0];
      this.ContentLength = 0;
      this.ContentType = "application/octet-stream";
      this.Cookies = new Dictionary<string, string>();
      this.Headers = new Dictionary<string, string>();
      this.Files = new Dictionary<string, PostedFile>();
      this.Form = new Dictionary<string, string>();
      this.Params = new Dictionary<string, string>();
      this.QueryString = new Dictionary<string, string>();
      this.ServerVariables = new Dictionary<string, string>();
      this.ExtendedProperties = new Dictionary<string, string>();

      this.ForceStrict = false;
      this.InstanceMode = BaristaInstanceMode.PerCall;
      this.InstanceName = null;

      this.InstanceAbsoluteExpiration = null;
      this.InstanceSlidingExpiration = null;

      this.InstanceInitializationCode = null;
      this.InstanceInitializationCodePath = null;
    }

    #region Properties
    [DataMember]
    public string[] AcceptTypes
    {
      get;
      set;
    }

    [DataMember]
    public string AnonymousId
    {
      get;
      set;
    }

    [DataMember]
    public string ApplicationPath
    {
      get;
      set;
    }

    [DataMember]
    public string AppRelativeCurrentExecutionFilePath
    {
      get;
      set;
    }

    [DataMember]
    public byte[] Body
    {
      get;
      set;
    }

    [DataMember]
    public string Code
    {
      get;
      set;
    }

    [DataMember]
    public string CodePath
    {
      get;
      set;
    }

    [DataMember]
    public Byte[] Content
    {
      get;
      set;
    }

    [DataMember]
    public Encoding ContentEncoding
    {
      get;
      set;
    }

    [DataMember]
    public long ContentLength
    {
      get;
      set;
    }

    [DataMember]
    public string ContentType
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> Cookies
    {
      get;
      set;
    }

    [DataMember]
    public string CurrentExecutionFilePath
    {
      get;
      set;
    }

    [DataMember]
    public string FilePath
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, PostedFile> Files
    {
      get;
      set;
    }

    [DataMember]
    public bool ForceStrict
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> Headers
    {
      get;
      set;
    }

    [DataMember]
    public BaristaInstanceMode InstanceMode
    {
      get;
      set;
    }

    [DataMember]
    public string InstanceInitializationCode
    {
      get;
      set;
    }

    [DataMember]
    public string InstanceInitializationCodePath
    {
      get;
      set;
    }

    [DataMember]
    public DateTime? InstanceAbsoluteExpiration
    {
      get;
      set;
    }

    [DataMember]
    public TimeSpan? InstanceSlidingExpiration
    {
      get;
      set;
    }

    [DataMember]
    public string InstanceName
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> Form
    {
      get;
      set;
    }

    [DataMember]
    public bool IsAuthenticated
    {
      get;
      set;
    }

    [DataMember]
    public bool IsLocal
    {
      get;
      set;
    }

    [DataMember]
    public bool IsSecureConnection
    {
      get;
      set;
    }

    [DataMember]
    public string LogonUserName
    {
      get;
      set;
    }

    [DataMember]
    public string Method
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> Params
    {
      get;
      set;
    }

    [DataMember]
    public string Path
    {
      get;
      set;
    }

    [DataMember]
    public string PathInfo
    {
      get;
      set;
    }

    [DataMember]
    public string PhysicalApplicationPath
    {
      get;
      set;
    }

    [DataMember]
    public string PhysicalPath
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> QueryString
    {
      get;
      set;
    }

    [DataMember]
    public string RawUrl
    {
      get;
      set;
    }

    [DataMember]
    public string RequestType
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> ServerVariables
    {
      get;
      set;
    }

    [DataMember]
    public Uri Url
    {
      get;
      set;
    }

    [DataMember]
    public Uri UrlReferrer
    {
      get;
      set;
    }

    [DataMember]
    public string UserAgent
    {
      get;
      set;
    }

    [DataMember]
    public string UserHostAddress
    {
      get;
      set;
    }

    [DataMember]
    public string UserHostName
    {
      get;
      set;
    }

    [DataMember]
    public string[] UserLanguages
    {
      get;
      set;
    }

    [DataMember]
    public IDictionary<string, string> ExtendedProperties
    {
      get;
      set;
    }

    [NonSerialized]
    private ExtensionDataObject m_extensionData;

    public ExtensionDataObject ExtensionData
    {
      get { return m_extensionData; }
      set { m_extensionData = value; }
    }
    #endregion

    #region Static Methods

    public static BrewRequest CreateServiceApplicationRequestFromHttpRequest(HttpRequest request)
    {

      var result = new BrewRequest()
      {
        AcceptTypes = request.AcceptTypes,
        AnonymousId = request.AnonymousID,
        ApplicationPath = request.ApplicationPath,
        AppRelativeCurrentExecutionFilePath = request.AppRelativeCurrentExecutionFilePath,
        //Browser?
        ContentEncoding = request.ContentEncoding,
        ContentLength = request.TotalBytes,
        ContentType = request.ContentType,
        CurrentExecutionFilePath = request.CurrentExecutionFilePath,
        FilePath = request.FilePath,
        IsAuthenticated = request.IsAuthenticated,
        IsLocal = request.IsLocal,
        IsSecureConnection = request.IsSecureConnection,
        Method = request.HttpMethod,
        LogonUserName = request.LogonUserIdentity == null ? String.Empty : request.LogonUserIdentity.Name,
        Path = request.Path,
        PathInfo = request.PathInfo,
        PhysicalApplicationPath = request.PhysicalApplicationPath,
        PhysicalPath = request.PhysicalPath,
        RawUrl = request.RawUrl,
        RequestType = request.RequestType,
        Url = request.Url,
        UrlReferrer = request.UrlReferrer,
        UserAgent = request.UserAgent,
        UserHostAddress = request.UserHostAddress,
        UserHostName = request.UserHostName,
        UserLanguages = request.UserLanguages,
      };

      result.Body = request.InputStream.ToByteArray();

      foreach (var fileName in request.Files.AllKeys)
      {
        var file = request.Files[fileName];
        var content = file.InputStream.ToByteArray();

        result.Files.Add(fileName, new PostedFile()
        {
          Content = content,
          ContentLength = file.ContentLength,
          ContentType = file.ContentType,
          FileName = file.FileName
        });
      }

      //TODO: Make this more robust -- i.e. Support multiple cookies by name/domain key
      foreach (var cookieName in request.Cookies.AllKeys)
      {
        if (result.Cookies.ContainsKey(cookieName) == false)
          result.Cookies.Add(cookieName, request.Cookies[cookieName].Value);
      }

      foreach (var headerName in request.Headers.AllKeys)
      {
        if (result.Headers.ContainsKey(headerName) == false)
          result.Headers.Add(headerName, request.Headers[headerName]);
      }

      foreach (var formFieldName in request.Form.AllKeys)
      {
        if (result.Form.ContainsKey(formFieldName) == false)
          result.Form.Add(formFieldName, request.Form[formFieldName]);
      }

      foreach (var paramName in request.Params.AllKeys)
      {
        if (result.Params.ContainsKey(paramName) == false)
          result.Params.Add(paramName, request.Form[paramName]);
      }

      foreach (var queryStringName in request.QueryString.AllKeys)
      {
        if (result.QueryString.ContainsKey(queryStringName) == false)
          result.QueryString.Add(queryStringName, request.QueryString[queryStringName]);
      }

      foreach (var serverVariableName in request.ServerVariables.AllKeys)
      {
        if (result.ServerVariables.ContainsKey(serverVariableName) == false)
          result.ServerVariables.Add(serverVariableName, request.ServerVariables[serverVariableName]);
      }

      if (request.QueryString.AllKeys.Any( k => k == "Barista_ForceStrict") ||
          request.Headers.AllKeys.Any( k => k == "Barista_ForceStrict"))
      {
        result.ForceStrict = true;
      }

      //InstanceMode
      string instanceModeKey = request.Headers.AllKeys.FirstOrDefault(k => k == "Barista_InstanceMode");
      string instanceNameKey;
      string instanceAbsoluteExpirationKey;
      string instanceSlidingExpirationKey;
      if (instanceModeKey != null)
      {
        result.InstanceMode = (BaristaInstanceMode)Enum.Parse(typeof(BaristaInstanceMode), request.Headers["Barista_InstanceMode"]);

        if (result.InstanceMode == BaristaInstanceMode.Single || result.InstanceMode == BaristaInstanceMode.PerSession)
        {
          instanceNameKey = request.Headers.AllKeys.FirstOrDefault(k => k == "Barista_InstanceName");
          if (instanceNameKey == null)
            throw new InvalidOperationException("If a Barista Instance Mode of Single or Per-Sesson is specified, an Instance Name must also be specified.");

          result.InstanceName = request.Headers["Barista_InstanceName"];

          instanceAbsoluteExpirationKey = request.Headers.AllKeys.FirstOrDefault(k => k == "Barista_InstanceAbsoluteExpiration");
          if (instanceAbsoluteExpirationKey != null)
          {
            result.InstanceAbsoluteExpiration = DateTime.Parse(request.Headers["Barista_InstanceAbsoluteExpiration"]);
          }

          instanceSlidingExpirationKey = request.Headers.AllKeys.FirstOrDefault(k => k == "Barista_InstanceSlidingExpiration");
          if (instanceSlidingExpirationKey != null)
          {
            result.InstanceSlidingExpiration = TimeSpan.Parse(request.Headers["Barista_InstanceSlidingExpiration"]);
          }

        }
      }
      else
      {
        instanceModeKey = request.QueryString.AllKeys.FirstOrDefault(k => k == "Barista_InstanceMode");
        if (instanceModeKey != null)
        {
          result.InstanceMode = (BaristaInstanceMode)Enum.Parse(typeof(BaristaInstanceMode), request.QueryString["Barista_InstanceMode"]);

          if (result.InstanceMode == BaristaInstanceMode.Single || result.InstanceMode == BaristaInstanceMode.PerSession)
          {
            instanceNameKey = request.QueryString.AllKeys.FirstOrDefault(k => k == "Barista_InstanceName");

            if (instanceNameKey == null)
              throw new InvalidOperationException("If a Barista Instance Mode of Single or Per-Sesson is specified, an Instance Name must also be specified.");

            result.InstanceName = request.QueryString["Barista_InstanceName"];

            instanceAbsoluteExpirationKey = request.QueryString.AllKeys.FirstOrDefault(k => k == "Barista_InstanceAbsoluteExpiration");
            if (instanceAbsoluteExpirationKey != null)
            {
              result.InstanceAbsoluteExpiration = DateTime.Parse(request.QueryString["Barista_InstanceAbsoluteExpiration"]);
            }

            instanceSlidingExpirationKey = request.QueryString.AllKeys.FirstOrDefault(k => k == "Barista_InstanceSlidingExpiration");
            if (instanceSlidingExpirationKey != null)
            {
              result.InstanceSlidingExpiration = TimeSpan.Parse(request.QueryString["Barista_InstanceSlidingExpiration"]);
            }
          }
        }
      }

      //Instance Initialization Code
      string instanceInitializationCodeKey = request.Headers.AllKeys.FirstOrDefault(k => k == "Barista_InstanceInitializationCode");
      if (instanceInitializationCodeKey != null)
      {
        result.InstanceInitializationCode = request.Headers["Barista_InstanceInitializationCode"];
      }
      else
      {
        instanceInitializationCodeKey = request.QueryString.AllKeys.FirstOrDefault(k => k == "Barista_InstanceInitializationCode");
        if (instanceInitializationCodeKey != null)
        {
          result.InstanceInitializationCode = request.QueryString["Barista_InstanceInitializationCode"];
        }
      }

      return result;
    }

    #endregion
  }
}
