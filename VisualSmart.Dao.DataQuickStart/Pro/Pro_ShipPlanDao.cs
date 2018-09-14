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
                strSql.Append("MainId,ScheduleNo,Term,EditionNo,CityNo,ShipDetailNo,ShipTo,ShipToName,GoodNo,GoodName)");
                strSql.Append(" values (");
                strSql.Append("@MainId,@ScheduleNo,@Term,@EditionNo,@CityNo,@ShipDetailNo,@ShipTo,@ShipToName,@GoodNo,@GoodName)");
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
        /// 新增获取ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_ShipPlan entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_ShipPlan(");
                strSql.Append("MainId,ScheduleNo,Term,EditionNo,CityNo,ShipDetailNo,ShipTo,ShipToName,GoodNo,GoodName)");
                strSql.Append(" values (");
                strSql.Append("@MainId,@ScheduleNo,@Term,@EditionNo,@CityNo,@ShipDetailNo,@ShipTo,@ShipToName,@GoodNo,@GoodName)");
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
            strSql.Append("GoodName=@GoodName");

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
            strSql.Append("select ID,MainId,ScheduleNo,Term,EditionNo,CityNo,ShipDetailNo,ShipTo,ShipToName,GoodNo,GoodName");
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
                model.Id = (int)ojb;
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
            model.ShipDetailNo = dataReader["ShipDetailNo"].ToString();

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
            parameters.Add("ShipDetailNo", DbType.String).Value = entity.ShipDetailNo;

            return parameters;
        }
        
       

        /// <summary>
        /// 根据生产线信息获取对应的发运信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlan> GetAllDomainByLineNos(QueryCondition query)
        {
            var lineNos = query.GetCondition("LineNos").Value;
            var stockMainId = query.GetCondition("StockMainId").Value;
            var mainId = query.GetCondition("MainId").Value;

            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();           

            strSql.Append(" select allInfo.*,Base_Matching.RightGoodNo,StandardDays from ( ");
            //去除子产品
//            //子产品
//            strSql.AppendFormat(@"select ShipPkgQty,NEWTB.* from (select ProLineNo,Qty, TB.ID,TB.MainId,ScheduleNo,Term,EditionNo,CityNo,ShipTo,ShipToName,
//SonGoodNo as GoodNo,SonGoodName as GoodName,ParentGoodNo,ParentGoodName,BiLi,StandardDays
// from(
//select Pro_ShipPlan.ID,Pro_ShipPlan.MainId,ScheduleNo,Term,EditionNo,CityNo,Pro_ShipPlan.ShipTo,Pro_ShipPlan.ShipToName,
//Pro_ShipPlan.GoodNo,Pro_ShipPlan.GoodName,StandardDays
//FROM Pro_ShipPlan
//left join Base_Goods on Base_Goods.GoodNo=Pro_ShipPlan.GoodNo and Base_Goods.ShipTo=Pro_ShipPlan.ShipTo 
//where  Pro_ShipPlan.MainId={2}
//) AS TB inner join [dbo].[Base_Bom_View] on TB.GoodNo=Base_Bom_View.ParentGoodNo
//LEFT join [dbo].[Base_ProductionLine] on [dbo].[Base_ProductionLine].GoodNo=[Base_Bom_View].SonGoodNo
//left join [dbo].[Base_Stock_View] on [dbo].[Base_Stock_View].GoodNo=[Base_Bom_View].SonGoodNo and Base_Stock_View.MainId={1}
//where ProLineNo IN ({0})) AS NEWTB
//left join Base_Goods on Base_Goods.GoodNo=NEWTB.GoodNo and Base_Goods.ShipTo=NEWTB.ShipTo ", lineNos, stockMainId, mainId);

//            strSql.Append(" union all ");
            //主产品
            strSql.AppendFormat(@"select ShipPkgQty,ProLineNo,Qty, Pro_ShipPlan.ID,Pro_ShipPlan.MainId,ScheduleNo,Term,EditionNo,CityNo,Pro_ShipPlan.ShipTo,Pro_ShipPlan.ShipToName,
Pro_ShipPlan.GoodNo,Pro_ShipPlan.GoodName,'' as ParentGoodNo,'' as ParentGoodName,1 as BiLi,StandardDays
FROM Pro_ShipPlan
left join [dbo].[Base_ProductionLine] on [dbo].[Base_ProductionLine].GoodNo=Pro_ShipPlan.GoodNo
left join [dbo].[Base_Stock_View] on [dbo].[Base_Stock_View].GoodNo=Pro_ShipPlan.GoodNo and Base_Stock_View.MainId={0}
left join Base_Goods on Base_Goods.GoodNo=Pro_ShipPlan.GoodNo and Base_Goods.ShipTo=Pro_ShipPlan.ShipTo", stockMainId);
            strSql.AppendFormat(" where ProLineNo IN ({0})  and Pro_ShipPlan.MainId={1}", lineNos, mainId);

            strSql.Append(") AS allInfo left join Matching_View on allInfo.GoodNo=Matching_View.GoodNo  left join Base_Matching on Base_Matching.LeftGoodNo=allInfo.GoodNo order by Matching_View.GoodNo desc ");
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
                model.Id = (int)ojb;
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

            model.ParentGoodNo = dataReader["ParentGoodNo"].ToString();
            model.ParentGoodName = dataReader["ParentGoodName"].ToString();

            ojb = dataReader["Qty"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Qty = Convert.ToInt32(ojb);
            }
            ojb = dataReader["ShipPkgQty"];
            if (ojb != null && ojb != DBNull.Value && ojb.ToString() != "")
            {
                model.ShipPkgQty = Convert.ToInt32(ojb);
            }
            ojb = dataReader["BiLi"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BiLi = Convert.ToInt32(ojb);
            }
            ojb = dataReader["RightGoodNo"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RightGoodNo = Convert.ToString(ojb);
            }
            ojb = dataReader["StandardDays"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SafeDays = Convert.ToInt32(ojb);
            }
            return model;
        }
        /// <summary>
        /// 修改时 使用 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlan> GetPro_SchedulingByEdit(int Id)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            string sql = string.Format(@"select Pro_SchedulingGoods.Id,ProLineNo,Pro_SchedulingGoods.GoodNo,Pro_SchedulingGoods.GoodName,Pro_SchedulingGoods.ShipTo,Pro_SchedulingGoods.ShipToName,StockNum,PackNum,MorningNum,MiddleNum,EveningNum,ParentGoodNo,ParentGoodName,StandardDays from Pro_SchedulingLine
left join Pro_SchedulingGoods on Pro_SchedulingLine.Id=Pro_SchedulingGoods.SLineId
left join Base_Goods on Base_Goods.GoodNo=Pro_SchedulingGoods.GoodNo and Base_Goods.ShipTo=Pro_SchedulingGoods.ShipTo where [MainId]={0}", Id);
            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlan>(CommandType.Text, sql, MapPro_SchedulingByEdit, parameters);
        }
        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_ShipPlan MapPro_SchedulingByEdit(IDataReader dataReader, int rowNum)
        {
            Pro_ShipPlan model = new Pro_ShipPlan();
            model.Id = (int)dataReader["Id"];
            model.ProLineNo = dataReader["ProLineNo"].ToString();
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            model.Qty = Convert.ToInt32(dataReader["StockNum"]);
            model.MorningNum = Convert.ToInt32(dataReader["MorningNum"]);
            model.MiddleNum = Convert.ToInt32(dataReader["MiddleNum"]);
            model.EveningNum = Convert.ToInt32(dataReader["EveningNum"]);
            model.ParentGoodNo = dataReader["ParentGoodNo"].ToString();
            model.ParentGoodName = dataReader["ParentGoodName"].ToString();
            model.ShipPkgQty = Convert.ToInt32(dataReader["PackNum"]);
            var ojb = dataReader["StandardDays"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SafeDays = Convert.ToInt32(dataReader["StandardDays"]);
            }
            return model;
        }
    }
}
