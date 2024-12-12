using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class SellerDashboardController : Controller
    {
        Nhom17_WedBanSachEntities objGr17_WedBanSachEntities = new Nhom17_WedBanSachEntities();

        public ActionResult SachList()
        {
            var sach = objGr17_WedBanSachEntities.Saches.ToList();
            return View(sach);
        }

        public ActionResult QLDonHang()
        {
            var dh = objGr17_WedBanSachEntities.DonHangs.ToList();
            return View(dh);
        }
        public ActionResult DetailsDH(string id)
        {
            // Lấy tất cả chi tiết đơn hàng theo mã đơn hàng
            var chiTietdh = objGr17_WedBanSachEntities.ChiTietDonHangs
                                                     .Where(h => h.MaDonHang == id)
                                                     .ToList();
            return View(chiTietdh);
        }


        public ActionResult SearchSach(string query)
        {
            // Kiểm tra nếu query không rỗng
            if (string.IsNullOrEmpty(query))
            {
                return PartialView("Seller/_SachTableRows", new List<Sach>());
            }

            // Đảm bảo không phân biệt chữ hoa chữ thường
            query = query.ToLower();

            // Tìm kiếm trong trường 'TenSach' mà không phân biệt chữ hoa/chữ thường
            var allBooks = objGr17_WedBanSachEntities.Saches
                             .Where(s => s.TenSach.ToLower().Contains(query)) // Tìm kiếm gần đúng trong tên sách
                             .ToList();

            // Trả về kết quả dưới dạng PartialView (chỉ phần nội dung bảng sách)
            return PartialView("Seller/_SachTableRows", allBooks);
        }


        public PartialViewResult ShowAllBooks(string query)
        {
            var allBooks = objGr17_WedBanSachEntities.Saches.ToList();

            // Trả về danh sách sách ở dạng HTML (phần thân bảng)
            return PartialView("Seller/_SachTableRows", allBooks);
        }


        public ActionResult AddSach()
        {
            // Truyền danh sách thể loại sách (nếu cần) để hiển thị trong form
            ViewBag.TheLoaiList = new SelectList(objGr17_WedBanSachEntities.TheLoais, "MaTheLoai", "TenTheLoai");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSach(Sach model, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy mã sách cuối cùng trong cơ sở dữ liệu và tạo mã sách mới
                    string lastMaSach = objGr17_WedBanSachEntities.Saches
                                           .OrderByDescending(s => s.MaSach)
                                           .FirstOrDefault()?.MaSach;

                    int newNumber = 1;
                    string newMaSach = "S001"; // Mặc định bắt đầu từ "S001"

                    if (!string.IsNullOrEmpty(lastMaSach))
                    {
                        string numberPart = lastMaSach.Substring(1); // Lấy phần số (bỏ "S")
                        if (int.TryParse(numberPart, out int lastNumber))
                        {
                            newNumber = lastNumber + 1; // Tăng số cuối lên
                        }
                    }

                    // Tạo mã sách mới
                    newMaSach = "S" + newNumber.ToString("D3");

                    // Kiểm tra và đảm bảo mã sách không bị trùng
                    while (objGr17_WedBanSachEntities.Saches.Any(s => s.MaSach == newMaSach))
                    {
                        newNumber++;
                        newMaSach = "S" + newNumber.ToString("D3");
                    }

                    // Xử lý ảnh bìa
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Content/Images/"), fileName); // Đường dẫn lưu ảnh
                        imageFile.SaveAs(filePath); // Lưu ảnh vào thư mục trên server
                        model.URLAnhBia = fileName; // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                    }

                    // Gán mã sách mới và thêm vào cơ sở dữ liệu
                    model.MaSach = newMaSach;
                    model.NgayTao = DateTime.Now;
                    objGr17_WedBanSachEntities.Saches.Add(model);
                    objGr17_WedBanSachEntities.SaveChanges();

                    // Chuyển hướng về trang danh sách sách
                    //return RedirectToAction("SachList", "SellerDashboard");
                }
                catch (Exception ex)
                {
                    // Ghi nhận lỗi và hiển thị thông báo
                    Console.WriteLine("Error: " + ex.Message); // Log lỗi ra console hoặc log file
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }

            // Truyền lại danh sách thể loại để hiển thị dropdown
            ViewBag.TheLoaiList = new SelectList(objGr17_WedBanSachEntities.TheLoais, "MaTheLoai", "TenTheLoai", model.MaTheLoai);
            return View(model);
        }



        public ActionResult DetailsSach(string id)
        {
            // Lấy chi tiết sách theo mã sách
            var chiTietSach = objGr17_WedBanSachEntities.Saches.FirstOrDefault(s => s.MaSach == id);

            if (chiTietSach != null)
            {
                // Lấy thể loại của sách
                chiTietSach.TheLoai = objGr17_WedBanSachEntities.TheLoais
                    .FirstOrDefault(t => t.MaTheLoai == chiTietSach.MaTheLoai);
            }

            return View(chiTietSach);
        }


        public ActionResult EditSach(string id)
        {
            // Lấy thông tin sách cần chỉnh sửa từ cơ sở dữ liệu
            var sachedit = objGr17_WedBanSachEntities.Saches.FirstOrDefault(s => s.MaSach == id);

            // Nếu không tìm thấy sách theo mã, trả về lỗi NotFound
            if (sachedit == null)
            {
                return HttpNotFound();
            }

            // Truyền danh sách thể loại vào ViewBag
            ViewBag.TheLoaiList = new SelectList(objGr17_WedBanSachEntities.TheLoais, "MaTheLoai", "TenTheLoai", sachedit.MaTheLoai);

            // Trả về view với dữ liệu sách đã lấy và danh sách thể loại
            return View(sachedit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSach(Sach model, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sachedit = objGr17_WedBanSachEntities.Saches.FirstOrDefault(s => s.MaSach == model.MaSach);
                    if (sachedit == null)
                    {
                        return HttpNotFound();
                    }

                    // Cập nhật thông tin sách
                    sachedit.TenSach = model.TenSach;
                    sachedit.TacGia = model.TacGia;
                    sachedit.NhaXuatBan = model.NhaXuatBan;
                    sachedit.NamXuatBan = model.NamXuatBan;
                    sachedit.Gia = model.Gia;
                    sachedit.SoLuongTon = model.SoLuongTon;
                    sachedit.MaTheLoai = model.MaTheLoai; // Cập nhật mã thể loại từ dropdown
                    sachedit.MoTa = model.MoTa;

                    // Cập nhật thời gian chỉnh sửa (NgayTao)
                    sachedit.NgayTao = DateTime.Now; // Cập nhật thời gian hiện tại vào NgayTao

                    // Xử lý upload ảnh bìa nếu có
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(sachedit.URLAnhBia))
                        {
                            var oldImagePath = Path.Combine(Server.MapPath("~/Content/Images"), sachedit.URLAnhBia);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath); // Xóa ảnh cũ
                            }
                        }

                        // Lưu ảnh mới vào thư mục Images
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        imageFile.SaveAs(path);

                        // Cập nhật tên ảnh vào database
                        sachedit.URLAnhBia = fileName;
                    }

                    // Lưu vào cơ sở dữ liệu
                    objGr17_WedBanSachEntities.SaveChanges();

                    TempData["SuccessMessage"] = "Chỉnh sửa thông tin sách thành công!";  // Lưu thông báo thành công
                    return RedirectToAction("SachList");
                }
                else
                {
                    // Nếu có lỗi, trả về danh sách thể loại và tiếp tục hiển thị form
                    ViewBag.TheLoaiList = new SelectList(objGr17_WedBanSachEntities.TheLoais, "MaTheLoai", "TenTheLoai", model.MaTheLoai);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View("Error");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("SachList");
            }

            var sachDelete = objGr17_WedBanSachEntities.Saches.FirstOrDefault(s => s.MaSach == id);

            if (sachDelete != null)
            {
                try
                {
                    objGr17_WedBanSachEntities.Saches.Remove(sachDelete);
                    objGr17_WedBanSachEntities.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "Đã có lỗi xảy ra khi xóa sách. Vui lòng thử lại.";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Sách không tồn tại.";
                return View("Error");
            }

            return RedirectToAction("SachList");
        }


    }
}
