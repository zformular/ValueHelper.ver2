using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.Generic;
using ValueHelper.Image.Interface;
using ValueHelper.Math.Infrastructure;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Bit24
{
    public partial class ImageBit24 : ValueImage, IValueImage
    {
        #region 构造器

        private ImageBit24() { }
        private static ImageBit24 instance;
        public static new ImageBit24 GetInstance()
        {
            if (instance == null)
                instance = new ImageBit24();
            return instance;
        }

        #endregion

        #region 模板取值索引

        /// <summary>
        ///  获得2x2索引 当前元素为左下角
        /// </summary>
        protected override Int32[] getRectangle2x2(Int32 index, Int32 width)
        {
            Int32 row = index / width;
            Int32 col = index % width;
            Int32[] set = new Int32[12];
            #region Row1
            set[0] = (row - 1) * width + col;
            set[1] = (row - 1) * width + col + 1;
            set[2] = (row - 1) * width + col + 2;
            set[3] = (row - 1) * width + col + 3;
            set[4] = (row - 1) * width + col + 4;
            set[5] = (row - 1) * width + col + 5;
            #endregion
            #region Row2
            set[6] = index;
            set[7] = index + 1;
            set[8] = index + 2;
            set[9] = index + 3;
            set[10] = index + 4;
            set[11] = index + 5;
            #endregion
            return set;
        }

        protected override Int32[] getRectangle3x3(Int32 index, Int32 width)
        {
            Int32 row = index / width;
            Int32 col = index % width;
            Int32[] set = new Int32[27];
            #region Row1
            set[0] = (row - 1) * width + col - 3;
            set[1] = (row - 1) * width + col - 2;
            set[2] = (row - 1) * width + col - 1;
            set[3] = (row - 1) * width + col;
            set[4] = (row - 1) * width + col + 1;
            set[5] = (row - 1) * width + col + 2;
            set[6] = (row - 1) * width + col + 3;
            set[7] = (row - 1) * width + col + 4;
            set[8] = (row - 1) * width + col + 5;
            #endregion
            #region Row2
            set[9] = index - 3;
            set[10] = index - 2;
            set[11] = index - 1;
            set[12] = index;
            set[13] = index + 1;
            set[14] = index + 2;
            set[15] = index + 3;
            set[16] = index + 4;
            set[17] = index + 5;
            #endregion
            #region Row3
            set[18] = (row + 1) * width + col - 3;
            set[19] = (row + 1) * width + col - 2;
            set[20] = (row + 1) * width + col - 1;
            set[21] = (row + 1) * width + col;
            set[22] = (row + 1) * width + col + 1;
            set[23] = (row + 1) * width + col + 2;
            set[24] = (row + 1) * width + col + 3;
            set[25] = (row + 1) * width + col + 4;
            set[26] = (row + 1) * width + col + 5;
            #endregion
            return set;
        }

        protected override Int32[] getRectangle5x5(Int32 index, Int32 width)
        {
            Int32 row = index / width;
            Int32 col = index % width;
            Int32[] set = new Int32[75];
            #region Row1
            set[0] = (row - 2) * width + col - 6;
            set[1] = (row - 2) * width + col - 5;
            set[2] = (row - 2) * width + col - 4;
            set[3] = (row - 2) * width + col - 3;
            set[4] = (row - 2) * width + col - 2;
            set[5] = (row - 2) * width + col - 1;
            set[6] = (row - 2) * width + col;
            set[7] = (row - 2) * width + col + 1;
            set[8] = (row - 2) * width + col + 2;
            set[9] = (row - 2) * width + col + 3;
            set[10] = (row - 2) * width + col + 4;
            set[11] = (row - 2) * width + col + 5;
            set[12] = (row - 2) * width + col + 6;
            set[13] = (row - 2) * width + col + 7;
            set[14] = (row - 2) * width + col + 8;
            #endregion
            #region Row2
            set[15] = (row - 1) * width + col - 6;
            set[16] = (row - 1) * width + col - 5;
            set[17] = (row - 1) * width + col - 4;
            set[18] = (row - 1) * width + col - 3;
            set[19] = (row - 1) * width + col - 2;
            set[20] = (row - 1) * width + col - 1;
            set[21] = (row - 1) * width + col;
            set[22] = (row - 1) * width + col + 1;
            set[23] = (row - 1) * width + col + 2;
            set[24] = (row - 1) * width + col + 3;
            set[25] = (row - 1) * width + col + 4;
            set[26] = (row - 1) * width + col + 5;
            set[27] = (row - 1) * width + col + 6;
            set[28] = (row - 1) * width + col + 7;
            set[29] = (row - 1) * width + col + 8;
            #endregion
            #region Row3
            set[30] = index - 6;
            set[31] = index - 5;
            set[32] = index - 4;
            set[33] = index - 3;
            set[34] = index - 2;
            set[35] = index - 1;
            set[36] = index;
            set[37] = index + 1;
            set[38] = index + 2;
            set[39] = index + 3;
            set[40] = index + 4;
            set[41] = index + 5;
            set[42] = index + 6;
            set[43] = index + 7;
            set[44] = index + 8;
            #endregion
            #region Row4
            set[45] = (row + 1) * width + col - 6;
            set[46] = (row + 1) * width + col - 5;
            set[47] = (row + 1) * width + col - 4;
            set[48] = (row + 1) * width + col - 3;
            set[49] = (row + 1) * width + col - 2;
            set[50] = (row + 1) * width + col - 1;
            set[51] = (row + 1) * width + col;
            set[52] = (row + 1) * width + col + 1;
            set[53] = (row + 1) * width + col + 2;
            set[54] = (row + 1) * width + col + 3;
            set[55] = (row + 1) * width + col + 4;
            set[56] = (row + 1) * width + col + 5;
            set[57] = (row + 1) * width + col + 6;
            set[58] = (row + 1) * width + col + 7;
            set[59] = (row + 1) * width + col + 8;
            #endregion
            #region Row5
            set[60] = (row + 2) * width + col - 6;
            set[61] = (row + 2) * width + col - 5;
            set[62] = (row + 2) * width + col - 4;
            set[63] = (row + 2) * width + col - 3;
            set[64] = (row + 2) * width + col - 2;
            set[65] = (row + 2) * width + col - 1;
            set[66] = (row + 2) * width + col;
            set[67] = (row + 2) * width + col + 1;
            set[68] = (row + 2) * width + col + 2;
            set[69] = (row + 2) * width + col + 3;
            set[70] = (row + 2) * width + col + 4;
            set[71] = (row + 2) * width + col + 5;
            set[72] = (row + 2) * width + col + 6;
            set[73] = (row + 2) * width + col + 7;
            set[74] = (row + 2) * width + col + 8;
            #endregion
            return set;
        }

        protected override Int32[] getRectangle7x7(Int32 index, Int32 width)
        {
            Int32 row = index / width;
            Int32 col = index % width;
            Int32[] set = new Int32[147];
            #region Row1
            set[0] = (row - 3) * width + col - 9;
            set[1] = (row - 3) * width + col - 8;
            set[2] = (row - 3) * width + col - 7;
            set[3] = (row - 3) * width + col - 6;
            set[4] = (row - 3) * width + col - 5;
            set[5] = (row - 3) * width + col - 4;
            set[6] = (row - 3) * width + col - 3;
            set[7] = (row - 3) * width + col - 2;
            set[8] = (row - 3) * width + col - 1;
            set[9] = (row - 3) * width + col;
            set[10] = (row - 3) * width + col + 1;
            set[11] = (row - 3) * width + col + 2;
            set[12] = (row - 3) * width + col + 3;
            set[13] = (row - 3) * width + col + 4;
            set[14] = (row - 3) * width + col + 5;
            set[15] = (row - 3) * width + col + 6;
            set[16] = (row - 3) * width + col + 7;
            set[17] = (row - 3) * width + col + 8;
            set[18] = (row - 3) * width + col + 9;
            set[19] = (row - 3) * width + col + 10;
            set[20] = (row - 3) * width + col + 11;
            #endregion
            #region Row2
            set[21] = (row - 2) * width + col - 9;
            set[22] = (row - 2) * width + col - 8;
            set[23] = (row - 2) * width + col - 7;
            set[24] = (row - 2) * width + col - 6;
            set[25] = (row - 2) * width + col - 5;
            set[26] = (row - 2) * width + col - 4;
            set[27] = (row - 2) * width + col - 3;
            set[28] = (row - 2) * width + col - 2;
            set[29] = (row - 2) * width + col - 1;
            set[30] = (row - 2) * width + col;
            set[31] = (row - 2) * width + col + 1;
            set[32] = (row - 2) * width + col + 2;
            set[33] = (row - 2) * width + col + 3;
            set[34] = (row - 2) * width + col + 4;
            set[35] = (row - 2) * width + col + 5;
            set[36] = (row - 2) * width + col + 6;
            set[37] = (row - 2) * width + col + 7;
            set[38] = (row - 2) * width + col + 8;
            set[39] = (row - 2) * width + col + 9;
            set[40] = (row - 2) * width + col + 10;
            set[41] = (row - 2) * width + col + 11;
            #endregion
            #region Row3
            set[42] = (row - 1) * width + col - 9;
            set[43] = (row - 1) * width + col - 8;
            set[44] = (row - 1) * width + col - 7;
            set[45] = (row - 1) * width + col - 6;
            set[46] = (row - 1) * width + col - 5;
            set[47] = (row - 1) * width + col - 4;
            set[48] = (row - 1) * width + col - 3;
            set[49] = (row - 1) * width + col - 2;
            set[50] = (row - 1) * width + col - 1;
            set[51] = (row - 1) * width + col;
            set[52] = (row - 1) * width + col + 1;
            set[53] = (row - 1) * width + col + 2;
            set[54] = (row - 1) * width + col + 3;
            set[55] = (row - 1) * width + col + 4;
            set[56] = (row - 1) * width + col + 5;
            set[57] = (row - 1) * width + col + 6;
            set[58] = (row - 1) * width + col + 7;
            set[59] = (row - 1) * width + col + 8;
            set[60] = (row - 1) * width + col + 9;
            set[61] = (row - 1) * width + col + 10;
            set[62] = (row - 1) * width + col + 11;
            #endregion
            #region Row4
            set[63] = index - 9;
            set[64] = index - 8;
            set[65] = index - 7;
            set[66] = index - 6;
            set[67] = index - 5;
            set[68] = index - 4;
            set[69] = index - 3;
            set[70] = index - 2;
            set[71] = index - 1;
            set[72] = index;
            set[73] = index + 1;
            set[74] = index + 2;
            set[75] = index + 3;
            set[76] = index + 4;
            set[77] = index + 5;
            set[78] = index + 6;
            set[79] = index + 7;
            set[80] = index + 8;
            set[81] = index + 9;
            set[82] = index + 10;
            set[83] = index + 11;
            #endregion
            #region Row5
            set[84] = (row + 1) * width + col - 9;
            set[85] = (row + 1) * width + col - 8;
            set[86] = (row + 1) * width + col - 7;
            set[87] = (row + 1) * width + col - 6;
            set[88] = (row + 1) * width + col - 5;
            set[89] = (row + 1) * width + col - 4;
            set[90] = (row + 1) * width + col - 3;
            set[91] = (row + 1) * width + col - 2;
            set[92] = (row + 1) * width + col - 1;
            set[93] = (row + 1) * width + col;
            set[94] = (row + 1) * width + col + 1;
            set[95] = (row + 1) * width + col + 2;
            set[96] = (row + 1) * width + col + 3;
            set[97] = (row + 1) * width + col + 4;
            set[98] = (row + 1) * width + col + 5;
            set[99] = (row + 1) * width + col + 6;
            set[100] = (row + 1) * width + col + 7;
            set[101] = (row + 1) * width + col + 8;
            set[102] = (row + 1) * width + col + 9;
            set[103] = (row + 1) * width + col + 10;
            set[104] = (row + 1) * width + col + 11;
            #endregion
            #region Row6
            set[105] = (row + 2) * width + col - 9;
            set[106] = (row + 2) * width + col - 8;
            set[107] = (row + 2) * width + col - 7;
            set[108] = (row + 2) * width + col - 6;
            set[109] = (row + 2) * width + col - 5;
            set[110] = (row + 2) * width + col - 4;
            set[111] = (row + 2) * width + col - 3;
            set[112] = (row + 2) * width + col - 2;
            set[113] = (row + 2) * width + col - 1;
            set[114] = (row + 2) * width + col;
            set[115] = (row + 2) * width + col + 1;
            set[116] = (row + 2) * width + col + 2;
            set[117] = (row + 2) * width + col + 3;
            set[118] = (row + 2) * width + col + 4;
            set[119] = (row + 2) * width + col + 5;
            set[120] = (row + 2) * width + col + 6;
            set[121] = (row + 2) * width + col + 7;
            set[122] = (row + 2) * width + col + 8;
            set[123] = (row + 2) * width + col + 9;
            set[124] = (row + 2) * width + col + 10;
            set[125] = (row + 2) * width + col + 11;
            #endregion
            #region Row7
            set[126] = (row + 3) * width + col - 9;
            set[127] = (row + 3) * width + col - 8;
            set[128] = (row + 3) * width + col - 7;
            set[129] = (row + 3) * width + col - 6;
            set[130] = (row + 3) * width + col - 5;
            set[131] = (row + 3) * width + col - 4;
            set[132] = (row + 3) * width + col - 3;
            set[133] = (row + 3) * width + col - 2;
            set[134] = (row + 3) * width + col - 1;
            set[135] = (row + 3) * width + col;
            set[136] = (row + 3) * width + col + 1;
            set[137] = (row + 3) * width + col + 2;
            set[138] = (row + 3) * width + col + 3;
            set[139] = (row + 3) * width + col + 4;
            set[140] = (row + 3) * width + col + 5;
            set[141] = (row + 3) * width + col + 6;
            set[142] = (row + 3) * width + col + 7;
            set[143] = (row + 3) * width + col + 8;
            set[144] = (row + 3) * width + col + 9;
            set[145] = (row + 3) * width + col + 10;
            set[146] = (row + 3) * width + col + 11;
            #endregion
            return set;
        }

        #endregion

        #region 内存法处理图像

        protected override byte[] LockBits(Bitmap srcImage, ImageLockMode mode)
        {
            Debug.Assert(srcImage.PixelFormat == PixelFormat.Format24bppRgb, "图片必须为24位像素,才能调用该类方法");
            if (srcImage.PixelFormat != PixelFormat.Format24bppRgb) throw new InvalidOperationException();

            RealWidth = srcImage.Width * 3;
            return base.LockBits(srcImage, mode);
        }

        #endregion

        #region 灰度图

        /// <summary>
        ///  转化为灰度图
        /// </summary>
        public void ConvertToGrayscale(Bitmap srcImage, GrayscaleType type)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            var g = 0;
            // 灰度化
            for (int i = 0; i < rgbBytes.Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                switch (type)
                {
                    case GrayscaleType.Maximum:
                        g = mathHelper.Max(rgbBytes[i + 2], rgbBytes[i + 1], rgbBytes[i]);
                        break;
                    case GrayscaleType.Minimal:
                        g = mathHelper.Min(rgbBytes[i + 2], rgbBytes[i + 1], rgbBytes[i]);
                        break;
                    case GrayscaleType.Average:
                        g = mathHelper.Average(rgbBytes[i + 2], rgbBytes[i + 1], rgbBytes[i]);
                        break;
                }

                rgbBytes[i + 2] = rgbBytes[i + 1] = rgbBytes[i] = (Byte)g;
            }
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  转化为灰度图(加权灰度图)
        /// </summary>
        public void ConvertToGrayscale(Bitmap srcImage, float weightR, float weightG, float weightB)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            var g = 0F;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                g = weightR * rgbBytes[i + 2] + weightG * rgbBytes[i + 1] + weightB * rgbBytes[i];
                rgbBytes[i + 2] = rgbBytes[i + 1] = rgbBytes[i] = (Byte)g;
            }
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 线性点运算

        /// <summary>
        ///  对图像已指定像素按 kx+b 线性变换
        /// </summary>
        /// <param name="slope">斜率</param>
        /// <param name="displacements">平移</param>
        public void LinearChange(Bitmap srcImage, float slope, float displacements)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            float gr = 0, gg = 0, gb = 0;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth)
                {
                    // 因为为垂直匹配所以减三,防止跳过一个元素
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                gr = rgbBytes[i + 2] * slope + displacements;
                gg = rgbBytes[i + 1] * slope + displacements;
                gb = rgbBytes[i] * slope + displacements;
                gr = gr > 255 ? 255 : gr < 0 ? 0 : gr;
                gg = gg > 255 ? 255 : gg < 0 ? 0 : gg;
                gb = gb > 255 ? 255 : gb < 0 ? 0 : gb;

                rgbBytes[i + 2] = Convert.ToByte(gr);
                rgbBytes[i + 1] = Convert.ToByte(gg);
                rgbBytes[i] = Convert.ToByte(gb);
            }
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 灰度拉伸

        /// <summary>
        ///  灰度拉伸
        /// </summary>
        /// <param name="srcImage"></param>
        public void GrayscaleStretch(Bitmap srcImage)
        {
            Int32[] frequency = GetFrequency(srcImage);
            Int32 max = mathHelper.MaxIndex(frequency);
            Int32 min = mathHelper.MinIndex(frequency);
            float p = 255.0F / (max - min);

            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Int32 gr = 0, gg = 0, gb = 0;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }
                // 灰度拉伸
                gr = (Int32)(p * (rgbBytes[i + 2] - min) + 0.5);
                gg = (Int32)(p * (rgbBytes[i + 1] - min) + 0.5);
                gb = (Int32)(p * (rgbBytes[i] - min) + 0.5);
                rgbBytes[i + 2] = (Byte)gr;
                rgbBytes[i + 1] = (Byte)gg;
                rgbBytes[i] = (Byte)gb;
            }
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  灰度拉伸
        /// </summary>
        /// <param name="x1">拐点1横坐标</param>
        /// <param name="y1">拐点1纵坐标</param>
        /// <param name="x2">拐点2横坐标</param>
        /// <param name="y2">拐点2纵坐标</param>
        public void GrayscaleStretch(Bitmap srcImage, int x1, int y1, int x2, int y2)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Int32 gr = 0, gg = 0, gb = 0;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                gr = grayscaleStretchCalc(rgbBytes[i + 2], x1, y1, x2, y2);
                gg = grayscaleStretchCalc(rgbBytes[i + 1], x1, y1, x2, y2);
                gb = grayscaleStretchCalc(rgbBytes[i], x1, y1, x2, y2);

                rgbBytes[i + 2] = (Byte)gr;
                rgbBytes[i + 1] = (Byte)gg;
                rgbBytes[i] = (Byte)gb;
            }

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  灰度拉伸拉伸点算法
        /// </summary>
        private Int32 grayscaleStretchCalc(Int32 x, Int32 x1, Int32 y1, Int32 x2, Int32 y2)
        {
            var g = 0;
            if (x < x1)
                g = (Int32)((y2 / x1) * x);
            else if (x >= x1 && x <= x2)
                g = (Int32)(((y2 - y1) / (x2 - x1)) * (x - x1) + y1);
            else if (x > x2)
                g = (Int32)(((255 - y2) / (255 - x2)) * (x - x2) + y2);
            return g;
        }

        #endregion

        #region 直方图

        /// <summary>
        ///  直方图均衡化
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        public void HistEqualization(Bitmap srcImage)
        {
            Int32[] frequency = GetFrequency(srcImage);

            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            Byte temp;
            var tempArray = new Int32[256];
            // 映射的像素集
            var pixelMap = new Byte[256];

            // 生成累计归一化直方图
            // 并生成映射表
            for (int i = 0; i < 256; i++)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                if (i != 0)
                {
                    tempArray[i] = tempArray[i - 1] + frequency[i];
                }
                else
                    tempArray[0] = frequency[0];
                pixelMap[i] = (Byte)(255.0 * tempArray[i] / Length + 0.5);
            }

            for (int i = 0; i < Length; i++)
            {
                temp = rgbBytes[i];
                rgbBytes[i] = pixelMap[temp];
            }
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  直方图匹配
        /// </summary>
        public void HistMatch(Bitmap srcImage, Int32[] histogram)
        {
            // 获得源图像直方图
            Int32[] frequency = this.GetFrequency(srcImage);
            Int32 maxPixel = mathHelper.MaxIndex(frequency);
            Int32 length = frequency.Length;

            // 内存法操作图像
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            // 计算该直方图各灰度的累计分布函数
            Double[] Hc = new Double[length];
            Hc[0] = frequency[0];
            for (int i = 1; i < length; i++)
            {
                Hc[i] = (Hc[i - 1] + frequency[i]) / (Double)Length;
            }

            // 直方图匹配算法
            Double diffA = 0D, diffB = 0D;
            Int32 k = 0;
            Byte[] mapPixel = new Byte[length];
            for (int i = 0; i < length; i++)
            {
                diffB = 1;
                for (int j = 0; j < length; j++)
                {
                    diffA = System.Math.Abs(Hc[i] - histogram[j]);

                    //                 1.0乘以10的-8次方
                    // 找到2个累计分布函数最相似的位置
                    if (diffA - diffB < 1.0E-08)
                    {
                        // 记下差值
                        diffB = diffA;
                        k = j;
                    }
                    else
                    {
                        // 已找到为相似位置,记录并退出
                        k = j - 1;
                        break;
                    }
                }

                // 如果达到最大灰度级,标志未处理灰度数,并推出循环
                if (k == 255)
                {
                    for (int l = 0; l < length; l++)
                    {
                        mapPixel[l] = (Byte)k;
                    }
                    break;
                }
                mapPixel[i] = (Byte)k;
            }

            for (int i = 0; i < rgbBytes.Length; i++)
            {
                rgbBytes[i] = mapPixel[rgbBytes[i]];
            }

            UnlockBits(rgbBytes);
        }

        #endregion

        #region 反色

        /// <summary>
        ///  反色
        /// </summary>
        public void InvertColor(Bitmap srcImage)
        {
            this.InvertColor(srcImage, FrequencyDimension.RGB);
        }

        /// <summary>
        ///  反色
        /// </summary>
        public void InvertColor(Bitmap srcImage, FrequencyDimension diemnsion)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                rgbBytes[i] = (Byte)(255 - rgbBytes[i]);
                rgbBytes[i + 1] = (Byte)(255 - rgbBytes[i + 1]);
                rgbBytes[i + 2] = (Byte)(255 - rgbBytes[i + 2]);
            }
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 平移

        /// <summary>
        ///  将图像移动x,y个单位
        /// </summary>
        /// <param name="x">水平位移</param>
        /// <param name="y">垂直位移</param>
        public void Move(Bitmap srcImage, Int32 x, Int32 y)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[rgbBytes.Length];
            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = 255;
            }

            Int32 extend = Width - RealWidth;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                // 行
                Int32 row = i / Width;
                // 列
                Int32 col = i % Width;

                Int32 nx = col + 3 + x * 3;
                if (nx > RealWidth) continue;
                if (nx < 0) continue;

                Int32 ny = (row + y) * Width;
                if (row + y > Height) break;

                Int32 newPos = nx + ny;
                if (newPos < 0) continue;
                if (newPos >= Length) break;

                tempArray[newPos] = rgbBytes[i];
                tempArray[newPos + 1] = rgbBytes[i + 1];
                tempArray[newPos + 2] = rgbBytes[i + 2];
            }

            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }

        #endregion

        #region 镜像

        /// <summary>
        ///  水平镜像
        /// </summary>
        public void HoriMirror(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Int32 splitPoint = 0;

            // 奇偶取中值的区别
            if (RealWidth / 3 % 2 == 0)
                splitPoint = RealWidth / 6 * 3;
            else
                splitPoint = ((RealWidth / 3 - 1) / 2) * 3;

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }

            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                Int32 row = i / Width;
                Int32 col = i % Width;

                Int32 nx = 2 * splitPoint - col;
                Int32 newPos = row * Width + nx;
                tempArray[newPos] = rgbBytes[i];
                tempArray[newPos + 1] = rgbBytes[i + 1];
                tempArray[newPos + 2] = rgbBytes[i + 2];
                tempArray[i] = rgbBytes[newPos];
                tempArray[i + 1] = rgbBytes[newPos + 1];
                tempArray[i + 2] = rgbBytes[newPos + 2];

                if (col == splitPoint)
                {
                    i = (row + 1) * Width;
                    continue;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  垂直镜像
        /// </summary>
        public void VertMirror(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Int32 splitPoint = 0;

            // 奇偶取中值的区别
            if (Height % 2 == 0)
                splitPoint = Height / 2;
            else
                splitPoint = (Height - 1) / 2;

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }

            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                Int32 row = i / Width;
                Int32 col = i % Width;

                Int32 ny = 2 * splitPoint - row - 1;
                Int32 newPos = ny * Width + col;
                tempArray[newPos] = rgbBytes[i];
                tempArray[newPos + 1] = rgbBytes[i + 1];
                tempArray[newPos + 2] = rgbBytes[i + 2];
                tempArray[i] = rgbBytes[newPos];
                tempArray[i + 1] = rgbBytes[newPos + 1];
                tempArray[i + 2] = rgbBytes[newPos + 2];

                if (ny == splitPoint && col == RealWidth)
                    break;
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 缩放

        /// <summary>
        ///  缩放图片
        /// </summary>
        public void Zoom(Bitmap srcImage, float zoomX, float zoomY, ZoomType type)
        {
            switch (type)
            {
                case ZoomType.NearestInterpolation:
                    ZoomNearest(srcImage, zoomX, zoomY);
                    break;
                case ZoomType.BilinearInterpolation:
                    ZoomBilinear(srcImage, zoomX, zoomY);
                    break;
            }
        }

        /// <summary>
        ///  最近邻插值法
        /// </summary>
        /// <param name="zoomX">横向缩放量</param>
        /// <param name="zoomY">纵向缩放量</param>
        private void ZoomNearest(Bitmap srcImage, float zoomX, float zoomY)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Int32 xz = 0, yz = 0;
            Int32 tempWidth = 0, tempHeight = 0;
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }

            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                Int32 row = i / Width;
                Int32 col = i % Width;

                // 以图像几何中心为原点,进行坐标变换,
                // 按逆时针映射法得到输入图片的坐标
                tempHeight = row - HalfHeight;
                tempWidth = col - HalfWidth;

                // 四舍五入处理
                if (tempWidth > 0)
                    xz = (Int32)(tempWidth / 3 / zoomX + 0.5);
                else
                    xz = (Int32)(tempWidth / 3 / zoomX - 0.5);

                if (tempHeight > 0)
                    yz = (Int32)(tempHeight / zoomY + 0.5);
                else
                    yz = (Int32)(tempHeight / zoomY - 0.5);

                // 坐标逆变换
                tempWidth = xz * 3 + HalfWidth;
                tempHeight = yz + HalfHeight;
                // 得到输出图像像素值
                if (tempWidth < 0 || tempWidth >= Width ||
                    tempHeight < 0 || tempHeight > Height)
                {
                    tempArray[i] = 255;
                    tempArray[i + 1] = 255;
                    tempArray[i + 2] = 255;
                }
                else
                {
                    if (tempHeight * Width + tempWidth + 2 >= Length) break;
                    tempArray[i] = rgbBytes[tempHeight * Width + tempWidth];
                    tempArray[i + 1] = rgbBytes[tempHeight * Width + tempWidth + 1];
                    tempArray[i + 2] = rgbBytes[tempHeight * Width + tempWidth + 2];
                }
            }

            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  双线性插值法
        ///  公式: f(i+p,j+q)=(1-p)*(1-q)*f(i,j)+(1-p)*q*f(i,j+1)+p*(1-q)*f(i+1,j)*p*q*f(i+1,j+1)
        /// </summary>
        /// <param name="zoomX">横向缩放量</param>
        /// <param name="zoomY">纵向缩放量</param>
        private void ZoomBilinear(Bitmap srcImage, float zoomX, float zoomY)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }

            float tempX, tempY, p, q;
            Int32 xz = 0, yz = 0;
            Int32 tempWidth = 0, tempHeight = 0;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                Int32 row = i / Width;
                Int32 col = i % Width;

                tempHeight = row - HalfHeight;
                tempWidth = col - HalfWidth;

                // 计算变换后的坐标(还未乘3)
                tempX = (tempWidth / 3) / zoomX;
                tempY = tempHeight / zoomY;

                if (tempWidth > 0)
                    xz = (Int32)tempX;
                else
                    xz = (Int32)(tempX - 1);

                if (tempHeight > 0)
                    yz = (Int32)tempY;
                else
                    yz = (Int32)(tempY - 1);

                p = tempX - xz;
                q = tempY - yz;

                tempWidth = xz * 3 + HalfWidth;
                tempHeight = yz + HalfHeight;


                if (tempWidth < 0 || (tempWidth + 1) >= Width ||
                    tempHeight < 0 || (tempHeight + 1) >= Height)
                {
                    tempArray[i] = 255;
                    tempArray[i + 1] = 255;
                    tempArray[i + 2] = 255;
                }
                else
                {
                    if ((tempHeight + 1) * Width + tempWidth + 5 > Length) break;

                    tempArray[i] = (Byte)((1.0F - p) * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth] +
                        (1.0F - p) * q * rgbBytes[(tempHeight + 1) * Width + tempWidth] +
                        p * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 3] +
                        p * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 3]);

                    tempArray[i + 1] = (Byte)((1.0F - p) * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 1] +
                        (1.0F - p) * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 1] +
                        p * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 4] +
                        p * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 4]);

                    tempArray[i + 2] = (Byte)((1.0F - p) * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 2] +
                        (1.0F - p) * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 2] +
                        p * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 5] +
                        p * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 5]);
                }
            }

            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 旋转

        /// <summary>
        ///  失败的作品
        ///  用算法旋转会出现噪声
        ///  双线性插值法
        /// </summary>
        public void Gyrate(Bitmap srcImage, int degree)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double radian = degree * System.Math.PI / 180.0;
            Double sin = System.Math.Sin(radian);
            Double cos = System.Math.Cos(radian);

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }

            Int32 xz = 0, yz = 0;
            Int32 tempWidth = 0, tempHeight = 0;
            Double tempX, tempY, p, q;

            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }

                Int32 row = i / Width;
                Int32 col = i % Width;

                tempHeight = row - HalfHeight;
                tempWidth = (col - HalfWidth) / 3;

                // 以图像的几何中心为坐标原点进行坐标变换
                // 按逆向映射法得到输入图像的坐标
                tempX = tempWidth * cos - tempHeight * sin;
                tempY = tempWidth * sin + tempHeight * cos;

                xz = tempWidth > 0 ? ((Int32)tempX) : ((Int32)(tempX - 1));
                yz = tempHeight > 0 ? ((Int32)tempY) : ((Int32)(tempY - 1));

                // 公式需要用到
                p = tempX - xz;
                q = tempY - yz;

                tempWidth = xz * 3 + HalfWidth;
                tempHeight = yz + HalfHeight;

                if (tempWidth < 0 || (tempWidth + 1) >= Width || tempHeight < 0 || (tempHeight + 1) >= Height)
                {
                    tempArray[i] = 255;
                    tempArray[i + 1] = 255;
                    tempArray[i + 2] = 255;
                }
                else
                {
                    tempArray[i] = (Byte)((1.0F - p) * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth] +
                        (1.0F - p) * q * rgbBytes[(tempHeight + 1) * Width + tempWidth] +
                        p * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 3] +
                        p * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 3]);

                    tempArray[i + 1] = (Byte)((1.0F - p) * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 1] +
                       (1.0F - p) * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 1] +
                       p * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 4] +
                       p * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 4]);

                    tempArray[i + 2] = (Byte)((1.0F - p) * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 2] +
                      (1.0F - p) * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 2] +
                      p * (1.0F - q) * rgbBytes[tempHeight * Width + tempWidth + 5] +
                      p * q * rgbBytes[(tempHeight + 1) * Width + tempWidth + 5]);
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        #endregion

        #region IEdge 成员


        public void Canny(Bitmap srcImage, double sigma, byte[] thresholding)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
