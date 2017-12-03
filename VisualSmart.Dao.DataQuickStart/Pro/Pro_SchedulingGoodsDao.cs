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
    public class Pro_SchedulingGoodsDao : EntityDao<Pro_SchedulingGoods>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_SchedulingGoods entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_SchedulingGoods(");
                strSql.Append("SLineId,GoodNo,GoodName,ShipTo,ShipToName,StockNum,PackNum,MorningNum,MiddleNum,EveningNum)");
                strSql.Append(" values (");
                strSql.Append("@SLineId,@GoodNo,@GoodName,@ShipTo,@ShipToName,@StockNum,@PackNum,@MorningNum,@MiddleNum,@EveningNum)");
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
        public override bool Update(Pro_SchedulingGoods entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_SchedulingGoods set ");
            strSql.Append("SLineId=@SLineId,");
            strSql.Append("GoodNo=@GoodNo,");
            strSql.Append("GoodName=@GoodName,");
            strSql.Append("ShipTo=@ShipTo,");
            strSql.Append("ShipToName=@ShipToName,");
            strSql.Append("StockNum=@StockNum,");
            strSql.Append("PackNum=@PackNum,");
            strSql.Append("MorningNum=@MorningNum,");
            strSql.Append("MiddleNum=@MiddleNum,");
            strSql.Append("EveningNum=@EveningNum");
            strSql.Append(" where Id=@Id");
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
            strSql.Append("delete from Pro_SchedulingGoods ");
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
        public override IList<Pro_SchedulingGoods> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,SLineId,GoodNo,GoodName,ShipTo,ShipToName,StockNum,PackNum,MorningNum,MiddleNum,EveningNum ");
            strSql.Append(" FROM Pro_SchedulingGoods ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_SchedulingGoods>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_SchedulingGoods MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_SchedulingGoods model = new Pro_SchedulingGoods();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["SLineId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SLineId = (int)ojb;
            }
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            ojb = dataReader["StockNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StockNum = (decimal)ojb;
            }
            ojb = dataReader["PackNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PackNum = (int)ojb;
            }
            ojb = dataReader["MorningNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MorningNum = (int)ojb;
            }
            ojb = dataReader["MiddleNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MiddleNum = (int)ojb;
            }
            ojb = dataReader["EveningNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EveningNum = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Pro_SchedulingGoods entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("SLineId", DbType.String).Value = entity.SLineId;
            parameters.Add("GoodNo", DbType.String).Value = entity.GoodNo;
            parameters.Add("GoodName", DbType.String).Value = entity.GoodName;
            parameters.Add("ShipTo", DbType.String).Value = entity.ShipTo;
            parameters.Add("ShipToName", DbType.String).Value = entity.ShipToName;
            parameters.Add("StockNum", DbType.Decimal).Value = entity.StockNum;
            parameters.Add("PackNum", DbType.Int32).Value = entity.PackNum;
            parameters.Add("MorningNum", DbType.Int32).Value = entity.MorningNum;
            parameters.Add("MiddleNum", DbType.Int32).Value = entity.MiddleNum;
            parameters.Add("EveningNum", DbType.Int32).Value = entity.EveningNum;
            return parameters;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_SchedulingGoods> GetDetailList(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select  GoodNo,GoodName,ProLineNo,ShipTo,ShipToName,PackNum,Pro_SchedulingGoods.Id,SType from Pro_SchedulingLine 
LEFT JOIN Pro_SchedulingGoods ON Pro_SchedulingGoods.SLineId=Pro_SchedulingLine.Id 
LEFT JOIN Pro_SchedulingGoodsNum ON Pro_SchedulingGoodsNum.SGoodId=Pro_SchedulingGoods.Id");

            strSql.AppendFormat(" where SType IN (2,3,4) and Pro_SchedulingLine.MainId={0}", query.GetCondition("MainId").Value);
            strSql.Append(" and GoodNo is not null");
            strSql.Append(" GROUP BY  GoodNo,GoodName,ProLineNo,ShipTo,ShipToName,PackNum,Pro_SchedulingGoods.Id,SType;");

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_SchedulingGoods>(CommandType.Text, strSql.ToString(), MapRowByDetailList, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_SchedulingGoods MapRowByDetailList(IDataReader dataReader, int rowNum)
        {
            Pro_SchedulingGoods model = new Pro_SchedulingGoods();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }           
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();           
            ojb = dataReader["PackNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PackNum = (int)ojb;
            }
            model.ProLineNo = dataReader["ProLineNo"].ToString();
            model.SType = Convert.ToInt32(dataReader["SType"]);
            return model;
        }
    }
}
