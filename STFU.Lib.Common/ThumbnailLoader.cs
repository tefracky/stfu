using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using ImageProcessor;
using log4net;

namespace STFU.Lib.Common
{
	public static class ThumbnailLoader
	{
		private static readonly ILog Logger = LogManager.GetLogger(nameof(ThumbnailLoader));

		public static Image Load(string path)
		{
			Logger.Info($"Loading Image from path '{path}'");
			Image result = new Bitmap(1, 1);
			((Bitmap)result).SetPixel(0, 0, Color.Transparent);

			if (File.Exists(path))
			{
				try
				{
					ImageFactory imageFactory = new ImageFactory().Load(path);
					result = imageFactory.Image;
					Logger.Info($"Loaded image successfully");
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}
			}
			else
			{
				try
				{
					Logger.Warn($"Image '{path}' does not exist, using fallback image instead");
					Assembly myAssembly = Assembly.GetExecutingAssembly();
					Stream myStream = myAssembly.GetManifestResourceStream("STFU.Lib.Common.Kein-Thumbnail.png");
					result = new Bitmap(myStream);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}
			}

			return result;
		}

		public static Image Load(string path, int width, int height)
		{
			Logger.Info($"Loading Image from path '{path}' with resolution {width}x{height}");
			return ResizeImage(Load(path), width, height);
		}

		public static string LoadAsBase64(string path, int width, int height)
		{
			Logger.Info($"Loading Image from path '{path}' with resolution {width}x{height} as base64");
			return Convert.ToBase64String(ImageToByteArray(Load(path, width, height)));
		}

		private static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		private static byte[] ImageToByteArray(Image image)
		{
			MemoryStream ms = new MemoryStream();
			//image.Save(ms, GetEncoder(ImageFormat.Jpeg), GetJpegQualityEncoderParams(99L));
			image.Save(ms, ImageFormat.Png);
			return ms.ToArray();
		}

		private static EncoderParameters GetJpegQualityEncoderParams()
        {
            return new EncoderParameters(1);
		}

		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}
	}
}
