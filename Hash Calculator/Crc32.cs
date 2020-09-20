using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Hash_Calculator
{
    public sealed class Crc32 : HashAlgorithm
    {
        public const UInt32 default_polynomial = 0xedb88320u;
        public const UInt32 default_seed = 0xffffffffu;

        static UInt32[] default_table;

        readonly UInt32 seed;
        readonly UInt32[] table;

        UInt32 hash;

        public Crc32() : this(default_polynomial, default_seed) { }

        public Crc32(UInt32 polynomial, UInt32 seed)
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("Not supported on Big Endian processors");

            table = Initialize_Table(polynomial);
            this.seed = hash = seed;
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            hash = Calculate_Hash(table, hash, array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt32_To_Big_Endian_Bytes(~hash);

            HashValue = hashBuffer;

            return hashBuffer;
        }

        public override int HashSize
        {
            get
            {
                return 32;
            }
        }

        public static UInt32 Compute(byte[] buffer)
        {
            return Compute(default_seed, buffer);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return Compute(default_polynomial, seed, buffer);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~Calculate_Hash(Initialize_Table(polynomial), seed, buffer, 0, buffer.Length);
        }

        static UInt32[] Initialize_Table(UInt32 polynomial)
        {
            if (polynomial == default_polynomial && default_table != null)
                return default_table;

            var createTable = new UInt32[256];

            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)i;

                for (var j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry >>= 1;
                }

                createTable[i] = entry;
            }

            if (polynomial == default_polynomial)
                default_table = createTable;

            return createTable;
        }
        
        public string Calculate_Hash_In_House(string file_path)
		{
            string hash = string.Empty;

            using (var stream = new BufferedStream(File.OpenRead(file_path), 1048576))
            {
                foreach (byte b in ComputeHash(stream))
                    hash += b.ToString("x2").ToLower();
            }

            return hash;
        }

        static UInt32 Calculate_Hash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size)
        {
            var hash = seed;

            for (var i = start; i < start + size; i++)
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];

            return hash;
        }

        static byte[] UInt32_To_Big_Endian_Bytes(UInt32 uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
    }
}