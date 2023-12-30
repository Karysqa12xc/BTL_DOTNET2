using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models.Process
{
    public class UploadVideoProcess
    {
        public async Task<string?> UploadVideo(IFormFile file, string pathFolder, string options)
        {
            // Tạo folder nếu chưa tồn tại
            string PathVideo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", options);
            string strKey = Guid.NewGuid().ToString();
            // Tạo tên file duy nhất
            string uniqueFileName = options + $"_{strKey}" + "_" + file.FileName;

            // Lưu hình ảnh vào folder
            string filePath = Path.Combine(PathVideo, uniqueFileName);
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn của hình ảnh để lưu vào cơ sở dữ liệu
            return pathFolder + uniqueFileName;
        }
    }
}