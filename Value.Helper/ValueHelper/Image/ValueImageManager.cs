using System;
using System.Drawing.Imaging;
using ValueHelper.Image.Interface;

namespace ValueHelper.Image
{
    public class ValueImageManager
    {
        public static IValueImage GetImageHelper(PixelFormat format)
        {
            return ValueImage.GetImageHelper(format);
        }
    }
}
