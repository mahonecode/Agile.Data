﻿using Agile.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data.Realization
{
    public class MySqlInsertable<T> : InsertableProvider<T> where T : class, new()
    {

    }
}
