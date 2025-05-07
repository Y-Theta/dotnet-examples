using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileformat.PNG
{
    public class PNGGenerator
    {

        public void GeneratePng(Stream outstream, IList<byte[]> idats, int width, int height)
        {
            var bitDepth = 8;
            outstream.Write(HeaderValidationResult.ExpectedHeader, 0, HeaderValidationResult.ExpectedHeader.Length);

            var stream = new PngStreamWriteHelper(outstream);

            stream.WriteChunkLength(13);
            stream.WriteChunkHeader(ImageHeader.HeaderBytes);
            StreamHelper.WriteBigEndianInt32(stream, width);
            StreamHelper.WriteBigEndianInt32(stream, height);
            stream.WriteByte((byte)bitDepth);
            var colorType = ColorType.ColorUsed | ColorType.AlphaChannelUsed;
            stream.WriteByte((byte)colorType);
            stream.WriteByte((byte)CompressionMethod.DeflateWithSlidingWindow);
            stream.WriteByte((byte)FilterMethod.AdaptiveFiltering);
            stream.WriteByte((byte)InterlaceMethod.None);
            stream.WriteCrc();

            //Compress(stream, idats);
            byte[] dataarray = Compress(idats);

            var length2 = dataarray.Length / 2;
            var data1 = new byte[length2];
            Array.Copy(dataarray, 0, data1, 0, length2);

            var data2 = new byte[length2];
            Array.Copy(dataarray, length2, data2, 0, length2);

            stream.WriteChunkLength(data1.Length);
            stream.WriteChunkHeader(Encoding.ASCII.GetBytes("IDAT"));
            stream.Write(data1, 0, data1.Length);
            stream.WriteCrc();

            stream.WriteChunkLength(data2.Length);
            stream.WriteChunkHeader(Encoding.ASCII.GetBytes("IDAT"));
            stream.Write(data2, 0, data2.Length);
            stream.WriteCrc();

            stream.WriteChunkLength(0);
            stream.WriteChunkHeader(Encoding.ASCII.GetBytes("IEND"));
            stream.WriteCrc();
        }


        private static byte[] Compress(IList<byte[]> idats)
        {
            const byte Deflate32KbWindow = 120;
            const byte ChecksumBits = 1;
            const int headerLength = 2;
            const int checksumLength = 4;

            var compressionLevel = CompressionLevel.Optimal;

            var compressStream = new MemoryStream();
            int index = 0;

            Adler32Checksum sum = new Adler32Checksum();
            using (var compressor = new DeflateStream(compressStream, compressionLevel, true))
            {
                foreach (var data in idats)
                {
                    //compressStream.Seek(index, SeekOrigin.Begin);
                    var nd = GenerateDataStream(data, new Rectangle(0, 0, 256, 128));
                    sum.AppendData(nd);
                    compressor.Write(nd, 0, nd.Length);
                }

                compressor.Close();
            }

            compressStream.Seek(0, SeekOrigin.Begin);
            var result = new byte[headerLength + compressStream.Length + checksumLength];

            // Write the ZLib header.
            result[0] = Deflate32KbWindow;
            result[1] = ChecksumBits;

            // Write the compressed data.
            int streamValue;
            var i = 0;
            while ((streamValue = compressStream.ReadByte()) != -1)
            {
                result[headerLength + i] = (byte)streamValue;
                i++;
            }

            // Write Checksum of raw data.

            var offset = headerLength + compressStream.Length;

            var checksum = sum.CheckSum;
            //var checksum1 = BigGustave.Adler32Checksum.Calculate(idats[0], idats[0].Length);

            result[offset++] = (byte)(checksum >> 24);
            result[offset++] = (byte)(checksum >> 16);
            result[offset++] = (byte)(checksum >> 8);
            result[offset] = (byte)(checksum >> 0);

            return result;
        }

        private static byte[] GenerateDataStream(byte[] pixelarray, Rectangle rect)
        {
            var bytesPerPixel = 4;
            byte[] rawData = new byte[rect.Width * rect.Height * 4 + rect.Height];
            int index = 0;
            for (int y = rect.Top; y < rect.Height; y++)
            {
                for (int x = rect.Left; x < rect.Width; x++)
                {
                    var start = (y * ((rect.Width * bytesPerPixel) + 1)) + 1 + (x * bytesPerPixel);
                    rawData[start++] = pixelarray[index++];
                    rawData[start++] = pixelarray[index++];
                    rawData[start++] = pixelarray[index++];
                    rawData[start] = pixelarray[index++];
                }
            }

            return rawData;
        }
    }
}
