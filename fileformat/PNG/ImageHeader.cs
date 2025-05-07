using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileformat.PNG
{
    [Flags]
    public enum ColorType : byte
    {
        /// <summary>
        /// Grayscale.
        /// </summary>
        None = 0,
        /// <summary>
        /// Colors are stored in a palette rather than directly in the data.
        /// </summary>
        PaletteUsed = 1,
        /// <summary>
        /// The image uses color.
        /// </summary>
        ColorUsed = 2,
        /// <summary>
        /// The image has an alpha channel.
        /// </summary>
        AlphaChannelUsed = 4
    }

    /// <summary>
    /// The method used to compress the image data.
    /// </summary>
    public enum CompressionMethod : byte
    {
        /// <summary>
        /// Deflate/inflate compression with a sliding window of at most 32768 bytes.
        /// </summary>
        DeflateWithSlidingWindow = 0
    }

    /// <summary>
    /// Indicates the pre-processing method applied to the image data before compression.
    /// </summary>
    public enum FilterMethod
    {
        /// <summary>
        /// Adaptive filtering with five basic filter types.
        /// </summary>
        AdaptiveFiltering = 0
    }

    /// <summary>
    /// Indicates the transmission order of the image data.
    /// </summary>
    public enum InterlaceMethod : byte
    {
        /// <summary>
        /// No interlace.
        /// </summary>
        None = 0,
        /// <summary>
        /// Adam7 interlace.
        /// </summary>
        Adam7 = 1
    }

    public readonly struct ImageHeader
    {
        public static readonly byte[] HeaderBytes = {
            73, 72, 68, 82
        };

        private static readonly IReadOnlyDictionary<ColorType, HashSet<byte>> PermittedBitDepths = new Dictionary<ColorType, HashSet<byte>>
        {
            {ColorType.None, new HashSet<byte> {1, 2, 4, 8, 16}},
            {ColorType.ColorUsed, new HashSet<byte> {8, 16}},
            {ColorType.PaletteUsed | ColorType.ColorUsed, new HashSet<byte> {1, 2, 4, 8}},
            {ColorType.AlphaChannelUsed, new HashSet<byte> {8, 16}},
            {ColorType.AlphaChannelUsed | ColorType.ColorUsed, new HashSet<byte> {8, 16}},
        };

        /// <summary>
        /// The width of the image in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the image in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The bit depth of the image.
        /// </summary>
        public byte BitDepth { get; }

        /// <summary>
        /// The color type of the image.
        /// </summary>
        public ColorType ColorType { get; }

        /// <summary>
        /// The compression method used for the image.
        /// </summary>
        public CompressionMethod CompressionMethod { get; }

        /// <summary>
        /// The filter method used for the image.
        /// </summary>
        public FilterMethod FilterMethod { get; }

        /// <summary>
        /// The interlace method used by the image..
        /// </summary>
        public InterlaceMethod InterlaceMethod { get; }

        /// <summary>
        /// Create a new <see cref="ImageHeader"/>.
        /// </summary>
        public ImageHeader(int width, int height, byte bitDepth, ColorType colorType, CompressionMethod compressionMethod, FilterMethod filterMethod, InterlaceMethod interlaceMethod)
        {
            if (width == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Invalid width (0) for image.");
            }

            if (height == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Invalid height (0) for image.");
            }

            if (!PermittedBitDepths.TryGetValue(colorType, out var permitted)
                || !permitted.Contains(bitDepth))
            {
                throw new ArgumentException($"The bit depth {bitDepth} is not permitted for color type {colorType}.");
            }

            Width = width;
            Height = height;
            BitDepth = bitDepth;
            ColorType = colorType;
            CompressionMethod = compressionMethod;
            FilterMethod = filterMethod;
            InterlaceMethod = interlaceMethod;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"w: {Width}, h: {Height}, bitDepth: {BitDepth}, colorType: {ColorType}, " +
                   $"compression: {CompressionMethod}, filter: {FilterMethod}, interlace: {InterlaceMethod}.";
        }
    }
}
