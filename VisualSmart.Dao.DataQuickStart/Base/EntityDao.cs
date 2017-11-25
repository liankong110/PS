using Spring.Context.Support;
using Spring.Data.Common;
using Spring.Data.Config;
using Spring.Data.Generic;
using Spring.Objects.Factory.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.Base
{
    public  class EntityDao<T> : AdoDaoSupport, IEntityDao<T>
       where T : IEntity, new()
    {

        private XmlApplicationContext GetXmlApplicationContext()
        {
            NamespaceParserRegistry.RegisterParser(typeof(DatabaseNamespaceParser));
            XmlApplicationContext ctx = new XmlApplicationContext(
                "assembly://VisualSmart.Dao.DataQuickStart/VisualSmart.Dao.DataQuickStart.Config/Dao.xml");
            return ctx;
        }
        private AdoTemplate writeAdoTemplate;
        public AdoTemplate WriteAdoTemplate //{ get; set; }
        {
            get
            {
                if (writeAdoTemplate == null)
                {
                    writeAdoTemplate = VisualSmart.BizService.Core.Smart.ctx["writeAdoTemplate"] as AdoTemplate;
                    //VisualSmart.Util.ServiceAppSetting.LoggerHander(writeAdoTemplate.GetType().GUID.ToString(), "Error");
                    //Class1 c = new Class1();
                    //VisualSmart.Util.ServiceAppSetting.LoggerHander("c"+c.GetType().GUID.ToString(), "Error");
                    //Class1 c1 = new Class1();
                    //VisualSmart.Util.ServiceAppSetting.LoggerHander("c1" + c1.GetType().GUID.ToString(), "Error");
                    //Class1 c2 = new Class1();
                    //VisualSmart.Util.ServiceAppSetting.LoggerHander("c2" + c2.GetType().GUID.ToString(), "Error");

                }
                return writeAdoTemplate;
            }
        }
        private AdoTemplate readAdoTemplate;
        public AdoTemplate ReadAdoTemplate //{ get; set; }
        {
            get
            {
                if (readAdoTemplate == null)
                {
                    readAdoTemplate =VisualSmart.BizService.Core.Smart.ctx["readAdoTemplate"] as AdoTemplate;
                }
                return readAdoTemplate;
            }
        }

        private Spring.Data.Core.AdoTemplate dataSet_ReadAdoTemplate;
        public Spring.Data.Core.AdoTemplate DataSet_ReadAdoTemplate //{ get; set; }
        {
            get
            {
                if (dataSet_ReadAdoTemplate == null)
                {
                    dataSet_ReadAdoTemplate = VisualSmart.BizService.Core.Smart.ctx["dataSet_ReadAdoTemplate"] as Spring.Data.Core.AdoTemplate;
                }
                return dataSet_ReadAdoTemplate;
            }
        }
        public virtual bool Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>       ;       
        public virtual IList<T> GetAllDomain(QueryCondition query)
        {
            throw new NotImplementedException();
        }


        public string GetPagerSql(string sqlBody, QueryCondition query, IDbParameters parameters,string otherWhere="",string otherSum="")
        {
            PagerDomain domain = query.GetPager();
            string strWhere = query.GetSQL_Where(parameters).ToString();
            string orderby = query.GetSQL_Order().ToString();
            #region 查询总条数
            var strSql = new StringBuilder();
            strSql.Append("select count(1) ");
            if (!string.IsNullOrEmpty(otherSum))
            {
                strSql.Append(","+otherSum);
            }
            strSql.Append(sqlBody.Substring(sqlBody.ToUpper().IndexOf(" FROM ")));
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(strWhere);
            }
            if (!string.IsNullOrEmpty(otherWhere.Trim()))
            {
                strSql.Append(otherWhere);
            }
            var allCount = DataSet_ReadAdoTemplate.DataSetCreateWithParams(CommandType.Text, strSql.ToString(), parameters);
           
            domain.TotalCount = Convert.ToInt32(allCount.Tables[0].Rows[0][0]);
            domain.SumDT=allCount.Tables[0];
            #endregion

            #region 拼接好的分页SQL
            strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append(orderby);
            }
            strSql.AppendFormat(")AS Row, {0} ", sqlBody.Substring(sqlBody.ToUpper().IndexOf("SELECT ") + 7));
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(strWhere);
            }
            if (!string.IsNullOrEmpty(otherWhere.Trim()))
            {
                strSql.Append(otherWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", ((domain.CurrentPage - 1) * domain.PageSize+1), domain.CurrentPage * domain.PageSize);
            #endregion

            return strSql.ToString();
        }
    }
}
