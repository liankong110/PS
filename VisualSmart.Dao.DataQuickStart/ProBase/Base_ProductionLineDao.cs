using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;


namespace VisualSmart.Dao.DataQuickStart.ProBase
{
    public class Base_ProductionLineDao : EntityDao<Base_ProductionLine>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Base_ProductionLine entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_ProductionLine(");
                strSql.Append("ProLineNo,GoodNo,GoodName,PCS,StandPers,MinProNum,BoxNum,LineMins,ProShift,ProCapacityDesc,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProLineNo,@GoodNo,@GoodName,@PCS,@StandPers,@MinProNum,@BoxNum,@LineMins,@ProShift,@ProCapacityDesc,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public int AddGetId(Base_ProductionLine entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_ProductionLine(");
                strSql.Append("ProLineNo,GoodNo,GoodName,PCS,StandPers,MinProNum,BoxNum,LineMins,ProShift,ProCapacityDesc,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProLineNo,@GoodNo,@GoodName,@PCS,@StandPers,@MinProNum,@BoxNum,@LineMins,@ProShift,@ProCapacityDesc,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Base_ProductionLine entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Base_ProductionLine set ");
            strSql.Append("ProLineNo=@ProLineNo,");
            strSql.Append("GoodNo=@GoodNo,");
            strSql.Append("GoodName=@GoodName,");
            strSql.Append("PCS=@PCS,");
            strSql.Append("StandPers=@StandPers,");
            strSql.Append("MinProNum=@MinProNum,");
            strSql.Append("BoxNum=@BoxNum,");
            strSql.Append("LineMins=@LineMins,");
            strSql.Append("ProShift=@ProShift,");
            strSql.Append("ProCapacityDesc=@ProCapacityDesc,");
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
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Base_ProductionLine ");
            strSql.Append(" where ID=@ID ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }
       
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Base_ProductionLine> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ProLineNo,GoodNo,GoodName,PCS,StandPers,MinProNum,BoxNum,LineMins,ProShift,ProCapacityDesc,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Base_ProductionLine ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Base_ProductionLine>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }
        /// <summary>
        /// 获取id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetId(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id ");
            strSql.Append(" FROM Base_ProductionLine ");
            strSql.Append(query.GetSQL_Where(parameters));
            strSql.Append(query.GetSQL_Order());
            var result = ReadAdoTemplate.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            if (result == null || result is DBNull)
            {
                return -1;
            }
            return Convert.ToInt32(result);
        }
        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Base_ProductionLine MapRow(IDataReader dataReader, int rowNum)
        {
            Base_ProductionLine model = new Base_ProductionLine();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.ProLineNo = dataReader["ProLineNo"].ToString();
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            ojb = dataReader["PCS"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PCS = (int)ojb;
            }
            ojb = dataReader["StandPers"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StandPers = (int)ojb;
            }
            ojb = dataReader["MinProNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MinProNum = (int)ojb;
            }
            ojb = dataReader["BoxNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BoxNum = (int)ojb;
            }
            ojb = dataReader["LineMins"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LineMins = (int)ojb;
            }
            ojb = dataReader["ProShift"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProShift = (int)ojb;
            }
            model.ProCapacityDesc = dataReader["ProCapacityDesc"].ToString();
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
        private IDbParameters GetBaseParams(Base_ProductionLine entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();

            parameters.Add("ProLineNo", DbType.String).Value = entity.ProLineNo ?? "";
            parameters.Add("GoodNo", DbType.String).Value = entity.GoodNo ?? "";
            parameters.Add("GoodName", DbType.String).Value = entity.GoodName ?? "";
            parameters.Add("PCS", DbType.Int32).Value = entity.PCS;
            parameters.Add("StandPers", DbType.Int32).Value = entity.StandPers;

            parameters.Add("MinProNum", DbType.Int32).Value = entity.MinProNum;
            parameters.Add("BoxNum", DbType.Int32).Value = entity.BoxNum;
            parameters.Add("LineMins", DbType.Int32).Value = entity.LineMins;
            parameters.Add("ProShift", DbType.Int32).Value = entity.ProShift;
            parameters.Add("ProCapacityDesc", DbType.String).Value = entity.ProCapacityDesc ?? "";

            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }

        /// <summary>
        /// 获取所有产线信息
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllProLineNos(int ShipPlanMainId)
        {
            string sql = string.Format(@" select ProLineNo from [dbo].[Base_ProductionLine] 
 left join[dbo].[Pro_ShipPlan]  on Base_ProductionLine.GoodNo = Pro_ShipPlan.GoodNo
 where Pro_ShipPlan.MainId = {0}
 group by ProLineNo", ShipPlanMainId);
            return ReadAdoTemplate.QueryWithRowMapperDelegate<string>(CommandType.Text, sql, delegate (IDataReader dataReader, int rowNum)
            {
                return dataReader["ProLineNo"].ToString(); ;
            });

        }
    }
}
