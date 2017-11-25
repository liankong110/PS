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
    public class SceneryDao : EntityDao<SceneryDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(SceneryDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Scenery(");
                strSql.Append("SceneryName,SceneryTCId,AlipayId,WeChatId,AccountId,AccountPwd,Rate,WeChatRate,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@SceneryName,@SceneryTCId,@AlipayId,@WeChatId,@AccountId,@AccountPwd,@Rate,@WeChatRate,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(SceneryDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Scenery set ");
            strSql.Append("SceneryName=@SceneryName,");
            strSql.Append("SceneryTCId=@SceneryTCId,");
            strSql.Append("AlipayId=@AlipayId,");
            strSql.Append("WeChatId=@WeChatId,");
            strSql.Append("AccountId=@AccountId,");
            strSql.Append("AccountPwd=@AccountPwd,");           
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("RowState=@RowState,");
            strSql.Append("Rate=@Rate,");
            strSql.Append("WeChatRate=@WeChatRate");

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
            strSql.Append("update Scenery set RowState=0 ");
            strSql.Append(" where ID=@ID ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }


        /// <summary>
        /// 获取单个景区信息
        /// </summary>
        /// <param name="AccountId">账户ID</param>
        /// <param name="SceneryTCId">同程景区ID</param>
        /// <returns></returns>
        public SceneryDomain GetDomain(string AccountId, string SceneryTCId)
        {
            QueryCondition query = QueryCondition.Instance.AddEqual("Scenery","RowState","1")
                .AddEqual("Scenery","AccountId", AccountId)
                .AddEqual("Scenery","SceneryTCId", SceneryTCId);

            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rate,AccountId,AccountPwd,AppName,Scenery.Id,SceneryName,SceneryTCId,AlipayId,APP_ID,ALIPAY_PUBLIC_KEY,APP_PRIVATE_KEY,APP_PUBLIC_KEY,PID");
            strSql.Append(",WeChatId,WeChatRate,[APPID],[MCHID],[PAYKEY],[APPSECRET],[SSLCERT_PATH],[SSLCERT_PASSWORD] ");
            strSql.Append(" FROM Scenery left join Alipay on Alipay.id=Scenery.AlipayId ");
            strSql.Append(" LEFT JOIN WxPayConfig ON WxPayConfig.Id=Scenery.WeChatId");
            strSql.Append(query.GetSQL_Where(parameters));
            strSql.Append(query.GetSQL_Order());

            var list = ReadAdoTemplate.QueryWithRowMapperDelegate<SceneryDomain>(CommandType.Text, strSql.ToString(), MapRow_Model, parameters);
            if (list.Count ==1)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 获取所有景区信息
        /// </summary>      
        /// <returns></returns>
        public List<SceneryDomain> GetStaticDomain()
        {
            QueryCondition query = QueryCondition.Instance.AddEqual("Scenery", "RowState", "1");

            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rate,AccountId,AccountPwd,AppName,Scenery.Id,SceneryName,SceneryTCId,AlipayId,APP_ID,ALIPAY_PUBLIC_KEY,APP_PRIVATE_KEY,APP_PUBLIC_KEY");         
            strSql.Append(",WeChatId,WeChatRate,[APPID],[MCHID],[PAYKEY],[APPSECRET],[SSLCERT_PATH],[SSLCERT_PASSWORD] ");
            strSql.Append(" FROM Scenery left join Alipay on Alipay.id=Scenery.AlipayId ");
            strSql.Append(" LEFT JOIN WxPayConfig ON WxPayConfig.Id=Scenery.WeChatId");
            strSql.Append(query.GetSQL_Where(parameters));
            strSql.Append(query.GetSQL_Order());
            return ReadAdoTemplate.QueryWithRowMapperDelegate<SceneryDomain>(CommandType.Text, strSql.ToString(), MapRow_Model, parameters).ToList();
         }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<SceneryDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rate,AccountId,AccountPwd,AppName,WeAppName,Scenery.Id,SceneryName,SceneryTCId,AlipayId,Scenery.CreateTime,Scenery.Creater,Scenery.UpdateTime,Scenery.Updater,Scenery.RowState,WeChatRate,WeChatId ");
            strSql.Append(" FROM Scenery left join Alipay on Alipay.id=Scenery.AlipayId");
            strSql.Append(" LEFT JOIN WxPayConfig ON WxPayConfig.Id=Scenery.WeChatId");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<SceneryDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public SceneryDomain MapRow(IDataReader dataReader, int rowNum)
        {
            SceneryDomain model = new SceneryDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.SceneryName = dataReader["SceneryName"].ToString();
            ojb = dataReader["AppName"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.AlipayName = ojb.ToString();
            }
            ojb = dataReader["WeAppName"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.WeChatName = ojb.ToString();
            } 
            model.SceneryTCId = Convert.ToInt32(dataReader["SceneryTCId"]);
            model.AlipayId =Convert.ToInt32(dataReader["AlipayId"]);
            model.WeChatId = Convert.ToInt32(dataReader["WeChatId"]);
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
            model.AccountId = dataReader["AccountId"].ToString();
            model.AccountPwd = dataReader["AccountPwd"].ToString();
            ojb = dataReader["RowState"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RowState = (byte)ojb;
            }

            model.Rate = Convert.ToDecimal(dataReader["Rate"]);
            model.WeChatRate = Convert.ToDecimal(dataReader["WeChatRate"]);
            return model;
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public SceneryDomain MapRow_Model(IDataReader dataReader, int rowNum)
        {
            SceneryDomain model = new SceneryDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.SceneryName = dataReader["SceneryName"].ToString();
            model.AlipayName = dataReader["AppName"].ToString();
            model.SceneryTCId = Convert.ToInt32(dataReader["SceneryTCId"]);
            model.AlipayId = Convert.ToInt32(dataReader["AlipayId"]);          
            model.AccountId = dataReader["AccountId"].ToString();
            model.AccountPwd = dataReader["AccountPwd"].ToString();
            model.WeChatId = Convert.ToInt32(dataReader["WeChatId"]);   
            if (model.AlipayId > 0)
            {
                model.Apipay.ALIPAY_PUBLIC_KEY = dataReader["ALIPAY_PUBLIC_KEY"].ToString();
                model.Apipay.APP_ID = dataReader["APP_ID"].ToString();
                model.Apipay.APP_PRIVATE_KEY = dataReader["APP_PRIVATE_KEY"].ToString();
                model.Apipay.APP_PUBLIC_KEY = dataReader["APP_PUBLIC_KEY"].ToString();
                model.Apipay.PID = dataReader["PID"].ToString();
            }
            if (model.WeChatId > 0)
            {
                model.WeChat.APPID = dataReader["APPID"].ToString();
                model.WeChat.MCHID = dataReader["MCHID"].ToString();
                model.WeChat.PAYKEY = dataReader["PAYKEY"].ToString();
                model.WeChat.APPSECRET = dataReader["APPSECRET"].ToString();
                model.WeChat.SSLCERT_PATH = dataReader["SSLCERT_PATH"].ToString();
                model.WeChat.SSLCERT_PATH = dataReader["SSLCERT_PASSWORD"].ToString();
            }
            model.Rate = Convert.ToDecimal(dataReader["Rate"]);
            model.WeChatRate = Convert.ToDecimal(dataReader["WeChatRate"]);
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(SceneryDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("Rate", SqlDbType.Decimal).Value = entity.Rate;
            parameters.Add("WeChatRate", SqlDbType.Decimal).Value = entity.WeChatRate;
            parameters.Add("AccountId", SqlDbType.NVarChar, 50).Value = entity.AccountId;
            parameters.Add("AccountPwd", SqlDbType.NVarChar, 50).Value = entity.AccountPwd;
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = entity.SceneryName;
            parameters.Add("SceneryTCId", SqlDbType.Int).Value = entity.SceneryTCId;
            parameters.Add("AlipayId", SqlDbType.Int).Value = entity.AlipayId;
            parameters.Add("WeChatId", SqlDbType.Int).Value = entity.WeChatId;        
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = entity.CreateTime;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value =DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value =1;             
            return parameters;
        } 
    }
}
