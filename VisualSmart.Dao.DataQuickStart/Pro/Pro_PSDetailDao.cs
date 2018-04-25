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
    public class Pro_PSDetailDao : EntityDao<Pro_PSDetail>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_PSDetail entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_PSDetail(");
                strSql.Append("MainId,GoodNo,GoodName,ShipTo,ShipToName,PackNum,Qty,ChanNeng,ProOrderIndex,SType,StartTime,EndTime)");
                strSql.Append(" values (");
                strSql.Append("@MainId,@GoodNo,@GoodName,@ShipTo,@ShipToName,@PackNum,@Qty,@ChanNeng,@ProOrderIndex,@SType,@StartTime,@EndTime)");
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
        public override bool Update(Pro_PSDetail entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_PSDetail set ");
            strSql.Append("MainId=@MainId,");            
            strSql.Append("GoodNo=@GoodNo,");
            strSql.Append("GoodName=@GoodName,");
            strSql.Append("ShipTo=@ShipTo,");
            strSql.Append("ShipToName=@ShipToName,");
            strSql.Append("PackNum=@PackNum,");
            strSql.Append("Qty=@Qty,");
            strSql.Append("ChanNeng=@ChanNeng,");            
            strSql.Append("ProOrderIndex=@ProOrderIndex,");
            strSql.Append("SType=@SType,");
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("EndTime=@EndTime");
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
            strSql.Append("delete from Pro_PSDetail ");
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
        public override IList<Pro_PSDetail> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,MainId,GoodNo,GoodName,ShipTo,ShipToName,PackNum,Qty,ChanNeng,ProOrderIndex,SType,StartTime,EndTime ");
            strSql.Append(" FROM Pro_PSDetail ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_PSDetail>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_PSDetail MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_PSDetail model = new Pro_PSDetail();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.MainId = Convert.ToInt32(dataReader["MainId"]);
            model.ChanNeng = Convert.ToInt32(dataReader["ChanNeng"]);            
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            ojb = dataReader["PackNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PackNum = (int)ojb;
            }
            ojb = dataReader["Qty"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Qty = (int)ojb;
            }
            ojb = dataReader["ProOrderIndex"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProOrderIndex = (int)ojb;
            }
            ojb = dataReader["SType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SType = (int)ojb;
            }
            ojb = dataReader["StartTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StartTime = (DateTime)ojb;
            }
            ojb = dataReader["EndTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EndTime = (DateTime)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Pro_PSDetail entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("MainId", DbType.Int32).Value = entity.MainId;
            parameters.Add("ChanNeng", DbType.Int32).Value = entity.ChanNeng;            
            parameters.Add("GoodNo", DbType.String).Value = entity.GoodNo;
            parameters.Add("GoodName", DbType.String).Value = entity.GoodName;
            parameters.Add("ShipTo", DbType.String).Value = entity.ShipTo;
            parameters.Add("ShipToName", DbType.String).Value = entity.ShipToName;
            parameters.Add("PackNum", DbType.Int32).Value = entity.PackNum;
            parameters.Add("Qty", DbType.Int32).Value = entity.Qty;
            parameters.Add("ProOrderIndex", DbType.Int32).Value = entity.ProOrderIndex;
            parameters.Add("SType", DbType.Int32).Value = entity.SType;
            parameters.Add("StartTime", DbType.DateTime).Value = entity.StartTime;
            parameters.Add("EndTime", DbType.DateTime).Value = entity.EndTime;             
            return parameters;
        }

    }
}
