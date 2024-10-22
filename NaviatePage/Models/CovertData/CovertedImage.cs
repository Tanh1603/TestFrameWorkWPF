using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NaviatePage.Models.CovertData
{
    public class CovertedImage
    {
        // Chuyển từ ảnh sang byte để lưu vào cơ sở dữ liệu
        public byte[] ConvertImageToBytes(string imagePath)
        {
            return File.ReadAllBytes(imagePath);
        }

        // Đọc ảnh từ cơ sở dữ liệu
        public byte[] GetImageToString(string imageString)
        {
            return Encoding.UTF8.GetBytes(imageString);
        }

        // Chuyển đổi byte thành BitmapImage
        public BitmapImage ConvertBytesToImage(byte[] imageBytes)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}