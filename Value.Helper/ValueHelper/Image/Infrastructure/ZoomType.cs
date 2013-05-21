using System;

namespace ValueHelper.Image.Infrastructure
{
    public enum ZoomType
    {
        /// <summary>
        ///  最近邻插值法
        /// </summary>
        NearestInterpolation,
        /// <summary>
        ///  双线性插值法
        /// </summary>
        BilinearInterpolation
    }
}
