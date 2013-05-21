using System;

namespace ValueHelper.Math.Infrastructure
{
    /// <summary>
    ///  复数结构
    /// </summary>
    public class Complex
    {
        #region 构造器

        public Complex() { }

        public Complex(Complex other)
        {
            real = other.real;
            imaginary = other.imaginary;
        }

        public Complex(Double dbreal, Double dbimag)
        {
            real = dbreal;
            imaginary = dbimag;
        }

        #endregion

        #region 属性

        /// <summary>
        ///  复数的实部
        /// </summary>
        private Double real = 0.0;
        /// <summary>
        ///  复数的虚部
        /// </summary>
        private Double imaginary = 0.0;

        /// <summary>
        ///  实部的属性
        /// </summary>
        public Double Real
        {
            get
            {
                return real;
            }
            set
            {
                real = value;
            }
        }

        /// <summary>
        ///  虚部的属性
        /// </summary>
        public Double Imaginary
        {
            get
            {
                return imaginary;
            }
            set
            {
                imaginary = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///  复数加法
        /// </summary>
        public Complex Add(Complex comp)
        {
            double x = real + comp.real;
            double y = imaginary + comp.imaginary;

            return new Complex(x, y);
        }

        /// <summary>
        ///  复数减法
        /// </summary>
        public Complex Subtract(Complex comp)
        {
            double x = real - comp.real;
            double y = imaginary - comp.imaginary;

            return new Complex(x, y);
        }

        /// <summary>
        ///  复数乘法
        /// </summary>
        public Complex Multiply(Complex comp)
        {
            double x = real * comp.real - imaginary * comp.imaginary;
            double y = real * comp.imaginary + imaginary * comp.real;

            return new Complex(x, y);
        }

        /// <summary>
        ///  求幅度
        /// </summary>
        public double Abs()
        {
            double x = System.Math.Abs(real);
            double y = System.Math.Abs(imaginary);

            if (real == 0)
            {
                return y;
            }
            if (imaginary == 0)
            {
                return x;
            }

            if (x > y)
            {
                return (x * System.Math.Sqrt(1 + (y / x) * (y / x)));
            }
            else
            {
                return (y * System.Math.Sqrt(1 + (x / y) * (x / y)));
            }
        }

        /// <summary>
        ///  求相位角
        /// </summary>
        public double Angle()
        {
            if (real == 0 && imaginary == 0)
                return 0;

            if (real == 0)
            {
                if (imaginary > 0)
                    return System.Math.PI / 2;
                else
                    return -System.Math.PI / 2;
            }
            else
            {
                if (real > 0)
                    return System.Math.Atan2(imaginary, real);
                else
                {
                    if (imaginary >= 0)
                        return System.Math.Atan2(imaginary, real) + System.Math.PI;
                    else
                        return System.Math.Atan2(imaginary, real) - System.Math.PI;
                }
            }
        }

        /// <summary>
        ///  共轭复数
        /// </summary>
        public Complex Conjugate()
        {
            return new Complex(this.real, -this.imaginary);
        }

        #endregion

        #region 操作符重载

        /// <summary>
        ///  +运算符重载
        /// </summary>
        public static Complex operator +(Complex comp1, Complex comp2)
        {
            return comp1.Add(comp2);
        }

        /// <summary>
        ///  -运算符重载
        /// </summary>
        public static Complex operator -(Complex comp1, Complex comp2)
        {
            return comp1.Subtract(comp2);
        }

        /// <summary>
        ///  *运算符重载
        /// </summary>
        public static Complex operator *(Complex comp1, Complex comp2)
        {
            return comp1.Multiply(comp2);
        }

        #endregion
    }
}
