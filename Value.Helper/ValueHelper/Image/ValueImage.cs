using System;
using System.Drawing;
using ValueHelper.Math;
using ValueHelper.Image.Infrastructure;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ValueHelper.Image.Bit24;
using ValueHelper.Image.Interface;
using System.Diagnostics;

namespace ValueHelper.Image
{
    public partial class ValueImage
    {
        protected ValueMath mathHelper;

        protected ValueImage()
        {
            mathHelper = ValueMath.GetInstance();
        }

        public ValueImage(String test)
        {
            mathHelper = ValueMath.GetInstance();
        }

        public static IValueImage GetImageHelper(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format24bppRgb:
                    return ImageBit24.GetInstance();
                default:
                    Debug.Fail("暂不支持改图片像素位数");
                    break;
            }
            return null;
        }

        #region 内存法处理图像

        private BitmapData bmpData;
        private Bitmap sourceImage;
        private IntPtr ptr;
        // 图像总长度
        protected Int32 Length;
        // 图像长度
        protected Int32 Height;
        // 图像宽度
        protected Int32 Width;
        // 图像实际宽度(已x3)
        protected Int32 RealWidth;
        // 图像实际半宽度(已x3)
        protected Int32 HalfWidth;
        // 图像长度
        protected Int32 HalfHeight;

        protected virtual Byte[] LockBits(Bitmap srcImage, ImageLockMode mode)
        {
            sourceImage = srcImage;

            Int32 tempWidth = sourceImage.Width;
            Int32 tempHeight = sourceImage.Height;
            Rectangle rect = new Rectangle(0, 0, tempWidth, tempHeight);
            bmpData = sourceImage.LockBits(rect, ImageLockMode.ReadOnly, sourceImage.PixelFormat);
            ptr = bmpData.Scan0;

            var asd = bmpData.Width;

            Int32 byteLength = bmpData.Stride * tempHeight;
            Byte[] rgbBytes = new Byte[byteLength];
            Marshal.Copy(ptr, rgbBytes, 0, byteLength);

            Length = rgbBytes.Length;
            Height = srcImage.Height;
            Width = bmpData.Stride;

            // 奇偶取中值的区别
            if (RealWidth / 3 % 2 == 0)
                HalfWidth = RealWidth / 6 * 3;
            else
                HalfWidth = ((RealWidth / 3 - 1) / 2) * 3;
            // 奇偶取中值的区别
            if (Height % 2 == 0)
                HalfHeight = Height / 2;
            else
                HalfHeight = (Height - 1) / 2;

            return rgbBytes;
        }

        protected void UnlockBits(Byte[] rgbData)
        {
            Marshal.Copy(rgbData, 0, ptr, rgbData.Length);
            sourceImage.UnlockBits(bmpData);
        }

        protected void UnlockBits()
        {
            sourceImage.UnlockBits(bmpData);
        }

        #endregion

        #region 模板取值索引

        /// <summary>
        ///  获得2x2索引 当前元素为左下角
        /// </summary>
        protected virtual Int32[] getRectangle2x2(Int32 index, Int32 width)
        {
            throw new NotImplementedException();
        }

        protected virtual Int32[] getRectangle3x3(Int32 index, Int32 width)
        {
            throw new NotImplementedException();
        }

        protected virtual Int32[] getRectangle5x5(Int32 index, Int32 width)
        {
            throw new NotImplementedException();
        }

        protected virtual Int32[] getRectangle7x7(Int32 index, Int32 width)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 灰度直方图

        /// <summary>
        ///  获得指定维度图像数值个数(灰度直方图)
        /// </summary>
        public Int32[] GetFrequency(Bitmap srcImage)
        {
            var frequnce = new Int32[256];
            var rgbBytes = LockBits(srcImage, ImageLockMode.ReadOnly);

            var f = 0;
            for (int i = 0; i < Length; i++)
            {
                f = rgbBytes[i];
                frequnce[f]++;
            }

            UnlockBits();

            return frequnce;
        }

        #endregion

        #region 高斯滤波模板

        protected Double[] LogTemplate(Double sigma)
        {
            Double std2 = 2 * sigma * sigma;
            Int32 radius = Convert.ToInt16(System.Math.Ceiling(3 * sigma));
            Int32 filterWidth = 2 * radius + 1;
            Double[] filter = new Double[filterWidth * filterWidth];
            Double sum = 0, average = 0;

            // 因为模板是中心对称的, 所以先得到模板左上角的值,再赋到全部模板

            // 计算模板左上角
            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    Int32 xx = (j - radius) * (j - radius);
                    Int32 yy = (i - radius) * (i - radius);
                    filter[i * filterWidth + j] = (xx + yy - std2) * System.Math.Exp(-(xx + yy) / std2);
                    sum += 4 * filter[i * filterWidth + j];
                }
            }

            // 水平和垂直对称轴单独处理
            for (int i = 0; i < radius; i++)
            {
                Int32 xx = (i - radius) * (i - radius);
                filter[i * filterWidth + radius] = (xx - std2) * System.Math.Exp(-xx / std2);
                sum += 2 * filter[i * filterWidth + radius];
            }

            for (int j = 0; j < radius; j++)
            {
                Int32 yy = (j - radius) * (j - radius);
                filter[radius * filterWidth + j] = (yy - std2) * System.Math.Exp(-yy / std2);
                sum += 2 * filter[radius * filterWidth + j];
            }
            // 中心点
            filter[radius * filterWidth + radius] = -std2;
            // 所以模板数据和
            sum += filter[radius * filterWidth + radius];
            // 计算平均值
            average = sum / filter.Length;
            // 赋值
            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    filter[i * filterWidth + j] = filter[i * filterWidth + j] - average;
                    filter[filterWidth - 1 - j + i * filterWidth] = filter[i * filterWidth + j];
                    filter[j + (filterWidth - 1 - j) * filterWidth] = filter[i * filterWidth + j];
                    filter[filterWidth - 1 - j + (filterWidth - 1 - i) * filterWidth] = filter[i * filterWidth + j];
                }
            }
            // 赋值水平和垂直对称轴
            for (int i = 0; i < radius; i++)
            {
                filter[i * filterWidth + radius] = filter[i * filterWidth + radius] - average;
                filter[(filterWidth - 1 - i) * filterWidth + radius] = filter[i * filterWidth + radius];
            }

            for (int j = 0; j < radius; j++)
            {
                filter[radius * filterWidth + j] = filter[radius * filterWidth + j] - average;
                filter[radius * filterWidth + filterWidth - 1 - j] = filter[radius * filterWidth + j];
            }
            // 赋值中心点
            filter[radius * filterWidth + radius] = filter[radius * filterWidth + radius] - average;
            return filter;
        }

        protected Double[] DogTemplate(Double sigma)
        {
            Double std2 = 2 * sigma * sigma;
            Int32 radius = Convert.ToInt16(System.Math.Ceiling(3 * sigma));
            Int32 filterWidth = 2 * radius + 1;
            Double[] filter = new Double[filterWidth * filterWidth];
            Double sum = 0, average = 0;

            // 因为模板是中心对称的,所以先得到模板左上角的值, 再赋值到全部模板

            // 计算模板左上角
            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    Int32 xx = (j - radius) * (j - radius);
                    Int32 yy = (i - radius) * (i - radius);
                    filter[i * filterWidth + j] = 1.6 * System.Math.Exp(-(xx + yy) * 1.6 * 1.6 / std2) / sigma -
                        System.Math.Exp(-(xx + yy) / -std2) / sigma;
                    sum += 4 * filter[i * filterWidth + j];
                }
            }
            // 水平和垂直对称轴单独处理
            for (int i = 0; i < radius; i++)
            {
                Int32 xx = (i - radius) * (i - radius);
                filter[i * filterWidth + radius] = 1.6 * System.Math.Exp(-xx * 16 * 16 / std2) / sigma -
                    System.Math.Exp(-xx / std2) / sigma;
                sum += 2 * filter[i * filterWidth + radius];
            }

            for (int j = 0; j < radius; j++)
            {
                Int32 yy = (j - radius) * (j - radius);
                filter[radius * filterWidth + j] = 1.6 * System.Math.Exp(-yy * 1.6 * 1.6 / std2) / sigma -
                    System.Math.Exp(-yy / std2) / sigma;
                sum += 2 * filter[radius * filterWidth + j];
            }
            // 中心点
            filter[radius * filterWidth + radius] = 1.6 / sigma - 1 / sigma;
            // 所有模板数据和
            sum += filter[radius * filterWidth + radius];
            // 计算平均值
            average = sum / filter.Length;
            // 赋值
            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    filter[i * filterWidth + j] = filter[i * filterWidth + j] - average;
                    filter[filterWidth - 1 - j + i * filterWidth] = filter[i * filterWidth + j];
                    filter[j + (filterWidth - 1 - i) * filterWidth] = filter[i * filterWidth + j];
                    filter[filterWidth - 1 - j + (filterWidth - 1 - i) * filterWidth] = filter[i * filterWidth + j];
                }
            }
            // 赋值水平和垂直对称轴
            for (int i = 0; i < radius; i++)
            {
                filter[i * filterWidth + radius] = filter[i * filterWidth + radius] - average;
                filter[(filterWidth - 1 - i) * filterWidth + radius] = filter[i * filterWidth + radius];
            }
            for (int j = 0; j < radius; j++)
            {
                filter[radius * filterWidth + j] = filter[radius * filterWidth + j] - average;
                filter[radius * filterWidth + filterWidth - 1 - j] = filter[radius * filterWidth + j];
            }
            // 赋值中心点
            filter[radius * filterWidth + radius] = filter[radius * filterWidth + radius] * average;
            return filter;

        }

        #endregion

        #region 高斯平滑
        /// <summary>
        ///  高斯平滑
        /// </summary>
        /// <param name="borderLength">二维数据边长(必须是正方形)</param>
        /// <param name="sigma">方差</param>
        protected Double[] GaussSmooth(Double[] rectangleData, Int32 borderLength, Double sigma)
        {
            if (rectangleData.Length / borderLength != borderLength)
                Debug.Fail("必须输入正方形二维数据");

            // 方差
            Double std2 = 2 * sigma * sigma;
            // 半径=3sigma(方差的3倍)
            Int32 radius = Convert.ToInt16(System.Math.Ceiling(3 * sigma));
            Int32 filterWidth = 2 * radius + 1;
            Double[] filter = new Double[filterWidth];
            Double[] tempData = new Double[rectangleData.Length];

            Double sum = 0;
            // 产生一维高斯函数
            for (int i = 0; i < filterWidth; i++)
            {
                Int32 xx = (i - radius) * (i - radius);
                filter[i] = System.Math.Exp(-xx / std2);
                sum += filter[i];
            }
            // 归一化
            for (int i = 0; i < filterWidth; i++)
            {
                filter[i] = filter[i] / sum;
            }
            // 水平方向滤波
            for (int i = 0; i < borderLength; i++)
            {
                for (int j = 0; j < borderLength; j++)
                {
                    Double temp = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        // 循环延拓
                        Int32 rem = (System.Math.Abs(j + k)) % borderLength;
                        // 计算卷积和
                        temp += rectangleData[i * borderLength + rem] * filter[k + radius];
                    }
                    tempData[i * borderLength + j] = temp;
                }
            }

            // 垂直方向滤波
            Double[] result = new Double[rectangleData.Length];
            for (int j = 0; j < borderLength; j++)
            {
                for (int i = 0; i < borderLength; i++)
                {
                    Double temp = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        // 循环延拓
                        Int32 rem = (System.Math.Abs(i + k)) % borderLength;
                        // 计算卷积和
                        temp += tempData[rem * borderLength + j] * filter[k + radius];
                    }
                    result[i * borderLength + j] = temp;
                }
            }
            return result;
        }

        #endregion

        #region 零交叉方法

        /// <summary>
        ///  零交叉法阈值处理(二值化)
        /// </summary>
        /// <param name="rectangleData">二维数据</param>
        /// <param name="width">宽度</param>
        /// <param name="height">长度</param>
        /// <param name="thresh">阈值</param>
        protected void ZeroCross(ref Double[] rectangleData, Int32 width, Int32 height, Double thresh, out Byte[] result)
        {
            result = new Byte[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (rectangleData[i * width + j] < 0 && rectangleData[((i + 1) % height) * width + j] > 0 && System.Math.Abs(rectangleData[i * width + j] - rectangleData[((i + 1) % height) * width + j]) > thresh)
                    {
                        result[i * width + j] = 255;
                    }
                    else if (rectangleData[i * width + j] < 0 && rectangleData[((System.Math.Abs(i - 1)) % height) * width + j] > 0 && System.Math.Abs(rectangleData[i * width + j] - rectangleData[((System.Math.Abs(i - 1)) % height) * width + j]) > thresh)
                    {
                        result[i * width + j] = 255;
                    }
                    else if (rectangleData[i * width + j] < 0 && rectangleData[i * width + ((j + 1) % width)] > 0 && System.Math.Abs(rectangleData[i * width + j] - rectangleData[i * width + ((j + 1) % width)]) > thresh)
                    {
                        result[i * width + j] = 255;
                    }
                    else if (rectangleData[i * width + j] < 0 && rectangleData[i * width + ((System.Math.Abs(j - 1)) % width)] > 0 && System.Math.Abs(rectangleData[i * width + j] - rectangleData[i * width + ((System.Math.Abs(j - 1)) % width)]) > thresh)
                    {
                        result[i * width + j] = 255;
                    }
                    else if (rectangleData[i * width + j] == 0)
                    {
                        if (rectangleData[((i + 1) % height) * width + j] > 0 && rectangleData[((System.Math.Abs(i - 1)) % height) * width + j] < 0 && System.Math.Abs(rectangleData[((System.Math.Abs(i - 1)) % height) * width + j] - rectangleData[((i + 1) % height) * width + j]) > 2 * thresh)
                        {
                            result[i * width + j] = 255;
                        }
                        else if (rectangleData[((i + 1) % height) * width + j] < 0 && rectangleData[((System.Math.Abs(i - 1)) % height) * width + j] > 0 && System.Math.Abs(rectangleData[((System.Math.Abs(i - 1)) % height) * width + j] - rectangleData[((i + 1) % height) * width + j]) > 2 * thresh)
                        {
                            result[i * width + j] = 255;
                        }
                        else if (rectangleData[i * width + ((j + 1) % width)] > 0 && rectangleData[i * width + ((System.Math.Abs(j - 1)) % width)] < 0 && System.Math.Abs(rectangleData[i * width + ((j + 1) % width)] - rectangleData[i * width + ((System.Math.Abs(j - 1)) % width)]) > 2 * thresh)
                        {
                            result[i * width + j] = 255;
                        }
                        else if (rectangleData[i * width + ((j + 1) % width)] < 0 && rectangleData[i * width + ((System.Math.Abs(j - 1)) % width)] > 0 && System.Math.Abs(rectangleData[i * width + ((j + 1) % width)] - rectangleData[i * width + ((System.Math.Abs(j - 1)) % width)]) > 2 * thresh)
                        {
                            result[i * width + j] = 255;
                        }
                        else
                        {
                            result[i * width + j] = 0;
                        }
                    }
                    else
                    {
                        result[i * width + j] = 0;
                    }
                }
            }
        }

        #endregion
    }
}
