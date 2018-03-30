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
    public class Pro_SchedulingLineDao : EntityDao<Pro_SchedulingLine>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_SchedulingLine entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_SchedulingLine(");
                strSql.Append("MainId,ProLineNo,MorningShift,MorningNum,MiddleShift,MiddleNum,EveningShift,EveningNum)");
                strSql.Append(" values (");
                strSql.Append("@MainId,@ProLineNo,@MorningShift,@MorningNum,@MiddleShift,@MiddleNum,@EveningShift,@EveningNum)");
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
        public override bool Update(Pro_SchedulingLine entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_SchedulingLine set ");
            strSql.Append("MainId=@MainId,");
            strSql.Append("ProLineNo=@ProLineNo,");
            strSql.Append("MorningShift=@MorningShift,");
            strSql.Append("MorningNum=@MorningNum,");
            strSql.Append("MiddleShift=@MiddleShift,");
            strSql.Append("MiddleNum=@MiddleNum,");
            strSql.Append("EveningShift=@EveningShift,");
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
            strSql.Append("delete from Pro_SchedulingLine ");
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
        public override IList<Pro_SchedulingLine> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,MainId,ProLineNo,MorningShift,MorningNum,MiddleShift,MiddleNum,EveningShift,EveningNum ");
            strSql.Append(" FROM Pro_SchedulingLine ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_SchedulingLine>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_SchedulingLine MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_SchedulingLine model = new Pro_SchedulingLine();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["MainId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MainId = (int)ojb;
            }
            model.ProLineNo = dataReader["ProLineNo"].ToString();
            ojb = dataReader["MorningShift"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MorningShift = (int)ojb;
            }
            ojb = dataReader["MorningNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MorningNum = (int)ojb;
            }
            ojb = dataReader["MiddleShift"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MiddleShift = (int)ojb;
            }
            ojb = dataReader["MiddleNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MiddleNum = (int)ojb;
            }
            ojb = dataReader["EveningShift"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EveningShift = (int)ojb;
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
        private IDbParameters GetBaseParams(Pro_SchedulingLine entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("MainId", DbType.Int32).Value = entity.MainId;
            parameters.Add("ProLineNo", DbType.String).Value = entity.ProLineNo;
            parameters.Add("MorningShift", DbType.Int32).Value = entity.MorningShift;
            parameters.Add("MorningNum", DbType.Int32).Value = entity.MorningNum;
            parameters.Add("MiddleShift", DbType.Int32).Value = entity.MiddleShift;
            parameters.Add("MiddleNum", DbType.Int32).Value = entity.MiddleNum;
            parameters.Add("EveningShift", DbType.Int32).Value = entity.EveningShift;
            parameters.Add("EveningNum", DbType.Int32).Value = entity.EveningNum;
            return parameters;
        }


       
    }
}
