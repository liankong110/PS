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
    public class Base_ProductionLinesDao : EntityDao<Base_ProductionLines>
    {  /// <summary>
       /// 新增
       /// </summary>
       /// <param name="entity"></param>
       /// <returns></returns>
        public override bool Add(Base_ProductionLines entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_ProductionLines(");
                strSql.Append("ProLineId,People,Number)");
                strSql.Append(" values (");
                strSql.Append("@ProLineId,@People,@Number)");
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
        public override bool Update(Base_ProductionLines entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Base_ProductionLines set ");
            strSql.Append("ProLineId=@ProLineId,");
            strSql.Append("People=@People,");
            strSql.Append("Number=@Number");
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
            strSql.Append("delete from Base_ProductionLines ");
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
        public override IList<Base_ProductionLines> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ProLineId,People,Number ");
            strSql.Append(" FROM Base_ProductionLines ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Base_ProductionLines>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Base_ProductionLines MapRow(IDataReader dataReader, int rowNum)
        {
            Base_ProductionLines model = new Base_ProductionLines();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["ProLineId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProLineId = (int)ojb;
            }
            ojb = dataReader["People"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.People = (int)ojb;
            }
            ojb = dataReader["Number"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Number = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Base_ProductionLines entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("ProLineId", DbType.Int32).Value = entity.ProLineId;
            parameters.Add("People", DbType.Int32).Value = entity.People;
            parameters.Add("Number", DbType.Int32).Value = entity.Number;
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
