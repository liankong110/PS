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
    public class AlipayQRCodeDao : EntityDao<AlipayQRCodeDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(AlipayQRCodeDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into AlipayQRCode(");
                strSql.Append("Phone,AppId,Out_trade_no,SceneryName,BatchNumber,RequestJson,QrCode,Remark,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@Phone,@AppId,@Out_trade_no,@SceneryName,@BatchNumber,@RequestJson,@QrCode,@Remark,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(AlipayQRCodeDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AlipayQRCode set ");
            strSql.Append("Phone=@Phone,");        
            strSql.Append("AppId=@AppId,");            
            strSql.Append("Out_trade_no=@Out_trade_no,");
            strSql.Append("SceneryName=@SceneryName,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("RequestJson=@RequestJson,");
            strSql.Append("QrCode=@QrCode,");
            strSql.Append("Remark=@Remark,");
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
            strSql.Append("delete from AlipayQRCode ");
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
        public override IList<AlipayQRCodeDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Select Phone,AppId,Id,Out_trade_no,SceneryName,BatchNumber,RequestJson,QrCode,Remark,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM AlipayQRCode ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<AlipayQRCodeDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public AlipayQRCodeDomain MapRow(IDataReader dataReader, int rowNum)
        {
            AlipayQRCodeDomain model = new AlipayQRCodeDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.Out_trade_no = dataReader["Out_trade_no"].ToString();
            model.SceneryName = dataReader["SceneryName"].ToString();
            model.BatchNumber = dataReader["BatchNumber"].ToString();
            model.RequestJson = dataReader["RequestJson"].ToString();
            model.QrCode = dataReader["QrCode"].ToString();
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
            model.AppId = dataReader["AppId"].ToString();
            model.Phone = dataReader["Phone"].ToString();
            
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(AlipayQRCodeDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("Out_trade_no", SqlDbType.NVarChar, 50).Value = entity.Out_trade_no??"";
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = entity.SceneryName ?? "";
            parameters.Add("BatchNumber", SqlDbType.NVarChar, 50).Value = entity.BatchNumber ?? "";
            parameters.Add("RequestJson", SqlDbType.Text).Value = entity.RequestJson ?? "";
            parameters.Add("QrCode", SqlDbType.NVarChar, 200).Value = entity.QrCode ?? "";
            parameters.Add("Remark", SqlDbType.NVarChar, 500).Value = entity.Remark ?? "";
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value = 1;
            parameters.Add("AppId", SqlDbType.NVarChar, 50).Value = entity.AppId ?? "";
            parameters.Add("Phone", SqlDbType.NVarChar, 50).Value = entity.Phone ?? "";
            
            return parameters;
        }
        /// <summary>
        ///  检查二维码表中是否存在商户号，如果存在 检查 支付信息中是否存在该商户信息，如果没有则返回景区名称
        ///  -1 没有生成二维码创建记录 异常订单
        ///  -2 已经存在支付信息
        ///  其他 景区名称
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>

        public AlipayQRCodeDomain IsExistOut_trade_no(string out_trade_no, decimal Total_amount, string APP_ID)
        {
            //检查二维码表中是否存在商户号，如果存在 检查 支付信息中是否存在该商户信息，如果没有则返回景区名称
            string sql = string.Format(@"declare @sceneryName varchar(50)
declare @Out_trade_no varchar(50)
declare @Total_amount decimal(18, 2)
declare @APP_ID varchar(50)
declare @BatchNumber varchar(50)
declare @Rate decimal(18, 6)
set @sceneryName=''
set @BatchNumber=''
set @Out_trade_no='{0}'
set @Total_amount={1}
set @APP_ID='{2}'
select @sceneryName=SceneryName,@BatchNumber=BatchNumber from AlipayQRCode with (nolock) where Out_trade_no=@Out_trade_no and AppId=@APP_ID

if(isnull(@sceneryName,'') !='' )
begin
	if exists(select Out_trade_no from AlipayDetail with (nolock) where  Out_trade_no=@Out_trade_no and AppId=@APP_ID and Total_amount=@Total_amount)
	begin
	--已经存在支付信息
	select -2
	end
	else
	begin
    --赋值费率
    select @Rate=Rate from Scenery with (nolock) where SceneryName=@SceneryName and RowState=1
	--景区名称
	select @sceneryName,@BatchNumber,@Rate
	end
end
else
begin
--没有生成二维码创建记录
select -1
end
", out_trade_no, Total_amount, APP_ID);
            //LogHelper.AlipayLog(sql);
            var result= ReadAdoTemplate.QueryWithRowMapperDelegate<AlipayQRCodeDomain>(CommandType.Text, sql, MapRow_GetModel);
            if (result.Count >=1)
            {
                return result[0];
            }
            return null;
        }

        public AlipayQRCodeDomain MapRow_GetModel(IDataReader dataReader, int rowNum)
        {
            AlipayQRCodeDomain model = new AlipayQRCodeDomain();          
            model.SceneryName = dataReader.GetValue(0).ToString();
            if (model.SceneryName != "-1" && model.SceneryName != "-2")
            {
                model.BatchNumber = dataReader.GetValue(1).ToString();
                model.SceneryRate =Convert.ToDecimal(dataReader.GetValue(2));
            }
           

            return model;
        }

        /// <summary>
        /// 检查是否可以提出申请功能
        /// -1--已经存在申请记录
        /// -2 --没有原始支付记录
        /// -3--已经存在退款记录	 
        /// 0--验证通过
        /// -10;//系统异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CheckApplayRefund(int id)
        {
            try
            {
                string sql = string.Format(@"declare @Out_trade_no nvarchar(50)
declare @SceneryName  nvarchar(50)
declare @BatchNumber  nvarchar(50)
declare @AppId nvarchar(50)
select @AppId=AppId,@BatchNumber=BatchNumber, @SceneryName=SceneryName,@Out_trade_no=Out_trade_no from AlipayQRCode with (Nolock) where Id={0}
--查询是否有退款申请记录
--申请中和已经审核 2种状态
if exists (select * from SceneryOrderRefund  with (Nolock) where BatchNumber=@BatchNumber and SceneryName=@SceneryName and RowState=1
and ApprovalStatus in (0,1) )
begin
select -1--已经存在申请记录
end
else
begin 
--查询是否有原始的支付记录
 if exists (select * from AlipayDetail with (Nolock) where BatchNumber=@BatchNumber and SceneryName=@SceneryName and RowState=1
 and AppId=@AppId and Out_trade_no=@Out_trade_no and SerialId='' and Total_amount>0)
 begin 
 --查询是否有已经退款的记录
	if exists (select * from AlipayDetail with (Nolock) where BatchNumber=@BatchNumber and SceneryName=@SceneryName and RowState=1
 and AppId=@AppId and Out_trade_no=@Out_trade_no  and Total_amount<0)
    begin
	select -3--已经存在退款记录	 
	end
	else
	begin
	select 0--验证通过
	end
 end
 else
 begin
 select -2 --没有原始支付记录
 end
end", id);

               return (int)ReadAdoTemplate.ExecuteScalar(CommandType.Text, sql);
            }
            catch (Exception)
            {

                return -10;//系统异常
            }

           
            
        }
    }
}
