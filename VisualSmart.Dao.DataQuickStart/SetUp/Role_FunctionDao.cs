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
    public class Role_FunctionDao : EntityDao<Role_FunctionDomain>
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Role_FunctionDomain entity)
        {
            try
            {                
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_Role_Function(");
                strSql.Append("FormId,FunctionId,RoleId,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@FormId,@FunctionId,@RoleId,@CreateTime,@Creater,@UpdateTime,@Updater,1)");
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
        public override bool Update(Role_FunctionDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_Role_Function set ");
            strSql.Append("FormId=@FormId,");
            strSql.Append("FunctionId=@FunctionId,");
            strSql.Append("RoleId=@RoleId,");     
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
            strSql.Append("delete from SetUp_Role_Function ");
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
        public override IList<Role_FunctionDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,FormId,FunctionId,RoleId,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM SetUp_Role_Function ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Role_FunctionDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }  

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Role_FunctionDomain MapRow(IDataReader dataReader, int rowNum)
        {
            Role_FunctionDomain model = new Role_FunctionDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.FormId = (int)dataReader["FormId"];
            model.FunctionId = (int)dataReader["FunctionId"];
            model.RoleId = (int)dataReader["RoleId"]; 
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
        private IDbParameters GetBaseParams(Role_FunctionDomain entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("FormId", DbType.Int32).Value = entity.FormId;
            parameters.Add("FunctionId", DbType.Int32).Value = entity.FunctionId;
            parameters.Add("RoleId", DbType.Int32).Value = entity.RoleId;   
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
