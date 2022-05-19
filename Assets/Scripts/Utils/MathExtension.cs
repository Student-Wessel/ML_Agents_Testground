using System;


namespace Utils
{
    public static class MathExtension
    {
        public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh) 
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
