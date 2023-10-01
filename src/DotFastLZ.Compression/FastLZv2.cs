/*
  FastLZ - Byte-aligned LZ77 compression library
  Copyright (C) 2005-2020 Ariya Hidayat <ariya.hidayat@gmail.com>
  Copyright (C) 2023 Choi Ikpil <ikpil@naver.com> https://github.com/ikpil/DotFastLZ

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/

using System;

namespace DotFastLZ.Compression
{
    public class FastLZv2
    {
        public const int FASTLZ_VERSION = 0x000500;
        public const int FASTLZ_VERSION_MAJOR = 0;
        public const int FASTLZ_VERSION_MINOR = 5;
        public const int FASTLZ_VERSION_REVISION = 0;
        public const string FASTLZ_VERSION_STRING = "0.5.0";

        public const int MAX_COPY = 32;
        public const int MAX_LEN = 264; /* 256 + 8 */
        public const int MAX_L1_DISTANCE = 8192;
        public const int MAX_L2_DISTANCE = 8191;
        public const int MAX_FARDISTANCE = (65535 + MAX_L2_DISTANCE - 1);
        public const int HASH_LOG = 13;
        public const int HASH_SIZE = (1 << HASH_LOG);
        public const int HASH_MASK = (HASH_SIZE - 1);

        /**
          Compress a block of data in the input buffer and returns the size of
          compressed block. The size of input buffer is specified by length. The
          minimum input buffer size is 16.

          The output buffer must be at least 5% larger than the input buffer
          and can not be smaller than 66 bytes.

          If the input is not compressible, the return value might be larger than
          length (input buffer size).

          The input buffer and the output buffer can not overlap.

          Compression level can be specified in parameter level. At the moment,
          only level 1 and level 2 are supported.
          Level 1 is the fastest compression and generally useful for short data.
          Level 2 is slightly slower but it gives better compression ratio.

          Note that the compressed data, regardless of the level, can always be
          decompressed using the function fastlz_decompress below.
        */
        public static long fastlz_compress_level(int level, byte[] input, int length, byte[] output)
        {
            if (level == 1)
            {
                return fastlz1_compress(input, length, output);
            }

            if (level == 2)
            {
                return fastlz2_compress(input, length, output);
            }

            return 0;
        }

        public static long fastlz1_compress(byte[] input, long length, byte[] output)
        {
            long ip = 0;
            long ip_start = ip;
            long ip_bound = ip + length - 4;
            long ip_limit = ip + length - 12 - 1;

            long op = 0;

            long[] htab = new long[HASH_SIZE];
            uint seq, hash;

            // Initializes hash table
            for (hash = 0; hash < HASH_SIZE; ++hash)
            {
                htab[hash] = 0;
            }

            // We start with literal copy
            long anchor = ip;
            ip += 2;

            // Main loop
            while (ip < ip_limit)
            {
                long refIdx;
                long distance, cmp;

                // Find potential match
                do
                {
                    seq = flz_readu32(input, ip) & 0xffffff;
                    hash = flz_hash(seq);
                    refIdx = ip_start + htab[hash];
                    htab[hash] = ip - ip_start;
                    distance = ip - refIdx;
                    cmp = distance < MAX_L1_DISTANCE
                        ? flz_readu32(input, refIdx) & 0xffffff
                        : 0x1000000;

                    if (ip >= ip_limit)
                    {
                        break;
                    }

                    ++ip;
                } while (seq != cmp);

                if (ip >= ip_limit)
                {
                    break;
                }

                --ip;

                if (ip > anchor)
                {
                    op = flz_literals(ip - anchor, input, anchor, output, op);
                }

                long len = flz_cmp(input, refIdx + 3, input, ip + 3, ip_bound);
                op = flz1_match(len, distance, output, op);

                // Update the hash at the match boundary
                ip += len;
                seq = flz_readu32(input, ip);
                hash = flz_hash(seq & 0xffffff);
                htab[hash] = ip++ - ip_start;
                seq >>= 8;
                hash = flz_hash(seq);
                htab[hash] = ip++ - ip_start;

                anchor = ip;
            }

            long copy = length - anchor;
            op = flz_literals(copy, input, anchor, output, op);
            return op;
        }

        public static int fastlz2_compress(byte[] input, int length, byte[] output)
        {
            return 0;
        }

        /**
          Decompress a block of compressed data and returns the size of the
          decompressed block. If error occurs, e.g. the compressed data is
          corrupted or the output buffer is not large enough, then 0 (zero)
          will be returned instead.

          The input buffer and the output buffer can not overlap.

          Decompression is memory safe and guaranteed not to write the output buffer
          more than what is specified in maxout.

          Note that the decompression will always work, regardless of the
          compression level specified in fastlz_compress_level above (when
          producing the compressed block).
         */
        public static int fastlz_decompress(byte[] input, int length, byte[] output, int maxout)
        {
            /* magic identifier for compression level */
            int level = (input[0] >> 5) + 1;

            if (level == 1) return fastlz1_decompress(input, length, output, maxout);
            if (level == 2) return fastlz2_decompress(input, length, output, maxout);

            return 0;
        }

        public static int fastlz1_decompress(byte[] input, int length, byte[] output, int maxout)
        {
            return 0;
        }

        public static int fastlz2_decompress(byte[] input, int length, byte[] output, int maxout)
        {
            return 0;
        }

        private static uint flz_readu32(byte[] data, long offset)
        {
            return ((uint)data[offset + 3] & 0xff) << 24 |
                   ((uint)data[offset + 2] & 0xff) << 16 |
                   ((uint)data[offset + 1] & 0xff) << 8 |
                   ((uint)data[offset + 0] & 0xff);
        }

        private static ushort flz_hash(uint v)
        {
            ulong h = (ulong)(v * 2654435769UL) >> (32 - HASH_LOG);
            return (ushort)(h & HASH_MASK);
        }

        /* special case of memcpy: at most MAX_COPY bytes */
        public static void flz_smallcopy(byte[] dest, long destOffset, byte[] src, long srcOffset, long count)
        {
            // if (count >= 4)
            // {
            //     count -= count % 4;
            //     Array.Copy(src, srcOffset, dest, destOffset, count);
            // }
            Array.Copy(src, srcOffset, dest, destOffset, count);
        }

        /* special case of memcpy: exactly MAX_COPY bytes */
        static void flz_maxcopy(byte[] dest, long destOffset, byte[] src, long secOffset)
        {
            Array.Copy(src, secOffset, dest, destOffset, MAX_COPY);
        }

        private static long flz_literals(long runs, byte[] src, long srcOffset, byte[] dest, long destOffset)
        {
            while (runs >= MAX_COPY)
            {
                dest[destOffset++] = MAX_COPY - 1;
                flz_maxcopy(dest, destOffset, src, srcOffset);
                srcOffset += MAX_COPY;
                destOffset += MAX_COPY;
                runs -= MAX_COPY;
            }

            if (runs > 0)
            {
                dest[destOffset++] = (byte)(runs - 1);
                flz_smallcopy(dest, destOffset, src, srcOffset, runs);
                destOffset += runs;
            }

            return destOffset;
        }

        private static long flz1_match(long len, long distance, byte[] output, long op)
        {
            --distance;
            if (len > MAX_LEN - 2)
            {
                while (len > MAX_LEN - 2)
                {
                    output[op++] = (byte)((7 << 5) + (distance >> 8));
                    output[op++] = (byte)(MAX_LEN - 2 - 7 - 2);
                    output[op++] = (byte)(distance & 255);
                    len -= MAX_LEN - 2;
                }
            }

            if (len < 7)
            {
                output[op++] = (byte)((len << 5) + (distance >> 8));
                output[op++] = (byte)(distance & 255);
            }
            else
            {
                output[op++] = (byte)((7 << 5) + (distance >> 8));
                output[op++] = (byte)(len - 7);
                output[op++] = (byte)((distance & 255));
            }

            return op;
        }

        private static long flz_cmp(byte[] p, long pOffset, byte[] q, long qOffset, long r)
        {
            long start = pOffset;

            if (flz_readu32(p, pOffset) == flz_readu32(q, qOffset))
            {
                pOffset += 4;
                qOffset += 4;
            }

            while (qOffset < r)
            {
                if (p[pOffset++] != q[qOffset++])
                    break;
            }

            return pOffset - start;
        }
    }
}