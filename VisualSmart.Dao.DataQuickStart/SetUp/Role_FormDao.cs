using Spring.Data.Common; 
using Spring.Transaction.Interceptor;
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
    public class Role_FormDao : EntityDao<Role_FormDomain>
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Role_FormDomain entity)
        {
            try
            {                
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_Role_Form(");
                strSql.Append("Role_Id,Form_Id,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@Role_Id,@Form_Id,@CreateTime,@Creater,@UpdateTime,@Updater,1)");
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
        public override bool Update(Role_FormDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_Role_Form set ");
            strSql.Append("Role_Id=@Role_Id,");
            strSql.Append("Form_Id=@Form_Id,");          
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
            strSql.Append("delete from SetUp_Role_Form ");
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
        public override IList<Role_FormDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,Role_Id,Form_Id,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM SetUp_Role_Form ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Role_FormDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Role_FormDomain MapRow(IDataReader dataReader, int rowNum)
        {
            Role_FormDomain model = new Role_FormDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.Role_Id = (int)dataReader["Role_Id"];
            model.Form_Id = (int)dataReader["Form_Id"];           
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
        private IDbParameters GetBaseParams(Role_FormDomain entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("Role_Id", DbType.Int32).Value = entity.Role_Id;
            parameters.Add("Form_Id", DbType.Int32).Value = entity.Form_Id;           
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater;
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater;
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
        
         
        /// <summary>
        /// 保存 权限
        /// </summary>
        /// <param name="roleId">角色</param>
        /// <param name="allMenuids">所有展开的窗口</param>
        /// <param name="selectedFormIds">所有选中的窗口</param>
        /// <param name="selectedFunctionIds">所有选中的窗口方法</param>
        /// <param name="userName">用户名称</param>
        /// <returns></returns>
        //[Transaction(ReadOnly = false)]
        public bool SaveRole_FormAndFunctioin(int roleId, string allMenuids, string selectedFormIds, string selectedFunctionIds, string userName="")
        {
            Role_FormDao roleFormDao = new Role_FormDao();
            Role_FunctionDao roleFunctionDao = new Role_FunctionDao();
            string deleteFromAndFunction = string.Format("delete from SetUp_Role_Function where RoleId={0} and FormId in (select Id from SetUp_Form where MenuId in ({1})); delete from SetUp_Role_Form where Role_Id={0} and Form_Id in (select Id from SetUp_Form where MenuId in ({1}));", roleId, allMenuids);

            ReadAdoTemplate.ExecuteNonQuery(CommandType.Text,deleteFromAndFunction);

            foreach (var formId in selectedFormIds.Split(','))
            {
                var model = new Role_FormDomain { Form_Id=Convert.ToInt32(formId), Role_Id=roleId, CreateTime=DateTime.Now,UpdateTime=DateTime.Now,Creater=userName,Updater=userName };
                roleFormDao.Add(model);
            }

            if (!string.IsNullOrEmpty(selectedFunctionIds))
            {
                foreach (var function_FormId in selectedFunctionIds.Split(','))
                {
                    var functionFormId = function_FormId.Split('_');//0 FORMID 1 FUNCTIONID
                    var model = new Role_FunctionDomain { FormId = Convert.ToInt32(functionFormId[0]), FunctionId = Convert.ToInt32(functionFormId[1]), RoleId = roleId, CreateTime = DateTime.Now, UpdateTime = DateTime.Now, Creater = userName, Updater = userName };
                    roleFunctionDao.Add(model);
                }
            }
            //ReadAdoTemplate.DbProvider.
            //var a = 0;
            //if (1 / a==0)
            //{ 
                
            //}
            return true;
        }

        /// <summary>
        /// 对于'不能编辑' 来说 如果返回为false 说明 不能编辑  反正 可以
        /// 对于'查看所有' 来说 如果返回为false 说明 不能查看所有  反正 能查看所有
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="displayName"></param>
        /// <param name="textName"></param>
        /// <returns></returns>
        public bool GetUserRight(int currentUserId,string displayName, string textName)
        {
            string sql = string.Format(@"select COUNT(*) from SetUp_Role_Function left join SetUp_Function
 on SetUp_Function.ID=SetUp_Role_Function.FunctionId 
where  RoleId in (select Role_Id from SetUp_Role_User where User_Id={0}) 
and FormId in(select ID from SetUp_Form where FormName='{1}')
and FunctionText='{2}' ", currentUserId, displayName, textName);


//            if (textName != "不能编辑" && textName != "禁止含税设置")
//            {
//                sql = string.Format(@"declare @result int;
//set @result=0;
//select @result=COUNT(*) from role_sys_form left join sys_Object on sys_Object.FormID=role_sys_form.sys_form_Id and sys_Object.roleId=role_sys_form.role_Id and textName='{2}'
//where  role_Id in (select roleId from Role_User where userId={0}) 
//and sys_form_Id in(select formID from sys_form where displayName='{1}')
//and sys_Object.AutoID is not null;
//select @result=COUNT(*)-@result from role_sys_form left join sys_Object on sys_Object.FormID=role_sys_form.sys_form_Id and sys_Object.roleId=role_sys_form.role_Id and textName='{2}'
//where  role_Id in (select roleId from Role_User where userId={0}) 
//and sys_form_Id in(select formID from sys_form where displayName='{1}')
//select @result;
//", currentUserId, displayName, textName);
//            }
//            else
//            {
//                sql = string.Format(@"declare @result int;
//set @result=0;
//select @result=COUNT(*) from role_sys_form left join sys_Object on sys_Object.FormID=role_sys_form.sys_form_Id and sys_Object.roleId=role_sys_form.role_Id and textName='{2}'
//where  role_Id in (select roleId from Role_User where userId={0}) 
//and sys_form_Id in(select formID from sys_form where displayName='{1}')
//and sys_Object.AutoID is not null;
//select @result;
//", currentUserId, displayName, textName);
//            }

            if (Convert.ToInt32(WriteAdoTemplate.ExecuteScalar(CommandType.Text,sql)) == 0)
            {
                return false;
            }
            return true;

        }
    }
}
