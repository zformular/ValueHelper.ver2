using System;

namespace ValueHelper.Image.Infrastructure
{
    public enum WaveletLowpassType
    {
        Haar,
        Daubechies2,
        Daubechies3,
        Daubechies4,
        Daubechies5,
        Daubechies6
    }

    /// <summary>
    ///  小波变化低通值
    /// </summary>
    class WaveletLowpass
    {
        public static Double[] Haar = new Double[] { 
            0.70710678118655, 0.70710678118655 };
        public static Double[] Daubechies2 = new Double[] { 
            0.48296291314453, 0.83651630373780,
            0.22414386804201, -0.12940952255126 };
        public static Double[] Daubechies3 = new Double[] { 
            0.33267055295008, 0.80689150931109,
            0.45987750211849, -0.13501102001025,
            -0.08544127388203, 0.03522629188571 };
        public static Double[] Daubechies4 = new Double[] { 
            0.23037781330889, 0.71484657055291, 
            0.63088076792986, -0.02798376941686,
            -0.18703481171909, 0.03084138183556, 
            0.03288301166689, -0.01059740178507 };
        public static Double[] Daubechies5 = new Double[] { 
            0.16010239797419, 0.60382926979719,
            0.72430852843778, 0.13842814590132,
            -0.24229488706638, -0.03224486958464, 
            0.07757149384005, -0.00624149021280,
            -0.01258075199908, 0.00333572528547 };
        public static Double[] Daubechies6 = new Double[] { 
            0.11154074335011, 0.49462389039845, 
            0.75113390802110, 0.31525035170920,
            -0.22626469396544, -0.12976686756726,
            0.09750160558732, 0.02752286553031, 
            -0.03158203931849, 0.00055384220116,
            0.00477725751195, -0.00107730108531 };
    }
}
