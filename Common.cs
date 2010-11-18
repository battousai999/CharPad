using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

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
    }
}
