using Spring.Data.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;


namespace VisualSmart.Dao.DataQuickStart.Pro
{
    public class Pro_ShipPlanMainDao : EntityDao<Pro_ShipPlanMain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_ShipPlanMain entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_ShipPlanMain", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_ShipPlanMain(");
                strSql.Append("ProNo,PlanFromDate,PlanFromTo,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@PlanFromDate,@PlanFromTo,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
                strSql.Append(";select @@IDENTITY");
                var parameters = GetBaseParams(entity);
                return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 新增 返回ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_ShipPlanMain entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_ShipPlanMain", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_ShipPlanMain(");
                strSql.Append("ProNo,PlanFromDate,PlanFromTo,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@PlanFromDate,@PlanFromTo,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
                strSql.Append(";select @@IDENTITY");
                var parameters = GetBaseParams(entity);
                return Convert.ToInt32(ReadAdoTemplate.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Update(Pro_ShipPlanMain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_ShipPlanMain set ");
            strSql.Append("PlanFromDate=@PlanFromDate,");
            strSql.Append("PlanFromTo=@PlanFromTo,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("Creater=@Creater,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("RowState=@RowState");
            strSql.Append(" where ID=@ID");
            var parameters = GetBaseParams(entity);
            parameters.Add("ID", DbType.Int32, 0).Value = entity.Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除- 三个表一起删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Pro_ShipPlans  where PlanId in( select ID from Pro_ShipPlan where MainId=@ID);");
            strSql.Append("delete from Pro_ShipPlan  where MainId=@ID;");
            strSql.Append("delete from Pro_ShipPlanMain  where ID=@ID;");        
           
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Pro_ShipPlanMain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ProNo,PlanFromDate,PlanFromTo,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Pro_ShipPlanMain ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlanMain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_ShipPlanMain MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_ShipPlanMain model = new Pro_ShipPlanMain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["PlanFromDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanFromDate = (DateTime)ojb;
            }
            ojb = dataReader["PlanFromTo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanFromTo = (DateTime)ojb;
            }
            ojb = dataReader["CreateTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreateTime = (DateTime)ojb;
            }
            model.Creater = dataReader["Creater"].ToString();
            ojb = dataReader["UpdateTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UpdateTime = (DateTime)ojb;
            }

            model.ProNo = dataReader["ProNo"].ToString();
            model.Updater = dataReader["Updater"].ToString();
            ojb = dataReader["RowState"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RowState = (byte)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Pro_ShipPlanMain entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("PlanFromDate", DbType.DateTime).Value = entity.PlanFromDate;
            parameters.Add("PlanFromTo", DbType.DateTime).Value = entity.PlanFromTo;
            parameters.Add("ProNo", SqlDbType.NVarChar).Value = entity.ProNo;
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlanMain> GetList(QueryCondition query, Hashtable hs)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();          
            strSql.Append("select ID,ProNo,PlanFromDate,PlanFromTo,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Pro_ShipPlanMain ");
            string otherWhere = "";
            if (hs.ContainsKey("PlanDate"))
            {
                otherWhere += string.Format(" and '{0}'>=PlanFromDate and '{0}'<=PlanFromTo", hs["PlanFromDate"]);
            }
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters, otherWhere));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlanMain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }
    }
}
