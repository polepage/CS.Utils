using System;
using Android.OS;

namespace Droid.Utils.Serialization
{
    public class ParcelableCreator<T> : Java.Lang.Object, IParcelableCreator
        where T : Java.Lang.Object, new()
    {
        private readonly Func<Parcel, T> _creator;

        public ParcelableCreator(Func<Parcel, T> creator)
        {
            _creator = creator;
        }

        public Java.Lang.Object CreateFromParcel(Parcel source)
        {
            return _creator(source);
        }

        public Java.Lang.Object[] NewArray(int size)
        {
            return new T[size];
        }
    }
}