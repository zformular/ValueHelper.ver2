using System;

namespace ValueHelper.Image.Infrastructure
{
    /// <summary>
    ///  滤波窗口类型
    /// </summary>
    public enum FilterWindowType
    {
        /// <summary>
        ///  3位水平形状
        ///    □■□
        /// </summary>
        Hori3 = 0x11,
        /// <summary>
        ///  3位垂直形状
        ///      □
        ///      ■
        ///      □
        /// </summary>
        Vert3 = 0x12,
        /// <summary>
        ///  3位十字形状 
        ///      □
        ///    □■□
        ///      □
        /// </summary>
        Cros3 = 0x14,
        /// <summary>
        ///  3位方形
        ///   □□□
        ///   □■□
        ///   □□□
        /// </summary>
        Rect3 = 0x18,
        /// <summary>
        ///  5位水平形状   
        ///  □□■□□   
        /// </summary>
        Hori5 = 0x21,
        /// <summary>
        ///  5位垂直形状
        ///      □
        ///      □
        ///      ■
        ///      □
        ///      □
        /// </summary>
        Vert5 = 0x22,
        /// <summary>
        ///  5位十字形状
        ///      □
        ///      □
        ///  □□■□□
        ///      □
        ///      □
        /// </summary>
        Cros5 = 0x24,
        /// <summary>
        ///  5位方形       
        /// □□□□□
        /// □□□□□
        /// □□■□□
        /// □□□□□
        /// □□□□□               
        /// </summary>
        Rect5 = 0x28
    }
}
