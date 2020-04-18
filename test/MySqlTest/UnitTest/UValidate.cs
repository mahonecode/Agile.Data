﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlTest.UnitTest
{
    public class UValidate
    {
        public static void Check(object a, object b, object name)
        {
            if (a?.ToString()?.Trim() != b?.ToString()?.Trim())
            {
                throw new Exception(name + " error");
            }
        }
    }
}
