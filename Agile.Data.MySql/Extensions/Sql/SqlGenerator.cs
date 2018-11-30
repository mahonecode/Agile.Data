using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.MySql.Extensions
{
    /// <summary>
    /// sql语句生成器
    /// </summary>
    public interface ISqlGenerator
    {
        IDapperExtensionsConfiguration Configuration { get; }
        
        string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters, string schemaName, string tableName);
        string SelectPaged(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters);
        string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters, string schemaName, string tableName);
        string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters, string schemaName, string tableName);
        string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters, string schemaName, string tableName);
        string PageCount(string sql);
        string Insert(IClassMapper classMap, string schemaName, string tableName);
        string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters, string schemaName, string tableName);
        string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters, string schemaName, string tableName);

        string IdentitySql(IClassMapper classMap, string schemaName, string tableName);
        string GetTableName(IClassMapper map, string schemaName, string tableName);
        string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias, string schemaName, string tableName);
        string GetColumnName(IClassMapper map, string propertyName, bool includeAlias, string schemaName, string tableName);
        bool SupportsMultipleStatements();
    }

    public class SqlGeneratorImpl : ISqlGenerator
    {
        public SqlGeneratorImpl(IDapperExtensionsConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDapperExtensionsConfiguration Configuration { get; private set; }

        public virtual string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters, string schemaName, string tableName)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap, schemaName, tableName),
                GetTableName(classMap, schemaName, tableName)));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters,schemaName,tableName));
            }

            if (sort != null && sort.Any())
            {
                sql.Append(" ORDER BY ")
                    .Append(sort.Select(s => GetColumnName(classMap, s.PropertyName, false, schemaName, tableName) + (s.Ascending ? " ASC" : " DESC")).AppendStrings());
            }
            return sql.ToString();
        }


        public string SelectPaged(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            string pageSql = Configuration.Dialect.GetPagingSql(sql, page, resultsPerPage, parameters);
            return pageSql;
        }

        public virtual string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters, string schemaName, string tableName)
        {
            if (sort == null || !sort.Any())
            {
                sort = classMap.Properties.Where(w => w.KeyType != KeyType.NotAKey).Select(w => (ISort)new Sort() { PropertyName = w.Name, Ascending = true }).ToList();
                //throw new ArgumentNullException("Sort", "Sort cannot be null or empty.");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap, schemaName, tableName),
                GetTableName(classMap, schemaName, tableName)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters,schemaName,tableName));
            }

            string orderBy = sort.Select(s => GetColumnName(classMap, s.PropertyName, false, schemaName, tableName) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
            innerSql.Append(" ORDER BY " + orderBy);

            string sql = Configuration.Dialect.GetPagingSql(innerSql.ToString(), page, resultsPerPage, parameters);
            return sql;
        }

        public virtual string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters, string schemaName, string tableName)
        {
            if (sort == null || !sort.Any())
            {
                sort = classMap.Properties.Where(w => w.KeyType != KeyType.NotAKey).Select(w => (ISort)new Sort() { PropertyName = w.Name, Ascending = true }).ToList();
                //throw new ArgumentNullException("Sort", "Sort cannot be null or empty.");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap, schemaName, tableName),
                GetTableName(classMap, schemaName, tableName)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters,schemaName,tableName));
            }

            string orderBy = sort.Select(s => GetColumnName(classMap, s.PropertyName, false, schemaName, tableName) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
            innerSql.Append(" ORDER BY " + orderBy);

            string sql = Configuration.Dialect.GetSetSql(innerSql.ToString(), firstResult, maxResults, parameters);
            return sql;
        }


        public virtual string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters, string schemaName, string tableName)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT COUNT(*) AS {0}Total{1} FROM {2}",
                                Configuration.Dialect.OpenQuote,
                                Configuration.Dialect.CloseQuote,
                                GetTableName(classMap, schemaName, tableName)));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters,schemaName,tableName));
            }

            return sql.ToString();
        }

        public virtual string PageCount(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sqlCount = new StringBuilder();
            //sqlCount.Append(Configuration.Dialect.BatchSeperator);
            sqlCount.Append(string.Format("SELECT COUNT(*) AS {0}Total{1} FROM ({2}) {0}TempCountData{1}",
                                     Configuration.Dialect.OpenQuote,
                                     Configuration.Dialect.CloseQuote,
                                     sql));

            return sqlCount.ToString();
        }

        public virtual string Insert(IClassMapper classMap, string schemaName, string tableName)
        {
            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.TriggerIdentity));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var columnNames = columns.Select(p => GetColumnName(classMap, p, false, schemaName, tableName));
            var parameters = columns.Select(p => Configuration.Dialect.ParameterPrefix + p.Name);

            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                                       GetTableName(classMap, schemaName, tableName),
                                       columnNames.AppendStrings(),
                                       parameters.AppendStrings());

            var triggerIdentityColumn = classMap.Properties.Where(p => p.KeyType == KeyType.TriggerIdentity).ToList();

            if (triggerIdentityColumn.Count > 0)
            {
                if (triggerIdentityColumn.Count > 1)
                    throw new ArgumentException("TriggerIdentity generator cannot be used with multi-column keys");

                sql += string.Format(" RETURNING {0} INTO {1}IdOutParam", triggerIdentityColumn.Select(p => GetColumnName(classMap, p, false, schemaName, tableName)).First(), Configuration.Dialect.ParameterPrefix);
            }
            return sql;
        }

        public virtual string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters, string schemaName, string tableName)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.Assigned));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var setSql =
                columns.Select(
                    p =>
                    string.Format(
                        "{0} = {1}{2}", GetColumnName(classMap, p, false, schemaName, tableName), Configuration.Dialect.ParameterPrefix, p.Name));

            string sql= string.Format("UPDATE {0} SET {1} WHERE {2}",
                GetTableName(classMap, schemaName, tableName),
                setSql.AppendStrings(),
                predicate.GetSql(this, parameters,schemaName,tableName));
            return sql;
        }
        
        public virtual string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters, string schemaName, string tableName)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("DELETE FROM {0}", GetTableName(classMap, schemaName, tableName)));
            sql.Append(" WHERE ").Append(predicate.GetSql(this, parameters,schemaName,tableName));

            return sql.ToString();
        }
        
        public virtual string IdentitySql(IClassMapper classMap, string schemaName, string tableName)
        {
            string sql= Configuration.Dialect.GetIdentitySql(GetTableName(classMap, schemaName, tableName));
            return sql;
        }

        public virtual string GetTableName(IClassMapper map, string schemaName, string tableName)
        {
            schemaName = string.IsNullOrWhiteSpace(schemaName) ? map.SchemaName : schemaName;
            tableName = string.IsNullOrWhiteSpace(tableName) ? map.TableName : tableName;
            return Configuration.Dialect.GetTableName(schemaName, tableName, null);
        }

        public virtual string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias, string schemaName, string tableName)
        {
            string alias = null;
            if (property.ColumnName != property.Name && includeAlias)
            {
                alias = property.Name;
            }

            return Configuration.Dialect.GetColumnName(GetTableName(map, schemaName, tableName), property.ColumnName, alias);
        }

        public virtual string GetColumnName(IClassMapper map, string propertyName, bool includeAlias, string schemaName, string tableName)
        {
            IPropertyMap propertyMap = map.Properties.SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (propertyMap == null)
            {
                throw new ArgumentException(string.Format("Could not find '{0}' in Mapping.", propertyName));
            }

            return GetColumnName(map, propertyMap, includeAlias,schemaName, tableName);
        }

        public virtual bool SupportsMultipleStatements()
        {
            return Configuration.Dialect.SupportsMultipleStatements;
        }

        public virtual string BuildSelectColumns(IClassMapper classMap, string schemaName, string tableName)
        {
            var columns = classMap.Properties
                .Where(p => !p.Ignored)
                .Select(p => GetColumnName(classMap, p, true, schemaName, tableName));
            return columns.AppendStrings();
        }
    }
}