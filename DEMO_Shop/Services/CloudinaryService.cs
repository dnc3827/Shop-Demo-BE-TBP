using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DEMO_Shop.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            // Lấy biến môi trường từ Railway (bạn đã lấy từ Dashboard Cloudinary trước đó)
            var url = Environment.GetEnvironmentVariable("CLOUDINARY_URL");

            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("Chưa cấu hình CLOUDINARY_URL trong Environment Variables!");
            }

            _cloudinary = new Cloudinary(url);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "shop_demo_products", // Tên thư mục sẽ tạo trên Cloudinary
                Transformation = new Transformation().Width(800).Height(800).Crop("limit") // Tự động tối ưu ảnh
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            // Trả về URL HTTPS của ảnh
            return uploadResult.SecureUrl.ToString();
        }
    }
}
