using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CharPad
{
    public static class Common
    {
        public static void LogException(Exception ex)
        {
            try
            {
                string filename = String.Format("{0}errorlogs\\{1:yyyy_MM_dd}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now);
                string filepath = Path.GetDirectoryName(filename);

                string text = String.Format("[{0:MM/dd/yyyy hh:mm:ss}] <{1}> {2}{3}{4}{3}",
                    DateTime.Now,
                    ex.GetType().Name,
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace);

                Directory.CreateDirectory(filepath);
                File.AppendAllText(filename, text);
            }
            catch (Exception)
            {
                // Consume exception--at this point, we're already in the unhandled exception handler
            }
        }

        // Window.ShowDialog()
        public static bool ShowDialog(this Window dialog, Window owner)
        {
            if (owner != null)
            {
                dialog.Owner = owner;
            }

            bool? dialogResult = dialog.ShowDialog();
            return (dialogResult.HasValue ? dialogResult.Value : false);
        }

        public static BitmapImage BuildBitmapImage(byte[] bytes)
        {
            if (bytes == null)
                return null;

            using (MemoryStream stream = new MemoryStream(bytes, false))
            {
                stream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static BitmapImage BuildBitmapImage(System.Drawing.Image image)
        {
            if (image == null)
                return null;

            if (!(image is System.Drawing.Bitmap))
                throw new InvalidOperationException("Cannot handle non-bitmap images.");

            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)image;

            using (MemoryStream stream = new MemoryStream())
            {
                System.Drawing.Bitmap copyBitmap = new System.Drawing.Bitmap(bitmap);

                copyBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static byte[] ConvertToByteArray(BitmapSource image)
        {
            if (image == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                System.Windows.Media.Imaging.BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);

                return stream.ToArray();
            }
        }

        public static System.Drawing.Image ConvertToBitmap(BitmapSource image)
        {
            if (image == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                System.Windows.Media.Imaging.BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);

                return new System.Drawing.Bitmap(stream);
            }
        }
    }
}
