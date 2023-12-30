using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models.Process
{
    public class UploadImgProcess
    {
        public async Task<string?> UploadImage(IFormFile file, string pathFolder, string options)
        {
            // Tạo folder nếu chưa tồn tại
            string PathImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", options);
            string strKey = Guid.NewGuid().ToString();
            // Tạo tên file duy nhất
            string uniqueFileName = options + $"_{strKey}" + "_" + file.FileName;

            // Lưu hình ảnh vào folder
            string filePath = Path.Combine(PathImg, uniqueFileName);
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn của hình ảnh để lưu vào cơ sở dữ liệu
            return pathFolder + uniqueFileName;
        }
    }
}