using System;
using System.Collections.Generic;
using System.Text;

namespace HomeQI.ADream.Infrastructure.Utilities
{
    /// <summary>
    /// mysql加密
    /// </summary>
    public class SHA1Hash
    {
        private const int SHA1_HASH_SIZE = 20;

        private static readonly uint[] K = new uint[4]
        {
        1518500249u,
        1859775393u,
        2400959708u,
        3395469782u
        };

        private static readonly uint[] sha_const_key = new uint[5]
        {
        1732584193u,
        4023233417u,
        2562383102u,
        271733878u,
        3285377520u
        };

        private ulong length;

        private uint[] intermediateHash;

        private bool computed;

        private short messageBlockIndex;

        private byte[] messageBlock;
        /// <summary>
        /// 
        /// </summary>
        public SHA1Hash()
        {
            intermediateHash = new uint[5];
            messageBlock = new byte[64];
            Reset();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            length = 0uL;
            messageBlockIndex = 0;
            intermediateHash[0] = sha_const_key[0];
            intermediateHash[1] = sha_const_key[1];
            intermediateHash[2] = sha_const_key[2];
            intermediateHash[3] = sha_const_key[3];
            intermediateHash[4] = sha_const_key[4];
            computed = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] ComputeHash(byte[] buffer)
        {
            Reset();
            Input(buffer, 0, buffer.Length);
            return Result();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public string ToString(byte[] buffer)
        {
            return Encoding.UTF8.GetString(ComputeHash(buffer));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="bufLen"></param>
        public void Input(byte[] buffer, int index, int bufLen)
        {
            if (buffer != null && bufLen != 0)
            {
                if (index < 0 || index > buffer.Length - 1)
                {
                    throw new ArgumentException("Index must be a value between 0 and buffer.Length-1", "index");
                }
                if (bufLen < 0)
                {
                    throw new ArgumentException("Length must be a value > 0", "length");
                }
                if (bufLen + index > buffer.Length)
                {
                    throw new ArgumentException("Length + index would extend past the end of buffer", "length");
                }
                while (bufLen-- > 0)
                {
                    messageBlock[messageBlockIndex++] = (byte)(buffer[index++] & 0xFF);
                    length += 8uL;
                    if (messageBlockIndex == 64)
                    {
                        ProcessMessageBlock();
                    }
                }
            }
        }

        private void ProcessMessageBlock()
        {
            uint[] array = new uint[80];
            for (int i = 0; i < 16; i++)
            {
                int num = i * 4;
                array[i] = (uint)(messageBlock[num] << 24);
                array[i] |= (uint)(messageBlock[num + 1] << 16);
                array[i] |= (uint)(messageBlock[num + 2] << 8);
                array[i] |= messageBlock[num + 3];
            }
            for (int j = 16; j < 80; j++)
            {
                array[j] = CircularShift(1, array[j - 3] ^ array[j - 8] ^ array[j - 14] ^ array[j - 16]);
            }
            uint num2 = intermediateHash[0];
            uint num3 = intermediateHash[1];
            uint num4 = intermediateHash[2];
            uint num5 = intermediateHash[3];
            uint num6 = intermediateHash[4];
            for (int k = 0; k < 20; k++)
            {
                uint num7 = CircularShift(5, num2) + ((num3 & num4) | (~num3 & num5)) + num6 + array[k] + K[0];
                num6 = num5;
                num5 = num4;
                num4 = CircularShift(30, num3);
                num3 = num2;
                num2 = num7;
            }
            for (int l = 20; l < 40; l++)
            {
                uint num8 = CircularShift(5, num2) + (num3 ^ num4 ^ num5) + num6 + array[l] + K[1];
                num6 = num5;
                num5 = num4;
                num4 = CircularShift(30, num3);
                num3 = num2;
                num2 = num8;
            }
            for (int m = 40; m < 60; m++)
            {
                uint num9 = CircularShift(5, num2) + ((num3 & num4) | (num3 & num5) | (num4 & num5)) + num6 + array[m] + K[2];
                num6 = num5;
                num5 = num4;
                num4 = CircularShift(30, num3);
                num3 = num2;
                num2 = num9;
            }
            for (int n = 60; n < 80; n++)
            {
                uint num10 = CircularShift(5, num2) + (num3 ^ num4 ^ num5) + num6 + array[n] + K[3];
                num6 = num5;
                num5 = num4;
                num4 = CircularShift(30, num3);
                num3 = num2;
                num2 = num10;
            }
            intermediateHash[0] += num2;
            intermediateHash[1] += num3;
            intermediateHash[2] += num4;
            intermediateHash[3] += num5;
            intermediateHash[4] += num6;
            messageBlockIndex = 0;
        }

        private static uint CircularShift(int bits, uint word)
        {
            return word << bits | word >> 32 - bits;
        }

        private void PadMessage()
        {
            int num = messageBlockIndex;
            if (num > 55)
            {
                messageBlock[num++] = 128;
                Array.Clear(messageBlock, num, 64 - num);
                messageBlockIndex = 64;
                ProcessMessageBlock();
                Array.Clear(messageBlock, 0, 56);
                messageBlockIndex = 56;
            }
            else
            {
                messageBlock[num++] = 128;
                Array.Clear(messageBlock, num, 56 - num);
                messageBlockIndex = 56;
            }
            messageBlock[56] = (byte)(length >> 56);
            messageBlock[57] = (byte)(length >> 48);
            messageBlock[58] = (byte)(length >> 40);
            messageBlock[59] = (byte)(length >> 32);
            messageBlock[60] = (byte)(length >> 24);
            messageBlock[61] = (byte)(length >> 16);
            messageBlock[62] = (byte)(length >> 8);
            messageBlock[63] = (byte)length;
            ProcessMessageBlock();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] Result()
        {
            if (!computed)
            {
                PadMessage();
                Array.Clear(messageBlock, 0, 64);
                length = 0uL;
                computed = true;
            }
            byte[] array = new byte[20];
            for (int i = 0; i < 20; i++)
            {
                array[i] = (byte)(intermediateHash[i >> 2] >> 8 * (3 - (i & 3)));
            }
            return array;
        }
    }
}
