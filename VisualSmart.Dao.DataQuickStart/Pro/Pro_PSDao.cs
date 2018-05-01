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
    public class Pro_PSDao : EntityDao<Pro_PS>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_PS entity)
        {
            try
            {
                entity.ProNo= GetProNo("Pro_PS", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_PS(");
                strSql.Append("ProNo,SchedulingProNo,LineId,ProLineNo,ProDate,FinalMorningNum,FinalMiddleNum,FinalEveningNum,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@SchedulingProNo,@LineId,@ProLineNo,@ProDate,@FinalMorningNum,@FinalMiddleNum,@FinalEveningNum,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public int AddGetId(Pro_PS entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_PS", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_PS(");
                strSql.Append("ProNo,SchedulingProNo,LineId,ProLineNo,ProDate,FinalMorningNum,FinalMiddleNum,FinalEveningNum,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@SchedulingProNo,@LineId,@ProLineNo,@ProDate,@FinalMorningNum,@FinalMiddleNum,@FinalEveningNum,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Pro_PS entity)
        {
            //修改数据时 同时删除子数据明细
            string deleteSQL = string.Format("delete from Pro_PSDetail where MainId={0}",entity.Id);
            ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, deleteSQL);

            //修改数据
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_PS set ");
            strSql.Append("SchedulingProNo=@SchedulingProNo,");
            strSql.Append("ProNo=@ProNo,");
            strSql.Append("ProLineNo=@ProLineNo,");
            strSql.Append("LineId=@LineId,");
            strSql.Append("ProDate=@ProDate,");
            strSql.Append("FinalMorningNum=@FinalMorningNum,");
            strSql.Append("FinalMiddleNum=@FinalMiddleNum,");
            strSql.Append("FinalEveningNum=@FinalEveningNum,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("Creater=@Creater,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("RowState=@RowState");
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
            strSql.Append("delete from Pro_PSDetail  where MainId=@ID;");
            strSql.Append("delete from Pro_PS  where ID=@ID;");
          
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Pro_PS> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,SchedulingProNo,ProNo,LineId,ProLineNo,ProDate,FinalMorningNum,FinalMiddleNum,FinalEveningNum,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Pro_PS ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_PS>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_PS MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_PS model = new Pro_PS();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.ProNo = dataReader["ProNo"].ToString();
            model.ProLineNo = dataReader["ProLineNo"].ToString();
            model.LineId = Convert.ToInt32(dataReader["LineId"]);
            model.SchedulingProNo = dataReader["SchedulingProNo"].ToString();

            ojb = dataReader["ProDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProDate = (DateTime)ojb;
            }
            ojb = dataReader["FinalMorningNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FinalMorningNum = (int)ojb;
            }
            ojb = dataReader["FinalMiddleNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FinalMiddleNum = (int)ojb;
            }
            ojb = dataReader["FinalEveningNum"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FinalEveningNum = (int)ojb;
            }
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
        private IDbParameters GetBaseParams(Pro_PS entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("SchedulingProNo", DbType.String).Value = entity.SchedulingProNo;
            parameters.Add("ProNo", DbType.String).Value = entity.ProNo;
            parameters.Add("LineId", DbType.Int32).Value = entity.LineId;
            parameters.Add("ProLineNo", DbType.String).Value = entity.ProLineNo;
            parameters.Add("ProDate", DbType.Date).Value = entity.ProDate;
            parameters.Add("FinalMiddleNum", DbType.Int32).Value = entity.FinalMiddleNum ?? 0;
            parameters.Add("FinalEveningNum", DbType.Int32).Value = entity.FinalEveningNum ?? 0;
            parameters.Add("FinalMorningNum", DbType.Int32).Value = entity.FinalMorningNum ?? 0;

            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }

    }
}
