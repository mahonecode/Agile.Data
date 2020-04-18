using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Utilities
{
    internal static partial class ErrorMessage
    {
        internal static string ObjNotExist
        {
            get
            {
                return GetThrowMessage("{0} does not exist.",
                                       "{0}不存在。");
            }
        }
        internal static string EntityMappingError
        {
            get
            {
                return GetThrowMessage("Entity mapping error.{0}",
                                       "实体与表映射出错。{0}");
            }
        }

        public static string NotSupportedDictionary
        {
            get
            {
                return GetThrowMessage("This type of Dictionary is not supported for the time being. You can try Dictionary<string, string>, or contact the author!!",
                                       "暂时不支持该类型的Dictionary 你可以试试 Dictionary<string ,string>或者联系作者！！");
            }
        }

        public static string NotSupportedArray
        {
            get
            {
                return GetThrowMessage("This type of Array is not supported for the time being. You can try object[] or contact the author!!",
                                       "暂时不支持该类型的Array 你可以试试 object[] 或者联系作者！！");
            }
        }

        internal static string GetThrowMessage(string enMessage, string cnMessage, params string[] args)
        {
            List<string> formatArgs = new List<string>() { enMessage, cnMessage };
            formatArgs.AddRange(args);
            return string.Format(@"English Message : {0}
Chinese Message : {1}", formatArgs.ToArray());
        }
    }


    internal static partial class ErrorMessage
    {
        internal static string OperatorError
        {
            get
            {
                return ErrorMessage.GetThrowMessage("Lambda parsing error: {0} does not support the operator to find!", "拉姆达解析出错：不支持{0}此种运算符查找！");
            }
        }
        internal static string ExpFileldError
        {
            get
            {
                return ErrorMessage.GetThrowMessage("Expression format error, correct format: it=>it.fieldName", "表达式格式错误，正确格式： it=>it.fieldName");
            }
        }

        internal static string MethodError
        {
            get
            {
                return ErrorMessage.GetThrowMessage("Expression parsing does not support the current function {0}. There are many functions available in the SqlFunc class, for example, it=>SqlFunc.HasValue(it.Id)", "拉姆达解析不支持当前函数{0}，SqlFunc这个类里面有大量函数可用,也许有你想要的，例如： it=>SqlFunc.HasValue(it.Id)");
            }
        }

        public static string ConnnectionOpen
        {
            get
            {
                return ErrorMessage.GetThrowMessage("Connection open error . {0}", " 连接数据库过程中发生错误，检查服务器是否正常连接字符串是否正确，实在找不到原因请先Google错误信息：{0}.");
            }
        }
        public static string ExpressionCheck
        {
            get
            {
                return ErrorMessage.GetThrowMessage("Join {0} needs to be the same as {1} {2}", "多表查询存在别名不一致,请把{1}中的{2}改成{0}就可以了，特殊需求可以使用.Select((x,y)=>new{{ id=x.id,name=y.name}}).MergeTable().Orderby(xxx=>xxx.Id)功能将Select中的多表结果集变成单表，这样就可以不限制别名一样");
            }
        }

        public static string WhereIFCheck
        {
            get
            {
                return ErrorMessage.GetThrowMessage("Subquery.WhereIF.IsWhere {0} not supported", "Subquery.WhereIF 第一个参数不支持表达式中的变量，只支持外部变量");
            }
        }
    }
}
