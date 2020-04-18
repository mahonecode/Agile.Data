﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Data.ExpressionsToSql
{
    public class BinaryExpressionInfo
    {
        public bool IsLeft { get; set; }
        public Type ExpressionType { get;set;}
        public object Value { get; set; }
    }
}
