using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.Generic;
using ValueHelper.Image.Interface;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Bit24
{
    /// <summary>
    ///  图像去噪
    /// </summary>
    public partial class ImageBit24 : IDislodgeNoise
    {
        #region 腐蚀

        /// <summary>
        ///  腐蚀
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="type"></param>
        public void Erode(Bitmap srcImage, FilterWindowType type)
        {
            if (srcImage.PixelFormat != PixelFormat.Format24bppRgb)
                return;

            switch (type)
            {
                case FilterWindowType.Hori3:
                    this.ErodeHori3(srcImage);
                    break;
                case FilterWindowType.Vert3:
                    this.ErodeVert3(srcImage);
                    break;
                case FilterWindowType.Cros3:
                    this.ErodeCros3(srcImage);
                    break;
                case FilterWindowType.Rect3:
                    this.ErodeRect3(srcImage);
                    break;
                case FilterWindowType.Hori5:
                    this.ErodeHori5(srcImage);
                    break;
                case FilterWindowType.Vert5:
                    this.ErodeVert5(srcImage);
                    break;
                case FilterWindowType.Cros5:
                    this.ErodeCros5(srcImage);
                    break;
                case FilterWindowType.Rect5:
                    this.ErodeRect5(srcImage);
                    break;
            }
        }
        // 3位水平腐蚀
        private void ErodeHori3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            if (RealWidth == 0) return;

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }
            if (srcImage.PixelFormat == PixelFormat.Format24bppRgb)
            {
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
                    var result = set.Where(x => rgbBytes[x] != 0);
                    if (result.Count() == 0)
                    {
                        tempArray[i] = 0;
                        tempArray[i + 1] = 0;
                        tempArray[i + 2] = 0;
                    }
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }
        // 3位垂直腐蚀
        private void ErodeVert3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 3位十字腐蚀
        private void ErodeCros3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    rgbBytes[i] = 0;
                    rgbBytes[i + 1] = 0;
                    rgbBytes[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 3位方形腐蚀
        private void ErodeRect3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位水平腐蚀
        private void ErodeHori5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位垂直腐蚀
        private void ErodeVert5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位十字腐蚀
        private void ErodeCros5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位方形腐蚀
        private void ErodeRect5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] != 0);
                if (result.Count() == 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  灰度形态学腐蚀
        /// </summary>
        /// <param name="template">模板集合</param>
        public void GrayErode(Bitmap srcImage, Byte[] template)
        {
            Debug.Assert(template.Where(x => x == 1 || x == 255).Count() == template.Length, "模板只能为255或1");
            if (template.Where(x => x == 1 || x == 255).Count() != template.Length) return;
            switch (template.Length)
            {
                case 9:
                    this.grayErode3x3(srcImage, template);
                    break;
                case 25:
                    this.grayErode5x5(srcImage, template);
                    break;
                case 49:
                    this.grayErode7x7(srcImage, template);
                    break;
                default:
                    Debug.Fail("模板的边长只能为3,5,7");
                    break;
            }
        }
        // 灰度形态学腐蚀3x3模板
        private void grayErode3x3(Bitmap srcImage, Byte[] template)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            List<Int32> listb = new List<int>();
            List<Int32> listg = new List<int>();
            List<Int32> listr = new List<int>();

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
                    listb.Add(rgbBytes[set[j]] * template[j / 3]);
                    listg.Add(rgbBytes[set[j + 1]] * template[j / 3]);
                    listr.Add(rgbBytes[set[j + 2]] * template[j / 3]);
                }
                Int32 minb = mathHelper.Min(listb.ToArray());
                Int32 ming = mathHelper.Min(listg.ToArray());
                Int32 minr = mathHelper.Min(listr.ToArray());

                tempBytes[i] = Convert.ToByte(minb);
                tempBytes[i + 1] = Convert.ToByte(ming);
                tempBytes[i + 2] = Convert.ToByte(minr);
                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            rgbBytes = (Byte[])tempBytes.Clone();
            UnlockBits(rgbBytes);
        }
        // 灰度形态学腐蚀5x5模板
        private void grayErode5x5(Bitmap srcImage, Byte[] template)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            List<Int32> listb = new List<int>();
            List<Int32> listg = new List<int>();
            List<Int32> listr = new List<int>();

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
                    listb.Add(rgbBytes[set[j]] * template[j / 3]);
                    listg.Add(rgbBytes[set[j + 1]] * template[j / 3]);
                    listr.Add(rgbBytes[set[j + 2]] * template[j / 3]);
                }

                Int32 minb = mathHelper.Min(listb.ToArray());
                Int32 ming = mathHelper.Min(listg.ToArray());
                Int32 minr = mathHelper.Min(listr.ToArray());
                tempBytes[i] = Convert.ToByte(minb);
                tempBytes[i + 1] = Convert.ToByte(ming);
                tempBytes[i + 2] = Convert.ToByte(minr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }

            rgbBytes = (Byte[])tempBytes.Clone();
            UnlockBits(rgbBytes);
        }
        // 灰度形态学腐蚀5x5模板
        private void grayErode7x7(Bitmap srcImage, Byte[] template)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            List<Int32> listb = new List<int>();
            List<Int32> listg = new List<int>();
            List<Int32> listr = new List<int>();

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
                    listb.Add(rgbBytes[set[j]] * template[j / 3]);
                    listg.Add(rgbBytes[set[j + 1]] * template[j / 3]);
                    listr.Add(rgbBytes[set[j + 2]] * template[j / 3]);
                }

                Int32 minb = mathHelper.Min(listb.ToArray());
                Int32 ming = mathHelper.Min(listg.ToArray());
                Int32 minr = mathHelper.Min(listr.ToArray());
                tempBytes[i] = Convert.ToByte(minb);
                tempBytes[i + 1] = Convert.ToByte(ming);
                tempBytes[i + 2] = Convert.ToByte(minr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            rgbBytes = (Byte[])tempBytes.Clone();
            UnlockBits(rgbBytes);
        }

        #endregion

        #region 膨胀

        /// <summary>
        ///  膨胀处理
        /// </summary>
        /// <param name="type">滤窗格式</param>
        public void Delation(Bitmap srcImage, FilterWindowType type)
        {
            switch (type)
            {
                case FilterWindowType.Hori3:
                    this.DilateHori3(srcImage);
                    break;
                case FilterWindowType.Vert3:
                    this.DilateVert3(srcImage);
                    break;
                case FilterWindowType.Cros3:
                    this.DilateCros3(srcImage);
                    break;
                case FilterWindowType.Rect3:
                    this.DilateRect3(srcImage);
                    break;
                case FilterWindowType.Hori5:
                    this.DilateHori5(srcImage);
                    break;
                case FilterWindowType.Vert5:
                    this.DilateVert5(srcImage);
                    break;
                case FilterWindowType.Cros5:
                    this.DilateCros5(srcImage);
                    break;
                case FilterWindowType.Rect5:
                    this.DilateRect5(srcImage);
                    break;
            }
        }
        // 3位水平膨胀
        public void DilateHori3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            if (RealWidth == 0) return;

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
            }
            if (srcImage.PixelFormat == PixelFormat.Format24bppRgb)
            {
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
                    var result = set.Where(x => rgbBytes[x] == 0);
                    if (result.Count() > 0)
                    {
                        tempArray[i] = 0;
                        tempArray[i + 1] = 0;
                        tempArray[i + 2] = 0;
                    }
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();

            UnlockBits(rgbBytes);
        }
        // 3位垂直膨胀
        private void DilateVert3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 3位十字膨胀
        private void DilateCros3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    rgbBytes[i] = 0;
                    rgbBytes[i + 1] = 0;
                    rgbBytes[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 3位方形膨胀
        private void DilateRect3(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位水平膨胀
        private void DilateHori5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位垂直膨胀
        private void DilateVert5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位十字膨胀
        private void DilateCros5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }
        // 5位方形膨胀
        private void DilateRect5(Bitmap srcImage)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempArray = new Byte[Length];
            for (int i = 0; i < Length; i++)
            {
                tempArray[i] = 255;
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
                var result = set.Where(x => rgbBytes[x] == 0);
                if (result.Count() > 0)
                {
                    tempArray[i] = 0;
                    tempArray[i + 1] = 0;
                    tempArray[i + 2] = 0;
                }
            }
            rgbBytes = (Byte[])tempArray.Clone();
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  灰度形态学膨胀
        /// </summary>
        /// <param name="template">模板集合</param>
        public void GrayDelation(Bitmap srcImage, Byte[] template)
        {
            Debug.Assert(template.Where(x => x == 1 || x == 0).Count() == template.Length, "模板只能为0或1");
            if (template.Where(x => x == 1 || x == 0).Count() != template.Length) return;

            switch (template.Length)
            {
                case 9:
                    this.grayDelation3x3(srcImage, template);
                    break;
                case 25:
                    this.grayDelation5x5(srcImage, template);
                    break;
                case 49:
                    this.grayDelation7x7(srcImage, template);
                    break;
                default:
                    Debug.Fail("模板的变长只能为3,5,7");
                    break;
            }
        }
        // 灰度形态学膨胀3x3模板
        private void grayDelation3x3(Bitmap srcImage, Byte[] template)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            List<Int32> listb = new List<int>();
            List<Int32> listg = new List<int>();
            List<Int32> listr = new List<int>();

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
                    listb.Add(rgbBytes[set[j]] * template[j / 3]);
                    listg.Add(rgbBytes[set[j + 1]] * template[j / 3]);
                    listr.Add(rgbBytes[set[j + 2]] * template[j / 3]);
                }
                Int32 maxb = mathHelper.Max(listb.ToArray());
                Int32 maxg = mathHelper.Max(listg.ToArray());
                Int32 maxr = mathHelper.Max(listr.ToArray());

                tempBytes[i] = Convert.ToByte(maxb);
                tempBytes[i + 1] = Convert.ToByte(maxg);
                tempBytes[i + 2] = Convert.ToByte(maxr);
                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            rgbBytes = (Byte[])tempBytes.Clone();
            UnlockBits(rgbBytes);
        }
        // 灰度形态学膨胀5x5模板
        private void grayDelation5x5(Bitmap srcImage, Byte[] template)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            List<Int32> listb = new List<int>();
            List<Int32> listg = new List<int>();
            List<Int32> listr = new List<int>();

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
                    listb.Add(rgbBytes[set[j]] * template[j / 3]);
                    listg.Add(rgbBytes[set[j + 1]] * template[j / 3]);
                    listr.Add(rgbBytes[set[j + 2]] * template[j / 3]);
                }

                Int32 maxb = mathHelper.Max(listb.ToArray());
                Int32 maxg = mathHelper.Max(listg.ToArray());
                Int32 maxr = mathHelper.Max(listr.ToArray());
                tempBytes[i] = Convert.ToByte(maxb);
                tempBytes[i + 1] = Convert.ToByte(maxg);
                tempBytes[i + 2] = Convert.ToByte(maxr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }

            rgbBytes = (Byte[])tempBytes.Clone();
            UnlockBits(rgbBytes);
        }
        // 灰度形态学膨胀7x7模板
        private void grayDelation7x7(Bitmap srcImage, Byte[] template)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Byte[] tempBytes = (Byte[])rgbBytes.Clone();
            List<Int32> listb = new List<int>();
            List<Int32> listg = new List<int>();
            List<Int32> listr = new List<int>();

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
                    listb.Add(rgbBytes[set[j]] * template[j / 3]);
                    listg.Add(rgbBytes[set[j + 1]] * template[j / 3]);
                    listr.Add(rgbBytes[set[j + 2]] * template[j / 3]);
                }

                Int32 maxb = mathHelper.Max(listb.ToArray());
                Int32 maxg = mathHelper.Max(listg.ToArray());
                Int32 maxr = mathHelper.Max(listr.ToArray());
                tempBytes[i] = Convert.ToByte(maxb);
                tempBytes[i + 1] = Convert.ToByte(maxg);
                tempBytes[i + 2] = Convert.ToByte(maxr);

                listb.Clear();
                listg.Clear();
                listr.Clear();
            }
            rgbBytes = (Byte[])tempBytes.Clone();
            UnlockBits(rgbBytes);
        }
        #endregion

        #region 开运算

        /// <summary>
        ///  开运算
        /// </summary>
        /// <param name="type">滤窗的格式</param>
        public void Open(Bitmap srcImage, FilterWindowType type)
        {
            this.Erode(srcImage, type);
            this.Delation(srcImage, type);
        }

        /// <summary>
        ///  灰度开运算
        /// </summary>
        /// <param name="template">模板集合</param>
        public void GrayOpen(Bitmap srcImage, Byte[] template)
        {
            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] != 1)
                    template[i] = 255;
            }

            this.GrayErode(srcImage, template);

            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] != 1)
                    template[i] = 0;
            }

            this.GrayDelation(srcImage, template);
        }

        #endregion

        #region 闭运算

        /// <summary>
        ///  闭运算
        /// </summary>
        /// <param name="type">滤窗的格式</param>
        public void Close(Bitmap srcImage, FilterWindowType type)
        {
            this.Delation(srcImage, type);
            this.Erode(srcImage, type);
        }

        /// <summary>
        ///  闭运算
        /// </summary>
        /// <param name="template">模板集合</param>
        public void GrayClose(Bitmap srcImage, Byte[] template)
        {
            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] != 1)
                    template[i] = 0;
            }
            this.GrayDelation(srcImage, template);
            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] != 1)
                    template[i] = 255;
            }
            this.GrayErode(srcImage, template);
        }

        #endregion

        #region 灰度形态学

        public void GrayMorphologic(Bitmap srcImage, Byte[] template)
        {
            this.GrayClose(srcImage, template);
            this.GrayOpen(srcImage, template);
        }

        #endregion
    }
}
