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
    public class Base_GoodsDao : EntityDao<Base_Goods>
    {/// <summary>
     /// 新增
     /// </summary>
     /// <param name="entity"></param>
     /// <returns></returns>
        public override bool Add(Base_Goods entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_Goods(");
                strSql.Append("GoodNo,GoodName,ShipTo,ShipToName,PML,ShipPkgQty,UM,StandardDays,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@GoodNo,@GoodName,@ShipTo,@ShipToName,@PML,@ShipPkgQty,@UM,@StandardDays,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Base_Goods entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Base_Goods set ");
            strSql.Append("GoodNo=@GoodNo,");
            strSql.Append("GoodName=@GoodName,");
            strSql.Append("ShipTo=@ShipTo,");
            strSql.Append("ShipToName=@ShipToName,");
            strSql.Append("PML=@PML,");
            strSql.Append("ShipPkgQty=@ShipPkgQty,");
            strSql.Append("UM=@UM,");
            strSql.Append("StandardDays=@StandardDays,");
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
            strSql.Append("delete from Base_Goods ");
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
        public override IList<Base_Goods> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,GoodNo,GoodName,ShipTo,ShipToName,PML,ShipPkgQty,UM,StandardDays,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Base_Goods ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Base_Goods>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<string> GetGoodName(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodNo+','+GoodName as good from Base_Goods where RowState=1 group by GoodNo,GoodName");   
            return ReadAdoTemplate.QueryWithRowMapperDelegate<string>(CommandType.Text, strSql.ToString(), MapRowToString, parameters);
        }
        /// <summary>
        /// 获取id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetId(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id ");
            strSql.Append(" FROM Base_Goods ");            
            strSql.Append(query.GetSQL_Where(parameters));
            strSql.Append(query.GetSQL_Order());
            var result = ReadAdoTemplate.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            if (result == null || result is DBNull)
            {
                return -1;
            }
            return Convert.ToInt32(result);
        }
        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public string MapRowToString(IDataReader dataReader, int rowNum)
        {            
            return  dataReader["good"].ToString(); 
        }
        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Base_Goods MapRow(IDataReader dataReader, int rowNum)
        {
            Base_Goods model = new Base_Goods();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.GoodNo = dataReader["GoodNo"].ToString();
            model.GoodName = dataReader["GoodName"].ToString();
            model.ShipTo = dataReader["ShipTo"].ToString();
            model.ShipToName = dataReader["ShipToName"].ToString();
            model.PML = dataReader["PML"].ToString();
            model.ShipPkgQty = dataReader["ShipPkgQty"].ToString();
            model.UM = dataReader["UM"].ToString();
            ojb = dataReader["StandardDays"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StandardDays = (decimal)ojb;
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
        private IDbParameters GetBaseParams(Base_Goods entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("GoodNo", DbType.String).Value = entity.GoodNo??"";
            parameters.Add("GoodName", DbType.String).Value = entity.GoodName ?? "";
            parameters.Add("ShipTo", DbType.String).Value = entity.ShipTo ?? "";
            parameters.Add("ShipToName", DbType.String).Value = entity.ShipToName ?? "";
            parameters.Add("PML", DbType.String).Value = entity.PML ?? "";
            parameters.Add("ShipPkgQty", DbType.String).Value = entity.ShipPkgQty ?? "";
            parameters.Add("UM", DbType.String).Value = entity.UM ?? "";
            parameters.Add("StandardDays", DbType.Decimal).Value = entity.StandardDays;          
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
