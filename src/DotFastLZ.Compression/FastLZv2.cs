namespace DotFastLZ.Compression
{
    public class FastLZv2
    {
        public const int FASTLZ_VERSION = 0x000500;
        public const int FASTLZ_VERSION_MAJOR = 0;
        public const int FASTLZ_VERSION_MINOR = 5;
        public const int FASTLZ_VERSION_REVISION = 0;
        public const string FASTLZ_VERSION_STRING = "0.5.0";

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
        public static int fastlz_compress_level(int level, byte[] input, int length, byte[] output)
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

        public static int fastlz1_compress(byte[] input, int length, byte[] output)
        {
            return 0;
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
    }
}