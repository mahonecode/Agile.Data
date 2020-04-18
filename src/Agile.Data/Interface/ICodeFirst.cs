using Agile.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Interface
{
    public partial interface ICodeFirst
    {
        AgileProvider Context { get; set; }
        ICodeFirst BackupTable(int maxBackupDataRows = int.MaxValue);
        ICodeFirst SetStringDefaultLength(int length);
        void InitTables(string entitiesNamespace);
        void InitTables(string[] entitiesNamespaces);
        void InitTables(params Type[] entityTypes);
        void InitTables(Type entityType);
        void InitTables<T>();
        void InitTables<T, T2>();
        void InitTables<T, T2, T3>();
        void InitTables<T, T2, T3, T4>();
    }
}
