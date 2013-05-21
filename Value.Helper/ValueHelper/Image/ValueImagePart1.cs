using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image
{
    public partial class ValueImage
    {

        /// <summary>
        ///  3x3掩膜算法
        /// </summary>
        public Bitmap Marsk3Operator(Bitmap srcImage, Int32[] operators, FrequencyDimension dimension)
        {
            var width = srcImage.Width;
            var height = srcImage.Height;
            var dstImage = new Bitmap(width, height);
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    var list = getFrameList(srcImage, 3, i, j, dimension);
                    var g = 0;

                    for (int ii = 0; ii < 9; ii++)
                    {
                        g += list[ii] * operators[ii];
                    }
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    dstImage.SetPixel(i, j, Color.FromArgb(g, g, g));
                }
            }

            return dstImage;
        }

        /// <summary>
        ///  4x4掩膜算法
        /// </summary>
        public Bitmap Marsk5Operator(Bitmap srcImage, Int32[] operators, FrequencyDimension dimension)
        {
            var width = srcImage.Width;
            var height = srcImage.Height;
            var dstImage = new Bitmap(width, height);
            for (int i = 2; i < width - 2; i++)
            {
                for (int j = 2; j < height - 2; j++)
                {
                    var list = getFrameList(srcImage, 5, i, j, dimension);
                    var g = 0;
                    for (int ii = 0; ii < 25; ii++)
                    {
                        g += list[ii] * operators[ii];
                    }
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;

                    dstImage.SetPixel(i, j, Color.FromArgb(g, g, g));
                }
            }

            return dstImage;
        }

        /// <summary>
        ///  17x17掩膜算法
        /// </summary>
        public Bitmap Marsk17Operator(Bitmap srcImage, Int32[] operators, FrequencyDimension dimension)
        {
            var width = srcImage.Width;
            var height = srcImage.Height;
            var dstImage = new Bitmap(width, height);
            for (int i = 8; i < width - 8; i++)
            {
                for (int j = 8; j < height - 8; j++)
                {
                    var list = getFrameList(srcImage, 17, i, j, dimension);
                    var g = 0;
                    for (int ii = 0; ii < 17; ii++)
                    {
                        g += list[ii] * operators[ii];
                    }
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;

                    dstImage.SetPixel(i, j, Color.FromArgb(g, g, g));
                }
            }

            return dstImage;
        }

        /// <summary>
        ///  获得图像的当前像素的为中心的 sizeXsize 数组
        /// </summary>
        private Int32[] getFrameList(Bitmap srcImage, Int32 size, Int32 i, Int32 j, FrequencyDimension dimension)
        {
            var list = new Int32[size * size];
            var index = 0;
            var border = (size - 1) / 2;
            for (int ii = -border; ii <= border; ii++)
            {
                for (int ij = -border; ij <= border; ij++)
                {
                    switch (dimension)
                    {
                        case FrequencyDimension.RGB:
                            list[index] = srcImage.GetPixel(i - ii, j - ij).R;
                            break;
                        case FrequencyDimension.R:
                            list[index] = srcImage.GetPixel(i - ii, j - ij).R;
                            break;
                        case FrequencyDimension.G:
                            list[index] = srcImage.GetPixel(i - ii, j - ij).G;
                            break;
                        case FrequencyDimension.B:
                            list[index] = srcImage.GetPixel(i - ii, j - ij).B;
                            break;
                    }

                    index++;
                }
            }
            return list;
        }

        /// <summary>
        ///  8-邻接逆时针x,y增量
        /// </summary>
        private Int32[][] antiClockwiseDirection = new Int32[8][] { 
            new Int32[] { 1, 0 }, new Int32[] { 1, -1 }, new Int32[] { 0, -1 }, new Int32[] { -1, -1 }, 
            new Int32[] { -1, 0 }, new Int32[] { -1, 1 }, new Int32[] { 0, 1 }, new Int32[] { 1, 1 } };

        /// <summary>
        ///  4-邻接逆时针x,y增量
        /// </summary>
        private Int32[][] antiClockwiseDirection4 = new Int32[4][] { new Int32[] { 1, 0 }, new Int32[] { 0, -1 }, new Int32[] { -1, 0 }, new Int32[] { 0, 1 } };

        /// <summary>
        ///  获得图像8-邻接逆时针数组
        /// </summary>
        private Int32[] getFrameListAnticlockwise(Bitmap srcImage, Int32 adjoinType, Int32 i, Int32 j)
        {
            if (adjoinType == 8)
            {
                var list = new Int32[8];
                for (int ii = 0; ii < 8; ii++)
                {
                    list[ii] = srcImage.GetPixel(i + antiClockwiseDirection[ii][0], j + antiClockwiseDirection[ii][1]).R;
                }
                return list;
            }
            else
            {
                var list = new Int32[4];
                for (int ii = 0; ii < 4; ii++)
                {
                    list[ii] = srcImage.GetPixel(i + antiClockwiseDirection4[ii][0], j + antiClockwiseDirection4[ii][1]).R;
                }
                return list;
            }
        }

        /// <summary>
        ///  8-邻接顺时针的x,y 增量
        /// </summary>
        private Int32[][] clockwiseDirection = new Int32[8][] { 
            new Int32[] { -1, 0 }, new Int32[] { -1, -1 }, new Int32[] { 0, -1 }, new Int32[] { 1, -1 }, 
            new Int32[] { 1, 0 }, new Int32[] { 1, 1 }, new Int32[] { 0, 1 }, new Int32[] { -1, 1 } };

        /// <summary>
        ///  4-邻接顺时针的x,y 增量
        /// </summary>
        private Int32[][] clockwiseDirection4 = new Int32[4][] { new Int32[] { -1, 0 }, new Int32[] { 0, -1 }, new Int32[] { 1, 0 }, new Int32[] { 0, 1 } };

        /// <summary>
        ///  获得图像8-邻接顺时针数组
        /// </summary>
        private Int32[] getFrameListClockwise(Bitmap srcImage, Int32 adjoinType, Int32 i, Int32 j)
        {
            if (adjoinType == 8)
            {
                var list = new Int32[8];
                for (int ii = 0; ii < 8; ii++)
                {
                    list[ii] = srcImage.GetPixel(i + clockwiseDirection[ii][0], j + clockwiseDirection[ii][1]).R;
                }
                return list;
            }
            else
            {
                var list = new Int32[4];
                for (int ii = 0; ii < 4; ii++)
                {
                    list[ii] = srcImage.GetPixel(i + clockwiseDirection4[ii][0], j + clockwiseDirection4[ii][1]).R;
                }
                return list;
            }
        }

        /// <summary>
        ///  获得指定滤波窗口形状对应的元素序列
        /// </summary>
        protected Int32[] getFilterWindow(Int32 index, Int32 width, Int32 realWidth, FilterWindowType type, PixelFormat format)
        {
            if (format == PixelFormat.Format8bppIndexed)
            {
                return getFilterWindow8(index, width, type);
            }
            else if (format == PixelFormat.Format24bppRgb)
            {
                return getFilterWindow24(index, width, realWidth, type);
            }

            return null;
        }

        /// <summary>
        ///  获得8位像素指定滤波窗口形状对应的元素序列
        /// </summary>
        protected Int32[] getFilterWindow8(Int32 index, Int32 width, FilterWindowType type)
        {
            Int32 row = index / width;
            Int32 col = index % width;
            Int32[] set = null;

            switch (type)
            {
                case FilterWindowType.Hori3:
                    set = new Int32[3];
                    set[0] = index - 1;
                    set[1] = index;
                    set[2] = index + 1;
                    break;
                case FilterWindowType.Vert3:
                    set = new Int32[3];
                    set[0] = (row - 1) * width + col;
                    set[1] = index;
                    set[2] = (row + 1) * width + col;
                    break;
                case FilterWindowType.Cros3:
                    set = new Int32[5];
                    set[0] = (row - 1) * width + col;
                    set[1] = index - 1;
                    set[2] = index;
                    set[3] = index + 1;
                    set[4] = (row + 1) * width + col;
                    break;
                case FilterWindowType.Rect3:
                    set = new Int32[9];
                    set[0] = (row - 1) * width + col - 1;
                    set[1] = (row - 1) * width + col;
                    set[2] = (row - 1) * width + col + 1;
                    set[3] = index - 1;
                    set[4] = index;
                    set[5] = index + 1;
                    set[6] = (row + 1) * width + col - 1;
                    set[7] = (row + 1) * width + col;
                    set[8] = (row + 1) * width + col + 1;
                    break;
                case FilterWindowType.Hori5:
                    set = new Int32[5];
                    set[0] = index - 2;
                    set[1] = index - 1;
                    set[2] = index;
                    set[3] = index + 1;
                    set[4] = index + 2;
                    break;
                case FilterWindowType.Vert5:
                    set = new Int32[5];
                    set[0] = (row - 2) * width + col;
                    set[1] = (row - 1) * width + col;
                    set[3] = index;
                    set[4] = (row + 1) * width + col;
                    set[5] = (row + 2) * width + col;
                    break;
                case FilterWindowType.Cros5:
                    set = new Int32[9];
                    set[0] = (row - 2) * width + col;
                    set[1] = (row - 1) * width + col;
                    set[2] = index - 2;
                    set[3] = index - 1;
                    set[4] = index;
                    set[5] = index + 1;
                    set[6] = index + 2;
                    set[7] = (row + 1) * width + col;
                    set[8] = (row + 2) * width + col;
                    break;
                case FilterWindowType.Rect5:
                    set = new Int32[25];
                    set[0] = (row - 2) * width + col - 2;
                    set[1] = (row - 2) * width + col - 1;
                    set[2] = (row - 2) * width + col;
                    set[3] = (row - 2) * width + col + 1;
                    set[4] = (row - 2) * width + col + 2;
                    set[5] = (row - 1) * width + col - 2;
                    set[6] = (row - 1) * width + col - 1;
                    set[7] = (row - 1) * width + col;
                    set[8] = (row - 1) * width + col + 1;
                    set[9] = (row - 1) * width + col + 2;
                    set[10] = index - 2;
                    set[11] = index - 1;
                    set[12] = index;
                    set[13] = index + 1;
                    set[14] = index + 2;
                    set[15] = (row + 1) * width + col - 2;
                    set[16] = (row + 1) * width + col - 1;
                    set[17] = (row + 1) * width + col;
                    set[18] = (row + 1) * width + col + 1;
                    set[19] = (row + 1) * width + col + 2;
                    set[20] = (row + 2) * width + col - 2;
                    set[21] = (row + 2) * width + col - 1;
                    set[22] = (row + 2) * width + col;
                    set[23] = (row + 2) * width + col + 1;
                    set[24] = (row + 2) * width + col + 2;
                    break;
            }
            return set;
        }

        /// <summary>
        ///  获得24位像素指定滤波窗口形状对应的元素序列
        /// </summary>
        protected Int32[] getFilterWindow24(Int32 index, Int32 width, Int32 realWidth, FilterWindowType type)
        {
            Int32 row = index / width;
            Int32 col = index % width;

            Int32 extend = width - realWidth;
            Int32[] set = null;

            switch (type)
            {
                case FilterWindowType.Hori3:
                    set = new Int32[9];
                    set[0] = index - 3;
                    set[1] = index - 2;
                    set[2] = index - 1;
                    set[3] = index;
                    set[4] = index + 1;
                    set[5] = index + 2;
                    set[6] = index + 3;
                    set[7] = index + 4;
                    set[8] = index + 5;
                    break;
                case FilterWindowType.Vert3:
                    set = new Int32[9];
                    set[0] = (row - 1) * width + col;
                    set[1] = (row - 1) * width + col + 1;
                    set[2] = (row - 1) * width + col + 2;
                    set[3] = index;
                    set[4] = index + 1;
                    set[5] = index + 2;
                    set[6] = (row + 1) * width + col;
                    set[7] = (row + 1) * width + col + 1;
                    set[8] = (row + 1) * width + col + 2;
                    break;
                case FilterWindowType.Cros3:
                    set = new Int32[15];
                    set[0] = (row - 1) * width + col;
                    set[1] = (row - 1) * width + col + 1;
                    set[2] = (row - 1) * width + col + 2;
                    set[3] = index - 3;
                    set[4] = index - 2;
                    set[5] = index - 1;
                    set[6] = index;
                    set[7] = index + 1;
                    set[8] = index + 2;
                    set[9] = index + 3;
                    set[10] = index + 4;
                    set[11] = index + 5;
                    set[12] = (row + 1) * width + col;
                    set[13] = (row + 1) * width + col + 1;
                    set[14] = (row + 1) * width + col + 2;
                    break;
                case FilterWindowType.Rect3:
                    set = new Int32[27];
                    set[0] = (row - 1) * width + col - 3;
                    set[1] = (row - 1) * width + col - 2;
                    set[2] = (row - 1) * width + col - 1;
                    set[3] = (row - 1) * width + col;
                    set[4] = (row - 1) * width + col + 1;
                    set[5] = (row - 1) * width + col + 2;
                    set[6] = (row - 1) * width + col + 3;
                    set[7] = (row - 1) * width + col + 4;
                    set[8] = (row - 1) * width + col + 5;
                    set[9] = index - 3;
                    set[10] = index - 2;
                    set[11] = index - 1;
                    set[12] = index;
                    set[13] = index + 1;
                    set[14] = index + 2;
                    set[15] = index + 3;
                    set[16] = index + 4;
                    set[17] = index + 5;
                    set[18] = (row + 1) * width + col - 3;
                    set[19] = (row + 1) * width + col - 2;
                    set[20] = (row + 1) * width + col - 1;
                    set[21] = (row + 1) * width + col;
                    set[22] = (row + 1) * width + col + 1;
                    set[23] = (row + 1) * width + col + 2;
                    set[24] = (row + 1) * width + col + 3;
                    set[25] = (row + 1) * width + col + 4;
                    set[26] = (row + 1) * width + col + 5;
                    break;
                case FilterWindowType.Hori5:
                    set = new Int32[15];
                    set[0] = index - 6;
                    set[1] = index - 5;
                    set[2] = index - 4;
                    set[3] = index - 3;
                    set[4] = index - 2;
                    set[5] = index - 1;
                    set[6] = index;
                    set[7] = index + 1;
                    set[8] = index + 2;
                    set[9] = index + 3;
                    set[10] = index + 4;
                    set[11] = index + 5;
                    set[12] = index + 6;
                    set[13] = index + 7;
                    set[14] = index + 8;
                    break;
                case FilterWindowType.Vert5:
                    set = new Int32[15];
                    set[0] = (row - 2) * width + col;
                    set[1] = (row - 2) * width + col + 1;
                    set[2] = (row - 2) * width + col + 2;
                    set[3] = (row - 1) * width + col;
                    set[4] = (row - 1) * width + col + 1;
                    set[5] = (row - 1) * width + col + 2;
                    set[6] = index;
                    set[7] = index + 1;
                    set[8] = index + 2;
                    set[9] = (row + 1) * width + col;
                    set[10] = (row + 1) * width + col + 1;
                    set[11] = (row + 1) * width + col + 2;
                    set[12] = (row + 2) * width + col;
                    set[13] = (row + 2) * width + col + 1;
                    set[14] = (row + 2) * width + col + 2;
                    break;
                case FilterWindowType.Cros5:
                    set = new Int32[27];
                    set[0] = (row - 2) * width + col;
                    set[1] = (row - 2) * width + col + 1;
                    set[2] = (row - 2) * width + col + 2;
                    set[3] = (row - 1) * width + col;
                    set[4] = (row - 1) * width + col + 1;
                    set[5] = (row - 1) * width + col + 2;
                    set[6] = index - 6;
                    set[7] = index - 5;
                    set[8] = index - 4;
                    set[9] = index - 3;
                    set[10] = index - 2;
                    set[11] = index - 1;
                    set[12] = index;
                    set[13] = index + 1;
                    set[14] = index + 2;
                    set[15] = index + 3;
                    set[16] = index + 4;
                    set[17] = index + 5;
                    set[18] = index + 6;
                    set[19] = index + 7;
                    set[20] = index + 8;
                    set[21] = (row + 1) * width + col;
                    set[22] = (row + 1) * width + col + 1;
                    set[23] = (row + 1) * width + col + 2;
                    set[24] = (row + 2) * width + col;
                    set[25] = (row + 2) * width + col + 1;
                    set[26] = (row + 2) * width + col + 2;
                    break;
                case FilterWindowType.Rect5:
                    set = new Int32[75];
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
                    break;
            }

            return set;
        }

        /// <summary>
        ///  查看各种滤窗固定编程格式
        /// </summary>
        private void filterWindowTemplate()
        {
            var srcImage = new Bitmap(100, 100);

            // 3位水平线
            for (var i = 3; i < Length - 3; i += 3)
            {
                if (i % Width >= RealWidth - 3)
                {
                    // 因为换行要移去头3位数即一个像素, 所以不减三
                    i = (i / Width + 1) * Width;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Hori3, srcImage.PixelFormat);
            }

            // 3位垂直线
            for (int i = Width; i < Length - Width - 3; i += 3)
            {
                if (i % Width >= RealWidth)
                {
                    // 因为为垂直匹配所以减三,防止跳过一个元素
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Vert3, srcImage.PixelFormat);
            }

            // 3位十字形
            for (int i = Width + 3; i < Length - Width - 6; i += 3)
            {
                if (i % Width >= RealWidth - 3)
                {
                    i = (i / Width + 1) * Width;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Cros3, srcImage.PixelFormat);
            }

            // 3位正方形
            for (int i = Width + 3; i < Length - Width - 3; i += 3)
            {
                if (i % Width >= RealWidth - 3)
                {
                    i = (i / Width + 1) * Width;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Rect3, srcImage.PixelFormat);
            }

            // 5位水平线
            for (int i = 6; i < Length - 6; i += 3)
            {
                if (i % Width >= RealWidth - 6)
                {
                    i = (i / Width + 1) * Width + 3;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Hori5, srcImage.PixelFormat);
            }

            // 5位垂直线
            for (int i = Width * 2; i < Length - Width * 2; i += 3)
            {
                if (i % Width >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Vert5, srcImage.PixelFormat);
            }

            // 5位十字形
            for (int i = 2 * Width + 6; i < Length - 2 * Width - 6; i += 3)
            {
                if (i % Width >= RealWidth - 6)
                {
                    i = (i / Width + 1) * Width + 3;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Cros5, srcImage.PixelFormat);
            }

            // 5位正方形
            for (int i = 2 * Width + 6; i < Length - 2 * Width - 6; i += 3)
            {
                if (i % Width >= RealWidth - 6)
                {
                    i = (i / Width + 1) * Width + 3;
                    continue;
                }
                Int32[] set = getFilterWindow(i, Width, RealWidth, FilterWindowType.Rect5, srcImage.PixelFormat);
            }
        }
    }
}
