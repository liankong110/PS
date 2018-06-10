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
    public class Pro_SchedulingDao : EntityDao<Pro_Scheduling>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_Scheduling entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_Scheduling", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_Scheduling(");
                strSql.Append("ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@ShipMainProNo,@PlanFromDate,@PlanToDate,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_Scheduling entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_Scheduling", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_Scheduling(");
                strSql.Append("ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@ShipMainProNo,@PlanFromDate,@PlanToDate,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Pro_Scheduling entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_Scheduling set ");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater");
            strSql.Append(" where Id=@Id;");
            var parameters = GetBaseParams(entity);
            parameters.Add("ID", DbType.Int32, 0).Value = entity.Id;

            //删除所有除Pro_Scheduling 的所有信息
            strSql.Append(@"delete from Pro_SchedulingGoodsNum  where SGoodId IN (select id from [dbo].[Pro_SchedulingGoods]
 where SLineId IN(select id from[dbo].[Pro_SchedulingLine] where MainId =@ID)); ");
            strSql.Append("delete from [Pro_SchedulingGoods] where SLineId IN ( select id from[dbo].[Pro_SchedulingLine] where MainId=@ID);");
            strSql.Append("delete from [Pro_SchedulingLine] where MainId=@ID;");

            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"delete from Pro_SchedulingGoodsNum  where SGoodId IN (select id from [dbo].[Pro_SchedulingGoods]
 where SLineId IN(select id from[dbo].[Pro_SchedulingLine] where MainId =@ID)); ");
            strSql.Append("delete from [Pro_SchedulingGoods] where SLineId IN ( select id from[dbo].[Pro_SchedulingLine] where MainId=@ID);");
            strSql.Append("delete from [Pro_SchedulingLine] where MainId=@ID;");
            strSql.Append("delete from Pro_Scheduling where ID=@ID;");       
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Pro_Scheduling> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   ");
            strSql.Append(" Id,ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" from Pro_Scheduling ");
            string otherWhere = "";
            if (query.GetCondition("PlanFromDate") != null)
            {
                
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

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_Scheduling>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public  IList<Pro_Scheduling> GetList(QueryCondition query,Hashtable hs)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   ");
            strSql.Append(" Id,ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" from Pro_Scheduling ");
            string otherWhere = "";
            if (hs.ContainsKey("PlanFromDate"))
            {
                otherWhere += string.Format(" and '{0}'>=PlanFromDate and '{0}'<=PlanToDate", hs["PlanFromDate"]);
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

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_Scheduling>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_Scheduling MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_Scheduling model = new Pro_Scheduling();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.ProNo = dataReader["ProNo"].ToString();
            model.ShipMainProNo = dataReader["ShipMainProNo"].ToString();
            
            ojb = dataReader["PlanFromDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanFromDate = (DateTime)ojb;
            }
            ojb = dataReader["PlanToDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanToDate = (DateTime)ojb;
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
        private IDbParameters GetBaseParams(Pro_Scheduling entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();

            parameters.Add("ShipMainProNo", DbType.String).Value = entity.ShipMainProNo;
            parameters.Add("ProNo", DbType.String).Value = entity.ProNo;
            parameters.Add("PlanFromDate", DbType.Date).Value = entity.PlanFromDate;
            parameters.Add("PlanToDate", DbType.Date).Value = entity.PlanToDate;
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }

    }
}
