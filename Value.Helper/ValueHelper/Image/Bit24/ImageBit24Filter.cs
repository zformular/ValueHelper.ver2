using System;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.Generic;
using ValueHelper.Image.Interface;
using ValueHelper.Math.Infrastructure;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Bit24
{
    /// <summary>
    ///  图像过滤
    /// </summary>
    public partial class ImageBit24 : IFilter
    {
        #region 成分滤波

        /// <summary>
        ///  成分滤波
        /// </summary>
        /// <param name="type">滤波类型</param>
        public void ComponentFilter(Bitmap srcImage, RateFilterType type)
        {
            switch (type)
            {
                case RateFilterType.LowPass:
                    this.LowpassFilter(srcImage, 0);
                    break;
                case RateFilterType.BandStop:
                    this.bandstopFilter(srcImage, 0, 0);
                    break;
                case RateFilterType.BandPass:
                    this.bandpassFilter(srcImage, 0, 0);
                    break;
                case RateFilterType.HighPass:
                    this.highpassFilter(srcImage, 0);
                    break;
            }
        }

        /// <summary>
        ///  低通滤波
        /// </summary>
        /// <param name="radius">大于该边界都不可通过</param>
        public void LowpassFilter(Bitmap srcImage, Int32 radius)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);
            Int32 minLen = System.Math.Min(RealWidth / 3, Height);
            if (radius == 0) radius = RateFilterRadius.LowPass * minLen / 100;

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;

                Double distance = (Double)((col - HalfWidth) * (col - HalfWidth) + (row - HalfHeight) * (row - HalfHeight));
                distance = System.Math.Sqrt(distance);

                // 大于低通边界的都赋为0
                if (distance > radius)
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }

            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  带阻滤波
        /// </summary>
        /// <param name="innerRadius">带阻的内边界</param>
        /// <param name="outerRadius">带阻的外边界</param>
        private void bandstopFilter(Bitmap srcImage, Int32 innerRadius, Int32 outerRadius)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);
            Int32 minLen = System.Math.Min(RealWidth / 3, Height);
            if (innerRadius == 0 && outerRadius == 0)
            {
                outerRadius = RateFilterRadius.BandStopOuter * minLen / 100;
                innerRadius = RateFilterRadius.BandStopInner * minLen / 100;
            }

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;

                Double distance = (Double)((col - HalfWidth) * (col - HalfWidth) + (row - HalfHeight) * (row - HalfHeight));
                distance = System.Math.Sqrt(distance);

                if (distance < outerRadius && distance > innerRadius)
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }

            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  带通滤波
        /// </summary>
        /// <param name="innerRadius">带通内边界</param>
        /// <param name="outerRadius">带通外边界</param>
        private void bandpassFilter(Bitmap srcImage, Int32 innerRadius, Int32 outerRadius)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);
            Int32 minLen = System.Math.Min(RealWidth / 3, Height);
            if (innerRadius == 0 && outerRadius == 0)
            {
                innerRadius = RateFilterRadius.BandPassInner * minLen / 100;
                outerRadius = RateFilterRadius.BandPassOuter * minLen / 100;
            }

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;

                Double distance = (Double)((col - HalfWidth) * (col - HalfWidth) + (row - HalfHeight) * (row - HalfHeight));
                distance = System.Math.Sqrt(distance);

                if (distance < innerRadius || distance > outerRadius)
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }

            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  高通滤波
        /// </summary>
        /// <param name="radius">小于该边界都不可通过</param>
        private void highpassFilter(Bitmap srcImage, Int32 radius)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);
            Int32 minLen = System.Math.Min(RealWidth / 3, Height);
            if (radius == 0) radius = RateFilterRadius.HighPass * minLen / 100;

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;

                Double distance = (Double)((col - HalfWidth) * (col - HalfWidth) + (row - HalfHeight) * (row - HalfHeight));
                distance = System.Math.Sqrt(distance);

                if (distance < radius)
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }

            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }

        #endregion

        #region 方位滤波

        /// <summary>
        ///  方位滤波
        /// </summary>
        /// <param name="startOrient">起始方位</param>
        /// <param name="endOrient">结束方位</param>
        public void OrientationFilter(Bitmap srcImage, Int32 startOrient, Int32 endOrient)
        {
            Debug.Assert(endOrient > startOrient, "终止角度必须大于起始角度");
            if (endOrient < startOrient) return;

            Debug.Assert(endOrient - startOrient < 90, "起始角度与终止角度之间不能大于90");
            if (endOrient - startOrient > 90) return;

            if (endOrient <= 0)
            {
                this.orientLess0(srcImage, startOrient, endOrient);
            }
            else if (endOrient <= 90)
            {
                this.orientLess90(srcImage, startOrient, endOrient);
            }
            else if (endOrient <= 180)
            {
                this.orientLess180(srcImage, startOrient, endOrient);
            }
            else
            {
                this.orientMore180(srcImage, startOrient, endOrient);
            }
        }

        // 终止角度小于零
        private void orientLess0(Bitmap srcImage, Int32 startOrient, Int32 endOrient)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;
                Double currOrient = (System.Math.Atan2(HalfHeight - row, col - HalfWidth)) * 180 / System.Math.PI;

                if ((currOrient <= endOrient && currOrient >= startOrient) || (currOrient <= (endOrient + 180) && currOrient >= (startOrient + 180)))
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }
            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        // 终止角度小于90
        private void orientLess90(Bitmap srcImage, Int32 startOrient, Int32 endOrient)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;
                Double currOrient = (System.Math.Atan2(HalfHeight - row, col - HalfWidth)) * 180 / System.Math.PI;

                if ((currOrient <= endOrient && currOrient >= startOrient) || (currOrient <= (endOrient - 180) && currOrient > (startOrient + 180)))
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }
            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        // 终止角度小于180
        private void orientLess180(Bitmap srcImage, Int32 startOrient, Int32 endOrient)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;
                Double currOrient = (System.Math.Atan2(HalfHeight - row, col - HalfWidth)) * 180 / System.Math.PI;

                if ((currOrient <= endOrient && currOrient >= startOrient) || (currOrient <= endOrient - 180) && (currOrient >= startOrient - 180))
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }
            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        // 终止角度大于180
        private void orientMore180(Bitmap srcImage, Int32 startOrient, Int32 endOrient)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Complex[] freDom = this.FFT(rgbBytes, Width, RealWidth, Height, true);

            for (int i = 0; i < Length; i += 3)
            {
                if (i % Width >= RealWidth) continue;

                Int32 col = i % Width;
                Int32 row = i / Width;
                Double currOrient = (System.Math.Atan2(HalfHeight - row, col - HalfWidth)) * 180 / System.Math.PI;

                if (((currOrient <= endOrient - 180) && (currOrient >= startOrient - 180)) ||
                    (currOrient <= endOrient - 360 && currOrient >= -180) ||
                    (currOrient >= startOrient && currOrient <= 180))
                {
                    freDom[i] = new Complex(0.0, 0.0);
                    freDom[i + 1] = new Complex(0.0, 0.0);
                    freDom[i + 2] = new Complex(0.0, 0.0);
                }
            }
            Byte[] tempArray = this.IFFT(freDom, Width, RealWidth, Height, true);
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 均值滤波

        /// <summary>
        ///  均值滤波
        /// </summary>
        /// <param name="model">模板类型</param>
        public void MeanFilter(Bitmap srcImage, TemplateModel model)
        {
            switch (model)
            {
                case TemplateModel.T3x3:
                    this.meanFilter3x3(srcImage);
                    break;
                case TemplateModel.T5x5:
                    this.meanFilter5x5(srcImage);
                    break;
                case TemplateModel.T7x7:
                    this.meanFilter7x7(srcImage);
                    break;
            }
        }
        // 均值滤波3x3模板
        private void meanFilter3x3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;

                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Int32[] set = getRectangle3x3(i, Width);
                Int32 aver = 0, aveg = 0, aveb = 0;
                for (int j = 0; j < set.Length; j += 3)
                {
                    aveb += rgbBytes[set[j]];
                    aveg += rgbBytes[set[j + 1]];
                    aver += rgbBytes[set[j + 2]];
                }
                aveb /= 9;
                aveg /= 9;
                aver /= 9;

                aveb = aveb > 255 ? 255 : aveb < 0 ? 0 : aveb;
                aveg = aveg > 255 ? 255 : aveg < 0 ? 0 : aveg;
                aver = aver > 255 ? 255 : aver < 0 ? 0 : aver;

                rgbBytes[i] = Convert.ToByte(aveb);
                rgbBytes[i + 1] = Convert.ToByte(aveg);
                rgbBytes[i + 2] = Convert.ToByte(aver);
            }

            UnlockBits(rgbBytes);
        }
        // 均值滤波5x5模板
        private void meanFilter5x5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;
                if (row == Height - 2) break;
                if (row < 2) { i = Width * 2; continue; }
                if (col < 6) continue;
                if (col > RealWidth - 9) { col = row * Width + 3; continue; }

                Int32[] set = getRectangle5x5(i, Width);
                Int32 aver = 0, aveg = 0, aveb = 0;
                for (int j = 0; j < set.Length; j += 3)
                {
                    aveb += rgbBytes[set[j]];
                    aveg += rgbBytes[set[j + 1]];
                    aver += rgbBytes[set[j + 2]];
                }

                aveb /= 25;
                aveg /= 25;
                aver /= 25;

                aveb = aveb > 255 ? 255 : aveb < 0 ? 0 : aveb;
                aveg = aveg > 255 ? 255 : aveg < 0 ? 0 : aveg;
                aver = aver > 255 ? 255 : aver < 0 ? 0 : aver;

                rgbBytes[i] = Convert.ToByte(aveb);
                rgbBytes[i + 1] = Convert.ToByte(aveg);
                rgbBytes[i + 2] = Convert.ToByte(aver);
            }

            UnlockBits(rgbBytes);
        }
        // 均值滤波7x7模板
        private void meanFilter7x7(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;
                if (row == Height - 3) break;
                if (row < 3) { i = Width * 3; continue; }
                if (col < 9) continue;
                if (col > RealWidth - 11) { col = row * Width + 6; continue; }

                Int32[] set = getRectangle7x7(i, Width);
                Int32 aver = 0, aveg = 0, aveb = 0;
                for (int j = 0; j < set.Length; j += 3)
                {
                    aveb += rgbBytes[set[j]];
                    aveg += rgbBytes[set[j + 1]];
                    aver += rgbBytes[set[j + 2]];
                }

                aveb /= 49;
                aveg /= 49;
                aver /= 49;

                aveb = aveb > 255 ? 255 : aveb < 0 ? 0 : aveb;
                aveg = aveg > 255 ? 255 : aveg < 0 ? 0 : aveg;
                aver = aver > 255 ? 255 : aver < 0 ? 0 : aver;

                rgbBytes[i] = Convert.ToByte(aveb);
                rgbBytes[i + 1] = Convert.ToByte(aveg);
                rgbBytes[i + 2] = Convert.ToByte(aver);
            }

            UnlockBits(rgbBytes);
        }

        #endregion

        #region 中值滤波

        /// <summary>
        ///  中值滤波
        /// </summary>
        /// <param name="model">模板类型</param>
        public void MedianFilter(Bitmap srcImage, TemplateModel model)
        {
            switch (model)
            {
                case TemplateModel.T3x3:
                    this.medianFilter3x3(srcImage);
                    break;
                case TemplateModel.T5x5:
                    this.medianFilter5x5(srcImage);
                    break;
                case TemplateModel.T7x7:
                    this.medianFilter7x7(srcImage);
                    break;
            }
        }
        // 中值滤波3x3模板
        private void medianFilter3x3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            IList<Int32> listr = new List<Int32>();
            IList<Int32> listg = new List<Int32>();
            IList<Int32> listb = new List<Int32>();
            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;

                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Int32[] set = getRectangle3x3(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    listb.Add(rgbBytes[set[j]]);
                    listg.Add(rgbBytes[set[j + 1]]);
                    listr.Add(rgbBytes[set[j + 2]]);
                }
                mathHelper.BubbleSort(listb, SortMode.Ascending);
                mathHelper.BubbleSort(listg, SortMode.Ascending);
                mathHelper.BubbleSort(listr, SortMode.Ascending);

                Int32 medianb = listb[4];
                Int32 mediang = listg[4];
                Int32 medianr = listr[4];

                medianb = medianb > 255 ? 255 : medianb < 0 ? 0 : medianb;
                mediang = mediang > 255 ? 255 : mediang < 0 ? 0 : mediang;
                medianr = medianr > 255 ? 255 : medianr < 0 ? 0 : medianr;

                rgbBytes[i] = Convert.ToByte(medianb);
                rgbBytes[i + 1] = Convert.ToByte(mediang);
                rgbBytes[i + 2] = Convert.ToByte(medianr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            UnlockBits(rgbBytes);
        }
        // 中值滤波5x5模板
        private void medianFilter5x5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            IList<Int32> listr = new List<Int32>();
            IList<Int32> listg = new List<Int32>();
            IList<Int32> listb = new List<Int32>();
            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;
                if (row == Height - 2) break;
                if (row < 2) { i = Width * 2; continue; }
                if (col < 6) continue;
                if (col > RealWidth - 9) { col = row * Width + 3; continue; }

                Int32[] set = getRectangle5x5(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    listb.Add(rgbBytes[set[j]]);
                    listg.Add(rgbBytes[set[j + 1]]);
                    listr.Add(rgbBytes[set[j + 2]]);
                }
                mathHelper.BubbleSort(listb, SortMode.Ascending);
                mathHelper.BubbleSort(listg, SortMode.Ascending);
                mathHelper.BubbleSort(listr, SortMode.Ascending);

                Int32 medianb = listb[12];
                Int32 mediang = listg[12];
                Int32 medianr = listr[12];

                medianb = medianb > 255 ? 255 : medianb < 0 ? 0 : medianb;
                mediang = mediang > 255 ? 255 : mediang < 0 ? 0 : mediang;
                medianr = medianr > 255 ? 255 : medianr < 0 ? 0 : medianr;

                rgbBytes[i] = Convert.ToByte(medianb);
                rgbBytes[i + 1] = Convert.ToByte(mediang);
                rgbBytes[i + 2] = Convert.ToByte(medianr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            UnlockBits(rgbBytes);
        }
        // 中值滤波7x7模板
        private void medianFilter7x7(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            IList<Int32> listr = new List<Int32>();
            IList<Int32> listg = new List<Int32>();
            IList<Int32> listb = new List<Int32>();
            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;
                if (row == Height - 3) break;
                if (row < 3) { i = Width * 3; continue; }
                if (col < 9) continue;
                if (col > RealWidth - 11) { col = row * Width + 6; continue; }

                Int32[] set = getRectangle7x7(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    listb.Add(rgbBytes[set[j]]);
                    listg.Add(rgbBytes[set[j + 1]]);
                    listr.Add(rgbBytes[set[j + 2]]);
                }
                mathHelper.BubbleSort(listb, SortMode.Ascending);
                mathHelper.BubbleSort(listg, SortMode.Ascending);
                mathHelper.BubbleSort(listr, SortMode.Ascending);

                Int32 medianb = listb[24];
                Int32 mediang = listg[24];
                Int32 medianr = listr[24];

                medianb = medianb > 255 ? 255 : medianb < 0 ? 0 : medianb;
                mediang = mediang > 255 ? 255 : mediang < 0 ? 0 : mediang;
                medianr = medianr > 255 ? 255 : medianr < 0 ? 0 : medianr;

                rgbBytes[i] = Convert.ToByte(medianb);
                rgbBytes[i + 1] = Convert.ToByte(mediang);
                rgbBytes[i + 2] = Convert.ToByte(medianr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 小波

        /// <summary>
        ///  二维小波变换
        /// </summary>
        /// <param name="lowpassType">低通滤波类型</param>
        /// <param name="series">小波变换级数的参数</param>
        public void Wavelet(Bitmap srcImage, WaveletLowpassType lowpassType, Boolean hardThreshold, Byte thresholding, Int32 series)
        {
            //Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            //Int32 singleWidth = RealWidth / 3;
            //Double[] scaler = new Double[singleWidth * Height];
            //Double[] scaleg = new Double[singleWidth * Height];
            //Double[] scaleb = new Double[singleWidth * Height];

            //for (int i = 0; i < Height; i++)
            //{
            //    for (int j = 0; j < singleWidth; j++)
            //    {
            //        scaleb[i * Height + j] = Convert.ToDouble(rgbBytes[(i * Height + j) * 3]);
            //        scaleg[i * Height + j] = Convert.ToDouble(rgbBytes[(i * Height + j) * 3 + 1]);
            //        scaler[i * Height + j] = Convert.ToDouble(rgbBytes[(i * Height + j) * 3 + 2]);
            //    }
            //}

            //#region 设置滤波器参数
            //// 低通滤波器
            //Double[] lowpassFilter = null;
            //switch (lowpassType)
            //{
            //    case WaveletLowpassType.Haar:
            //        lowpassFilter = WaveletLowpass.Haar;
            //        break;
            //    case WaveletLowpassType.Daubechies2:
            //        lowpassFilter = WaveletLowpass.Daubechies2;
            //        break;
            //    case WaveletLowpassType.Daubechies3:
            //        lowpassFilter = WaveletLowpass.Daubechies3;
            //        break;
            //    case WaveletLowpassType.Daubechies4:
            //        lowpassFilter = WaveletLowpass.Daubechies4;
            //        break;
            //    case WaveletLowpassType.Daubechies5:
            //        lowpassFilter = WaveletLowpass.Daubechies5;
            //        break;
            //    case WaveletLowpassType.Daubechies6:
            //        lowpassFilter = WaveletLowpass.Daubechies6;
            //        break;
            //    default:
            //        Debug.Fail("低通滤波器的类型选择无效");
            //        break;
            //}

            //// 高通滤波器
            //Double[] highpassFilter = new Double[lowpassFilter.Length];
            //for (int i = 0; i < lowpassFilter.Length; i++)
            //{
            //    highpassFilter[i] = System.Math.Pow(-1, i) * lowpassFilter[lowpassFilter.Length - 1 - i];
            //}
            //#endregion

            //#region 小波变换
            //Double[] tempb = new Double[singleWidth * Height];
            //Double[] tempg = new Double[singleWidth * Height];
            //Double[] tempr = new Double[singleWidth * Height];
            //for (int k = 0; k < series; k++)
            //{
            //    Int32 coef = (Int32)System.Math.Pow(2, k);
            //    for (int i = 0; i < Height; i++)
            //    {
            //        if (i < Height / coef)
            //        {
            //            for (int j = 0; j < singleWidth; j++)
            //            {
            //                if (j < singleWidth / coef)
            //                {
            //                    tempb[i * singleWidth / coef + j] = scaleb[i * singleWidth + j];
            //                    tempg[i * singleWidth / coef + j] = scaleg[i * singleWidth + j];
            //                    tempr[i * singleWidth / coef + j] = scaler[i * singleWidth + j];
            //                }
            //            }
            //        }
            //    }
            //    mathHelper.Wavelet2D(ref tempb, singleWidth, Height, lowpassFilter, highpassFilter, series);
            //    mathHelper.Wavelet2D(ref tempg, singleWidth, Height, lowpassFilter, highpassFilter, series);
            //    mathHelper.Wavelet2D(ref tempr, singleWidth, Height, lowpassFilter, highpassFilter, series);

            //    for (int i = 0; i < Height; i++)
            //    {
            //        if (i < Height / coef)
            //        {
            //            for (int j = 0; j < singleWidth; j++)
            //            {
            //                if (j < singleWidth / coef)
            //                {
            //                    scaleb[i * singleWidth + j] = tempb[i * singleWidth / coef + j];
            //                    scaleg[i * singleWidth + j] = tempg[i * singleWidth / coef + j];
            //                    scaler[i * singleWidth + j] = tempr[i * singleWidth / coef + j];
            //                }
            //            }
            //        }
            //    }

            //    #region 阈值处理
            //    // 硬阈值
            //    if (hardThreshold)
            //    {
            //        for (int i = 0; i < singleWidth; i++)
            //        {
            //            if (scaleb[i] < thresholding && scaleb[i] > -thresholding)
            //                scaleb[i] = 0;
            //            if (scaleg[i] < thresholding && scaleg[i] > -thresholding)
            //                scaleg[i] = 0;
            //            if (scaler[i] < thresholding && scaler[i] > -thresholding)
            //                scaler[i] = 0;
            //        }
            //    }
            //    // 软阈值
            //    else
            //    {
            //        for (int i = 0; i < singleWidth; i++)
            //        {
            //            if (scaleb[i] >= thresholding)
            //                scaleb[i] = scaleb[i] - thresholding;
            //            else
            //            {
            //                if (scaleb[i] <= -thresholding)
            //                    scaleb[i] = scaleb[i] + thresholding;
            //                else
            //                    scaleb[i] = 0;
            //            }
            //            if (scaleg[i] >= thresholding)
            //                scaleg[i] = scaleg[i] - thresholding;
            //            else
            //            {
            //                if (scaleg[i] <= -thresholding)
            //                    scaleg[i] = scaleg[i] + thresholding;
            //                else
            //                    scaleg[i] = 0;
            //            }
            //            if (scaler[i] >= thresholding)
            //                scaler[i] = scaler[i] - thresholding;
            //            else
            //            {
            //                if (scaler[i] <= -thresholding)
            //                    scaler[i] = scaler[i] + thresholding;
            //                else
            //                    scaler[i] = 0;
            //            }
            //        }
            //    }
            //    #endregion
            //}
            //#endregion

            //#region 小波逆变换
            //for (int k = series - 1; k >= 0; k--)
            //{
            //    Int32 coef = (Int32)System.Math.Pow(2, k);
            //    for (int i = 0; i < Height; i++)
            //    {
            //        if (i < Height / coef)
            //        {
            //            for (int j = 0; j < singleWidth; j++)
            //            {
            //                if (j < singleWidth / coef)
            //                {
            //                    tempb[i * singleWidth / coef + j] = scaleb[i * singleWidth + j];
            //                    tempg[i * singleWidth / coef + j] = scaleg[i * singleWidth + j];
            //                    tempr[i * singleWidth / coef + j] = scaler[i * singleWidth + j];
            //                }
            //            }
            //        }
            //    }
            //    mathHelper.IWavelet2D(ref tempb, singleWidth, Height, lowpassFilter, highpassFilter, series);
            //    mathHelper.IWavelet2D(ref tempg, singleWidth, Height, lowpassFilter, highpassFilter, series);
            //    mathHelper.IWavelet2D(ref tempr, singleWidth, Height, lowpassFilter, highpassFilter, series);
            //    for (int i = 0; i < Height; i++)
            //    {
            //        if (i < Height / coef)
            //        {
            //            for (int j = 0; j < singleWidth; j++)
            //            {
            //                if (j < singleWidth / coef)
            //                {
            //                    scaleb[i * singleWidth + j] = tempb[i * singleWidth / coef + j];
            //                    scaleg[i * singleWidth + j] = tempg[i * singleWidth / coef + j];
            //                    scaler[i * singleWidth + j] = tempr[i * singleWidth / coef + j];
            //                }
            //            }
            //        }
            //    }


            //}
            //#endregion

            //for (int i = 0; i < Height; i++)
            //{
            //    for (int j = 0; j < singleWidth; j++)
            //    {
            //        scaleb[i * Height + j] = scaleb[i * Height + j] > 255 ? 255 : scaleb[i * Height + j] < 0 ? 0 : scaleb[i * Height + j];
            //        scaleg[i * Height + j] = scaleg[i * Height + j] > 255 ? 255 : scaleg[i * Height + j] < 0 ? 0 : scaleg[i * Height + j];
            //        scaler[i * Height + j] = scaler[i * Height + j] > 255 ? 255 : scaler[i * Height + j] < 0 ? 0 : scaler[i * Height + j];

            //        rgbBytes[(i * Height + j) * 3] = Convert.ToByte(scaleb[i * Height + j]);
            //        rgbBytes[(i * Height + j) * 3 + 1] = Convert.ToByte(scaleg[i * Height + j]);
            //        rgbBytes[(i * Height + j) * 3 + 2] = Convert.ToByte(scaler[i * Height + j]);
            //    }
            //}

            //UnlockBits(rgbBytes);
        }

        #endregion

        #region 高斯

        /// <summary>
        ///  高斯滤波
        /// </summary>
        /// <param name="sigma">方差</param>
        public void GaussFilter(Bitmap srcImage, Double sigma)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            Int32 length = RealWidth / 3 * Height;
            Double[] tempb = new Double[length];
            Double[] tempg = new Double[length];
            Double[] tempr = new Double[length];
            for (int i = 0; i < length; i++)
            {
                tempb[i] = rgbBytes[i * 3];
                tempg[i] = rgbBytes[i * 3 + 1];
                tempr[i] = rgbBytes[i * 3 + 2];
            }

            tempb = base.GaussSmooth(tempb, Height, sigma);
            tempg = base.GaussSmooth(tempg, Height, sigma);
            tempr = base.GaussSmooth(tempr, Height, sigma);

            for (int i = 0; i < length; i++)
            {
                tempb[i] = tempb[i] > 255 ? 255 : tempb[i] < 0 ? 0 : tempb[i];
                tempg[i] = tempg[i] > 255 ? 255 : tempg[i] < 0 ? 0 : tempg[i];
                tempr[i] = tempr[i] > 255 ? 255 : tempr[i] < 0 ? 0 : tempr[i];

                rgbBytes[i * 3] = Convert.ToByte(tempb[i]);
                rgbBytes[i * 3 + 1] = Convert.ToByte(tempg[i]);
                rgbBytes[i * 3 + 2] = Convert.ToByte(tempr[i]);
            }

            UnlockBits(rgbBytes);
        }

        #endregion

        #region 统计方法

        /// <summary>
        ///  统计方法滤波
        /// </summary>
        /// <param name="model">模板类型</param>
        /// <param name="thresholding">阀值</param>
        public void StatisticFilter(Bitmap srcImage, TemplateModel model, Double thresholding)
        {
            switch (model)
            {
                case TemplateModel.T3x3:
                    this.statisticFilter3x3(srcImage, thresholding);
                    break;
                case TemplateModel.T5x5:
                    this.statisticFilter5x5(srcImage, thresholding);
                    break;
                default:
                    Debug.Fail("必须选择边长为3,5的模板");
                    break;
            }
        }
        // 统计过滤3x3模板
        private void statisticFilter3x3(Bitmap srcImage, Double thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = (Byte[])rgbBytes.Clone();

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;
                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Double mub = 0, mug = 0, mur = 0;
                Double sigmab = 0, sigmag = 0, sigmar = 0;
                Int32[] set = getRectangle3x3(i, Width);
                #region 计算mu和sigma
                for (int j = 0; j < set.Length; j += 3)
                {
                    mub += rgbBytes[set[j]] / 9;
                    mug += rgbBytes[set[j + 1]] / 9;
                    mur += rgbBytes[set[j + 2]] / 9;
                }

                for (int j = 0; j < set.Length; j += 3)
                {
                    sigmab += System.Math.Pow((rgbBytes[set[j]] - mub), 2) / 9;
                    sigmag += System.Math.Pow((rgbBytes[set[j + 1]] - mug), 2) / 9;
                    sigmar += System.Math.Pow((rgbBytes[set[j + 2]] - mur), 2) / 9;
                }

                sigmab = System.Math.Sqrt(sigmab);
                sigmag = System.Math.Sqrt(sigmag);
                sigmar = System.Math.Sqrt(sigmar);
                #endregion
                #region 过滤赋值
                if (System.Math.Abs(rgbBytes[i] - mub) < sigmab * thresholding)
                    tempArray[i] = rgbBytes[i];
                else
                {
                    tempArray[i] = Convert.ToByte(mub);
                }

                if (System.Math.Abs(rgbBytes[i + 1] - mug) < sigmag * thresholding)
                    tempArray[i + 1] = rgbBytes[i + 1];
                else
                {
                    tempArray[i + 1] = Convert.ToByte(mug);
                }

                if (System.Math.Abs(rgbBytes[i + 2] - mur) < sigmar * thresholding)
                    tempArray[i + 2] = rgbBytes[i + 2];
                else
                {
                    tempArray[i + 2] = Convert.ToByte(mur);
                }
                #endregion
            }

            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 统计过滤5x5模板
        private void statisticFilter5x5(Bitmap srcImage, Double thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = (Byte[])rgbBytes.Clone();

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;
                if (row == Height - 2) break;
                if (row < 2) { i = Width * 2; continue; }
                if (col < 6) continue;
                if (col > RealWidth - 9) { col = row * Width + 3; continue; }

                Int32[] set = getRectangle5x5(i, Width);
                Double mub = 0, mug = 0, mur = 0;
                Double sigmab = 0, sigmag = 0, sigmar = 0;
                #region 计算mu和sigma
                for (int j = 0; j < set.Length; j += 3)
                {
                    mub += rgbBytes[set[j]] / 25;
                    mug += rgbBytes[set[j + 1]] / 25;
                    mur += rgbBytes[set[j + 2]] / 25;
                }

                for (int j = 0; j < set.Length; j += 3)
                {
                    sigmab += System.Math.Pow((rgbBytes[set[j]] - mub), 2) / 25;
                    sigmag += System.Math.Pow((rgbBytes[set[j + 1]] - mug), 2) / 25;
                    sigmar += System.Math.Pow((rgbBytes[set[j + 2]] - mur), 2) / 25;
                }

                sigmab = System.Math.Sqrt(sigmab);
                sigmag = System.Math.Sqrt(sigmag);
                sigmar = System.Math.Sqrt(sigmar);
                #endregion
                #region 过滤赋值
                if (System.Math.Abs(rgbBytes[i] - mub) < sigmab * thresholding)
                    tempArray[i] = rgbBytes[i];
                else
                {
                    tempArray[i] = Convert.ToByte(mub);
                }

                if (System.Math.Abs(rgbBytes[i + 1] - mug) < sigmag * thresholding)
                    tempArray[i + 1] = rgbBytes[i + 1];
                else
                {
                    tempArray[i + 1] = Convert.ToByte(mug);
                }

                if (System.Math.Abs(rgbBytes[i + 2] - mur) < sigmar * thresholding)
                    tempArray[i + 2] = rgbBytes[i + 2];
                else
                {
                    tempArray[i + 2] = Convert.ToByte(mur);
                }
                #endregion

            }

            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        #endregion
    }
}
