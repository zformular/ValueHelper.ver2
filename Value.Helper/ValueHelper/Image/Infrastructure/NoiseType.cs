using System;
namespace ValueHelper.Image.Infrastructure
{
    /// <summary>
    ///  噪声类型
    /// </summary>
    public enum NoiseType
    {
        /// <summary>
        ///  高斯噪声
        /// </summary>
        Gauss,
        /// <summary>
        ///  瑞利噪声
        /// </summary>
        Rayleigh,
        /// <summary>
        ///  指数噪声
        /// </summary>
        Index,
        /// <summary>
        ///  椒盐噪声
        /// </summary>
        Pepper
    }
}
