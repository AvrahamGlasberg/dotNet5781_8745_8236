﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineStation : Station
    {
        public int DitanceFromPrev { get; set; }
        public int TimeFromPrev { get; set; }
    }
}
