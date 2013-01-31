﻿namespace Barista.WebSocket.Config
{
  using System.Configuration;
  using Barista;

  /// <summary>
  /// SubProtocol configuration
  /// </summary>
  public class SubProtocolConfig : ConfigurationElementBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SubProtocolConfig"/> class.
    /// </summary>
    public SubProtocolConfig()
      : base(false)
    {

    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    [ConfigurationProperty("type", IsRequired = false)]
    public string Type
    {
      get
      {
        return (string)this["type"];
      }
    }

    /// <summary>
    /// Gets the commands.
    /// </summary>
    [ConfigurationProperty("commands")]
    public CommandConfigCollection Commands
    {
      get
      {
        return this["commands"] as CommandConfigCollection;
      }
    }
  }
}