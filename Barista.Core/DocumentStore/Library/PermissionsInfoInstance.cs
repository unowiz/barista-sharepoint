﻿namespace Barista.DocumentStore.Library
{
  using Barista.DocumentStore;
  using Jurassic;
  using Jurassic.Library;
  using System;
  using System.Linq;

  [Serializable]
  public class PermissionsInfoInstance : ObjectInstance
  {
    private readonly PermissionsInfo m_permissionsInfo;

    public PermissionsInfoInstance(ScriptEngine engine, PermissionsInfo permissionsInfo)
      : base(engine)
    {
      if (permissionsInfo == null)
        throw new ArgumentNullException("permissionsInfo");

      m_permissionsInfo = permissionsInfo;

      this.PopulateFields();
      this.PopulateFunctions();
    }

    [JSProperty(Name = "hasUniqueRoleAssignments")]
    public bool HasUniqueRoleAssignments
    {
      get { return m_permissionsInfo.HasUniqueRoleAssignments; }
      set { m_permissionsInfo.HasUniqueRoleAssignments = value; }
    }

    [JSProperty(Name = "principals")]
    public ArrayInstance Principals
    {
      get
      {
// ReSharper disable CoVariantArrayConversion
        var result = this.Engine.Array.Construct(m_permissionsInfo.Principals.Select(p => new PrincipalRoleInfoInstance(this.Engine, p)).ToArray());
// ReSharper restore CoVariantArrayConversion
        return result;
      }
    }

    //TODO: Setter?
  }
}
