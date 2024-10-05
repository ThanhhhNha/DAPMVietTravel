using System.Linq;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class ToursController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        [HttpGet]
        public JsonResult GetProvinces(string searchTerm)
        {
            var provinces = db.TinhThanhs
                              .Where(p => p.TenTinh.Contains(searchTerm))
                              .Select(p => new
                              {
                                  MaTinh = p.MaTinh,
                                  Name = p.TenTinh
                              })
                              .ToList();

            return Json(provinces, JsonRequestBehavior.AllowGet);
        }
    }
}
