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
    public class RoleDao : EntityDao<RoleDomain>
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(RoleDomain entity)
        {
            try
            {                
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_Role(");
                strSql.Append("RoleName,RoleDec,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@RoleName,@RoleDec,@CreateTime,@Creater,@UpdateTime,@Updater,1)");
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
        public override bool Update(RoleDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_Role set ");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("RoleDec=@RoleDec,");           
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
            strSql.Append("delete from SetUp_Role ");
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
        public override IList<RoleDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,RoleName,RoleDec,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM SetUp_Role ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<RoleDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public RoleDomain MapRow(IDataReader dataReader, int rowNum)
        {
            RoleDomain model = new RoleDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.RoleName = dataReader["RoleName"].ToString();
            model.RoleDec = dataReader["RoleDec"].ToString();
            
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
        private IDbParameters GetBaseParams(RoleDomain entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("RoleName", DbType.String).Value = entity.RoleName??"";
            parameters.Add("RoleDec", DbType.String).Value = entity.RoleDec ?? "";
            parameters.Add("CreateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<RoleDomain> GetUser_RoleList(int UserId)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SetUp_Role.ID,RoleName,SetUp_Role_User.Id as RU_ID ");
            strSql.Append(" FROM SetUp_Role left join SetUp_Role_User on SetUp_Role_User.Role_Id=SetUp_Role.Id and SetUp_Role_User.User_Id=" + UserId);


            return ReadAdoTemplate.QueryWithRowMapperDelegate<RoleDomain>(CommandType.Text, strSql.ToString(), GetUser_RoleList_MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public RoleDomain GetUser_RoleList_MapRow(IDataReader dataReader, int rowNum)
        {
            RoleDomain model = new RoleDomain();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.RoleName = dataReader["RoleName"].ToString();
            ojb = dataReader["RU_ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSelected = true;
            }
            return model;
        }

    }
}
