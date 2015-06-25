﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.DDay.iCal.Serialization
{
    public interface ISerializerFactory
    {
        ISerializer Build(Type objectType, ISerializationContext ctx);
    }
}
