﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Agile.Data.ExpressionsToSql
{
    public class CaseWhen
    {
        public CaseThen ElseIF(bool condition)
        {
            return null;
        }
        public T End<T>(T defaultValue)
        {
            return default(T);
        }
    }
    public class CaseThen
    {

        public CaseWhen Return<T>(T result)
        {
            return null;
        }
    }
}
