﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barista.SuperSocket.SocketBase.Config
{
    /// <summary>
    /// The basic interface for command assembly config
    /// </summary>
    public interface ICommandAssemblyConfig
    {
        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        string Assembly { get; }
    }
}
