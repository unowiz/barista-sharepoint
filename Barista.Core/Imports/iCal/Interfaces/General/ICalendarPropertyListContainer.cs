﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.DDay.iCal
{
    public interface ICalendarPropertyListContainer :
        ICalendarObject
    {
        ICalendarPropertyList Properties { get; }
    }
}
