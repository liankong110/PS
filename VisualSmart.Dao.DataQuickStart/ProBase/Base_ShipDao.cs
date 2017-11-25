using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.ProBase
{
    public class Base_ShipDao : EntityDao<Base_Ship>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Base_Ship entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_Ship(");
                strSql.Append("ShipTo,ShipToName,CityNo,City,ShipToHour,ShipToMins,LeadTime,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ShipTo,@ShipToName,@CityNo,@City,@ShipToHour,@ShipToMins,@LeadTime,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Base_Ship entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Base_Ship set ");
            strSql.Append("ShipTo=@ShipTo,");
            strSql.Append("ShipToName=@ShipToName,");
            strSql.Append("CityNo=@CityNo,");
            strSql.Append("City=@City,");
            strSql.Append("ShipToHour=@ShipToHour,");
            strSql.Append("ShipToMins=@ShipToMins,");
            strSql.Append("LeadTime=@LeadTime,");
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
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Base_Ship ");
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
        public override IList<Base_Ship> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ShipTo,ShipToName,CityNo,City,ShipToHour,ShipToMins,LeadTime,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Base_Ship ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Base_Ship>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Base_Ship MapRow(IDataReader dataReader, int rowNum)
        {
            Base_Ship model = new Base_Ship();         
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            model.CityNo = dataReader["CityNo"].ToString();
            model.City = dataReader["City"].ToString();
            ojb = dataReader["ShipToHour"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ShipToHour = (int)ojb;
            }
            ojb = dataReader["ShipToMins"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ShipToMins = (int)ojb;
            }
            ojb = dataReader["LeadTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.LeadTime = (decimal)ojb;
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
            return model;
        }

        /// <summary>
        /// 新增 修改 基本参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IDbParameters GetBaseParams(Base_Ship entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("ShipTo", DbType.String).Value = entity.ShipTo ?? "";
            parameters.Add("ShipToName", DbType.String).Value = entity.ShipToName ?? "";
            parameters.Add("CityNo", DbType.String).Value = entity.CityNo ?? "";
            parameters.Add("City", DbType.String).Value = entity.City ?? "";
            parameters.Add("ShipToHour", DbType.Int32).Value = entity.ShipToHour;
            parameters.Add("ShipToMins", DbType.Int32).Value = entity.ShipToMins;
            parameters.Add("LeadTime", SqlDbType.Date).Value = entity.LeadTime;
         
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
