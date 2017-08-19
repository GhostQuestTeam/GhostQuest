
using System.Collections;
using System.Collections.Generic;

namespace HauntedCity.Utils.Extensions
{
    public static  class StandartClasssesExtensions
    {

        public static void ReplaceTo<T>(this List<T> destination, List<T> source)
        {
            if (source != null)
            {
                destination.Clear();
                destination.AddRange(source);
            }
        }
    }
}