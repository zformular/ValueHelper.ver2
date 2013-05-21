using System;
using System.Drawing;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Interface
{
    /// <summary>
    ///  图像去噪接口
    /// </summary>
    public interface IDislodgeNoise
    {
        /// <summary>
        ///  腐蚀
        /// </summary>
        /// <param name="type">滤窗的格式</param>
        void Erode(Bitmap srcImage, FilterWindowType type);

        /// <summary>
        ///  膨胀
        /// </summary>
        /// <param name="type">滤窗的格式</param>
        void Delation(Bitmap srcImage, FilterWindowType type);

        /// <summary>
        ///  开运算
        /// </summary>
        /// <param name="type">滤窗的格式</param>
        void Open(Bitmap srcImage, FilterWindowType type);

        /// <summary>
        ///  闭运算
        /// </summary>
        /// <param name="type">滤窗的格式</param>
        void Close(Bitmap srcImage, FilterWindowType type);

        /// <summary>
        ///  灰度形态学膨胀
        /// </summary>
        /// <param name="template">模板集合</param>
        void GrayDelation(Bitmap srcImage, Byte[] template);

        /// <summary>
        ///  灰度形态学腐蚀
        /// </summary>
        /// <param name="template">模板集合</param>
        void GrayErode(Bitmap srcImage, Byte[] template);

        /// <summary>
        ///  灰度形态学开运算
        /// </summary>
        /// <param name="template">模板集合</param>
        void GrayOpen(Bitmap srcImage, Byte[] template);

        /// <summary>
        ///  灰度形态学闭运算
        /// </summary>
        /// <param name="template">模板集合</param>
        void GrayClose(Bitmap srcImage, Byte[] template);

        /// <summary>
        ///  灰度形态学
        /// </summary>
        /// <param name="template">模板集合</param>
        void GrayMorphologic(Bitmap srcImage, Byte[] template);
    }
}
