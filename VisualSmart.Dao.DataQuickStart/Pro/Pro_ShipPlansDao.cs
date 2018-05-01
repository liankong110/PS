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
    public class Pro_ShipPlansDao : EntityDao<Pro_ShipPlans>
    {/// <summary>
     /// 新增
     /// </summary>
     /// <param name="entity"></param>
     /// <returns></returns>
        public override bool Add(Pro_ShipPlans entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_ShipPlans(");
                strSql.Append("PlanId,PlanDate,PlanNum)");
                strSql.Append(" values (");
                strSql.Append("@PlanId,@PlanDate,@PlanNum)");
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
        public override bool Update(Pro_ShipPlans entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_ShipPlans set ");
            strSql.Append("PlanId=@PlanId,");
            strSql.Append("PlanDate=@PlanDate,");
            strSql.Append("PlanNum=@PlanNum");
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
            strSql.Append("delete from Pro_ShipPlans ");
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
        public override IList<Pro_ShipPlans> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,PlanId,PlanDate,PlanNum ");
            strSql.Append(" FROM Pro_ShipPlans ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlans>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }
        /// <summary>
        /// 获取主表中所有的子表明细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlans> GetAllDomainByMainId(int Id, QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select Pro_ShipPlans.ID,PlanId,PlanDate,PlanNum
FROM Pro_ShipPlans LEFT JOIN Pro_ShipPlan ON Pro_ShipPlans.PlanId = Pro_ShipPlan.ID WHERE Pro_ShipPlan.MainId = {0}", Id);
            if (query.GetCondition("GoodNo") != null)
            {
                strSql.AppendFormat(" and GoodNo like '%{0}%'", query.GetCondition("GoodNo").Value);
            }
            if (query.GetCondition("GoodName") != null)
            {
                strSql.AppendFormat(" and GoodName like '%{0}%'", query.GetCondition("GoodName").Value);
            }
            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlans>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_ShipPlans MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_ShipPlans model = new Pro_ShipPlans();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["PlanId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanId = (int)ojb;
            }
            ojb = dataReader["PlanDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanDate = (DateTime)ojb;
            }
            ojb = dataReader["PlanNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanNum = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Pro_ShipPlans entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("PlanId", DbType.Int32).Value = entity.PlanId;
            parameters.Add("PlanDate", DbType.Date).Value = entity.PlanDate;
            parameters.Add("PlanNum", DbType.Int32).Value = entity.PlanNum;           
            return parameters;
        }


        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlans> GetAllDomainByLineNos(QueryCondition query)
        {
            var mainId = query.GetCondition("MainId").Value;
            var lineNos = query.GetCondition("LineNos").Value;
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,PlanId,PlanDate,PlanNum ");
            strSql.Append(" FROM Pro_ShipPlans ");
            strSql.AppendFormat(@" WHERE [PlanId] IN (SELECT ID FROM [dbo].[Pro_ShipPlan] WHERE  MainId={1} and GoodNo IN (select GoodNo from [dbo].[Base_ProductionLine] where ProLineNo IN ({0})))", lineNos, mainId);
            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlans>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_ShipPlans> GetPro_SchedulingGoodsNumByEdit(int Id)
        {            
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select Pro_SchedulingGoodsNum.SGoodId,SType,SDate,SNum FROM Pro_SchedulingLine
left join Pro_SchedulingGoods  on Pro_SchedulingLine.Id=Pro_SchedulingGoods.SLineId
left join Pro_SchedulingGoodsNum on Pro_SchedulingGoodsNum.SGoodId=Pro_SchedulingGoods.Id");
            strSql.AppendFormat(" where [MainId]={0} and SType>-1", Id);
            
            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_ShipPlans>(CommandType.Text, strSql.ToString(), MapRow_Pro_SchedulingGoodsNum, parameters);
        }
        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_ShipPlans MapRow_Pro_SchedulingGoodsNum(IDataReader dataReader, int rowNum)
        {
            var model = new Pro_ShipPlans();
            model.PlanId = (int)dataReader["SGoodId"];
            model.SType=(int)dataReader["SType"];
            model.PlanDate = (DateTime)dataReader["SDate"];
            model.PlanNum = (int)dataReader["SNum"];           
            return model;
        }
    }
}
