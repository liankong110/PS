using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain;
using VisualSmart.Domain.SetUp;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.SetUp
{
    public class FunctionDao : EntityDao<FunctionDomain>
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(FunctionDomain entity)
        {
            try
            {                
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_Function(");
                strSql.Append("Form_Id,FunctionText,FunctionName,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@Form_Id,@FunctionText,@FunctionName,@CreateTime,@Creater,@UpdateTime,@Updater,1)");
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
        public override bool Update(FunctionDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_Function set ");
            strSql.Append("Form_Id=@Form_Id,");
            strSql.Append("FunctionText=@FunctionText,");
            strSql.Append("FunctionName=@FunctionName,");         
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("RowState=@RowState");
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
            strSql.Append("delete from SetUp_Function ");
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
        public IList<FunctionDomain> GetRoleFunctionList(int roleId)
        {             
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SetUp_Function.ID,SetUp_Function.Form_Id,SetUp_Function.FunctionText,SetUp_Function.FunctionName,SetUp_Role_Function.Id as FunId ");
            strSql.AppendFormat(" FROM SetUp_Function left join SetUp_Role_Function on SetUp_Role_Function.FunctionId=SetUp_Function.Id and RoleId={0}", roleId);

            return ReadAdoTemplate.QueryWithRowMapperDelegate<FunctionDomain>(CommandType.Text, strSql.ToString(), MapRow_RoleFunction);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public FunctionDomain MapRow_RoleFunction(IDataReader dataReader, int rowNum)
        {
            FunctionDomain model = new FunctionDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.Form_Id = (int)dataReader["Form_Id"];
            model.FunctionText = dataReader["FunctionText"].ToString();
            model.FunctionName = dataReader["FunctionName"].ToString();
            ojb = dataReader["FunId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSelected = true;
            }
            return model;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<FunctionDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,Form_Id,FunctionText,FunctionName,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM SetUp_Function ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<FunctionDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public FunctionDomain MapRow(IDataReader dataReader, int rowNum)
        {
            FunctionDomain model = new FunctionDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }           
            model.Form_Id = (int)dataReader["Form_Id"];
            model.FunctionText = dataReader["FunctionText"].ToString();
            model.FunctionName = dataReader["FunctionName"].ToString();          
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
        private IDbParameters GetBaseParams(FunctionDomain entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("Form_Id", DbType.Int32).Value = entity.Form_Id;
            parameters.Add("FunctionText", DbType.String).Value = entity.FunctionText;
            parameters.Add("FunctionName", DbType.String).Value = entity.FunctionName;           
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
