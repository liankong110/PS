using Spring.Data.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.Alipay;
using VisualSmart.Domain.SceneryOrder;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.SceneryOrder
{
    public class SceneryOrderRefundDao : EntityDao<SceneryOrderRefundDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(SceneryOrderRefundDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SceneryOrderRefund(");
                strSql.Append("SceneryTCId,SceneryName,BatchNumber,SerialId,PlayDate,Total,ApprovalStatus,PayType,CreateTime,Creater,UpdateTime,Updater,RowState)");

                strSql.Append(" values (");
                strSql.Append("@SceneryTCId,@SceneryName,@BatchNumber,@SerialId,@PlayDate,@Total,@ApprovalStatus,@PayType,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(SceneryOrderRefundDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SceneryOrderRefund set ");
            strSql.Append("SceneryTCId=@SceneryTCId,");
            strSql.Append("SceneryName=@SceneryName,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("SerialId=@SerialId,");
            strSql.Append("PlayDate=@PlayDate,");
            strSql.Append("Total=@Total,");
            strSql.Append("ApprovalStatus=@ApprovalStatus,");
            strSql.Append("PayType=@PayType,");
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
            strSql.Append("delete from SceneryOrderRefund ");
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
        public override IList<SceneryOrderRefundDomain> GetAllDomain(QueryCondition query)
        {
            query.AddEqual("RowState", "1").AddOrderBy("Id", false);


            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,SceneryTCId,SceneryName,BatchNumber,SerialId,PlayDate,Total,ApprovalStatus,PayType,CreateTime,Creater,UpdateTime,Updater,RowState from SceneryOrderRefund ");
            if (query.GetPager() != null)
            {
                string sumSql = "sum(Total) as SumTotal";
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters,"",sumSql));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<SceneryOrderRefundDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }


        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SceneryOrderRefundDomain> GetAllDomain(QueryCondition query, Hashtable hsWhere)
        {
            query.AddEqual("RowState", "1").AddOrderBy("Id", false);
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();

            StringBuilder otherWhere = new StringBuilder();
            if (hsWhere.ContainsKey("ApprovalStatus"))
            {
                otherWhere.AppendFormat(" and ApprovalStatus in (0,1)");
            }
            strSql.Append("select Id,SceneryTCId,SceneryName,BatchNumber,SerialId,PlayDate,Total,ApprovalStatus,PayType,CreateTime,Creater,UpdateTime,Updater,RowState from SceneryOrderRefund ");
            if (query.GetPager() != null)
            {
                string sumSql = "sum(Total) as SumTotal";
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters,otherWhere.ToString(),sumSql));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(otherWhere);
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<SceneryOrderRefundDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }


        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public SceneryOrderRefundDomain MapRow(IDataReader dataReader, int rowNum)
        {
            SceneryOrderRefundDomain model = new SceneryOrderRefundDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["SceneryTCId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SceneryTCId = (int)ojb;
            }
            model.SceneryName = dataReader["SceneryName"].ToString();
            model.BatchNumber = dataReader["BatchNumber"].ToString();
            model.SerialId = dataReader["SerialId"].ToString();
            ojb = dataReader["PlayDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlayDate = (DateTime)ojb;
            }
            ojb = dataReader["Total"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Total = (decimal)ojb;
            }
            ojb = dataReader["ApprovalStatus"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ApprovalStatus = (int)ojb;
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
            model.PayType =Convert.ToInt32(dataReader["PayType"]);
            
            return model;
        }

       
        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(SceneryOrderRefundDomain entity)
        {

            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("SceneryTCId", SqlDbType.Int).Value = entity.SceneryTCId;
            parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = entity.SceneryName;
            parameters.Add("BatchNumber", SqlDbType.NVarChar, 50).Value = entity.BatchNumber;
            parameters.Add("SerialId", SqlDbType.NVarChar, 50).Value = entity.SerialId;          
            parameters.Add("PlayDate", SqlDbType.DateTime).Value = entity.PlayDate;
            parameters.Add("Total", SqlDbType.Decimal).Value = entity.Total;
            parameters.Add("ApprovalStatus", SqlDbType.Int).Value = entity.ApprovalStatus;
            parameters.Add("PayType", SqlDbType.Int).Value = entity.PayType;
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar, 50).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value =DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = entity.Updater??"";
            parameters.Add("RowState", SqlDbType.TinyInt, 1).Value =1; 
            
            return parameters;
        }

        /// <summary>
        /// 将审批信息取消
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool CancelRefund(string ids, UserDomain CurrentUser)
        {
            try
            {
                var parameters = WriteAdoTemplate.CreateDbParameters();
                parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
                parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = CurrentUser.Name;
                string sql = string.Format(@"if not exists(select id from SceneryOrderRefund where  RowState=1 and ApprovalStatus<>0 and Id in ({0}))
update SceneryOrderRefund set ApprovalStatus=2,UpdateTime=@UpdateTime,Updater=@Updater where RowState=1 and Id in ({0})", ids.Trim(','));
                return WriteAdoTemplate.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 将审批信息-退款
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ComfirmRefund(int id, UserDomain CurrentUser)
        {
            try
            {
                var parameters = WriteAdoTemplate.CreateDbParameters();
                parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
                parameters.Add("Updater", SqlDbType.NVarChar, 50).Value = CurrentUser.Name;
                string sql = string.Format(@"if not exists(select id from SceneryOrderRefund where  RowState=1 and ApprovalStatus<>0 and Id={0})
update SceneryOrderRefund set ApprovalStatus=1,UpdateTime=@UpdateTime,Updater=@Updater where RowState=1 and Id={0}",id);
                return WriteAdoTemplate.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 退款申请 检查
        /// 检查数据是否存在
        /// 检查是否存在原始支付数据
        /// 0 可以插入
        /// 1 已经存在申请信息
        /// 2 没有找到原始支付数据
        /// -1 系统异常
        /// </summary>
        /// <param name="SceneryTCId"></param>
        /// <param name="SceneryName"></param>
        /// <param name="BatchNumber"></param>
        /// <param name="SerialId"></param>
        /// <returns></returns>
        public int RequestRefund(string SceneryTCId, string SceneryName, string BatchNumber, string SerialId,int PayType)
        {
            try
            {
                int result = 0;
                var parameters = WriteAdoTemplate.CreateDbParameters();
               
                parameters.Add("SceneryTCId", SqlDbType.NVarChar, 50).Value = SceneryTCId;
                parameters.Add("SceneryName", SqlDbType.NVarChar, 50).Value = SceneryName;
                parameters.Add("BatchNumber", SqlDbType.NVarChar, 50).Value = BatchNumber;
                parameters.Add("SerialId", SqlDbType.NVarChar, 50).Value = SerialId;
                string sql = "";
                //支付宝
                if (PayType == 0)
                {
                     sql = string.Format(@"--查询订单中是否存在
if exists(select id from SceneryOrderRefund with (nolock) where SceneryTCId=@SceneryTCId AND SceneryName=@SceneryName and BatchNumber=@BatchNumber
and SerialId=@SerialId and RowState=1)
begin
	select 1
end
else
begin 
--检查是否存在原始支付数据
	if exists (select id from AlipayDetail  with (nolock) where SceneryName=@SceneryName and BatchNumber=@BatchNumber and Total_amount>0 and  RowState=1
	and SerialId='')
	begin 
	select 0
	end
else
	begin
	select 2
	end
end");
                    result= (int)WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql, parameters);
                }
                //微信
                else if (PayType == 1)
                {
                    sql = string.Format(@"--查询订单中是否存在
if exists(select id from SceneryOrderRefund with (nolock) where SceneryTCId=@SceneryTCId AND SceneryName=@SceneryName and BatchNumber=@BatchNumber
and SerialId=@SerialId and RowState=1)
begin
	select 1
end
else
begin 
--检查是否存在原始支付数据
	if exists (select id from WeChatDetail  with (nolock) where SceneryName=@SceneryName and BatchNumber=@BatchNumber and Total_fee>0 and  RowState=1
	and SerialId='')
	begin 
	select 0
	end
else
	begin
	select 2
	end
end");
                    result = (int)WriteAdoTemplate.ExecuteScalar(CommandType.Text, sql, parameters);
                }

                return result;
            }
            catch (Exception)
            {

                return -1;
            }
        }
    }
}
