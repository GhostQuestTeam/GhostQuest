using System;

namespace HauntedCity.Utils.DataBinding
{
    public class Model
    {
        public event Action OnChange;

        protected void _NotifyChanges()
        {
            if (OnChange != null)
            {
                OnChange();
            }
        }
    }
}