using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using Agile.Data.Interface;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Agile.Data.Abstract
{
    public partial class AdoAccessory
    {
        protected IDbBind _DbBind;
        protected IDbFirst _DbFirst;
        protected ICodeFirst _CodeFirst;
        protected IDbMaintenance _DbMaintenance;
        protected IDbConnection _DbConnection;

        protected virtual AgileParameter[] GetParameters(object parameters, PropertyInfo[] propertyInfo, string sqlParameterKeyWord)
        {
            List<AgileParameter> result = new List<AgileParameter>();
            if (parameters != null)
            {
                var entityType = parameters.GetType();
                var isDictionary = entityType.IsIn(UtilConstants.DicArraySO, UtilConstants.DicArraySS);
                if (isDictionary)
                    DictionaryToParameters(parameters, sqlParameterKeyWord, result, entityType);
                else if (parameters is List<AgileParameter>)
                {
                    result = (parameters as List<AgileParameter>);
                }
                else if (parameters is AgileParameter[])
                {
                    result = (parameters as AgileParameter[]).ToList();
                }
                else
                {
                    Check.Exception(!entityType.IsAnonymousType(), "The parameter format is wrong. \nUse new{{xx=xx, xx2=xx2}}  or \nDictionary<string, object> or \nAgileParameter [] ");
                    ProperyToParameter(parameters, propertyInfo, sqlParameterKeyWord, result, entityType);
                }
            }
            return result.ToArray();
        }
        protected void ProperyToParameter(object parameters, PropertyInfo[] propertyInfo, string sqlParameterKeyWord, List<AgileParameter> listParams, Type entityType)
        {
            PropertyInfo[] properties = null;
            if (propertyInfo != null)
                properties = propertyInfo;
            else
                properties = entityType.GetProperties();

            foreach (PropertyInfo properyty in properties)
            {
                var value = properyty.GetValue(parameters, null);
                if (properyty.PropertyType.IsEnum())
                    value = Convert.ToInt64(value);
                if (value == null || value.Equals(DateTime.MinValue)) value = DBNull.Value;
                if (properyty.Name.ToLower().Contains("hierarchyid"))
                {
                    var parameter = new AgileParameter(sqlParameterKeyWord + properyty.Name, SqlDbType.Udt);
                    parameter.UdtTypeName = "HIERARCHYID";
                    parameter.Value = value;
                    listParams.Add(parameter);
                }
                else
                {
                    var parameter = new AgileParameter(sqlParameterKeyWord + properyty.Name, value);
                    listParams.Add(parameter);
                }
            }
        }
        protected void DictionaryToParameters(object parameters, string sqlParameterKeyWord, List<AgileParameter> listParams, Type entityType)
        {
            if (entityType == UtilConstants.DicArraySO)
            {
                var dictionaryParameters = (Dictionary<string, object>)parameters;
                var agileParameters = dictionaryParameters.Select(it => new AgileParameter(sqlParameterKeyWord + it.Key, it.Value));
                listParams.AddRange(agileParameters);
            }
            else
            {
                var dictionaryParameters = (Dictionary<string, string>)parameters;
                var agileParameters = dictionaryParameters.Select(it => new AgileParameter(sqlParameterKeyWord + it.Key, it.Value));
                listParams.AddRange(agileParameters); ;
            }
        }
    }
}