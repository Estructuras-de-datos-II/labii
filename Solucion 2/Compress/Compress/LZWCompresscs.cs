using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Compress
{
    public static class LZWCompresscs
    {

        public static int CompressedSize { get; set; }
        public static int DeCompressedSize { get; set; }
        public static double Ratio => (double)CompressedSize / DeCompressedSize * 100.0;

        public static byte[] LzwCompress(byte[] dataToCompress)
        {
            DeCompressedSize = dataToCompress.Length;
            var dictionary = new Dictionary<List<byte>, int>(new ArrayComparer());
            for (int i = 0; i < 256; i++)
            {
                var e = new List<byte> { (byte)i };
                dictionary.Add(e, i);
            }
            var window = new List<byte>();
            var compressedDataList = new List<int>();
            foreach (byte b in dataToCompress)
            {
                var windowChain = new List<byte>(window) { b };
                if (dictionary.ContainsKey(windowChain))
                {
                    window.Clear();
                    window.AddRange(windowChain);
                }
                else
                {
                    if (dictionary.ContainsKey(window))
                        compressedDataList.Add(dictionary[window]);
                    else
                        throw new Exception("Error Encoding.");
                    CompressedSize = compressedDataList.Count;
                    dictionary.Add(windowChain, dictionary.Count);
                    window.Clear();
                    window.Add(b);
                }
            }
            if (window.Count != 0)
            {
                compressedDataList.Add(dictionary[window]);
                CompressedSize = compressedDataList.Count;
            }
            return GetBytes(compressedDataList.ToArray());
        }

        public static byte[] LzwDecompress(this byte[] dataToDescompress)
        {
            var dataToDescompressToInt = Ia(dataToDescompress);
            var dataToDescompressToIntList = new List<int>(dataToDescompressToInt);
            CompressedSize = dataToDescompressToIntList.Count;
            var dictionary = new Dictionary<int, List<byte>>();

            for (int i = 0; i < 256; i++)
            {
                var e = new List<byte> { (byte)i };
                dictionary.Add(i, e);
            }

            var window = dictionary[dataToDescompressToIntList[0]];
            dataToDescompressToIntList.RemoveAt(0);
            var descompressedDataList = new List<byte>(window);

            foreach (int k in dataToDescompressToIntList)
            {
                var entry = new List<byte>();
                if (dictionary.ContainsKey(k))
                    entry.AddRange(dictionary[k]);
                else if (k == dictionary.Count)
                    entry.AddRange(Add(window.ToArray(), new[] { window.ToArray()[0] }));
                if (entry.Count > 0)
                {
                    descompressedDataList.AddRange(entry);
                    DeCompressedSize = descompressedDataList.Count;
                    dictionary.Add(dictionary.Count, new List<byte>(Add(window.ToArray(), new[] { entry.ToArray()[0] })));
                    window = entry;
                }
            }

            return descompressedDataList.ToArray();
        }

        private static byte[] GetBytes(int[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (int[]) object cannot be null.");
            var numArray = new byte[value.Length * 4];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        private static byte[] Add(byte[] left, byte[] right)
        {
            byte[] addedSizes = new byte[left.Length + right.Length];
            Buffer.BlockCopy(left, 0, addedSizes, 0, left.Length);
            Buffer.BlockCopy(right, 0, addedSizes, left.Length, right.Length);
            return addedSizes;
        }

        private static int[] Ia(byte[] ba)
        {
            var bal = ba.Length;
            var int32Count = bal / 4 + (bal % 4 == 0 ? 0 : 1);
            var arr = new int[int32Count];
            Buffer.BlockCopy(ba, 0, arr, 0, bal);
            return arr;
        }

    }
}
