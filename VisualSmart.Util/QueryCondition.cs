using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Util
{  /// <summary>
    /// 在此描述QueryCondition的说明
    /// </summary>
    [Serializable]
    public class QueryCondition
    {
        #region 变量
        //查询字段列表
        public IList<string> selectFields;
        //查询表格
        //string table;
        //排序规则列表
        readonly IList<KeyValuePair<string, bool>> orderBys;
        //基本查询条件
        readonly IList<Condition> conditions;
        //分页配置
        PagerDomain po;

        #endregion

        #region 构造函数
        private QueryCondition()
        {
            selectFields = new List<string>();
            orderBys = new List<KeyValuePair<string, bool>>();
            conditions = new List<Condition>();
        }

        public static QueryCondition Instance
        {
            get
            {
                return new QueryCondition();
            }
        }
        #endregion

        #region 查询条件
        /// <summary>
        /// 新增一个等于条件，单表
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddEqual(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.Equal, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个等于条件，多表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddEqual(string tableName, string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(tableName + "." + fieldName, value, QueryOperator.Equal, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个不相等条件，单表
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public QueryCondition AddNotEqual(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.NotEqual, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个不相等条件，多表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddNotEqual(string tableName, string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(tableName + "." + fieldName, value, QueryOperator.NotEqual, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个大于条件，单表
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public QueryCondition AddLarger(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.Larger, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个大于条件，多表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddLarger(string tableName, string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(tableName + "." + fieldName, value, QueryOperator.Larger, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个小于条件，单表
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public QueryCondition AddSmaller(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.Smaller, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个小于条件，多表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddSmaller(string tableName, string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(tableName + "." + fieldName, value, QueryOperator.Smaller, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个大于等于条件，单表
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddEqualLarger(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.EqualLarger, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个大于等于条件，多表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddEqualLarger(string tableName, string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(tableName + "." + fieldName, value, QueryOperator.EqualLarger, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个小于等于条件，单表
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddEqualSmaller(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.EqualSmaller, isJoinSql));
            return this;
        }

        /// <summary>
        /// 新增一个小于等于条件，多表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">等于值</param>
        /// <returns></returns>
        public QueryCondition AddEqualSmaller(string tableName, string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(tableName + "." + fieldName, value, QueryOperator.EqualSmaller, isJoinSql));
            return this;
        }

        public QueryCondition AddLike(string fieldName, string value, bool isJoinSql = true)
        {
            conditions.Add(new Condition(fieldName, value, QueryOperator.Like, isJoinSql));
            return this;
        }

        #endregion

        #region 排序
        /// <summary>
        /// 新增排序规则，单表
        /// </summary>
        /// <param name="fieldName">排序字段名称</param>
        /// <param name="asc">是否是ASC</param>
        /// <returns></returns>
        public QueryCondition AddOrderBy(string fieldName, bool asc)
        {
            orderBys.Add(new KeyValuePair<string, bool>(fieldName, asc));
            return this;
        }

        /// <summary>
        /// 新增排序规则，多表
        /// </summary>
        /// <param name="tableName">表名，用于JOIN时排序</param>
        /// <param name="fieldName">排序字段名称</param>
        /// <param name="asc">是否是ASC</param>
        /// <returns></returns>
        public QueryCondition AddOrderBy(string tableName, string fieldName, bool asc)
        {
            orderBys.Add(new KeyValuePair<string, bool>(tableName.ToLower() + "." + fieldName, asc));
            return this;
        }
        #endregion

        #region 分页
        public QueryCondition SetPager(int currentPage, int pageSize)
        {
            po = new PagerDomain(currentPage, pageSize);
            return this;
        }

        public PagerDomain GetPager()
        {
            return po;
        }
        #endregion

        #region 基本信息
        /// <summary>
        /// 设置单表查询字段
        /// </summary>
        /// <param name="fieldName">字段列表</param>
        /// <returns></returns>
        public QueryCondition AddSelectFields(string fieldName)
        {
            selectFields.Add(fieldName);
            return this;
        }

        /// <summary>
        /// 设置多表查询字段
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="fieldName">字段列表</param>
        /// <returns></returns>
        public QueryCondition AddSelectFields(string tableName, string fieldName)
        {
            selectFields.Add(tableName + "." + fieldName);
            return this;
        }
        #endregion

        #region 解析

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Condition GetCondition(string fieldName)
        {
            return conditions.ToList().Find(t => t.FieldName == fieldName);
        }

        /// <summary>
        /// 获取SQL 条件
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 转为为Where条件
        /// </summary>       
        /// <returns></returns>
        public StringBuilder GetSQL_Where(IDbParameters dbPrarms)
        {
            var sb = new StringBuilder(" where 1=1 ");
            #region 普通条件
            var current = 0;
            foreach (var c in conditions)
            {
                if (c.Op == QueryOperator.Like)
                {
                    sb.Append(" AND ").Append(c.FieldName).Append(" LIKE '%'+@PARM" + current + "+'%'");
                }
                else if (c.Op == QueryOperator.L_Like)
                {
                    sb.Append(" AND ").Append(c.FieldName).Append(" LIKE '%'+@PARM" + current + "+''");
                }
                else if (c.Op == QueryOperator.R_Like)
                {
                    sb.Append(" AND ").Append(c.FieldName).Append(" LIKE ''+@PARM" + current + "+'%'");
                }
                else
                {
                    sb.Append(" AND ").Append(c.FieldName).Append(QueryOperatorManager.GetOperator(c.Op) + "@PARM").
                        Append(current);
                }
                dbPrarms.AddWithValue("PARM" + current, c.Value);
                current++;
            }
            #endregion

            return sb;
        }


        /// <summary>
        /// 获取SQL 条件
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 转为为Where条件
        /// </summary>       
        /// <returns></returns>
        public void GetSQL_WhereParams(IDbParameters dbPrarms)
        {
            #region 普通条件
            var current = 0;
            foreach (var c in conditions)
            {
                dbPrarms.AddWithValue("PARM" + current, c.Value);
                current++;
            }
            #endregion
        }

        /// <summary>
        /// 获取排序
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetSQL_Order()
        {
            #region 排序
            var sb = new StringBuilder();
            if (orderBys.Count > 0)
            {
                var i = 0;
                sb.Append(" ORDER BY ");
                foreach (var order in orderBys)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(order.Key).Append(" ").Append(order.Value ? "ASC" : "DESC");
                    i++;
                }
            }
            return sb;
            #endregion
        }
        #endregion
    }

    #region 查询条件
    public class Condition
    {
        public string FieldName { get; set; }

        public string Value { get; set; }

        public bool IsJoinSql { get; set; }

        public QueryOperator Op { get; set; }

        public Condition(string fieldName, string value, QueryOperator op, bool isJoinSql)
        {
            FieldName = fieldName;
            Value = value;
            Op = op;
            IsJoinSql = isJoinSql;
        }
    }
    #endregion

    #region 分页设置类
    public class PagerDomain
    {
        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每页数据条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数据条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 汇总集合
        /// </summary>
        public DataTable SumDT { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                if (TotalCount % PageSize == 0)
                {
                    return TotalCount / PageSize;
                }
                return TotalCount / PageSize + 1;

            }

        }

        public PagerDomain(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }



    }
    #endregion

    #region 操作符
    /// <summary>
    /// 查询操作符枚举
    /// </summary>
    public enum QueryOperator
    {
        Equal,
        Larger,
        Smaller,
        EqualLarger,
        EqualSmaller,
        NotEqual,
        Like,
        L_Like,
        R_Like
    }

    public class QueryOperatorManager
    {
        public static string GetOperator(QueryOperator op)
        {
            switch (op)
            {
                case QueryOperator.EqualLarger:
                    return ">=";
                case QueryOperator.Equal:
                    return "=";
                case QueryOperator.EqualSmaller:
                    return "<=";
                case QueryOperator.Smaller:
                    return "<";
                case QueryOperator.NotEqual:
                    return "!=";
                case QueryOperator.Larger:
                    return ">";
            }
            return string.Empty;
        }
    }
    #endregion
}
