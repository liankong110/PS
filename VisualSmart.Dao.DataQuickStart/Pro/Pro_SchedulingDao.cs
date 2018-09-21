using Spring.Data.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;
namespace VisualSmart.Dao.DataQuickStart.Pro
{
    public class Pro_SchedulingDao : EntityDao<Pro_Scheduling>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Add(Pro_Scheduling entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_Scheduling", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_Scheduling(");
                strSql.Append("ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@ShipMainProNo,@PlanFromDate,@PlanToDate,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
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
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_Scheduling entity)
        {
            try
            {
                entity.ProNo = GetProNo("Pro_Scheduling", "ProNo");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Pro_Scheduling(");
                strSql.Append("ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState)");
                strSql.Append(" values (");
                strSql.Append("@ProNo,@ShipMainProNo,@PlanFromDate,@PlanToDate,@CreateTime,@Creater,@UpdateTime,@Updater,@RowState)");
                strSql.Append(";select @@IDENTITY");
                var parameters = GetBaseParams(entity);
                return Convert.ToInt32(ReadAdoTemplate.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters));
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
        public override bool Update(Pro_Scheduling entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_Scheduling set ");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Updater=@Updater");
            strSql.Append(" where Id=@Id;");
            var parameters = GetBaseParams(entity);
            parameters.Add("ID", DbType.Int32, 0).Value = entity.Id;

            //删除所有除Pro_Scheduling 的所有信息
            strSql.Append(@"delete from Pro_SchedulingGoodsNum  where SGoodId IN (select id from [dbo].[Pro_SchedulingGoods]
 where SLineId IN(select id from[dbo].[Pro_SchedulingLine] where MainId =@ID)); ");
            strSql.Append("delete from [Pro_SchedulingGoods] where SLineId IN ( select id from[dbo].[Pro_SchedulingLine] where MainId=@ID);");
            strSql.Append("delete from [Pro_SchedulingLine] where MainId=@ID;");

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
            strSql.Append(@"delete from Pro_SchedulingGoodsNum  where SGoodId IN (select id from [dbo].[Pro_SchedulingGoods]
 where SLineId IN(select id from[dbo].[Pro_SchedulingLine] where MainId =@ID)); ");
            strSql.Append("delete from [Pro_SchedulingGoods] where SLineId IN ( select id from[dbo].[Pro_SchedulingLine] where MainId=@ID);");
            strSql.Append("delete from [Pro_SchedulingLine] where MainId=@ID;");
            strSql.Append("delete from Pro_Scheduling where ID=@ID;");
            var parameters = WriteAdoTemplate.CreateDbParameters();
            parameters.Add("ID", DbType.Int32, 0).Value = Id;
            return ReadAdoTemplate.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<Pro_Scheduling> GetAllDomain(QueryCondition query)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   ");
            strSql.Append(" Id,ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" from Pro_Scheduling ");
            string otherWhere = "";
            if (query.GetCondition("PlanFromDate") != null)
            {

            }
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters, otherWhere));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_Scheduling>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_Scheduling> GetList(QueryCondition query, Hashtable hs)
        {
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   ");
            strSql.Append(" Id,ProNo,ShipMainProNo,PlanFromDate,PlanToDate,CreateTime,Creater,UpdateTime,Updater,RowState ");
            strSql.Append(" from Pro_Scheduling ");
            string otherWhere = "";
            if (hs.ContainsKey("PlanFromDate"))
            {
                otherWhere += string.Format(" and '{0}'>=PlanFromDate and '{0}'<=PlanToDate", hs["PlanFromDate"]);
            }
            if (hs.ContainsKey("LineNo"))
            {
                otherWhere += string.Format(" and Exists(select id from Pro_SchedulingLine where ProLineNo like '%{0}%' and Pro_SchedulingLine.MainId=Pro_Scheduling.Id)",
                    hs["LineNo"]);
            }
            if (hs.ContainsKey("GoodNo") || hs.ContainsKey("GoodName") || hs.ContainsKey("ShipTo"))
            {
                otherWhere += " and Id in (select MainId from Pro_SchedulingLine left join Pro_SchedulingGoods on Pro_SchedulingGoods.SLineId=Pro_SchedulingLine.Id where 1=1 ";
                if (hs.ContainsKey("GoodNo"))
                {
                    otherWhere += string.Format(" and GoodNo like '%{0}%'",hs["GoodNo"]);
                }
                if (hs.ContainsKey("GoodName"))
                {
                    otherWhere += string.Format(" and GoodName like '%{0}%'", hs["GoodName"]);
                }
                if (hs.ContainsKey("ShipTo"))
                {
                    otherWhere += string.Format(" and ShipTo like '%{0}%'", hs["ShipTo"]);
                }
                otherWhere += " )";
            }
            if (query.GetPager() != null)
            {
                strSql = new StringBuilder(GetPagerSql(strSql.ToString(), query, parameters, otherWhere));
            }
            else
            {
                strSql.Append(query.GetSQL_Where(parameters));
                strSql.Append(query.GetSQL_Order());
            }

            return ReadAdoTemplate.QueryWithRowMapperDelegate<Pro_Scheduling>(CommandType.Text, strSql.ToString(), MapRow, parameters);
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_Scheduling MapRow(IDataReader dataReader, int rowNum)
        {
            Pro_Scheduling model = new Pro_Scheduling();
            object ojb;
            ojb = dataReader["Id"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Id = (int)ojb;
            }
            model.ProNo = dataReader["ProNo"].ToString();
            model.ShipMainProNo = dataReader["ShipMainProNo"].ToString();

            ojb = dataReader["PlanFromDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanFromDate = (DateTime)ojb;
            }
            ojb = dataReader["PlanToDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PlanToDate = (DateTime)ojb;
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
        private IDbParameters GetBaseParams(Pro_Scheduling entity)
        {
            var parameters = ReadAdoTemplate.CreateDbParameters();

            parameters.Add("ShipMainProNo", DbType.String).Value = entity.ShipMainProNo;
            parameters.Add("ProNo", DbType.String).Value = entity.ProNo;
            parameters.Add("PlanFromDate", DbType.Date).Value = entity.PlanFromDate;
            parameters.Add("PlanToDate", DbType.Date).Value = entity.PlanToDate;
            parameters.Add("CreateTime", SqlDbType.NVarChar).Value = DateTime.Now;
            parameters.Add("Creater", SqlDbType.NVarChar).Value = entity.Creater ?? "";
            parameters.Add("UpdateTime", SqlDbType.DateTime).Value = DateTime.Now;
            parameters.Add("Updater", SqlDbType.NVarChar).Value = entity.Updater ?? "";
            parameters.Add("RowState", SqlDbType.TinyInt).Value = entity.RowState;
            return parameters;
        }


        /// <summary>
        /// 获取当前Bom 是否有下级商品信息 需要排产
        /// </summary>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int CreateNextBomList(QueryCondition query, string name)
        {
            var mainId = query.GetCondition("MainId").Value;
            var parameters = WriteAdoTemplate.CreateDbParameters();
            StringBuilder strSql = new StringBuilder();
            //获取BOM下一级的子产品 并剔除最后一级别
            strSql.AppendFormat(@"select distinct ShipTo,ShipToName,SonGoodNo as GoodNo,SonGoodName as GoodName,ParentGoodNo,ParentGoodName,BiLi
from(select ID,ShipTo,ShipToName,GoodNo,GoodName FROM Pro_SchedulingGoods  where  
SLineId IN (select ID from [dbo].[Pro_SchedulingLine] WHERE MainId={0})
) AS TB inner join [dbo].[Base_Bom] on TB.GoodNo=Base_Bom.ParentGoodNo where exists(select 1 from Base_Bom b where b.ParentGoodNo=Base_Bom.SonGoodNo)", mainId);

            var list = ReadAdoTemplate.QueryWithRowMapperDelegate(CommandType.Text, strSql.ToString(), MapRowByCreateNextBom, parameters);
            //如果检测到有下一级的BOM信息
            if (list.Count > 0)
            {
                strSql = new StringBuilder();
                strSql.AppendFormat(@"select GoodNo,ShipTo,SDate,sum(SNum) as SNums from [dbo].[Pro_SchedulingGoodsNum]
left join Pro_SchedulingGoods on Pro_SchedulingGoods.Id=Pro_SchedulingGoodsNum.SGoodId
 where SGoodId in
(select TB.ID
from(select ID,ShipTo,ShipToName,GoodNo,GoodName FROM Pro_SchedulingGoods  where  
SLineId IN (select ID from [dbo].[Pro_SchedulingLine] WHERE MainId={0})
) AS TB inner join [dbo].[Base_Bom] on TB.GoodNo=Base_Bom.ParentGoodNo)   
and SType in (2,3,4)
group by GoodNo,ShipTo,SDate", mainId);
                //排产信息 将早中晚 汇总在一起
                var num_list = ReadAdoTemplate.QueryWithRowMapperDelegate(CommandType.Text, strSql.ToString(), MapRow_BomNum, parameters).ToList();
                if (num_list.Count > 0)
                { //主信息
                    var scheduling_model = GetAllDomain(QueryCondition.Instance.AddEqual("Id", mainId)).ToList()[0];

                    Pro_ShipPlanMain mainModel = new Pro_ShipPlanMain();

                    mainModel.Updater = mainModel.Creater = name;
                    mainModel.RowState = 1;
                    mainModel.PlanFromDate = scheduling_model.PlanFromDate;
                    mainModel.PlanFromTo = scheduling_model.PlanToDate;
                    var main_Id = new Pro_ShipPlanMainDao().AddGetId(mainModel);

                    var _shipPlanBizService = new Pro_ShipPlanDao();
                    var _shipPlansBizService = new Pro_ShipPlansDao();
                    foreach (var good in list)
                    {
                        var model = new Pro_ShipPlan();
                        model.ScheduleNo = "";

                        model.Term = 0;
                        model.EditionNo = "";
                        model.CityNo = "";
                        model.ShipDetailNo = "";
                        model.ShipTo = good.ShipTo;
                        model.ShipToName = good.ShipToName;
                        model.GoodNo = good.GoodNo;
                        model.GoodName = good.GoodName;
                        model.MainId = main_Id;

                        //获取goodNO+ shipto的商品 如果没有对应的排产信息就不要出来了
                        var good_num_list = num_list.FindAll(t => t.GoodNo == good.ParentGoodNo && t.ShipTo == good.ShipTo);
                        if (good_num_list.Count == 0)
                        {
                            continue;
                        }
                        var shipId = _shipPlanBizService.AddGetId(model);



                        for (int i = 0; i < good_num_list.Count; i++)
                        {
                            var goodNum = good_num_list[i];
                            var sonModel = new Pro_ShipPlans();
                            sonModel.PlanId = shipId;
                            sonModel.PlanNum = Convert.ToInt32((goodNum.SNum ?? 0) * good.BiLi);
                            sonModel.PlanDate = goodNum.SDate;
                            _shipPlansBizService.Add(sonModel);
                        }
                    }
                    return main_Id;
                }
                else
                {
                    //说明没有下一级的BOM 信息
                    return -1;
                }
               
            }
            //说明没有下一级的BOM 信息
            return -1;
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_SchedulingGoods MapRowByCreateNextBom(IDataReader dataReader, int rowNum)
        {
            Pro_SchedulingGoods model = new Pro_SchedulingGoods();
            model.GoodNo = Convert.ToString(dataReader["GoodNo"]);
            model.GoodName = Convert.ToString(dataReader["GoodName"]);
            model.ParentGoodNo = Convert.ToString(dataReader["ParentGoodNo"]);
            model.ParentGoodName = Convert.ToString(dataReader["ParentGoodName"]);
            model.ShipTo = Convert.ToString(dataReader["ShipTo"]);
            model.ShipToName = Convert.ToString(dataReader["ShipToName"]);
            model.BiLi = Convert.ToDecimal(dataReader["BiLi"]);
            return model;
        }

        /// <summary>
        /// 列表基本参数
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Pro_SchedulingGoodsNum MapRow_BomNum(IDataReader dataReader, int rowNum)
        {
            Pro_SchedulingGoodsNum model = new Pro_SchedulingGoodsNum();
            object ojb;
            ojb = dataReader["SDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SDate = (DateTime)ojb;
            }
            ojb = dataReader["SNums"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.SNum = (int)ojb;
            }
            model.GoodNo = Convert.ToString(dataReader["GoodNo"]);
            model.ShipTo = Convert.ToString(dataReader["ShipTo"]);
            return model;
        }

    }
}
