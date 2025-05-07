using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileformat.PNG
{
    /// <summary>
    /// 校验和
    /// </summary>
    internal class Adler32Checksum
    {
        private const int UINT16L = 1 << 16;

        private int _s1 = 1;
        private int _s2 = 0;
        public int CheckSum => _s2 * UINT16L + _s1;

        private const int AdlerModulus = 65521;

        /// <summary>
        /// 用于直接计算已知长度的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int Calculate(IEnumerable<byte> data, int length = -1)
        {
            var s1 = 1;

            // s2 is the sum of all s1 values.
            var s2 = 0;

            var count = 0;
            foreach (var b in data)
            {
                if (length > 0 && count == length)
                {
                    break;
                }

                s1 = (s1 + b) % AdlerModulus;
                s2 = (s1 + s2) % AdlerModulus;
                count++;
            }

            // The Adler-32 checksum is stored as s2*65536 + s1.
            return s2 * UINT16L + s1;
        }

        /// <summary>
        /// 用于处理流式数据
        /// </summary>
        /// <param name="data"></param>
        public void AppendData (byte[] data)
        {
            foreach (var b in data)
            {
                _s1 = (_s1 + b) % AdlerModulus;
                _s2 = (_s1 + _s2) % AdlerModulus;
            }

        }

    }
}
