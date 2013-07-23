﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barista.SuperSocket.SocketBase
{
    /// <summary>
    /// AppServer instance running isolation mode
    /// </summary>
    public enum IsolationMode
    {
        /// <summary>
        /// No isolation
        /// </summary>
        None,
        /// <summary>
        /// Isolation by AppDomain
        /// </summary>
        AppDomain
    }
}
