using System;
using System.Linq;
using System.Collections.Generic;
using ValueHelper.Infrastructure;
using ValueHelper.Math.Infrastructure;
using System.Diagnostics;

namespace ValueHelper.Math
{
    public class ValueMath
    {
        private ValueMath() { }

        private static ValueMath instance;
        public static ValueMath GetInstance()
        {
            if (instance == null)
                instance = new ValueMath();
            return instance;
        }

        #region 合计

        public Int32 Sum(Int32[] list)
        {
            return Sum(list, 0, list.Length);
        }

        public Int32 Sum(Int32[] list, Int32 start, Int32 end)
        {
            var sum = 0;
            for (int i = start; i < end; i++)
            {
                sum += list[i];
            }
            return sum;
        }

        #endregion

        #region 最大值

        /// <summary>
        ///  获得最大值
        /// </summary>
        public float Max(params float[] parameters)
        {
            float max = parameters[0];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] > max)
                    max = parameters[i];
            }
            return max;
        }

        /// <summary>
        ///  获得最大值
        /// </summary>
        public Double Max(params Double[] parameters)
        {
            Double max = parameters[0];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] > max)
                    max = parameters[i];
            }
            return max;
        }

        /// <summary>
        ///  获得最大值
        /// </summary>
        public Int32 Max(params Int32[] parameters)
        {
            Int32 max = parameters[0];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] > max)
                    max = parameters[i];
            }
            return max;
        }

        /// <summary>
        ///  获得最大值索引值
        /// </summary>
        public float MaxIndex(params float[] parameters)
        {
            var max = this.Max(parameters);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == max)
                    return i;
            }
            return 0;
        }

        /// <summary>
        ///  获得最大值索引值
        /// </summary>
        public Double MaxIndex(params Double[] parameters)
        {
            var max = this.Max(parameters);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == max)
                    return i;
            }
            return 0;
        }

        /// <summary>
        ///  获得最大值索引值
        /// </summary>
        public Int32 MaxIndex(params Int32[] parameters)
        {
            var max = this.Max(parameters);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == max)
                    return i;
            }
            return 0;
        }

        #endregion

        #region 最小值

        /// <summary>
        ///  获得最小值
        /// </summary>
        public float Min(params float[] parameters)
        {
            float min = parameters[0];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] < min)
                    min = parameters[i];
            }
            return min;
        }

        /// <summary>
        ///  获得最小值
        /// </summary>
        public Double Min(params Double[] parameters)
        {
            Double min = parameters[0];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] < min)
                    min = parameters[i];
            }
            return min;
        }

        /// <summary>
        ///  获得最小值
        /// </summary>
        public Int32 Min(params Int32[] parameters)
        {
            Int32 min = parameters[0];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] < min)
                    min = parameters[i];
            }
            return min;
        }

        /// <summary>
        ///  获得最小值索引值
        /// </summary>
        public float MinIndex(params float[] parameters)
        {
            var min = this.Min(parameters);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == min)
                    return i;
            }
            return 0;
        }

        /// <summary>
        ///  获得最小值索引值
        /// </summary>
        public Double MinIndex(params Double[] parameters)
        {
            var min = this.Min(parameters);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == min)
                    return i;
            }
            return 0;
        }

        /// <summary>
        ///  获得最小值索引值
        /// </summary>
        public Int32 MinIndex(params Int32[] parameters)
        {
            var min = this.Min(parameters);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == min)
                    return i;
            }
            return 0;
        }

        #endregion

        #region 平均数

        /// <summary>
        ///  获得平均数
        /// </summary>
        public float Average(params float[] parameters)
        {
            Double sum = 0F;
            for (int i = 0; i < parameters.Length; i++)
            {
                sum += parameters[i];
            }

            return (float)(sum / parameters.Length);
        }

        /// <summary>
        ///  获得平均数
        /// </summary>
        public Double Average(params Double[] parameters)
        {
            Double sum = 0D;
            for (int i = 0; i < parameters.Length; i++)
            {
                sum += parameters[i];
            }

            return sum / parameters.Length;
        }

        /// <summary>
        ///  获得平均数
        /// </summary>
        public Int32 Average(params Int32[] parameters)
        {
            Int64 sum = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                sum += parameters[i];
            }

            return (Int32)(sum / parameters.Length);
        }

        #endregion

        #region 排序

        /// <summary>
        ///  冒泡排序
        /// </summary>
        public void BubbleSort(IList<Int32> list, SortMode mode)
        {
            switch (mode)
            {
                case SortMode.Descending:
                    this.BubbleDescending(list);
                    break;
                case SortMode.Ascending:
                default:
                    this.BubbleAscending(list);
                    break;
            }
        }

        /// <summary>
        ///  搅拌排序
        /// </summary>
        public void ChurningSort(IList<Int32> list, SortMode mode)
        {
            switch (mode)
            {

                case SortMode.Ascending:
                    this.ChurningAscending(list);
                    break;
                case SortMode.Descending:
                default:
                    this.ChurningDescending(list);
                    break;
            }
        }

        /// <summary>
        ///  插入算法
        ///  Φ(n^2)
        /// </summary>
        public void InsertionSort(IList<Int32> list, SortMode mode)
        {
            switch (mode)
            {

                case SortMode.Ascending:
                    this.InsertionAscending(list);
                    break;
                case SortMode.Descending:
                default:
                    this.InsertionDescending(list);
                    break;
            }
        }

        /// <summary>
        ///  归并算法
        ///  Φ(nlgn)
        /// </summary>
        public void MergenSort(IList<Int32> list, SortMode mode)
        {
            switch (mode)
            {
                case SortMode.Ascending:
                    this.MergenAscending(list, 0, list.Count);
                    break;
                case SortMode.Descending:
                default:
                    this.MergenDescending(list, 0, list.Count);
                    break;
            }
        }

        #region 排序升序

        public void BubbleAscending(IList<Int32> list)
        {
            var temp = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public void ChurningAscending(IList<Int32> list)
        {
            var temp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < list.Count - 1 - i; j++)
                {
                    if (list[j] < list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }

                for (int j = list.Count - 2 - i; j >= i; j--)
                {
                    if (list[j] < list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public void InsertionAscending(IList<Int32> list)
        {
            var temp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public void MergenAscending(IList<Int32> list, Int32 start, Int32 end)
        {
            if (start + 1 < end)
            {
                var mid = (start + end) / 2;
                MergenAscending(list, start, mid);
                MergenAscending(list, mid, end);

                var tempList = new Queue<Int32>();
                var iA = start;
                var iB = mid;
                while (iA < mid && iB < end)
                {
                    if (list[iA] > list[iB])
                    {
                        tempList.Enqueue(list[iB]);
                        iB++;
                    }
                    else
                    {
                        tempList.Enqueue(list[iA]);
                        iA++;
                    }
                }

                while (iA < mid)
                {
                    tempList.Enqueue(list[iA]);
                    iA++;
                }

                while (iB < end)
                {
                    tempList.Enqueue(list[iB]);
                    iB++;
                }

                var index = 0;
                while (tempList.Count > 0)
                {
                    list[start + index] = tempList.Dequeue();
                    index++;
                }
            }
        }

        #endregion

        #region 排序降序

        public void BubbleDescending(IList<Int32> list)
        {
            var temp = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j] < list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public void ChurningDescending(IList<Int32> list)
        {
            var temp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < list.Count - 1 - i; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }

                for (int j = list.Count - 2 - i; j >= i; j--)
                {
                    if (list[j] > list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public void InsertionDescending(IList<Int32> list)
        {
            var temp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j] < list[j + 1])
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public void MergenDescending(IList<Int32> list, Int32 start, Int32 end)
        {
            if (start + 1 < end)
            {
                var mid = (start + end) / 2;
                MergenDescending(list, start, mid);
                MergenDescending(list, mid, end);

                var tempList = new Queue<Int32>();
                var iA = start;
                var iB = mid;
                while (iA < mid && iB < end)
                {
                    if (list[iA] < list[iB])
                    {
                        tempList.Enqueue(list[iB]);
                        iB++;
                    }
                    else
                    {
                        tempList.Enqueue(list[iA]);
                        iA++;
                    }
                }

                while (iA < mid)
                {
                    tempList.Enqueue(list[iA]);
                    iA++;
                }

                while (iB < end)
                {
                    tempList.Enqueue(list[iB]);
                    iB++;
                }

                var index = 0;
                while (tempList.Count > 0)
                {
                    list[start + index] = tempList.Dequeue();
                    index++;
                }
            }
        }

        #endregion

        #endregion

        #region Ostu阈值

        public Int32 OstuThreshold(Int32[] frequency, Int32 radix)
        {
            var length = frequency.Length;
            var probability = new float[length];
            for (int i = 0; i < length; i++)
            {
                probability[i] = (float)frequency[i] / radix;
            }

            var max = 0F;
            var index = 0;
            for (int i = 1; i < length; i++)
            {
                var w0 = 0F;
                for (int t = 0; t < i; t++)
                {
                    w0 += probability[i];
                }
                var A = 0F;
                for (int t = 0; t < i; t++)
                {
                    A += probability[i] * i;
                }
                var uA = A / w0;

                var w1 = 1 - w0;
                var B = 0F;
                for (int t = i; t < length; t++)
                {
                    B += probability[i] * i;
                }
                var uB = B / w1;

                var ft = w0 * (uA - i) * (uA - i) + w1 * (uB - i) * (uB - i);
                if (ft > max)
                {
                    max = ft;
                    index = i;
                }
            }

            return index;
        }

        #endregion

        #region 高斯运算

        public float Gauss(float x, float sigma)
        {
            float value;
            if (sigma == 0) return 0F;
            value = (float)System.Math.Exp((Double)((-x * x) / (2 * sigma * sigma)));
            return value;
        }

        #endregion

        #region 傅里叶变换

        /// <summary>
        ///  快速傅里叶变化
        ///  条件: 长度必须是以2为底的整数次幂
        /// </summary>
        /// <param name="sourceData">待变化序列</param>
        /// <param name="countN">序列长度</param>
        public Complex[] FFT(Complex[] sourceData, Int32 countN)
        {
            // 傅里叶变化的级数
            Int32 r = Convert.ToInt32(System.Math.Log(countN, 2));

            // 加权系数
            Complex[] w = new Complex[countN / 2];
            Complex[] interVar1 = new Complex[countN];
            Complex[] interVar2 = new Complex[countN];

            interVar1 = (Complex[])sourceData.Clone();
            for (int i = 0; i < countN / 2; i++)
            {
                Double angle = -i * System.Math.PI * 2 / countN;
                w[i] = new Complex(System.Math.Cos(angle), System.Math.Sin(angle));
            }

            // 核心部分
            for (int i = 0; i < r; i++)
            {
                Int32 interval = 1 << i;
                Int32 halfN = 1 << (r - i);
                for (int j = 0; j < interval; j++)
                {
                    Int32 gap = j * halfN;
                    for (int k = 0; k < halfN / 2; k++)
                    {
                        interVar2[k + gap] = interVar1[k + gap] + interVar1[k + gap + halfN / 2];
                        interVar2[k + halfN / 2 + gap] = (interVar1[k + gap] - interVar1[k + gap + halfN / 2]) * w[k * interval];
                    }
                }
                interVar1 = (Complex[])interVar2.Clone();
            }

            for (uint j = 0; j < countN; j++)
            {
                uint rev = 0;
                uint num = j;
                for (int i = 0; i < r; i++)
                {
                    rev <<= 1;
                    rev |= num & 1;
                    num >>= 1;
                }
                interVar2[rev] = interVar1[j];
            }

            return interVar2;
        }

        /// <summary>
        ///  快速傅里叶逆变换
        /// </summary>
        /// <param name="sourceData">带变化序列</param>
        /// <param name="countN">序列长度</param>
        public Complex[] IFFT(Complex[] sourceData, Int32 countN)
        {
            // 共轭变换
            for (int i = 0; i < countN; i++)
            {
                sourceData[i] = sourceData[i].Conjugate();
            }

            Complex[] interVar = new Complex[countN];
            // 调用快速傅里叶变化
            interVar = this.FFT(sourceData, countN);

            // 共轭变化,并除以长度
            for (int i = 0; i < countN; i++)
            {
                interVar[i] = new Complex(interVar[i].Real / countN, -interVar[i].Imaginary / countN);
            }

            return interVar;
        }

        #endregion

        #region 小波变换

        /// <summary>
        ///  一维小波变换
        /// </summary>
        /// <param name="scaleIn">小波变换前的尺度,即平滑信号</param>
        /// <param name="lowpass">与尺度函数相关的低通滤波器</param>
        /// <param name="highpass">与小波函数相关的高通滤波器</param>
        /// <param name="scaleOut">小波变换后的尺度系数</param>
        /// <param name="wavelet">小波变换后的小波系数,即细节信号</param>
        public void Wavelet(Double[] scaleIn, Double[] lowpass, Double[] highpass, out Double[] scaleOut, out Double[] wavelet)
        {
            Int32 temp;
            // 变换前的尺度的长度
            Int32 scaleLen = scaleIn.Length;
            // 低通滤波的长度
            Int32 lowpassLen = lowpass.Length;
            // 输出的尺度
            scaleOut = new Double[scaleLen / 2];
            // 输出的小波系数
            wavelet = new Double[scaleLen / 2];

            for (int i = 0; i < scaleLen / 2; i++)
            {
                scaleOut[i] = 0.0;
                wavelet[i] = 0.0;

                for (int j = 0; j < lowpassLen; j++)
                {
                    temp = (j + i * 2) % scaleLen;

                    scaleOut[i] += lowpass[j] * scaleIn[temp];
                    wavelet[i] += highpass[j] * scaleIn[temp];
                }
            }
        }

        /// <summary>
        ///  一维小波逆变换
        /// </summary>
        /// <param name="lowpass">与尺度函数相关的低通滤波器</param>
        /// <param name="highpass">与尺度函数相关的高通滤波器</param>
        /// <param name="scaleIn">小波逆变换前的尺度系数,即平滑信号</param>
        /// <param name="wavelet">小波逆变换前的小波系数,即细节信号</param>
        /// <param name="scaleOut">小波逆变换后的尺度系数,即平滑信号</param>
        public void IWavelet(Double[] lowpass, Double[] highpass, Double[] scaleIn, Double[] wavelet, out Double[] scaleOut)
        {
            Int32 temp;
            Int32 scaleLen = scaleIn.Length;
            Int32 lowpassLen = lowpass.Length;
            scaleOut = new Double[scaleLen * 2];
            for (int i = 0; i < scaleLen; i++)
            {
                scaleOut[2 * i + 1] = 0.0;
                scaleOut[2 * i] = 0.0;
                for (int j = 0; j < lowpassLen / 2; j++)
                {
                    temp = (i - j + scaleLen) % scaleLen;
                    scaleOut[2 * i + 1] = lowpass[2 * j + 1] * scaleIn[temp] + highpass[2 * j + 1] * wavelet[temp];
                    scaleOut[2 * i] = lowpass[2 * j] * scaleIn[temp] + highpass[2 * j] * wavelet[temp];
                }
            }
        }

        /// <summary>
        ///  二维小波变换
        /// </summary>
        /// <param name="width">二维信号宽度</param>
        /// <param name="height">二维信号高度</param>
        /// <param name="lowpass">与尺度函数相关的低通滤波器</param>
        /// <param name="highpass">与小波函数相关的高通滤波器</param>
        /// <param name="series">关于小波变换级数的参数</param>
        public void Wavelet2D(ref Double[] rectangleData, Int32 width, Int32 height, Double[] lowpass, Double[] highpass, Int32 series)
        {
            Double[] hs = new Double[width / series];
            Double[] hs1 = new Double[width / (2 * series)];
            Double[] hw1 = new Double[width / (2 * series)];
            for (int i = 0; i < height / series; i++)
            {
                for (int j = 0; j < width / series; j++)
                {
                    hs[j] = rectangleData[i * width / series + j];
                }

                this.Wavelet(hs, lowpass, highpass, out hs1, out hw1);

                for (int j = 0; j < width / series; j++)
                {
                    if (j < width / (2 * series))
                        rectangleData[i * width / series + j] = hs1[j];
                    else
                        rectangleData[i + width / series + j] = hw1[j - width / (2 * series)];
                }
            }

            Double[] vs = new Double[height / series];
            Double[] vs1 = new Double[height / (2 * series)];
            Double[] vw1 = new Double[height / (2 * series)];
            for (int i = 0; i < width / series; i++)
            {
                for (int j = 0; j < height / series; j++)
                {
                    vs[j] = rectangleData[j * width / series + i];
                }

                this.Wavelet(vs, lowpass, highpass, out vs1, out vw1);

                for (int j = 0; j < height / series; j++)
                {
                    if (j < height / (2 * series))
                        rectangleData[j * width / series + i] = vs1[j];
                    else
                        rectangleData[j * width / series + i] = vw1[j - height / (2 * series)];
                }
            }
        }

        /// <summary>
        ///  二维小波逆变换
        /// </summary>
        /// <param name="width">二维信号宽度</param>
        /// <param name="height">二维信号高度</param>
        /// <param name="lowpass">与尺度函数相关的低通滤波器</param>
        /// <param name="highpass">与小波函数相关的高通滤波器</param>
        /// <param name="series">关于小波变换级数的参数</param>
        public void IWavelet2D(ref Double[] rectangleData, Int32 width, Int32 height, Double[] lowpass, Double[] highpass, Int32 series)
        {
            Double[] hs = new Double[width / series];
            Double[] hs1 = new Double[width / (2 * series)];
            Double[] hw1 = new Double[width / (2 * series)];

            for (int i = 0; i < width / series; i++)
            {
                for (int j = 0; j < height / series; j++)
                {
                    if (j < height / (2 * series))
                        hs1[j] = rectangleData[j * width / series + i];
                    else
                        hw1[j - height / (2 * series)] = rectangleData[j * width / series + i];
                }

                this.IWavelet(lowpass, highpass, hs1, hw1, out hs);

                for (int j = 0; j < height / series; j++)
                {
                    rectangleData[j * width / series + i] = hs[j];
                }
            }

            Double[] vs = new Double[height / series];
            Double[] vs1 = new Double[height / (2 * series)];
            Double[] vw1 = new Double[height / (2 * series)];
            for (int i = 0; i < height / series; i++)
            {
                for (int j = 0; j < width / series; j++)
                {
                    if (j < width / (2 * series))
                        vs1[j] = rectangleData[i * width / series + j];
                    else
                        vw1[j - width / (2 * series)] = rectangleData[i * width / series + j];
                }
                this.IWavelet(lowpass, highpass, vs1, vw1, out vs);

                for (int j = 0; j < width / series; j++)
                {
                    rectangleData[i * width / series + j] = vs[j];
                }
            }
        }

        #endregion
    }
}
