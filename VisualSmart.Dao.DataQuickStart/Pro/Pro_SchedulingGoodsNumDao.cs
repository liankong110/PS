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
    public class Pro_SchedulingGoodsNumDao : EntityDao<Pro_SchedulingGoodsNum>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_SchedulingGoodsNum entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_SchedulingGoodsNum(");
                strSql.Append("SGoodId,SType,SDate,SNum)");
                strSql.Append(" values (");
                strSql.Append("@SGoodId,@SType,@SDate,@SNum)");
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
        public override bool Update(Pro_SchedulingGoodsNum entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_SchedulingGoodsNum set ");
            strSql.Append("SGoodId=@SGoodId,");
            strSql.Append("SType=@SType,");
            strSql.Append("SDate=@SDate,");
            strSql.Append("SNum=@SNum");
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
            strSql.Append("delete from Pro_SchedulingGoodsNum ");
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
        public override IList<Pro_SchedulingGoodsNum> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,SGoodId,SType,SDate,SNum ");
            strSql.Append(" FROM Pro_SchedulingGoodsNum ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_SchedulingGoodsNum>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_SchedulingGoodsNum MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_SchedulingGoodsNum model = new Pro_SchedulingGoodsNum();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["SGoodId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SGoodId = (int)ojb;
            }
            ojb = dataReader["SType"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SType = (int)ojb;
            }
            ojb = dataReader["SDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SDate = (DateTime)ojb;
            }
            ojb = dataReader["SNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SNum = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Pro_SchedulingGoodsNum entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("SGoodId", DbType.Int32).Value = entity.SGoodId;
            parameters.Add("SType", DbType.Int32).Value = entity.SType;
            parameters.Add("SDate", DbType.Date).Value = entity.SDate;
            parameters.Add("SNum", DbType.Int32).Value = entity.SNum;            
            return parameters;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_SchedulingGoodsNum> GetDetailList(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select SType,SDate,SNum,SGoodId,Pro_SchedulingGoodsNum.Id from Pro_SchedulingLine 
LEFT JOIN Pro_SchedulingGoods ON Pro_SchedulingGoods.SLineId=Pro_SchedulingLine.Id
LEFT JOIN Pro_SchedulingGoodsNum ON Pro_SchedulingGoodsNum.SGoodId=Pro_SchedulingGoods.Id ");

            strSql.AppendFormat(" WHERE SType IN (2,3,4) and Pro_SchedulingLine.MainId={0}", query.GetCondition("MainId").Value);
            strSql.Append(" and GoodNo is not null");

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_SchedulingGoodsNum>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

    }
}
