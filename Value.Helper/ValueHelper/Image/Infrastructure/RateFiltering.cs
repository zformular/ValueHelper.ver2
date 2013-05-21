using System;

namespace ValueHelper.Image.Infrastructure
{
    /// <summary>
    ///  各滤波形式的半径大小
    ///  低通、带阻、带通、高通
    /// </summary>
    public static class RateFilterRadius
    {
        /// <summary>
        ///  低通滤波掩码半径
        /// </summary>
        public static Byte LowPass = 10;
        /// <summary>
        ///  带阻掩码外园半径
        /// </summary>
        public static Byte BandStopOuter = 50;
        /// <summary>
        ///  带阻掩码内圆半径
        /// </summary>
        public static Byte BandStopInner = 15;
        /// <summary>
        ///  带通掩码外圆半径
        /// </summary>
        public static Byte BandPassOuter = 60;
        /// <summary>
        ///  带通掩码内圆半径
        /// </summary>
        public static Byte BandPassInner = 18;
        /// <summary>
        ///  高通半径
        /// </summary>
        public static Byte HighPass = 20;
    }

    /// <summary>
    ///  频率滤波类型
    /// </summary>
    public enum RateFilterType
    {
        // 低通滤波
        LowPass,
        // 带阻滤波
        BandStop,
        // 带通滤波
        BandPass,
        // 高通滤波
        HighPass
    }
}
