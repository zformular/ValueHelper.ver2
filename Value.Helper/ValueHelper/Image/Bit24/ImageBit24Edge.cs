using System;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using ValueHelper.Image.Interface;
using ValueHelper.Image.Infrastructure;

namespace ValueHelper.Image.Bit24
{
    /// <summary>
    ///  边缘锐化
    /// </summary>
    public partial class ImageBit24 : IEdge
    {
        /// <summary>
        ///  运用算子边缘锐化
        /// </summary>
        /// <param name="type">算子类型</param>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        public void Mask(Bitmap srcImage, MaskType type, Int32 thresholding)
        {
            switch (type)
            {
                case MaskType.Roberts:
                    this.Roberts(srcImage, thresholding);
                    break;
                case MaskType.Prewitt:
                    this.Prewitt(srcImage, thresholding);
                    break;
                case MaskType.Sobel:
                    this.Sobel(srcImage, thresholding);
                    break;
                case MaskType.Laplacian1:
                    this.Laplacian(srcImage, thresholding, 1);
                    break;
                case MaskType.Laplacian2:
                    this.Laplacian(srcImage, thresholding, 2);
                    break;
                case MaskType.Laplacian3:
                    this.Laplacian(srcImage, thresholding, 3);
                    break;
                case MaskType.Kirsch:
                    this.Kirsch(srcImage, thresholding);
                    break;
                default:
                    Debug.Fail("模板算子不支持");
                    break;
            }
        }

        /// <summary>
        ///  Roberts算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        public void Roberts(Bitmap srcImage, Int32 thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double[] tempArray = new Double[Length];

            for (int i = Width; i < Length; i += 3)
            {
                if (i % Width > RealWidth - 6) continue;

                Double gradbX = 0, gradgX = 0, gradrX = 0;
                Double gradbY = 0, gradgY = 0, gradrY = 0;
                Int32[] set = this.getRectangle2x2(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    gradbX += rgbBytes[set[j]] * OperatorSet.robertOperatorX[j / 3];
                    gradgX += rgbBytes[set[j + 1]] * OperatorSet.robertOperatorX[j / 3];
                    gradrX += rgbBytes[set[j + 2]] * OperatorSet.robertOperatorX[j / 3];

                    gradbY += rgbBytes[set[j]] * OperatorSet.robertOperatorY[j / 3];
                    gradgY += rgbBytes[set[j + 1]] * OperatorSet.robertOperatorY[j / 3];
                    gradrY += rgbBytes[set[j + 2]] * OperatorSet.robertOperatorY[j / 3];
                }

                tempArray[i] = System.Math.Sqrt(gradbX * gradbX + gradbY * gradbY);
                tempArray[i + 1] = System.Math.Sqrt(gradgX * gradgX + gradgY * gradgY);
                tempArray[i + 2] = System.Math.Sqrt(gradrX * gradrX + gradrY * gradrY);
            }
            #region 是否进行二值化
            if (thresholding == 0)
            {
                // 不进行二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] < 0)
                        rgbBytes[i] = 0;
                    else
                    {
                        if (tempArray[i] > 255)
                            rgbBytes[i] = 255;
                        else
                            rgbBytes[i] = Convert.ToByte(tempArray[i]);
                    }
                }
            }
            else
            {
                // 二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] > thresholding)
                        rgbBytes[i] = 255;
                    else
                        rgbBytes[i] = 0;
                }
            }
            #endregion
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  Prewitt算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        public void Prewitt(Bitmap srcImage, Int32 thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double[] tempArray = new Double[Length];

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;

                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Double gradbX = 0, gradgX = 0, gradrX = 0;
                Double gradbY = 0, gradgY = 0, gradrY = 0;
                Int32[] set = getRectangle3x3(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    gradbX += rgbBytes[set[j]] * OperatorSet.prewittOperatorX[j / 3];
                    gradgX += rgbBytes[set[j + 1]] * OperatorSet.prewittOperatorX[j / 3];
                    gradrX += rgbBytes[set[j + 2]] * OperatorSet.prewittOperatorX[j / 3];

                    gradbY += rgbBytes[set[j]] * OperatorSet.prewittOperatorY[j / 3];
                    gradgY += rgbBytes[set[j + 1]] * OperatorSet.prewittOperatorY[j / 3];
                    gradrY += rgbBytes[set[j + 2]] * OperatorSet.prewittOperatorY[j / 3];
                }

                tempArray[i] = System.Math.Sqrt(gradbX * gradbX + gradbY * gradbY);
                tempArray[i + 1] = System.Math.Sqrt(gradgX * gradgX + gradgY * gradgY);
                tempArray[i + 2] = System.Math.Sqrt(gradrX * gradrX + gradrY * gradrY);
            }
            #region 是否进行二值化
            if (thresholding == 0)
            {
                // 不进行二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] < 0)
                        rgbBytes[i] = 0;
                    else
                    {
                        if (tempArray[i] > 255)
                            rgbBytes[i] = 255;
                        else
                            rgbBytes[i] = Convert.ToByte(tempArray[i]);
                    }
                }
            }
            else
            {
                // 二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] > thresholding)
                        rgbBytes[i] = 255;
                    else
                        rgbBytes[i] = 0;
                }
            }
            #endregion
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  Sobel算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        public void Sobel(Bitmap srcImage, Int32 thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double[] tempArray = new Double[Length];

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;

                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Double gradbX = 0, gradgX = 0, gradrX = 0;
                Double gradbY = 0, gradgY = 0, gradrY = 0;
                Int32[] set = getRectangle3x3(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    gradbX += rgbBytes[set[j]] * OperatorSet.sobelOperatorX[j / 3];
                    gradgX += rgbBytes[set[j + 1]] * OperatorSet.sobelOperatorX[j / 3];
                    gradrX += rgbBytes[set[j + 2]] * OperatorSet.sobelOperatorX[j / 3];

                    gradbY += rgbBytes[set[j]] * OperatorSet.sobelOperatorY[j / 3];
                    gradgY += rgbBytes[set[j + 1]] * OperatorSet.sobelOperatorY[j / 3];
                    gradrY += rgbBytes[set[j + 2]] * OperatorSet.sobelOperatorY[j / 3];
                }

                tempArray[i] = System.Math.Sqrt(gradbX * gradbX + gradbY * gradbY);
                tempArray[i + 1] = System.Math.Sqrt(gradgX * gradgX + gradgY * gradgY);
                tempArray[i + 2] = System.Math.Sqrt(gradrX * gradrX + gradrY * gradrY);
            }

            #region 是否进行二值化
            if (thresholding == 0)
            {
                // 不进行二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] < 0)
                        rgbBytes[i] = 0;
                    else
                    {
                        if (tempArray[i] > 255)
                            rgbBytes[i] = 255;
                        else
                            rgbBytes[i] = Convert.ToByte(tempArray[i]);
                    }
                }
            }
            else
            {
                // 二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] > thresholding)
                        rgbBytes[i] = 255;
                    else
                        rgbBytes[i] = 0;
                }
            }
            #endregion
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  拉普拉斯算子
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        /// <param name="number">拉普拉斯算子 1 2 3 </param>
        public void Laplacian(Bitmap srcImage, Int32 thresholding, Int32 number)
        {
            if (number > 3 || number < 1)
                Debug.Fail("只能实现拉普拉斯算子1,2,3");


            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double[] tempArray = new Double[Length];

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;

                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Double gradb = 0, gradg = 0, gradr = 0;
                Int32[] set = getRectangle3x3(i, Width);
                for (int j = 0; j < set.Length; j += 3)
                {
                    if (number == 1)
                    {
                        gradb += rgbBytes[set[j]] * OperatorSet.laplacianOperator1[j / 3];
                        gradg += rgbBytes[set[j + 1]] * OperatorSet.laplacianOperator1[j / 3];
                        gradr += rgbBytes[set[j + 2]] * OperatorSet.laplacianOperator1[j / 3];
                    }
                    else if (number == 2)
                    {
                        gradb += rgbBytes[set[j]] * OperatorSet.laplacianOperator2[j / 3];
                        gradg += rgbBytes[set[j + 1]] * OperatorSet.laplacianOperator2[j / 3];
                        gradr += rgbBytes[set[j + 2]] * OperatorSet.laplacianOperator2[j / 3];
                    }
                    else if (number == 3)
                    {
                        gradb += rgbBytes[set[j]] * OperatorSet.laplacianOperator3[j / 3];
                        gradg += rgbBytes[set[j + 1]] * OperatorSet.laplacianOperator3[j / 3];
                        gradr += rgbBytes[set[j + 2]] * OperatorSet.laplacianOperator3[j / 3];
                    }
                }
                tempArray[i] = gradb;
                tempArray[i + 1] = gradg;
                tempArray[i + 2] = gradr;
            }

            #region 是否进行二值化
            if (thresholding == 0)
            {
                // 不进行二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] < 0)
                        rgbBytes[i] = 0;
                    else
                    {
                        if (tempArray[i] > 255)
                            rgbBytes[i] = 255;
                        else
                            rgbBytes[i] = Convert.ToByte(tempArray[i]);
                    }
                }
            }
            else
            {
                // 二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] > thresholding)
                        rgbBytes[i] = 255;
                    else
                        rgbBytes[i] = 0;
                }
            }
            #endregion
            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  Kirsch算子锐化
        /// </summary>
        /// <param name="thresholding">阈值(为零的话不进行二值化)</param>
        public void Kirsch(Bitmap srcImage, Int32 thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double[] tempArray = new Double[Length];

            for (int i = 0; i < Length; i += 3)
            {
                Int32 row = i / Width;
                Int32 col = i % Width;

                if (row == (Height - 1)) break;
                if (row == 0) { i = Width; continue; }
                if (col == 0) continue;
                if (col >= RealWidth - 3) { col = row * Width - 3; continue; }

                Double[] gradbX = new Double[6], gradgX = new Double[6], gradrX = new Double[6];
                Int32[] set = getRectangle3x3(i, Width);
                #region 逐个计算所有算子,求的的最大值为理想结果
                for (int j = 0; j < set.Length; j += 3)
                {
                    gradbX[0] += rgbBytes[set[j]] * OperatorSet.kirschOperator1[j / 3];
                    gradgX[0] += rgbBytes[set[j + 1]] * OperatorSet.kirschOperator1[j / 3];
                    gradrX[0] += rgbBytes[set[j + 2]] * OperatorSet.kirschOperator1[j / 3];

                    gradbX[1] += rgbBytes[set[j]] * OperatorSet.kirschOperator2[j / 3];
                    gradgX[1] += rgbBytes[set[j + 1]] * OperatorSet.kirschOperator2[j / 3];
                    gradrX[1] += rgbBytes[set[j + 2]] * OperatorSet.kirschOperator2[j / 3];

                    gradbX[2] += rgbBytes[set[j]] * OperatorSet.kirschOperator3[j / 3];
                    gradgX[2] += rgbBytes[set[j + 1]] * OperatorSet.kirschOperator3[j / 3];
                    gradrX[2] += rgbBytes[set[j + 2]] * OperatorSet.kirschOperator3[j / 3];

                    gradbX[3] += rgbBytes[set[j]] * OperatorSet.kirschOperator4[j / 3];
                    gradgX[3] += rgbBytes[set[j + 1]] * OperatorSet.kirschOperator4[j / 3];
                    gradrX[3] += rgbBytes[set[j + 2]] * OperatorSet.kirschOperator4[j / 3];

                    gradbX[4] += rgbBytes[set[j]] * OperatorSet.kirschOperator5[j / 3];
                    gradgX[4] += rgbBytes[set[j + 1]] * OperatorSet.kirschOperator5[j / 3];
                    gradrX[4] += rgbBytes[set[j + 2]] * OperatorSet.kirschOperator5[j / 3];

                    gradbX[5] += rgbBytes[set[j]] * OperatorSet.kirschOperator6[j / 3];
                    gradgX[5] += rgbBytes[set[j + 1]] * OperatorSet.kirschOperator6[j / 3];
                    gradrX[5] += rgbBytes[set[j + 2]] * OperatorSet.kirschOperator6[j / 3];
                }

                tempArray[i] = mathHelper.Max(gradbX);
                tempArray[i + 1] = mathHelper.Max(gradgX);
                tempArray[i + 2] = mathHelper.Max(gradrX);
                #endregion
            }

            #region 是否进行二值化
            if (thresholding == 0)
            {
                // 不进行二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] < 0)
                        rgbBytes[i] = 0;
                    else
                    {
                        if (tempArray[i] > 255)
                            rgbBytes[i] = 255;
                        else
                            rgbBytes[i] = Convert.ToByte(tempArray[i]);
                    }
                }
            }
            else
            {
                // 二值化
                for (int i = 0; i < Length; i++)
                {
                    if (tempArray[i] > thresholding)
                        rgbBytes[i] = 255;
                    else
                        rgbBytes[i] = 0;
                }
            }
            #endregion
            UnlockBits(rgbBytes);
        }

        //public void Canny(Bitmap srcImage, Double sigma, Byte[] thresholding)
        //{
        //    Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);

        //    Int32 singleWidth = RealWidth / 3;
        //    Double[] tempb = new Double[singleWidth];
        //    Double[] tempg = new Double[singleWidth];
        //    Double[] tempr = new Double[singleWidth];
        //    for (int i = 0; i < singleWidth; i++)
        //    {
        //        tempb[i] = rgbBytes[i * 3];
        //        tempg[i] = rgbBytes[i * 3 + 1];
        //        tempr[i] = rgbBytes[i * 3 + 2];
        //    }

        //}

        private void canny(ref Byte[] rectangleData, Int32 width, Int32 height, Double sigma, Byte[] thresholding)
        {
            //Double gradX, gradY, angle;
            //Double[] tempData = new Double[rectangleData.Length];
            //Int32 rad = Convert.ToInt16(System.Math.Ceiling(3 * sigma));
            //for (int i = 0; i < rectangleData.Length; i++)
            //{
            //    tempData[i] = Convert.ToDouble(rectangleData);
            //}

            //Double[] tempArray = base.GaussSmooth(tempData, width, sigma);

            //for (int i = 0; i < height; i++)
            //{
            //    for (int j = 0; j < width; j++)
            //    {
            //        gradX= tempArray[]
            //    }
            //}
        }

        #region 高斯算子锐化

        /// <summary>
        ///  Gauss算子锐化
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="sigma"></param>
        /// <param name="thresholding"></param>
        public void Gauss(Bitmap srcImage, GaussFilterType type, Double sigma, Double thresholding)
        {
            Byte[] rgbBytes = LockBits(srcImage, ImageLockMode.ReadWrite);
            Double[] filt = null;
            if (type == GaussFilterType.LoG)
                filt = base.LogTemplate(sigma);
            else if (type == GaussFilterType.DoG)
                filt = base.DogTemplate(sigma);

            Int32 singleWidth = RealWidth / 3;
            Byte[] tempb = new Byte[singleWidth * Height];
            Byte[] tempg = new Byte[singleWidth * Height];
            Byte[] tempr = new Byte[singleWidth * Height];
            for (int i = 0; i < singleWidth * Height; i++)
            {
                tempb[i] = rgbBytes[i * 3];
                tempg[i] = rgbBytes[i * 3 + 1];
                tempr[i] = rgbBytes[i * 3 + 2];
            }
            Double[] doub, doug, dour;
            this.gaussConv(ref tempb, singleWidth, Height, filt, out doub);
            this.gaussConv(ref tempg, singleWidth, Height, filt, out doug);
            this.gaussConv(ref tempr, singleWidth, Height, filt, out dour);

            base.ZeroCross(ref doub, singleWidth, Height, thresholding, out tempb);
            base.ZeroCross(ref doug, singleWidth, Height, thresholding, out tempg);
            base.ZeroCross(ref dour, singleWidth, Height, thresholding, out tempr);

            for (int i = 0; i < singleWidth * Height; i++)
            {
                rgbBytes[i * 3] = tempb[i];
                rgbBytes[i * 3 + 1] = tempg[i];
                rgbBytes[i * 3 + 2] = tempr[i];
            }

            UnlockBits(rgbBytes);
        }

        /// <summary>
        ///  计算高斯卷积
        /// </summary>
        /// <param name="rectangleData">二维数据</param>
        /// <param name="width">数据宽度</param>
        /// <param name="height">数据高度</param>
        /// <param name="mask">掩膜</param>
        private void gaussConv(ref Byte[] rectangleData, Int32 width, Int32 height, Double[] mask, out Double[] result)
        {
            Int32 windWidth = Convert.ToInt16(System.Math.Sqrt(mask.Length));
            Int32 radius = windWidth / 2;
            Double temp;
            result = new Double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    temp = 0;
                    for (int x = -radius; x <= radius; x++)
                    {
                        for (int y = -radius; y <= radius; y++)
                        {
                            temp += rectangleData[(System.Math.Abs(i + x) % height) * width + System.Math.Abs(j + y) % width]
                                * mask[(x + radius) * windWidth + y + radius];
                        }
                    }
                    result[i * width + j] = temp;
                }
            }
        }

        #endregion
    }
}
