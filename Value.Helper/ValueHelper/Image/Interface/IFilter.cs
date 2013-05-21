using System;
using System.Drawing;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Interface
{
    /// <summary>
    ///  图像过滤接口
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        ///  成分滤波
        /// </summary>
        void ComponentFilter(Bitmap srcImage, RateFilterType type);

        /// <summary>
        ///  方位滤波
        /// </summary>
        /// <param name="startOrient">起始方位</param>
        /// <param name="endOrient">终止方位</param>
        void OrientationFilter(Bitmap srcImage, Int32 startOrient, Int32 endOrient);

        /// <summary>
        ///  均值滤波
        /// </summary>
        /// <param name="model">模板大小类型</param>
        void MeanFilter(Bitmap srcImage, TemplateModel model);

        /// <summary>
        ///  中值滤波
        /// </summary>
        /// <param name="model">模板大小类型</param>
        void MedianFilter(Bitmap srcImage, TemplateModel model);

        /// <summary>
        ///  二维小波变化
        /// </summary>
        /// <param name="lowpassType">低通滤波类型</param>
        /// <param name="hardThreshold">是否为硬阀值</param>
        /// <param name="thresholding">阀值</param>
        /// <param name="series">关于小波变换级数的参数</param>
        void Wavelet(Bitmap srcImage, WaveletLowpassType lowpassType, Boolean hardThreshold, Byte thresholding, Int32 series);

        /// <summary>
        ///  高斯滤波
        /// </summary>
        /// <param name="sigma">方差</param>
        void GaussFilter(Bitmap srcImage, Double sigma);

        /// <summary>
        ///  统计方法滤波
        /// </summary>
        /// <param name="thresholding">阀值</param>
        void StatisticFilter(Bitmap srcImage, TemplateModel model, Double thresholding);
    }
}
