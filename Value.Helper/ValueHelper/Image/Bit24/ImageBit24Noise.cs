using System;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using ValueHelper.Image.Interface;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Bit24
{
    /// <summary>
    ///  图像噪声
    /// </summary>
    public partial class ImageBit24 : INoise
    {
        /// <summary>
        ///  噪声
        /// </summary>
        /// <param name="type">噪声类型</param>
        public void Noise(Bitmap srcImage, NoiseType type)
        {
            switch (type)
            {
                case NoiseType.Gauss:
                    this.GaussNoise(srcImage, 0, 20);
                    break;
                case NoiseType.Rayleigh:
                    this.RayleighNoise(srcImage, 2, 500);
                    break;
                case NoiseType.Index:
                    this.IndexNoise(srcImage, 0.05);
                    break;
                case NoiseType.Pepper:
                    this.PepperNoise(srcImage, 0.05, 0.05);
                    break;
            }
        }

        /// <summary>
        ///  高斯噪声
        /// </summary>
        /// <param name="mean">均值</param>
        /// <param name="meanDeviation">均方差</param>
        public void GaussNoise(Bitmap srcImage, Double mean, Double meanDeviation)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Random r1, r2;
            r1 = new Random(unchecked((Int32)DateTime.Now.Ticks));
            r2 = new Random(~unchecked((Int32)DateTime.Now.Ticks));

            Double v1, v2;
            Double temp, tempr, tempg, tempb;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width > RealWidth) continue;

                do
                {
                    v1 = r1.NextDouble();
                }
                while (v1 <= 0.00000000001);
                v2 = r2.NextDouble();
                temp = System.Math.Sqrt(-2 * System.Math.Log(v1)) * System.Math.Cos(2 * System.Math.PI * v2) * meanDeviation + mean;
                tempb = temp + rgbBytes[i];
                tempg = temp + rgbBytes[i + 1];
                tempr = temp + rgbBytes[i + 2];

                tempb = tempb > 255 ? 255 : tempb < 0 ? 0 : tempb;
                tempg = tempg > 255 ? 255 : tempg < 0 ? 0 : tempg;
                tempr = tempr > 255 ? 255 : tempr < 0 ? 0 : tempr;

                rgbBytes[i] = Convert.ToByte(tempb);
                rgbBytes[i + 1] = Convert.ToByte(tempg);
                rgbBytes[i + 2] = Convert.ToByte(tempr);
            }

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  瑞利噪声
        /// </summary>
        /// <param name="paramA">参数A</param>
        /// <param name="paramB">参数B</param>
        public void RayleighNoise(Bitmap srcImage, Double paramA, Double paramB)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Random r = new Random(unchecked((Int32)DateTime.Now.Ticks));

            Double v;
            Double temp, tempr, tempg, tempb;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width > RealWidth) continue;

                do
                {
                    v = r.NextDouble();
                } while (v >= 0.9999999999);

                temp = paramA + System.Math.Sqrt(-1 * paramB * System.Math.Log(1 - v));
                tempb = temp + rgbBytes[i];
                tempg = temp + rgbBytes[i + 1];
                tempr = temp + rgbBytes[i + 2];

                tempb = tempb > 255 ? 255 : tempb < 0 ? 0 : tempb;
                tempg = tempg > 255 ? 255 : tempg < 0 ? 0 : tempg;
                tempr = tempr > 255 ? 255 : tempr < 0 ? 0 : tempr;

                rgbBytes[i] = Convert.ToByte(tempb);
                rgbBytes[i + 1] = Convert.ToByte(tempg);
                rgbBytes[i + 2] = Convert.ToByte(tempr);
            }

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  指数噪声
        /// </summary>
        /// <param name="param">参数a(a>0)</param>
        public void IndexNoise(Bitmap srcImage, Double param)
        {
            Debug.Assert(param > 0, "参数param必须大于零");
            if (param < 0) return;

            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Random r = new Random(unchecked((Int32)DateTime.Now.Ticks));

            Double v;
            Double temp, tempr, tempg, tempb;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width > RealWidth) continue;

                do
                {
                    v = r.NextDouble();
                } while (v >= 0.9999999999);
                temp = -1 * System.Math.Log(1 - v) / param;
                tempb = temp + rgbBytes[i];
                tempg = temp + rgbBytes[i + 1];
                tempr = temp + rgbBytes[i + 2];

                tempb = tempb > 255 ? 255 : tempb < 0 ? 0 : tempb;
                tempg = tempg > 255 ? 255 : tempg < 0 ? 0 : tempg;
                tempr = tempr > 255 ? 255 : tempr < 0 ? 0 : tempr;

                rgbBytes[i] = Convert.ToByte(tempb);
                rgbBytes[i + 1] = Convert.ToByte(tempg);
                rgbBytes[i + 2] = Convert.ToByte(tempr);
            }

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  椒盐噪声
        /// </summary>
        /// <param name="pepper">椒量</param>
        /// <param name="salt">盐量</param>
        public void PepperNoise(Bitmap srcImage, Double pepper, Double salt)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            Random r = new Random(unchecked((Int32)DateTime.Now.Ticks));
            Double v;
            Double temp = 0D, tempr, tempg, tempb;
            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width > RealWidth) continue;

                v = r.NextDouble();
                if (v <= pepper)
                    temp -= 500;
                else if (v >= (1 - salt))
                    temp = 500;
                else
                    temp = 0;
                tempb = temp + rgbBytes[i];
                tempg = temp + rgbBytes[i + 1];
                tempr = temp + rgbBytes[i + 2];

                tempb = tempb > 255 ? 255 : tempb < 0 ? 0 : tempb;
                tempg = tempg > 255 ? 255 : tempg < 0 ? 0 : tempg;
                tempr = tempr > 255 ? 255 : tempr < 0 ? 0 : tempr;

                rgbBytes[i] = Convert.ToByte(tempb);
                rgbBytes[i + 1] = Convert.ToByte(tempg);
                rgbBytes[i + 2] = Convert.ToByte(tempr);
            }

            UnlockBits(rgbBytes);
        }
    }
}
