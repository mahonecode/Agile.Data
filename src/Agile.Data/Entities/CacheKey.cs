﻿using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Entities
{
    public class CacheKey
    {
        public string Database { get; set; }
        public List<string> Tables { get; set; }
        public List<string> IdentificationList { get; set; }
        public new string ToString()
        {
            return "AgileDataCache" + UtilConstants.Dot + string.Join(UtilConstants.Dot, this.Tables) +UtilConstants.Dot+ string.Join(UtilConstants.Dot, this.IdentificationList.Where(it=>it.HasValue()));
        }
    }
}