using Aspose.Imaging.FileFormats.Jpeg;
using Aspose.Imaging.ImageOptions;
using Aspose.Imaging;
using Aspose.Imaging.FileFormats.Png;

namespace tablero_bi.Application.Common
{
    public static  class ImageHelper
    {
        public static void convertirPngASvg(PngImage pngImage, string outputPath)
        {
            var exportOptions = new Aspose.Imaging.ImageOptions.SvgOptions();
            pngImage.Save(outputPath, exportOptions);
        }

        public static void AdjustOrientation(JpegImage jpegImage)
        {
            if (jpegImage.ExifData != null)
            {
                var exifData = jpegImage.ExifData;
                int orientation = exifData.Orientation.GetHashCode();

                switch (orientation)
                {
                    case 1: // Normal
                        break;
                    case 2: // Flipped horizontally
                        jpegImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3: // Rotated 180 degrees
                        jpegImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4: // Flipped vertically
                        jpegImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        break;
                    case 5: // Flipped horizontally and rotated 90 degrees counterclockwise
                        jpegImage.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6: // Rotated 90 degrees clockwise
                        jpegImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7: // Flipped vertically and rotated 90 degrees clockwise
                        jpegImage.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8: // Rotated 90 degrees counterclockwise
                        jpegImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void CompressImage(Image original, string outputPath)
        {
            var jpegOptions = new JpegOptions
            {
                CompressionType = JpegCompressionMode.Progressive,
                ColorType = JpegCompressionColorMode.YCbCr,
                Quality = 75
            };
            original.Save(outputPath, jpegOptions);
        }
    }
}
