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
    public class Base_MatchingDao : EntityDao<Base_Matching>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Base_Matching entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_Matching(");
                strSql.Append("LeftGoodNo,LeftGoodName,RightGoodNo,RightGoodName,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@LeftGoodNo,@LeftGoodName,@RightGoodNo,@RightGoodName,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Base_Matching entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Base_Matching set ");
            strSql.Append("LeftGoodNo=@LeftGoodNo,");
            strSql.Append("LeftGoodName=@LeftGoodName,");
            strSql.Append("RightGoodNo=@RightGoodNo,");
            strSql.Append("RightGoodName=@RightGoodName,");
            
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater");
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
            strSql.Append("delete from Base_Matching ");
            strSql.Append(" where ID=@ID ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除-所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteAll(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("truncate table [dbo].[Base_Matching] ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Base_Matching> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,LeftGoodNo,LeftGoodName,RightGoodNo,RightGoodName,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Base_Matching ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Base_Matching>(CommandType.Text, strSql.ToString(), MapRow, parameters);
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
            strSql.Append(" FROM Base_Matching ");
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
        public Base_Matching MapRow(IDataReader dataReader, int rowNum)
        {
            Base_Matching model = new Base_Matching();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.LeftGoodNo = dataReader["LeftGoodNo"].ToString();
            model.LeftGoodName = dataReader["LeftGoodName"].ToString();
            model.RightGoodNo = dataReader["RightGoodNo"].ToString();
            model.RightGoodName = dataReader["RightGoodName"].ToString();           

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
        private IDbParameters GetBaseParams(Base_Matching entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("LeftGoodNo", DbType.String).Value = entity.LeftGoodNo ?? "";
            parameters.Add("LeftGoodName", DbType.String).Value = entity.LeftGoodName ?? "";
            parameters.Add("RightGoodNo", DbType.String).Value = entity.RightGoodNo ?? "";
            parameters.Add("RightGoodName", DbType.String).Value = entity.RightGoodName ?? "";            
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
