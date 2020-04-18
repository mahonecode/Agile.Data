using Agile.Data.Abstract;
using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Data.Interface
{
    public interface IDeleteable<T> where T : class, new()
    {
        DeleteBuilder DeleteBuilder { get; set; }
        int ExecuteCommand();
        bool ExecuteCommandHasChange();
        Task<int> ExecuteCommandAsync();
        Task<bool> ExecuteCommandHasChangeAsync();
        IDeleteable<T> AS(string tableName);
        IDeleteable<T> With(string lockString);
        IDeleteable<T> Where(T deleteObj);
        IDeleteable<T> Where(Expression<Func<T, bool>> expression);
        IDeleteable<T> Where(List<T> deleteObjs);
        IDeleteable<T> In<PkType>(PkType primaryKeyValue);
        IDeleteable<T> In<PkType>(PkType[] primaryKeyValues);
        IDeleteable<T> In<PkType>(List<PkType> primaryKeyValues);
        IDeleteable<T> In<PkType>(Expression<Func<T,object>> inField,PkType primaryKeyValue);
        IDeleteable<T> In<PkType>(Expression<Func<T, object>> inField,PkType[] primaryKeyValues);
        IDeleteable<T> In<PkType>(Expression<Func<T, object>> inField,List<PkType> primaryKeyValues);
        IDeleteable<T> Where(string whereString,object parameters=null);
        IDeleteable<T> Where(string whereString, AgileParameter parameter);
        IDeleteable<T> Where(string whereString, AgileParameter[] parameters);
        IDeleteable<T> Where(string whereString, List<AgileParameter> parameters);
        IDeleteable<T> EnableDiffLogEvent(object businessData = null);
        IDeleteable<T> RemoveDataCache();
        KeyValuePair<string, List<AgileParameter>> ToSql();
        void AddQueue();
    }
}
