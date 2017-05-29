namespace HauntedCity.Utils
{
    public class BoundedInt
    {
        private int _val;

        public int Max { get; set; }
        public int Min { get; set; }

        public int Val
        {
            get { return _val; }
            set
            {
                if (value > Max)
                {
                    _val = Max;
                }
                else if (value < Min)
                {
                    _val = Min;
                }
                else
                {
                    _val = value;
                }
            }
        }

        public BoundedInt(int max = int.MaxValue, int min = 0, int value = 0)
        {
            Max = max;
            Min = min;
            Val = value;
        }

        public static int Clamp(int val, int max = int.MaxValue, int min = 0)
        {
            if (val > max) return max;
            if (val < min) return min;
            return val;
        }
    }
}