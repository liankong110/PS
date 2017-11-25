using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.Alipay
{
    public class AlipayDao : EntityDao<AlipayDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(AlipayDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Alipay(");
                strSql.Append("AppName,APP_ID,ALIPAY_PUBLIC_KEY,APP_PRIVATE_KEY,APP_PUBLIC_KEY,PID,Remark,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@AppName,@APP_ID,@ALIPAY_PUBLIC_KEY,@APP_PRIVATE_KEY,@APP_PUBLIC_KEY,@PID,@Remark,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(AlipayDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Alipay set ");
            strSql.Append("AppName=@AppName,");
            strSql.Append("APP_ID=@APP_ID,");
            strSql.Append("ALIPAY_PUBLIC_KEY=@ALIPAY_PUBLIC_KEY,");
            strSql.Append("APP_PRIVATE_KEY=@APP_PRIVATE_KEY,");
            strSql.Append("APP_PUBLIC_KEY=@APP_PUBLIC_KEY,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("Creater=@Creater,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("PID=@PID,");
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
            strSql.Append("Update Alipay set RowState=0 ");
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
        public override IList<AlipayDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PID,Remark,Id,AppName,APP_ID,ALIPAY_PUBLIC_KEY,APP_PRIVATE_KEY,APP_PUBLIC_KEY,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Alipay ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<AlipayDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public AlipayDomain MapRow(IDataReader dataReader, int rowNum)
        {
            AlipayDomain model = new AlipayDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.AppName = dataReader["AppName"].ToString();
            model.APP_ID = dataReader["APP_ID"].ToString();
            model.ALIPAY_PUBLIC_KEY = dataReader["ALIPAY_PUBLIC_KEY"].ToString();
            model.APP_PRIVATE_KEY = dataReader["APP_PRIVATE_KEY"].ToString();
            model.APP_PUBLIC_KEY = dataReader["APP_PUBLIC_KEY"].ToString();
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
            model.Remark = dataReader["Remark"].ToString();
            model.PID = dataReader["PID"].ToString();
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(AlipayDomain entity)
        {

            var parameters = WriteAdoTemplate.CreateDbParameters();

            parameters.Add("AppName", SqlDbType.NVarChar, 100).Value = entity.AppName;
            parameters.Add("APP_ID", SqlDbType.NVarChar, 100).Value = entity.APP_ID;
            parameters.Add("ALIPAY_PUBLIC_KEY", SqlDbType.NVarChar, 1000).Value = entity.ALIPAY_PUBLIC_KEY;
            parameters.Add("APP_PRIVATE_KEY", SqlDbType.NVarChar, 1000).Value = entity.APP_PRIVATE_KEY;
            parameters.Add("APP_PUBLIC_KEY", SqlDbType.NVarChar, 1000).Value = entity.APP_PUBLIC_KEY ?? "";
            parameters.Add("Remark", SqlDbType.NVarChar, 500).Value = entity.Remark??"";
            parameters.Add("PID", SqlDbType.NVarChar, 100).Value = entity.PID ?? "";
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = entity.CreateTime;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value =DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value = 1; 
            
            return parameters;
        } 
    }
}
