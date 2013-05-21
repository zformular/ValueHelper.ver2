using System;
using System.Drawing;
using ValueHelper.Math.Infrastructure;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Interface
{
    public interface IFrequency
    {
        /// <summary>
        ///  快速傅里叶变化
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="realWidth">实际宽度</param>
        /// <param name="height">长度</param>
        /// <param name="inv">是否进行坐标位移变换</param>
        /// <returns></returns>
        Complex[] FFT(Byte[] rgbBytes, Int32 width, Int32 realWidth, Int32 height, Boolean inv);

        /// <summary>
        ///  快速傅里叶变化
        /// </summary>
        /// <param name="inv">是否进行坐标位移变换</param>
        /// <returns></returns>
        void FFT(Bitmap srcImage, Boolean inv);

        /// <summary>
        ///  幅度图像
        /// </summary>
        void Amplitude(Bitmap srcImage);

        /// <summary>
        ///  相位图像
        /// </summary>
        void Phase(Bitmap srcImage);
    }
}
