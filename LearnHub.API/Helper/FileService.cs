// for local
//using LearnHub.API.Models.Dto.MaterialDto;

//namespace LearnHub.API.Helper
//{
//    public class FileService
//    {
//        private readonly IWebHostEnvironment _env;

//        public FileService(IWebHostEnvironment env)
//        {
//            _env = env;
//        }

//        public async Task<string> UploadFileAsync(IFormFile file)
//        {

//            if (file == null || file.Length == 0)
//            {
//                throw new Exception("No file uploaded.");
//            }

//            // Create a unique file name
//            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

//            var folder = "Uploads/";

//            // Ensure the upload directory exists
//            var uploadsFolderPath = Path.Combine(_env.WebRootPath, folder);
//            if (!Directory.Exists(uploadsFolderPath))
//            {
//                Directory.CreateDirectory(uploadsFolderPath);
//            }

//            var serverFolder = Path.Combine(uploadsFolderPath, fileName).Replace("\\", "/");

//            await using (var fileStream = new FileStream(serverFolder, FileMode.Create))
//            {
//                await file.CopyToAsync(fileStream);
//            }
//            return serverFolder;
//        }
//        public bool DeleteFile(String filePath)
//        {
//            try
//            {
//                if (System.IO.File.Exists(filePath))
//                {
//                    System.IO.File.Delete(filePath);
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.ToString());
//            }
//        }

//        public string GenerateDownloadLink(string filePath)
//        {

//            var baseUrl = "https://localhost:44365";
//            var relativePath = Path.GetRelativePath(_env.WebRootPath, filePath).Replace("\\", "/");
//            return $"{baseUrl}/{relativePath}";
//        }
//    }
//}



using LearnHub.API.Models.Dto.MaterialDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LearnHub.API.Helper
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            // Create a unique file name
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folder = Path.Combine(_env.ContentRootPath, "Uploads");

            // Ensure the upload directory exists
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var serverFilePath = Path.Combine(folder, fileName);

            await using (var fileStream = new FileStream(serverFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return serverFilePath;
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public string GenerateDownloadLink(string filePath)
        {
            var baseUrl = "http://learnhub.runasp.net/wwwroot"; // Your server URL with the mapped request path
            var relativePath = Path.GetRelativePath("D:/Sites/site6168/wwwroot", filePath).Replace("\\", "/");
            return $"{baseUrl}/{relativePath}";
        }
    }
}
