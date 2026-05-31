using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Helpers
{
    public static class ImageHelper
    {
        // Hàm quét ổ cứng tìm ảnh theo Mã Sản Phẩm
        public static string GetProductImagePath(IWebHostEnvironment env, string maSp)
        {
            // Cấu hình các đuôi ảnh sếp muốn hỗ trợ
            string[] extensions = { ".jpg", ".png", ".jpeg", ".webp", ".gif" };

            // Cấu hình các thư mục sếp đã tạo
            string[] folders = { "thuoc", "vattu" };

            if (env == null || string.IsNullOrWhiteSpace(maSp)) return null;

            // Lặp để quét tìm file
            foreach (var folder in folders)
            {
                foreach (var ext in extensions)
                {
                    string relativePath = $"/images/{folder}/{maSp}{ext}"; // Đường dẫn web (VD: /images/thuoc/SP001.png)
                    string absolutePath = Path.Combine(env.WebRootPath, "images", folder, maSp + ext); // Đường dẫn vật lý trên ổ cứng

                    // Nếu C# thấy file thực sự tồn tại trên ổ cứng -> Trả về đường dẫn ngay lập tức
                    if (File.Exists(absolutePath))
                    {
                        return relativePath;
                    }
                }
            }

            // Quét sạch sành sanh mà không có file nào -> Trả về null
            return null;
        }
    }
}