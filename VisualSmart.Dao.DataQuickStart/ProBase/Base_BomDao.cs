﻿using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.ProBase;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.ProBase
{
    public class Base_BomDao : EntityDao<Base_Bom>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Base_Bom entity)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Base_Bom(");
                strSql.Append("ParentGoodNo,ParentGoodName,SonGoodNo,SonGoodName,BiLi,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ParentGoodNo,@ParentGoodName,@SonGoodNo,@SonGoodName,@BiLi,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        public override bool Update(Base_Bom entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Base_Bom set ");
            strSql.Append("ParentGoodNo=@ParentGoodNo,");
            strSql.Append("ParentGoodName=@ParentGoodName,");
            strSql.Append("SonGoodNo=@SonGoodNo,");
            strSql.Append("SonGoodName=@SonGoodName,");
            strSql.Append("BiLi=@BiLi,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater");
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
            strSql.Append("delete from Base_Bom ");
            strSql.Append(" where ID=@ID ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除-所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteAll(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("truncate table [dbo].[Base_Bom] ");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 重新填充Base_Bom_View 表信息
        /// </summary>      
        /// <returns></returns>
        public bool ReSetView()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"truncate table Base_Bom_View;
with t as
(select a.ParentGoodNo ,a.SonGoodNo ,a.BiLi,a.ParentGoodName,a.SonGoodName
  from Base_Bom a
  where not exists(select 1 from Base_Bom b where b.SonGoodNo=a.ParentGoodNo)
 union all
 select d.ParentGoodNo,c.SonGoodNo,c.BiLi*d.BiLi,d.ParentGoodName,c.SonGoodName
  from Base_Bom c
  inner join t d on c.ParentGoodNo=d.SonGoodNo
)
insert into Base_Bom_View 
select ParentGoodNo ,SonGoodNo ,BiLi,ParentGoodName,SonGoodName from (
select a.ParentGoodNo,a.SonGoodNo,a.BiLi,
       case when not exists(select 1 from Base_Bom b where b.ParentGoodNo=a.SonGoodNo)
            then '底层' else '' end  as 'BiaoShi',a.ParentGoodName,a.SonGoodName
 from t a ) as TopBom where BiaoShi=''
 union
 select a.ParentGoodNo ,a.SonGoodNo ,a.BiLi,a.ParentGoodName,a.SonGoodName
 from Base_Bom a");
            var parameters = WriteAdoTemplate.CreateDbParameters();           
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Base_Bom> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,ParentGoodNo,ParentGoodName,SonGoodNo,SonGoodName,BiLi,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" FROM Base_Bom ");
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Base_Bom>(CommandType.Text, strSql.ToString(), MapRow, parameters);
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
            strSql.Append(" FROM Base_Bom ");
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
        public Base_Bom MapRow(IDataReader dataReader, int rowNum)
        {
            Base_Bom model = new Base_Bom();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.ParentGoodNo = dataReader["ParentGoodNo"].ToString();
            model.ParentGoodName = dataReader["ParentGoodName"].ToString();
            model.SonGoodNo = dataReader["SonGoodNo"].ToString();
            model.SonGoodName = dataReader["SonGoodName"].ToString();
            model.BiLi = Convert.ToInt32(dataReader["BiLi"]);

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
        private IDbParameters GetBaseParams(Base_Bom entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();
            parameters.Add("ParentGoodNo", DbType.String).Value = entity.ParentGoodNo ?? "";
            parameters.Add("ParentGoodName", DbType.String).Value = entity.ParentGoodName ?? "";
            parameters.Add("SonGoodNo", DbType.String).Value = entity.SonGoodNo ?? "";
            parameters.Add("SonGoodName", DbType.String).Value = entity.SonGoodName ?? "";
            parameters.Add("BiLi", DbType.Int32).Value = entity.BiLi;
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }
    }
}
