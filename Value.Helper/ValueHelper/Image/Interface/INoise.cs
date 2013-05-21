using System;
using System.Drawing;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Interface
{
    /// <summary>
    ///  图像噪声接口
    /// </summary>
    public interface INoise
    {
        /// <summary>
        ///  高斯噪声
        /// </summary>
        void Noise(Bitmap srcImage, NoiseType type);

        /// <summary>
        ///  高斯噪声
        /// </summary>
        void GaussNoise(Bitmap srcImage, Double mean, Double meanDeviation);

        /// <summary>
        ///  瑞利噪声
        /// </summary>
        /// <param name="paramA">参数A</param>
        /// <param name="paramB">参数B</param>
        void RayleighNoise(Bitmap srcImage, Double paramA, Double paramB);

        /// <summary>
        ///  指数噪声
        /// </summary>
        /// <param name="param">参数a(a>0)</param>
        void IndexNoise(Bitmap srcImage, Double param);

        /// <summary>
        ///  椒盐噪声
        /// </summary>
        /// <param name="pepper">椒量</param>
        /// <param name="salt">盐量</param>
        void PepperNoise(Bitmap srcImage, Double pepper, Double salt);
    }
}
