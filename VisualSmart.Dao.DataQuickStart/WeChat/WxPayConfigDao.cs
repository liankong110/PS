using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.WeChat
{
    public class WxPayConfigDao : EntityDao<WxPayConfigDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(WxPayConfigDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WxPayConfig(");
                strSql.Append("WeAppName,APPID,MCHID,PAYKEY,APPSECRET,SSLCERT_PATH,SSLCERT_PASSWORD,Remark,CreateTime,Creater,UpdateTime,Updater,RowState)");

                strSql.Append(" values (");
                strSql.Append("@WeAppName,@APPID,@MCHID,@PAYKEY,@APPSECRET,@SSLCERT_PATH,@SSLCERT_PASSWORD,@Remark,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(WxPayConfigDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WxPayConfig set ");
            strSql.Append("WeAppName=@WeAppName,");
            strSql.Append("APPID=@APPID,");
            strSql.Append("MCHID=@MCHID,");
            strSql.Append("PAYKEY=@PAYKEY,");
            strSql.Append("APPSECRET=@APPSECRET,");
            strSql.Append("SSLCERT_PATH=@SSLCERT_PATH,");
            strSql.Append("SSLCERT_PASSWORD=@SSLCERT_PASSWORD,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("Creater=@Creater,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("RowState=@RowState");
            strSql.Append(" where Id=@Id ");
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
            strSql.Append("Update WxPayConfig set RowState=0 ");
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
        public override IList<WxPayConfigDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,WeAppName,APPID,MCHID,PAYKEY,APPSECRET,SSLCERT_PATH,SSLCERT_PASSWORD,Remark,CreateTime,Creater,UpdateTime,Updater,RowState from WxPayConfig ");
          
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<WxPayConfigDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public WxPayConfigDomain MapRow(IDataReader dataReader, int rowNum)
        {
            WxPayConfigDomain model = new WxPayConfigDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.WeAppName = dataReader["WeAppName"].ToString();
            model.APPID = dataReader["APPID"].ToString();
            model.MCHID = dataReader["MCHID"].ToString();
            model.PAYKEY = dataReader["PAYKEY"].ToString();
            model.APPSECRET = dataReader["APPSECRET"].ToString();
            model.SSLCERT_PATH = dataReader["SSLCERT_PATH"].ToString();
            model.SSLCERT_PASSWORD = dataReader["SSLCERT_PASSWORD"].ToString();
            model.Remark = dataReader["Remark"].ToString();
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
        private IDbParameters GetBaseParams(WxPayConfigDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("WeAppName", SqlDbType.NVarChar, 100).Value = entity.WeAppName;
            parameters.Add("APPID", SqlDbType.NVarChar, 100).Value = entity.APPID;
            parameters.Add("MCHID", SqlDbType.NVarChar, 100).Value = entity.MCHID;
            parameters.Add("PAYKEY", SqlDbType.NVarChar, 1000).Value = entity.PAYKEY;
            parameters.Add("APPSECRET", SqlDbType.NVarChar, 1000).Value = entity.APPSECRET;
            parameters.Add("SSLCERT_PATH", SqlDbType.NVarChar, 1000).Value = entity.SSLCERT_PATH??"";
            parameters.Add("SSLCERT_PASSWORD", SqlDbType.NVarChar, 500).Value = entity.SSLCERT_PASSWORD ?? "";
            parameters.Add("Remark", SqlDbType.NVarChar, 100).Value = entity.Remark ?? "";
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = entity.CreateTime;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value = 1;
            return parameters;
        }
    }
}
