using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class LFSR
    {
        private ulong reg;
        public LFSR(ulong reg)
        {
            this.reg = reg;
        }
        public byte[] GetKey(int strSize)
        {
            List<byte> resBytes = new List<byte>();
            var temp = reg;
            while (resBytes.Count < strSize)  
            {
                byte resByte = 0;
                for(int i = 0; i < 8; i++)
                {
                    resByte <<= 1;
                    bool b11 = GetBit(reg, 10);
                    bool b22 = GetBit(reg, 35);
                    bool b = b11 ^ b22;
                    reg = ShiftLeftWithSetBit(reg, b);
                    bool b2 = GetBit(reg, 36);
                    byte add = b2 ? (byte)1 : (byte)0;
                    resByte |= add;
                }
                resBytes.Add((resByte));
                if (reg == temp)
                    break;
            }
            while (resBytes.Count < strSize)
            {
                resBytes.AddRange(resBytes);
            }
            return resBytes.ToArray();
        }

        private bool GetBit(ulong number, int bitIndex)
        {
            ulong mask = (ulong)1 << bitIndex;
            return (number & mask) != 0;
        }

        static ulong ShiftLeftWithSetBit(ulong number, bool newBitValue)
        {
            ulong mask = newBitValue ? (ulong)1 : (ulong)0;

            return (number << 1) | mask;
        }

        public byte[] Shifr(byte[] key, byte[] text)
        {
            return XORByteArrays(key, text);
        }

        public byte[] XORByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                throw new ArgumentException("Arrays must have the same length");
            }

            byte[] result = new byte[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = (byte)(array1[i] ^ array2[i]);
            }

            return result;
        }

    }
}
