﻿using System;
using ServerQueries.Common.ByteHelper;

namespace ServerQueries.Common
{
    public static class Helper
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T[] InsertArray<T>(this T[] data, int index, T[] arrayToInsert)
        {
            int maxlength = data.Length - index;
            if (arrayToInsert.Length > maxlength)
                arrayToInsert = arrayToInsert.SubArray(0, arrayToInsert.Length - maxlength);
            Array.Copy(arrayToInsert, 0, data, index, arrayToInsert.Length);
            return data;
        }

        public static int GetNextNullCharPosition(this byte[] data, int startindex)
        {
            for (; startindex < data.Length; startindex++)
            {
                if (data[startindex].Equals(0x00))
                    return startindex;
            }
            return -1;
        }

        public static IByteReader GetByteReader(this byte[] data)
        {
            return new ByteReader(data);
        }

        public static Enums.ServerType ToServerType(this byte data)
        {
            if (!Constants.ByteServerTypeMapping.TryGetValue(data, out var returnval))
                throw new ArgumentException("Given byte cannot be parsed");
            else
                return returnval;
        }

        public static Enums.Environment ToEnvironment(this byte data)
        {
            if (!Constants.ByteEnvironmentMapping.TryGetValue(data, out var returnval))
                throw new ArgumentException("Given byte cannot be parsed");
            else
                return returnval;
        }

        public static Enums.Visibility ToVisibility(this byte data)
        {
            if (!Constants.ByteVisibilityMapping.TryGetValue(data, out var returnval))
                throw new ArgumentException("Given byte cannot be parsed");
            else
                return returnval;
        }

        public static Enums.TheShipMode ToTheShipMode(this byte data)
        {
            if (!Constants.ByteTheShipModeMapping.TryGetValue(data, out var returnval))
                throw new ArgumentException("Given byte cannot be parsed");
            else
                return returnval;
        }
    }
}