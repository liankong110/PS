using System.Web.Mvc;
using VisualSmart.BizService.Core;
using VisualSmart.Util;

namespace PS.Controllers.ProBase
{
    public class ProductionLineController : Controller
    {
        [ViewPageAttribute]
        public ActionResult Index(int page = 1)
        {
            var query = QueryCondition.Instance.AddOrderBy("Id", false).SetPager(page, 10);
            query.AddEqual("RowState", "1");
            ViewBag.Page = query.GetPager();
            var list = Smart.Instance.Base_ProductionLineBizService.GetAllDomain(query);
            return View(list);
        }
    }
}