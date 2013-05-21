using System;
using System.Drawing;
using System.Drawing.Imaging;
using ValueHelper.Image.Interface;
using ValueHelper.Math.Infrastructure;

namespace ValueHelper.Image.Bit24
{
    public partial class ImageBit24 : IFrequency
    {
        #region 傅里叶

        /// <summary>
        ///  快速傅里叶变化
        /// </summary>
        /// <param name="inv">是否进行坐标位移变换</param>
        /// <returns></returns>
        public void FFT(Bitmap srcImage, Boolean inv)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] tempComp = this.FFT(rgbBytes, Width, RealWidth, Height, inv);
            for (int i = 0; i < Length; i++)
            {
                rgbBytes[i] = (Byte)tempComp[i].Real;
            }
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  二维快速傅里叶变换
        /// </summary>
        /// <param name="rgbBytes">图像序列</param>
        /// <param name="width">图像宽度</param>
        /// <param name="realWidth">图像实际宽度</param>
        /// <param name="height">图像长度</param>
        /// <param name="inv">是否进行坐标位移变换</param>
        /// <returns></returns>
        public Complex[] FFT(Byte[] rgbBytes, Int32 width, Int32 realWidth, Int32 height, Boolean inv)
        {
            Int32 length = width * height;
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            Complex[] tempComp = new Complex[length];

            for (int i = 0; i < length; i += 3)
            {
                if ((i % width) > realWidth)
                {
                    tempComp[i] = new Complex(rgbBytes[i], 0);
                    tempComp[i + 1] = new Complex(rgbBytes[i + 1], 0);
                    tempComp[i + 2] = new Complex(rgbBytes[i + 2], 0);
                    continue;
                }

                if (inv)
                {
                    if ((i / width + i % realWidth) % 2 == 0)
                    {
                        tempComp[i] = new Complex(rgbBytes[i], 0);
                        tempComp[i + 1] = new Complex(rgbBytes[i + 1], 0);
                        tempComp[i + 2] = new Complex(rgbBytes[i + 2], 0);
                    }
                    else
                    {
                        tempComp[i] = new Complex(-rgbBytes[i], 0);
                        tempComp[i + 1] = new Complex(-rgbBytes[i + 1], 0);
                        tempComp[i + 2] = new Complex(-rgbBytes[i + 2], 0);
                    }
                }
                else
                {
                    tempComp[i] = new Complex(rgbBytes[i], 0);
                    tempComp[i + 1] = new Complex(rgbBytes[i + 1], 0);
                    tempComp[i + 2] = new Complex(rgbBytes[i + 2], 0);
                }
            }

            Int32 singleLength = realWidth / 3;
            Complex[] tempCompHr = new Complex[singleLength];
            Complex[] tempCompHg = new Complex[singleLength];
            Complex[] tempCompHb = new Complex[singleLength];
            // 水平方向变化
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < singleLength; j++)
                {
                    tempCompHb[j] = tempComp[i * width + j * 3];
                    tempCompHg[j] = tempComp[i * width + j * 3 + 1];
                    tempCompHr[j] = tempComp[i * width + j * 3 + 2];
                }
                tempCompHr = mathHelper.FFT(tempCompHr, singleLength);
                tempCompHg = mathHelper.FFT(tempCompHg, singleLength);
                tempCompHb = mathHelper.FFT(tempCompHb, singleLength);

                for (int j = 0; j < singleLength; j++)
                {
                    tempComp[i * width + j * 3] = tempCompHb[j];
                    tempComp[i * width + j * 3 + 1] = tempCompHg[j];
                    tempComp[i * width + j * 3 + 2] = tempCompHr[j];
                }
            }

            Complex[] tempCompVe = new Complex[height];
            // 垂直方向变化
            for (int i = 0; i < realWidth; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tempCompVe[j] = tempComp[j * width + i];
                }
                tempCompVe = mathHelper.FFT(tempCompVe, height);

                for (int j = 0; j < height; j++)
                {
                    tempComp[j * width + i] = tempCompVe[j];
                }
            }

            return tempComp;
        }

        /// <summary>
        ///  二维快速傅里叶逆变换
        /// </summary>
        /// <param name="freData">频域数据</param>
        /// <param name="width">图像宽度</param>
        /// <param name="realWidth">图像真是宽度</param>
        /// <param name="height">图像长度</param>
        /// <param name="inv">是否进行坐标位移变换,要与二维快速傅里叶正交变换一致</param>
        /// <returns></returns>
        public Byte[] IFFT(Complex[] freData, Int32 width, Int32 realWidth, Int32 height, Boolean inv)
        {
            Int32 length = width * height;
            Complex[] tempComp = (Complex[])freData.Clone();

            Int32 singleLength = realWidth / 3;
            Complex[] tempCompHr = new Complex[singleLength];
            Complex[] tempCompHg = new Complex[singleLength];
            Complex[] tempCompHb = new Complex[singleLength];
            // 水平方向变化
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < singleLength; j++)
                {
                    tempCompHb[j] = tempComp[i * width + j * 3];
                    tempCompHg[j] = tempComp[i * width + j * 3 + 1];
                    tempCompHr[j] = tempComp[i * width + j * 3 + 2];
                }
                tempCompHr = mathHelper.IFFT(tempCompHr, singleLength);
                tempCompHg = mathHelper.IFFT(tempCompHg, singleLength);
                tempCompHb = mathHelper.IFFT(tempCompHb, singleLength);

                for (int j = 0; j < singleLength; j++)
                {
                    tempComp[i * width + j * 3] = tempCompHb[j];
                    tempComp[i * width + j * 3 + 1] = tempCompHg[j];
                    tempComp[i * width + j * 3 + 2] = tempCompHr[j];
                }
            }

            Complex[] tempCompVe = new Complex[height];
            // 垂直方向变化
            for (int i = 0; i < realWidth; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tempCompVe[j] = tempComp[j * width + i];
                }
                tempCompVe = mathHelper.IFFT(tempCompVe, height);
                for (int j = 0; j < height; j++)
                {
                    tempComp[j * width + i] = tempCompVe[j];
                }
            }

            Byte[] rgbBytes = new Byte[length];
            Double tempr = 0, tempg = 0, tempb = 0;
            // 赋值,保留实数部分
            for (int i = 0; i < length; i += 3)
            {
                if (inv)
                {
                    if ((i / width + i % realWidth) % 2 == 0)
                    {
                        tempb = tempComp[i].Real;
                        tempg = tempComp[i + 1].Real;
                        tempr = tempComp[i + 2].Real;
                    }
                    else
                    {
                        tempb = -tempComp[i].Real;
                        tempg = -tempComp[i + 1].Real;
                        tempr = -tempComp[i + 2].Real;
                    }
                }
                else
                {
                    tempb = tempComp[i].Real;
                    tempg = tempComp[i + 1].Real;
                    tempr = tempComp[i + 2].Real;
                }
                tempb = tempb > 255 ? 255 : tempb < 0 ? 0 : tempb;
                tempg = tempg > 255 ? 255 : tempg < 0 ? 0 : tempg;
                tempr = tempr > 255 ? 255 : tempr < 0 ? 0 : tempr;

                rgbBytes[i] = Convert.ToByte(tempb);
                rgbBytes[i + 1] = Convert.ToByte(tempg);
                rgbBytes[i + 2] = Convert.ToByte(tempr);
            }

            return rgbBytes;
        }

        #endregion

        #region 幅度图像

        /// <summary>
        ///  幅度图像
        /// </summary>
        public void Amplitude(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            // 二维傅里叶变换,需要进行坐标移位
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);
            Double[] tempArray = new Double[Length];
            // 变量交换,取得幅度系数
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = System.Math.Log(1 + freDom[i].Abs(), 2);
            }

            #region 灰度拉伸

            Double max = mathHelper.Max(tempArray);
            Double min = mathHelper.Min(tempArray);
            Double p = 255.0 / (max - min);
            Double gr = 0, gg = 0, gb = 0;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }
                // 灰度拉伸
                gr = p * (tempArray[i + 2] - min) + 0.5;
                gg = p * (tempArray[i + 1] - min) + 0.5;
                gb = p * (tempArray[i] - min) + 0.5;
                rgbBytes[i + 2] = (Byte)gr;
                rgbBytes[i + 1] = (Byte)gg;
                rgbBytes[i] = (Byte)gb;
            }

            #endregion

            UnlockBits(rgbBytes);
        }

        #endregion

        #region 相位图像

        /// <summary>
        ///  相位图像
        /// </summary>
        public void Phase(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            // 二维傅里叶变换,不进行坐标移位
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, false);
            Double[] tempArray = new Double[Length];
            // 变量交换,取得相位系数
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = freDom[i].Angle() + 2 * System.Math.PI;
            }

            #region 灰度拉伸

            Double max = mathHelper.Max(tempArray);
            Double min = mathHelper.Min(tempArray);
            Double p = 255.0 / (max - min);
            Double gr = 0, gg = 0, gb = 0;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % RealWidth >= RealWidth)
                {
                    i = (i / Width + 1) * Width - 3;
                    continue;
                }
                // 灰度拉伸
                gr = p * (tempArray[i + 2] - min) + 0.5;
                gg = p * (tempArray[i + 1] - min) + 0.5;
                gb = p * (tempArray[i] - min) + 0.5;
                rgbBytes[i + 2] = (Byte)gr;
                rgbBytes[i + 1] = (Byte)gg;
                rgbBytes[i] = (Byte)gb;
            }

            #endregion

            UnlockBits(rgbBytes);
        }

        #endregion
    }
}
