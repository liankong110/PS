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
    public class AlipayDetailDao : EntityDao<AlipayDetailDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(AlipayDetailDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into AlipayDetail(");
                strSql.Append("AlipayPlayDate,SceneryRate,BatchNumber,SerialId,AppId,SceneryName,Buyer_logon_id,Out_trade_no,Send_pay_date,Total_amount,Trade_no,Trade_status,Code,Msg,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@AlipayPlayDate,@SceneryRate,@BatchNumber,@SerialId,@AppId,@SceneryName,@Buyer_logon_id,@Out_trade_no,@Send_pay_date,@Total_amount,@Trade_no,@Trade_status,@Code,@Msg,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");			
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
        public override bool Update(AlipayDetailDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AlipayDetail set ");
            strSql.Append("AppId=@AppId,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("AlipayPlayDate=@AlipayPlayDate,");
            strSql.Append("SceneryRate=@SceneryRate,");      
            strSql.Append("SceneryName=@SceneryName,");
            strSql.Append("Buyer_logon_id=@Buyer_logon_id,"); 
            strSql.Append("Out_trade_no=@Out_trade_no,");         
            strSql.Append("Send_pay_date=@Send_pay_date,");
            strSql.Append("Total_amount=@Total_amount,");
            strSql.Append("Trade_no=@Trade_no,");
            strSql.Append("Trade_status=@Trade_status,");
            strSql.Append("Code=@Code,");
            strSql.Append("Msg=@Msg,");            
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
            strSql.Append("delete from AlipayDetail ");
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
        public override IList<AlipayDetailDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AlipayPlayDate,SceneryRate,BatchNumber,SerialId,AppId,Id,SceneryName,Buyer_logon_id,Out_trade_no,Send_pay_date,Total_amount,Trade_no,Trade_status,Code,Msg,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM AlipayDetail ");
            if (query.GetPager() != null)
            {
                string sumSql = "sum(Total_amount) as SumTotal_amount";
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters, "", sumSql));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<AlipayDetailDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public AlipayDetailDomain MapRow(IDataReader dataReader, int rowNum)
        {
            AlipayDetailDomain model = new AlipayDetailDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.Buyer_logon_id = dataReader["Buyer_logon_id"].ToString();
            model.SerialId = dataReader["SerialId"].ToString();
            //ojb = dataReader["Buyer_pay_amount"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.Buyer_pay_amount = (decimal)ojb;
            //}
            //model.Buyer_user_id = dataReader["Buyer_user_id"].ToString();
            //ojb = dataReader["Invoice_amount"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.Invoice_amount = (decimal)ojb;
            //}
            //model.Open_id = dataReader["Open_id"].ToString();
            model.Out_trade_no = dataReader["Out_trade_no"].ToString();
            //ojb = dataReader["Receipt_amount"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.Receipt_amount = (decimal)ojb;
            //}
            ojb = dataReader["Send_pay_date"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Send_pay_date = (DateTime)ojb;
            }
            ojb = dataReader["Total_amount"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Total_amount = (decimal)ojb;
            }
            model.Trade_no = dataReader["Trade_no"].ToString();
            model.Trade_status = dataReader["Trade_status"].ToString();
            model.Code = dataReader["Code"].ToString();
            model.Msg = dataReader["Msg"].ToString();
            model.SceneryName = dataReader["SceneryName"].ToString();
            
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
            model.AppId = dataReader["AppId"].ToString();
            model.BatchNumber = dataReader["BatchNumber"].ToString();
            model.SceneryRate =Convert.ToDecimal(dataReader["SceneryRate"]);
            model.AlipayPlayDate = Convert.ToDateTime(dataReader["AlipayPlayDate"]);
            
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(AlipayDetailDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("BatchNumber", SqlDbType.NVarChar, 50).Value = entity.BatchNumber ?? "";
            parameters.Add("SerialId", SqlDbType.NVarChar, 50).Value = entity.SerialId ?? "";
            parameters.Add("SceneryRate", SqlDbType.Decimal).Value = entity.SceneryRate;
            parameters.Add("AlipayPlayDate", SqlDbType.DateTime).Value = entity.AlipayPlayDate;            
            parameters.Add("AppId", SqlDbType.NVarChar, 50).Value = entity.AppId ?? "";
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = entity.SceneryName ?? "";
            parameters.Add("Buyer_logon_id", SqlDbType.NVarChar, 100).Value = entity.Buyer_logon_id ?? "";
            parameters.Add("Out_trade_no", SqlDbType.NVarChar, 50).Value = entity.Out_trade_no ?? "";
            parameters.Add("Send_pay_date", SqlDbType.DateTime).Value = entity.Send_pay_date;
            parameters.Add("Total_amount", SqlDbType.Decimal, 9).Value = entity.Total_amount;
            parameters.Add("Trade_no", SqlDbType.NVarChar, 100).Value = entity.Trade_no ?? "";
            parameters.Add("Trade_status", SqlDbType.NVarChar, 100).Value = entity.Trade_status ?? "";
            parameters.Add("Code", SqlDbType.NVarChar, 50).Value = entity.Code ?? "";
            parameters.Add("Msg", SqlDbType.NVarChar, 50).Value = entity.Msg ?? "";
            parameters.Add("CreateTime", SqlDbType.DateTime).Value =  DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater??"";
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value =1;
            return parameters;
        }

        /// <summary>
        /// 检查是否是否存在
        /// </summary>
        /// <param name="Out_trade_no">商户号</param>
        /// <param name="Total_amount">订单金额</param>
        /// <returns></returns>
        public bool IsExistsOut_trade_no(string Out_trade_no, decimal Total_amount, string APP_ID)
        {
            string sql = string.Format("select count(*) from AlipayDetail with (nolock) where Total_amount={0} and Out_trade_no='{1}'", Total_amount, Out_trade_no);
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql)) == 1 ? true : false;
        }

        /// <summary>
        /// 在交易表中查询是否已经退款过
        /// </summary>
        /// <param name="BatchNumber"></param>
        /// <param name="SerialId"></param>
        /// <param name="Total_amount"></param>
        /// <returns></returns>
        public bool IsExistsSceneryOrderRefund(string BatchNumber,string SerialId, decimal Total_amount)
        {
            string sql = string.Format("select count(*) from AlipayDetail with (nolock) where Total_amount={0} and BatchNumber='{1}' and SerialId='{2}'",
                Total_amount, BatchNumber, SerialId);
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql)) >0 ? true : false;
        }

        /// <summary>
        /// 支付宝帐号信息是否存在支付记录
        /// </summary>
        /// <param name="alipayId">支付宝帐号ID</param>
        /// <returns></returns>
        public bool IsExistsAlipayDetail(int alipayId)
        {
            string sql = string.Format("select count(*) from AlipayDetail with (nolock)  where AppId=(select APP_ID from Alipay with (nolock) where id={0}) ",alipayId);
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql)) > 0 ? true : false;
        }


        /// <summary>
        /// 景区信息是否存在支付记录
        /// </summary>
        /// <param name="SceneryName">景区名称</param>
        /// <returns></returns>
        public bool IsExistsSceneryAlipay(string SceneryName)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = SceneryName;
            string sql = string.Format("select count(*) from AlipayDetail with (nolock)  where SceneryName=@SceneryName ");
            return Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql,parameters)) > 0 ? true : false;
        }
    }
}
