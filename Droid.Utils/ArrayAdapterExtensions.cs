using System.Collections.Generic;
using Android.Widget;

namespace Droid.Utils
{
    public static class ArrayAdapterExtensions
    {
        public static IEnumerable<T> EnumerateAdapter<T>(this ArrayAdapter<T> adapter)
        {
            for (int i = 0; i < adapter.Count; i++)
            {
                yield return adapter.GetItem(i);
            }
        }
    }
}