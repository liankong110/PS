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
    public class FormDao : EntityDao<FormDomain>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(FormDomain entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SetUp_Form(");
                strSql.Append("MenuId,FormName,Path,FormIndex,FormImgURL)");
                strSql.Append(" values (");
                strSql.Append("@MenuId,@FormName,@Path,@FormIndex,@FormImgURL)");
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
        public override bool Update(FormDomain entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SetUp_Form set ");
            strSql.Append("MenuId=@MenuId,");
            strSql.Append("FormName=@FormName,");
            strSql.Append("Path=@Path,");
            strSql.Append("FormIndex=@FormIndex,");
            strSql.Append("FormImgURL=@FormImgURL");
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
            strSql.Append("delete from SetUp_Form ");
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
        public override IList<FormDomain> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,MenuId,FormName,Path,FormIndex,FormImgURL ");
            strSql.Append(" FROM SetUp_Form ");
            if (query.GetPager() != null)
            {
                strSql =new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
               strSql.Append(query.GetSQL_Where(parameters));
               strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<FormDomain>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FormDomain> GetRoleFromList(int roleId)
        {            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SetUp_Form.Id,SetUp_Form.MenuId,SetUp_Form.FormName,SetUp_Form.Path,SetUp_Form.FormIndex,SetUp_Form.FormImgURL,SetUp_Role_Form.id as RF_Id ");
            strSql.AppendFormat(" FROM SetUp_Form left join SetUp_Role_Form on SetUp_Form.id=SetUp_Role_Form.Form_Id and SetUp_Role_Form.Role_Id={0} order by FormIndex", roleId);


            return ReadAdoTemplate.QueryWithRowMapperDelegate<FormDomain>(CommandType.Text, strSql.ToString(), MapRow_Selected);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public FormDomain MapRow_Selected(IDataReader dataReader, int rowNum)
        {
            FormDomain model = MapRow(dataReader, rowNum);

            object ojb;
            ojb = dataReader["RF_Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsSelected = true;
            }
            
            return model;
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public FormDomain MapRow(IDataReader dataReader, int rowNum)
        {
            FormDomain model = new FormDomain();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            ojb = dataReader["MenuId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MenuId = (int)ojb;
            }
            model.FormName = dataReader["FormName"].ToString();
            model.Path = dataReader["Path"].ToString();
            ojb = dataReader["FormIndex"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FormIndex = (int)ojb;
            }
            model.FormImgURL = dataReader["FormImgURL"].ToString();
            return model;
        }

      /// <summary>
      /// 跟进用户获取权限菜单
      /// </summary>
      /// <param name="UserId"></param>
      /// <returns></returns>
        public  IList<FormDomain> GetFormByUserId(int UserId)
        {            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SetUp_Form.Id,MenuId,FormName,Path,FormIndex,FormImgURL ");
            strSql.Append(" FROM SetUp_Role_Form left join SetUp_Form on SetUp_Form.id=SetUp_Role_Form.form_ID ");
            strSql.AppendFormat(" where SetUp_Role_Form.Role_Id in (select Role_Id from SetUp_Role_User where User_Id={0}) ORDER BY FormIndex ",UserId);            
            return ReadAdoTemplate.QueryWithRowMapperDelegate<FormDomain>(CommandType.Text, strSql.ToString(), MapRow);
        }

        /// <summary>
        /// 根据用户访问的地址 判断是否有权限访问
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="action">action</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        public int GetFormByUserId(string controller, string action, int UserId)
        {
            StringBuilder strSql = new StringBuilder();
          
            strSql.AppendFormat(@" DECLARE @FROMID INT; SET  @FROMID=0;
SELECT @FROMID=Id FROM SetUp_Form  WHERE Path='{0}'
IF(@FROMID<>0)
BEGIN
SELECT COUNT(*) AS COU FROM SetUp_Role_Form WHERE ROLE_ID IN ( SELECT ROLE_ID FROM SetUp_Role_User WHERE User_Id={1})
AND FORM_ID=@FROMID
END
ELSE
SELECT -1 AS COU ", "/" + controller + "/" + action, UserId);
            
            return (int)ReadAdoTemplate.ExecuteScalar(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(FormDomain entity)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();

            parameters.Add("@MenuId", SqlDbType.Int, 4).Value = entity.MenuId;
            parameters.Add("@FormName", SqlDbType.NVarChar, 50).Value = entity.FormName;
            parameters.Add("@Path", SqlDbType.NVarChar, 50).Value = entity.Path;
            parameters.Add("@FormIndex", SqlDbType.Int, 4).Value = entity.FormIndex;
            parameters.Add("@FormImgURL", SqlDbType.NVarChar, 50).Value = entity.FormImgURL;
            return parameters;
        }
    }
}
