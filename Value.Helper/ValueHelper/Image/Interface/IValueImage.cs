using System;
using System.Drawing;
using ValueHelper.Math.Infrastructure;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Interface
{
    public interface IValueImage : IDislodgeNoise, IFilter, INoise, IFrequency, IEdge
    {
        /// <summary>
        ///  灰度化
        /// </summary>
        /// <param name="type">灰度化类型</param>
        void ConvertToGrayscale(Bitmap srcImage, GrayscaleType type);

        /// <summary>
        ///  加权灰度化
        /// </summary>
        /// <param name="weightR">像素R的权重</param>
        /// <param name="weightG">像素G的权重</param>
        /// <param name="weightB">像素B的权重</param>
        void ConvertToGrayscale(Bitmap srcImage, float weightR, float weightG, float weightB);

        /// <summary>
        ///  灰度拉伸
        /// </summary>
        void GrayscaleStretch(Bitmap srcImage);

        /// <summary>
        ///  灰度拉伸
        /// </summary>
        /// <param name="x1">拐点1横坐标</param>
        /// <param name="y1">拐点1纵坐标</param>
        /// <param name="x2">拐点2横坐标</param>
        /// <param name="y2">拐点2纵坐标</param>
        void GrayscaleStretch(Bitmap srcImage, Int32 x1, Int32 y1, Int32 x2, Int32 y2);

        /// <summary>
        ///  对图像已指定像素按 kx+b 线性变换
        /// </summary>
        /// <param name="slope">斜率</param>
        /// <param name="displacements">平移</param>
        void LinearChange(Bitmap srcImage, float slope, float displacements);

        /// <summary>
        ///  直方图均衡化
        /// </summary>
        void HistEqualization(Bitmap srcImage);

        /// <summary>
        ///  直方图匹配
        /// </summary>
        /// <param name="histogram">要匹配的直方图模板</param>
        void HistMatch(Bitmap srcImage, Int32[] histogram);

        /// <summary>
        ///  反色
        /// </summary>
        void InvertColor(Bitmap srcImage);

        /// <summary>
        ///  反色
        /// </summary>
        void InvertColor(Bitmap srcImage, FrequencyDimension diemnsion);

        /// <summary>
        ///  平移
        /// </summary>
        /// <param name="x">横向位移</param>
        /// <param name="y">纵向位移</param>
        void Move(Bitmap srcImage, Int32 x, Int32 y);

        /// <summary>
        ///  水平镜像
        /// </summary>
        void HoriMirror(Bitmap srcImage);

        /// <summary>
        ///  垂直镜像
        /// </summary>
        void VertMirror(Bitmap srcImage);

        /// <summary>
        ///  缩放图片
        /// </summary>
        /// <param name="zoomX">横向缩放量</param>
        /// <param name="zoomY">纵向缩放量</param>
        void Zoom(Bitmap srcImage, float zoomX, float zoomY, ZoomType type);

        /// <summary>
        ///  旋转
        /// </summary>
        /// <param name="degree">角度</param>
        void Gyrate(Bitmap srcImage, Int32 degree);
    }
}
