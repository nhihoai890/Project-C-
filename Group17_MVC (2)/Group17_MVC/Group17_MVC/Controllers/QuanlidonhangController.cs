using Group17_MVC;
using System.Linq;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class QuanlidonhangController : Controller
    {
        private readonly Nhom17_WedBanSachEntities db = new Nhom17_WedBanSachEntities();

        public ActionResult Index(string searchString)
        {
            var donHangs = string.IsNullOrEmpty(searchString)
                ? db.DonHangs.ToList()
                : db.DonHangs.Where(d => d.MaDonHang.Contains(searchString)).ToList();

            return View(donHangs);
        }

       
        public JsonResult ChiTietDonHang(string maDonHang)
        {
            if (string.IsNullOrEmpty(maDonHang))
            {
                return Json(new { error = "Mã đơn hàng không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }

            var orderDetails = db.ChiTietDonHangs
                                 .Where(c => c.MaDonHang == maDonHang)
                                 .Select(c => new
                                 {
                                     MaDonHang = c.MaDonHang,
                                     NguoiDung = c.DonHang.NguoiDung.HoTen,
                                     ChiTiet = c.DonHang.ChiTietDonHangs.Select(d => new
                                     {
                                         MaSach = d.MaSach,
                                         TenSach = d.Sach.TenSach,
                                         TacGia = d.Sach.TacGia,
                                         SoLuong = d.SoLuong,
                                         GiaBan = d.GiaBan
                                     }).ToList(),
                                     TongTien = c.DonHang.TongTien
                                 })
                                 .FirstOrDefault();

            if (orderDetails == null)
            {
                return Json(new { error = "Không tìm thấy đơn hàng." }, JsonRequestBehavior.AllowGet);
            }

            return Json(orderDetails, JsonRequestBehavior.AllowGet);
        }
    }
}
