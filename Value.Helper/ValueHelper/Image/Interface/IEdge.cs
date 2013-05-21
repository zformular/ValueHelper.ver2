using System;
using System.Drawing;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Interface
{
    public interface IEdge
    {
        /// <summary>
        ///  模板算子计算
        /// </summary>
        /// <param name="thresholding">阈值</param>
        void Mask(Bitmap srcImage, MaskType type, Int32 thresholding);

        /// <summary>
        ///  Roberts算子边缘锐化
        /// </summary>
        /// <param name="thresholding">阈值</param>
        void Roberts(Bitmap srcImage, Int32 thresholding);

        /// <summary>
        ///  Prewitt算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        void Prewitt(Bitmap srcImage, Int32 thresholding);

        /// <summary>
        ///  Sobel算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        void Sobel(Bitmap srcImage, Int32 thresholding);

        /// <summary>
        ///  拉普拉斯算子
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        void Laplacian(Bitmap srcImage, Int32 thresholding, Int32 number);

        /// <summary>
        ///  Kirsch算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        void Kirsch(Bitmap srcImage, Int32 thresholding);

        /// <summary>
        ///  高斯算子锐化
        /// </summary>
        /// <param name="type">高斯过滤类型</param>
        /// <param name="sigma">方差</param>
        /// <param name="thresholding">阈值</param>
        void Gauss(Bitmap srcImage, GaussFilterType type, Double sigma, Double thresholding);

        /// <summary>
        ///  Canny算子
        /// </summary>
        /// <param name="sigma">均方差</param>
        /// <param name="thresholding">阈值</param>
        void Canny(Bitmap srcImage, Double sigma, Byte[] thresholding);
    }
}
