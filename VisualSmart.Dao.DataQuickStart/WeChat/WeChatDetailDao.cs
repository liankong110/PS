using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Dao.DataQuickStart.Base; 
using VisualSmart.Domain.SetUp;
using VisualSmart.Domain.WeChat;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.WeChat
{
    public class WeChatDetailDao : EntityDao<WeChatDetailDomain>
    {
        /// <summary>
        /// 微信
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(WeChatDetailDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WeChatDetail(");
                strSql.Append("AppId,SceneryName,BatchNumber,SerialId,Result_code,Err_code,Err_code_des,Openid,Trade_type,Bank_type,Total_fee,Settlement_total_fee,Transaction_id,Out_trade_no,Time_end,SceneryRate,WeChatPlayDate,Out_refund_no,Refund_id,CreateTime,Creater,UpdateTime,Updater,RowState)");

                strSql.Append(" values (");
                strSql.Append("@AppId,@SceneryName,@BatchNumber,@SerialId,@Result_code,@Err_code,@Err_code_des,@Openid,@Trade_type,@Bank_type,@Total_fee,@Settlement_total_fee,@Transaction_id,@Out_trade_no,@Time_end,@SceneryRate,@WeChatPlayDate,@Out_refund_no,@Refund_id,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        /// 微信
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Update(WeChatDetailDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChatDetail set ");
            strSql.Append("AppId=@AppId,");
            strSql.Append("SceneryName=@SceneryName,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("SerialId=@SerialId,");
            strSql.Append("Result_code=@Result_code,");
            strSql.Append("Err_code=@Err_code,");
            strSql.Append("Err_code_des=@Err_code_des,");
            strSql.Append("Openid=@Openid,");
            strSql.Append("Trade_type=@Trade_type,");
            strSql.Append("Bank_type=@Bank_type,");
            strSql.Append("Total_fee=@Total_fee,");
            strSql.Append("Settlement_total_fee=@Settlement_total_fee,");
            strSql.Append("Transaction_id=@Transaction_id,");
            strSql.Append("Out_trade_no=@Out_trade_no,");
            strSql.Append("Time_end=@Time_end,");
            strSql.Append("SceneryRate=@SceneryRate,");
            strSql.Append("WeChatPlayDate=@WeChatPlayDate,");
            strSql.Append("Out_refund_no=@Out_refund_no,");
            strSql.Append("Refund_id=@Refund_id,");
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
        /// 微信
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChatDetail ");
            strSql.Append(" where ID=@ID ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 微信
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<WeChatDetailDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,AppId,SceneryName,BatchNumber,SerialId,Result_code,Err_code,Err_code_des,Openid,Trade_type,Bank_type,Total_fee,Settlement_total_fee,Transaction_id,Out_trade_no,Time_end,SceneryRate,WeChatPlayDate,Out_refund_no,Refund_id,CreateTime,Creater,UpdateTime,Updater,RowState from WeChatDetail ");			
            if (query.GetPager() != null)
            {
                string sumSql = "sum(Total_fee) as SumTotal_fee";
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters, "", sumSql));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<WeChatDetailDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public WeChatDetailDomain MapRow(IDataReader dataReader, int rowNum)
        {
            WeChatDetailDomain model = new WeChatDetailDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.AppId = dataReader["AppId"].ToString();
            model.SceneryName = dataReader["SceneryName"].ToString();
            model.BatchNumber = dataReader["BatchNumber"].ToString();
            model.SerialId = dataReader["SerialId"].ToString();
            model.Result_code = dataReader["Result_code"].ToString();
            model.Err_code = dataReader["Err_code"].ToString();
            model.Err_code_des = dataReader["Err_code_des"].ToString();
            model.Openid = dataReader["Openid"].ToString();
            model.Trade_type = dataReader["Trade_type"].ToString();
            model.Bank_type = dataReader["Bank_type"].ToString();
            ojb = dataReader["Total_fee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Total_fee = (int)ojb;
            }
            ojb = dataReader["Settlement_total_fee"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Settlement_total_fee = (int)ojb;
            }
            model.Transaction_id = dataReader["Transaction_id"].ToString();
            model.Out_trade_no = dataReader["Out_trade_no"].ToString();
            ojb = dataReader["Time_end"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Time_end = (DateTime)ojb;
            }
            model.SceneryRate =Convert.ToDecimal(dataReader["SceneryRate"]);
            model.WeChatPlayDate = Convert.ToDateTime(dataReader["WeChatPlayDate"]);
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
            model.Out_refund_no = dataReader["Out_refund_no"].ToString();
            model.Refund_id = dataReader["Refund_id"].ToString();
            return model;
        }

        /// <summary>
        /// 微信
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(WeChatDetailDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("AppId", SqlDbType.NVarChar, 50).Value = entity.AppId ?? "";
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = entity.SceneryName ?? "";
            parameters.Add("BatchNumber", SqlDbType.NVarChar, 50).Value = entity.BatchNumber ?? "";
            parameters.Add("SerialId", SqlDbType.NVarChar, 50).Value = entity.SerialId ?? "";
            parameters.Add("Result_code", SqlDbType.NVarChar).Value = entity.Result_code ?? "";
            parameters.Add("Err_code", SqlDbType.NVarChar).Value = entity.Err_code ?? "";
            parameters.Add("Err_code_des", SqlDbType.NVarChar).Value = entity.Err_code_des ?? "";
            parameters.Add("Openid", SqlDbType.NVarChar).Value = entity.Openid ?? "";
            parameters.Add("Trade_type", SqlDbType.NVarChar).Value = entity.Trade_type ?? "";
            parameters.Add("Bank_type", SqlDbType.NVarChar).Value = entity.Bank_type ?? "";
            parameters.Add("Total_fee", SqlDbType.Int).Value =Convert.ToInt32(entity.Total_fee) ;
            parameters.Add("Settlement_total_fee", SqlDbType.Int).Value = entity.Settlement_total_fee;
            parameters.Add("Transaction_id", SqlDbType.NVarChar).Value = entity.Transaction_id ?? "";
            parameters.Add("Out_trade_no", SqlDbType.NVarChar).Value = entity.Out_trade_no ?? "";
            parameters.Add("Time_end", SqlDbType.DateTime).Value = entity.Time_end ;
            parameters.Add("SceneryRate", SqlDbType.Decimal).Value = entity.SceneryRate;
            parameters.Add("WeChatPlayDate", SqlDbType.DateTime).Value = entity.WeChatPlayDate;
            parameters.Add("Out_refund_no", SqlDbType.NVarChar).Value = entity.Out_refund_no ?? "";
            parameters.Add("Refund_id", SqlDbType.NVarChar).Value = entity.Refund_id ?? "";
           
            parameters.Add("CreateTime", SqlDbType.DateTime).Value =  DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater??"";
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value =1;
            return parameters;
        }

        /// <summary>
        /// 微信
        /// 检查是否是否存在
        /// </summary>
        /// <param name="Out_trade_no">商户号</param>
        /// <param name="Total_amount">订单金额 (单位：分)</param>
        /// <returns></returns>
        public bool IsExistsOut_trade_no(string Out_trade_no, int Total_fee, string APP_ID)
        {
            string sql = string.Format("select count(*) from WeChatDetail with (nolock) where Total_fee={0} and Out_trade_no='{1}'", Total_fee, Out_trade_no);
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql)) == 1 ? true : false;
        }

        /// <summary>
        /// 微信 在交易表中查询是否已经退款过
        /// </summary>
        /// <param name="BatchNumber"></param>
        /// <param name="SerialId"></param>
        /// <param name="Total_amount"></param>
        /// <returns></returns>
        public bool IsExistsSceneryOrderRefund(string BatchNumber, string SerialId, int Total_fee)
        {
            string sql = string.Format("select count(*) from WeChatDetail with (nolock) where Total_fee={0} and BatchNumber='{1}' and SerialId='{2}'",
                Total_fee, BatchNumber, SerialId);
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql)) >0 ? true : false;
        }

        /// <summary>
        /// 微信
        /// 微信帐号信息是否存在支付记录
        /// </summary>
        /// <param name="WeChatId">支付宝帐号ID</param>
        /// <returns></returns>
        public bool IsExistsWeChatDetail(int WeChatId)
        {
            string sql = string.Format("select count(*) from WeChatDetail with (nolock)  where AppId=(select APPID from WxPayConfig with (nolock) where id={0}) ", WeChatId);
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql)) > 0 ? true : false;
        }


        /// <summary>
        /// 微信
        /// 景区信息是否存在支付记录
        /// </summary>
        /// <param name="SceneryName">景区名称</param>
        /// <returns></returns>
        public bool IsExistsSceneryWeChat(string SceneryName)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = SceneryName;
            string sql = string.Format("select count(*) from WeChatDetail with (nolock)  where SceneryName=@SceneryName ");
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql,parameters)) > 0 ? true : false;
        }
    }
}
