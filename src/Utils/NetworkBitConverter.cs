using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Utils.Numerics;

namespace Utils
{
    public sealed class NetworkBitConverter
    {
        private static readonly bool IsLittleEndian
            = BitConverter.IsLittleEndian;

        public static byte[] GetBytes(Int64 number)
        {
            var buffer = BitConverter.GetBytes(number);
            MakeNetworkBuffer(buffer);
            return buffer;
        }
        public static byte[] GetBytes(UInt16 number)
        {
            var buffer = BitConverter.GetBytes(number);
            MakeNetworkBuffer(buffer);
            return buffer;
        }
        public static byte[] GetBytes(UInt32 number)
        {
            var buffer = BitConverter.GetBytes(number);
            MakeNetworkBuffer(buffer);
            return buffer;
        }
        public static byte[] GetBytes(UInt64 number)
        {
            var buffer = BitConverter.GetBytes(number);
            MakeNetworkBuffer(buffer);
            return buffer;
        }

        public static UInt32 ToUInt16(byte[] buffer, int offset)
        {
            var bytes = MakeLocalBuffer(buffer, offset, sizeof(UInt16));
            return BitConverter.ToUInt16(bytes, 0);
        }
        public static UInt32 ToUInt32(byte[] buffer, int offset)
        {
            var bytes = MakeLocalBuffer(buffer, offset, sizeof(UInt32));
            return BitConverter.ToUInt32(bytes, 0);
        }
        public static UInt64 ToUInt64(byte[] buffer, int offset)
        {
            var bytes = MakeLocalBuffer(buffer, offset, sizeof(UInt64));
            return BitConverter.ToUInt64(bytes, 0);
        }
        public static UInt128 ToUInt128(byte[] buffer, int offset)
        {
            var bytes = MakeLocalBuffer(buffer, offset, Marshal.SizeOf(typeof(UInt128)));
            var bigInt = new BigInteger(bytes);
            var ui128 = new UInt128(bigInt);
            return ui128;
        }

        private static byte[] MakeLocalBuffer(byte[] buffer, int offset, int count)
        {
            System.Diagnostics.Debug.Assert(buffer.Length >= offset + count);

            var bytes = new byte[count];
            for (int s = offset, t = 0; t < count; s++, t++)
                bytes[t] = buffer[s];

            if (IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
        private static void MakeNetworkBuffer(byte[] buffer)
        {
            if (IsLittleEndian)
                Array.Reverse(buffer);
        }

    }
}