using System;
using System.Collections;
using System.Collections.Generic;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_PSDetailBizService : EntityBizService<Pro_PSDetail, Pro_PSDetailDao>, IPro_PSDetailBizService
    {
        public IList<Pro_PSDetail> GetPSDetailBySchedulingLineId(Pro_PS psModel, Pro_SchedulingLine fristLine, DateTime SDate)
        {
            IList<Pro_PSDetail> psDetail = new List<Pro_PSDetail>();
             //产线对应的产品信息
            IList<Pro_SchedulingGoods> schedulingGoods = new Pro_SchedulingGoodsDao().GetListBySchedulingLineId(Util.QueryCondition.Instance.AddEqual("Id", fristLine.Id.ToString())
                .AddEqual("SDate", SDate.ToString("yyyy-MM-dd")));

            int classNum = 0;
            //计算班次
            if (psModel.FinalEveningNum != null)
            {
                classNum++;
            }
            if (psModel.FinalMiddleNum != null)
            {
                classNum++;
            }
            if (psModel.FinalMorningNum > 0)
            {
                classNum++;
            }

            //每天开始时间为8：00
            //1班次 8：00~08:00
            //2班次 8:00~ 12:45  12:45~08:00
            //3班次 8:00 ~ 16:00  16:00~00：00 00:00~08:00
            DateTime time = Convert.ToDateTime(SDate.ToString("yyyy-MM-dd") + " 08:00:00");            
            int i = 1;
            foreach (var good in schedulingGoods)
            {
                Pro_PSDetail model = new Pro_PSDetail
                {
                    GoodNo = good.GoodNo,
                    GoodName = good.GoodName,
                    ShipTo = good.ShipTo,
                    ShipToName = good.ShipToName,
                    PackNum = good.PackNum,
                    Qty = good.SNum,
                    ProOrderIndex = i,
                    SType = good.SType,
                    STypeString= EnumOperate.GetEnumDesc((VisualSmart.Util.Menus.ClassType)good.SType)
                };
               
                //产能 根据早中晚判断产能基本参数
                decimal channeng = 0;
                if (model.SType == 2 && good.MorningNum!=null)
                {
                    channeng = good.MorningNum > 0 ? (Convert.ToDecimal(good.SNum) / good.MorningNum.Value) : 0;
                    model.ChanNeng = good.MorningNum.Value;
                }
                else if (model.SType == 3 && good.MiddleNum != null)
                {
                    channeng = good.MiddleNum > 0 ? (Convert.ToDecimal(good.SNum) / good.MiddleNum.Value) : 0;
                    model.ChanNeng = good.MiddleNum.Value;
                }
                else if (model.SType == 4 && good.EveningNum != null)
                {
                    channeng = good.EveningNum > 0 ? (Convert.ToDecimal(good.SNum) / good.EveningNum.Value) : 0;
                    model.ChanNeng = good.EveningNum.Value;
                }
                channeng = channeng * 60 * 60;

                //1班次 8：00~08:00
                model.StartTime = time;
                time = time.AddSeconds(Convert.ToDouble(channeng));
                model.EndTime = time;

                //if (classNum == 1)
                //{
                //    //1班次 8：00~08:00
                //    model.StartTime = time;
                //    time = time.AddSeconds(channeng);
                //    model.EndTime = time;
                //}
                //else if (classNum == 2)
                //{
                //    //2班次 8:00~ 12:45  12:45~08:00
                //    if (model.SType > 2)
                //    {

                //    }
                //    else
                //    {
                //        model.StartTime = time;
                //        time = time.AddSeconds(channeng);
                //        model.EndTime = time;
                //    }
                //}
                //else if (classNum == 3)
                //{
                //    //3班次 8:00 ~ 16:00  16:00~00：00 00:00~08:00
                //    model.StartTime = time;
                //    time = time.AddSeconds(channeng);
                //    model.EndTime = time;
                //}
                psDetail.Add(model);
                i++;
            }

            return psDetail;
        }

        /// <summary>
        /// 获取信息列表 导出到Excel
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Pro_PSDetail> GetAllDomainToExcel(QueryCondition query, Hashtable hs)
        {
            return EntityDao.GetAllDomainToExcel(query,hs);
        }
    }
}
