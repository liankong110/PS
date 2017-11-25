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
    public class MenuDao : EntityDao<MenuDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(MenuDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_Menu(");
                strSql.Append("DisplayName,MenuIndex)");
                strSql.Append(" values (");
                strSql.Append("@DisplayName,@MenuIndex)");
                strSql.Append(";select @@IDENTITY");
                var parameters = GetBaseParams(entity);
                return WriteAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
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
        public override bool Update(MenuDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_Menu set ");
            strSql.Append("DisplayName=@DisplayName,");
            strSql.Append("MenuIndex=@MenuIndex");
            strSql.Append(" where Id=@Id");
            var parameters = GetBaseParams(entity);
            parameters.Add("ID", DbType.Int32, 0).Value = entity.Id;
            return WriteAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SetUp_Menu ");
            strSql.Append(" where ID=@ID ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return WriteAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<MenuDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,DisplayName,MenuIndex ");
            strSql.Append(" FROM SetUp_Menu ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<MenuDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public MenuDomain MapRow(IDataReader dataReader, int rowNum)
        {
            MenuDomain model = new MenuDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.DisplayName = dataReader["DisplayName"].ToString();
            ojb = dataReader["MenuIndex"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MenuIndex = (int)ojb;
            }
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(MenuDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters(); 
            parameters.Add("@DisplayName", SqlDbType.NVarChar, 50).Value = entity.DisplayName;
            parameters.Add("@MenuIndex", SqlDbType.Int, 4).Value = entity.MenuIndex;
            return parameters;
        }
    }
}
