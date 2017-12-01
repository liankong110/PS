using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.Pro
{
    public class Pro_ShipPlanDao : EntityDao<Pro_ShipPlan>
    {  /// <summary>
       /// 新增
       /// </summary>
       /// <param name="entity"></param>
       /// <returns></returns>
        public override bool Add(Pro_ShipPlan entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_ShipPlan(");
                strSql.Append("MainId,ScheduleNo,Term,EditionNo,CityNo,ShipTo,ShipToName,GoodNo,GoodName,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@MainId,@ScheduleNo,@Term,@EditionNo,@CityNo,@ShipTo,@ShipToName,@GoodNo,@GoodName,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Update(Pro_ShipPlan entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_ShipPlan set ");
            strSql.Append("MainId=@MainId,");
            strSql.Append("ScheduleNo=@ScheduleNo,");
            strSql.Append("Term=@Term,");
            strSql.Append("EditionNo=@EditionNo,");
            strSql.Append("CityNo=@CityNo,");
            strSql.Append("ShipTo=@ShipTo,");
            strSql.Append("ShipToName=@ShipToName,");
            strSql.Append("GoodNo=@GoodNo,");
            strSql.Append("GoodName=@GoodName,");
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
            strSql.Append("delete from Pro_ShipPlan ");
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
        public override IList<Pro_ShipPlan> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,MainId,ScheduleNo,Term,EditionNo,CityNo,ShipTo,ShipToName,GoodNo,GoodName,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Pro_ShipPlan ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlan>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_ShipPlan MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_ShipPlan model = new Pro_ShipPlan();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["MainId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MainId = (int)ojb;
            }
            model.ScheduleNo = dataReader["ScheduleNo"].ToString();
            ojb = dataReader["Term"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Term = (int)ojb;
            }
            model.EditionNo = dataReader["EditionNo"].ToString();
            model.CityNo = dataReader["CityNo"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
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
        private IDbParameters GetBaseParams(Pro_ShipPlan entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("MainId", DbType.Int32).Value = entity.MainId;
            parameters.Add("ScheduleNo", DbType.String).Value = entity.ScheduleNo;
            parameters.Add("Term", DbType.Int32).Value = entity.Term;
            parameters.Add("EditionNo", DbType.String).Value = entity.EditionNo;
            parameters.Add("CityNo", DbType.String).Value = entity.CityNo;
            parameters.Add("ShipTo", DbType.String).Value = entity.ShipTo;
            parameters.Add("ShipToName", DbType.String).Value = entity.ShipToName;
            parameters.Add("GoodNo", DbType.String).Value = entity.GoodNo;
            parameters.Add("GoodName", DbType.String).Value = entity.GoodName;

            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }


        /// <summary>
        /// 根据生产线信息获取对应的发运信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select Distinct ProLineNo,Qty, Pro_ShipPlan.ID,MainId,ScheduleNo,Term,EditionNo,CityNo,ShipTo,ShipToName,
Pro_ShipPlan.GoodNo,Pro_ShipPlan.GoodName
FROM Pro_ShipPlan
left join [dbo].[Base_ProductionLine] on [dbo].[Base_ProductionLine].GoodNo=Pro_ShipPlan.GoodNo
left join [dbo].[Base_Stock] on [dbo].[Base_Stock].GoodNo=Pro_ShipPlan.GoodNo");
            

            var lineNos=query.GetCondition("LineNos").Value;
            strSql.AppendFormat(" where ProLineNo IN ({0}) ", lineNos);          

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlan>(CommandType.Text, strSql.ToString(), MapRowByLineNos, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_ShipPlan MapRowByLineNos(IDataReader dataReader, int rowNum)
        {
            Pro_ShipPlan model = new Pro_ShipPlan();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["MainId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MainId = (int)ojb;
            }
            model.ScheduleNo = dataReader["ScheduleNo"].ToString();
            ojb = dataReader["Term"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Term = (int)ojb;
            }
            model.EditionNo = dataReader["EditionNo"].ToString();
            model.CityNo = dataReader["CityNo"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            model.ProLineNo = dataReader["ProLineNo"].ToString();
            ojb = dataReader["Qty"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Qty = Convert.ToInt32(ojb);
            }
            return model;
        }
    }
}
