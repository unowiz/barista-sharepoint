﻿namespace Barista.SharePoint.Library
{
  using System;
  using System.Collections.ObjectModel;
  using System.Linq;
  using Barista.Library;
  using Jurassic;
  using Jurassic.Library;
  using Microsoft.SharePoint.Administration;

  [Serializable]
  public class SPFarmInstance : ObjectInstance
  {
    [NonSerialized]
    private readonly SPFarm m_farm;

    public SPFarmInstance(ObjectInstance prototype)
      : base(prototype)
    {
      this.PopulateFields();
      this.PopulateFunctions();
    }

    public SPFarmInstance(ObjectInstance prototype, SPFarm farm)
      : this(prototype)
    {
      this.m_farm = farm;
    }

    [JSFunction(Name = "getFarmServers")]
    public ArrayInstance GetServersInFarm()
    {
      return this.Engine.Array.Construct(
// ReSharper disable CoVariantArrayConversion
        m_farm.Servers.Select(s => new SPServerInstance(this.Engine.Object.Prototype, s)).ToArray());
// ReSharper restore CoVariantArrayConversion
    }

    [JSFunction(Name = "getFarmServices")]
    public ArrayInstance GetServicesInFarm()
    {
      return this.Engine.Array.Construct(
        // ReSharper disable CoVariantArrayConversion
        m_farm.Services.Select(s =>
          {
            if (s is SPWindowsService)
              return new SPWindowsServiceInstance(this.Engine.Object.InstancePrototype, s as SPWindowsService);
            return new SPServiceInstance(this.Engine.Object.InstancePrototype, s);
          }).ToArray());
      // ReSharper restore CoVariantArrayConversion
    }

    [JSFunction(Name = "getServiceApplicationById")]
    public object GetServiceApplicationById(object id)
    {
      var guid = GuidInstance.ConvertFromJsObjectToGuid(id);

      foreach (var serviceApplication in m_farm.Services
        .SelectMany(service => service.Applications.Where(serviceApplication => serviceApplication.Id == guid)))
      {
        return new SPServiceApplicationInstance(this.Engine.Object.Prototype, serviceApplication);
      }

      return Null.Value;
    }

    [JSFunction(Name = "getFarmManagedAccounts")]
    public ArrayInstance GetFarmManagedAccounts()
    {
      var managedAccounts = new Collection<SPManagedAccountInstance>();

      var local = SPFarm.Local;
      var farmManagedAccountCollection = new SPFarmManagedAccountCollection(local);
      foreach (var farmManagedAccount in farmManagedAccountCollection)
      {
        managedAccounts.Add(new SPManagedAccountInstance(this.Engine.Object.InstancePrototype, farmManagedAccount));
      }

// ReSharper disable CoVariantArrayConversion
      return this.Engine.Array.Construct(managedAccounts.ToArray());
// ReSharper restore CoVariantArrayConversion
    }

    [JSFunction(Name = "getFarmKeyValue")]
    public object GetFarmKeyValueAsObject(string key)
    {
      if (m_farm == null|| m_farm.Properties.ContainsKey(key) == false)
        return Undefined.Value;

      string val = Convert.ToString(m_farm.Properties[key]);

      object result;

      //Attempt to convert the string into a JSON Object.
      try
      {
        result = JSONObject.Parse(this.Engine, val, null);
      }
      catch
      {
        result = val;
      }

      return result;
    }

    [JSFunction(Name = "setFarmKeyValue")]
    public void SetFarmKeyValue(string key, object value)
    {
      if (value == null || value == Undefined.Value || value == Null.Value)
        throw new ArgumentNullException("value");

      string stringValue;
      if (value is ObjectInstance)
      {
        stringValue = JSONObject.Stringify(this.Engine, value, null, null);
      }
      else
      {
        stringValue = value.ToString();
      }

      if (m_farm != null)
      {
        if (m_farm.Properties.ContainsKey(key))
          m_farm.Properties[key] = stringValue;
        else
          m_farm.Properties.Add(key, stringValue);

        m_farm.Update();
      }
    }
  }
}
