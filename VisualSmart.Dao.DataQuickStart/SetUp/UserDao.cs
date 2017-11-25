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
    public class UserDao : EntityDao<UserDomain>
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(UserDomain entity)
        {
            try
            {                
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_User(");
                strSql.Append("loginId,loginPwd,loginState,Name,UserState,Phone,Email,Sex,Position,Leader,Company,Avatar,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@loginId,@loginPwd,@loginState,@Name,@UserState,@Phone,@Email,@Sex,@Position,@Leader,@Company,@Avatar,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
                strSql.Append(";select @@IDENTITY");
                var parameters = GetBaseParams(entity);
                entity.Id = Convert.ToInt32(ReadAdoTemplate.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters));
                return entity.Id > 0;
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
        public override bool Update(UserDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_User set ");
            strSql.Append("loginId=@loginId,");
            //strSql.Append("loginPwd=@loginPwd,");
            strSql.Append("loginState=@loginState,");
            strSql.Append("Name=@Name,");
            strSql.Append("UserState=@UserState,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Email=@Email,");
            strSql.Append("Sex=@Sex,");
            strSql.Append("Position=@Position,");
            strSql.Append("Leader=@Leader,");
            strSql.Append("Company=@Company,");
            strSql.Append("Avatar=@Avatar,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("Creater=@Creater,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater,");
            strSql.Append("RowState=@RowState");
            strSql.Append(" where ID=@ID");
            var parameters = GetBaseParams(entity);
            parameters.Add("ID", DbType.Int32, 0).Value = entity.Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public bool ChangePassword(int Id, string loginPwd)
        {
            string sql = string.Format("update SetUp_User set loginPwd=@loginPwd where  ID=@ID");           
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("loginPwd", SqlDbType.VarChar).Value = loginPwd;
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SetUp_User ");
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
        public override IList<UserDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,loginId,loginPwd,loginState,Name,UserState,Phone,Email,Sex,Position,Leader,Company,Avatar,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM SetUp_User ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               // strSql.Append(" where 1=1  AND LoginId='admin'");
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<UserDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public UserDomain MapRow(IDataReader dataReader, int rowNum)
        {
            UserDomain model = new UserDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }           
            model.loginId = dataReader["loginId"].ToString();
            model.loginPwd = dataReader["loginPwd"].ToString();
            //ojb = dataReader["loginState"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.loginState = (byte)ojb;
            //}
            model.Name = dataReader["Name"].ToString();
            ojb = dataReader["UserState"];
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.UserState = (byte)ojb;
            //}
            model.Phone = dataReader["Phone"].ToString();
            model.Email = dataReader["Email"].ToString();
            ojb = dataReader["Sex"];
            if (ojb != null && ojb != DBNull.Value)
            {
                //model.Sex = (bool)ojb;
            }
            model.Position = dataReader["Position"].ToString();
            ojb = dataReader["Leader"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Leader = (int)ojb;
            }
            model.Company = dataReader["Company"].ToString();
            model.Avatar = dataReader["Avatar"].ToString();
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
            //if (ojb != null && ojb != DBNull.Value)
            //{
            //    model.RowState = (byte)ojb;
            //}           
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(UserDomain entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("loginId", DbType.String).Value = entity.loginId??"";
            parameters.Add("loginPwd", SqlDbType.VarChar).Value = entity.loginPwd ?? "";
            parameters.Add("loginState", SqlDbType.TinyInt).Value = entity.loginState;
            parameters.Add("Name", SqlDbType.NVarChar).Value = entity.Name ?? "";
            parameters.Add("UserState", SqlDbType.TinyInt).Value = entity.UserState;
            parameters.Add("Phone", SqlDbType.NVarChar).Value = entity.Phone ?? "";
            parameters.Add("Email", SqlDbType.NVarChar).Value = entity.Email ?? "";
            parameters.Add("Sex", SqlDbType.Bit).Value = entity.Sex;
            parameters.Add("Position", SqlDbType.NVarChar).Value = entity.Position ?? "";
            parameters.Add("Leader", SqlDbType.Int).Value = entity.Leader;
            parameters.Add("Company", SqlDbType.NVarChar).Value = entity.Company ?? "";
            parameters.Add("Avatar", SqlDbType.NVarChar).Value = entity.Avatar ?? "";
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }

        public bool Add_Update_User_Role(UserDomain entity, string roleIds, string userName)
        {
            if (entity.Id > 0)
            {
                Update(entity);
                string sql = "delete from SetUp_Role_User where User_Id="+entity.Id;
                ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, sql);
            }
            else
            {
                Add(entity);
            }
            if (!string.IsNullOrEmpty(roleIds))
            {
                var roleUserDao = new Role_UserDao();
                foreach (var roleId in roleIds.Split(','))
                {
                    var model = new Role_UserDomain
                    {
                        User_Id = entity.Id,
                        Role_Id = Convert.ToInt32(roleId),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        Creater = userName,
                        Updater = userName
                    };
                    roleUserDao.Add(model);
                }
            }
            return true;
        }
    }
}
