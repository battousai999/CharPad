using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CharPad.Framework;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Threading;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for CharacterViewWindow.xaml
    /// </summary>
    public partial class CharacterViewWindow : Window
    {
        private Player player;

        public RoutedCommand PrintCommand = new RoutedCommand();
        public RoutedCommand PrintPowersCommand = new RoutedCommand();

        public CharacterViewWindow(Player player)
        {
            this.player = player;

            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(PrintCommand, new ExecutedRoutedEventHandler(PrintCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(PrintPowersCommand, new ExecutedRoutedEventHandler(PrintPowersCommand_Executed)));

            this.InputBindings.Add(new KeyBinding(PrintCommand, Key.P, ModifierKeys.Control));
            this.InputBindings.Add(new KeyBinding(PrintPowersCommand, Key.P, ModifierKeys.Control | ModifierKeys.Shift));

            gridMain.DataContext = player;

            Title = String.Format("{0}{1} [Level {2}, {3} {4}{5}{6}]",
                player.CharacterName,
                (String.IsNullOrEmpty(player.PlayerName) ? "" : " (" + player.PlayerName + ")"),
                player.Level.ToString(),
                player.Race.Name,
                player.Class.Name,
                (String.IsNullOrEmpty(player.ParagonPath) ? "" : " (" + player.ParagonPath + ")"),
                (String.IsNullOrEmpty(player.EpicDestiny) ? "" : " (" + player.EpicDestiny + ")"));
        }

        public Player Player
        {
            get { return player; }
        }

        protected void PrintCommand_Executed(object sender, RoutedEventArgs e)
        {
            int height = (int)(double)gridMain.GetValue(FrameworkElement.ActualHeightProperty);
            int width = (int)(double)gridMain.GetValue(FrameworkElement.ActualWidthProperty);

            RenderTargetBitmap picture = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);

            picture.Render(gridMain);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(picture));
            Bitmap bitmap;

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }

            IDataObject data = new DataObject();
            data.SetData(DataFormats.Bitmap, bitmap, true);
            Clipboard.SetDataObject(data, true);
        }

        protected void PrintPowersCommand_Executed(object sender, RoutedEventArgs e)
        {
            var container = BuildPowersVisual();

            container.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            container.Arrange(new Rect(container.DesiredSize));

            using (var tempPresentationSource = new HwndSource(new HwndSourceParameters()) { RootVisual = container })
            {
                Dispatcher.Invoke(DispatcherPriority.SystemIdle, new Action(() => { }));
                var picture = new RenderTargetBitmap((int)container.ActualWidth, (int)container.ActualHeight, 96, 96, PixelFormats.Default);

                picture.Render(container);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(picture));
                Bitmap bitmap;

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bitmap = new Bitmap(stream);
                }

                IDataObject data = new DataObject();
                data.SetData(DataFormats.Bitmap, bitmap, true);
                Clipboard.SetDataObject(data, true);
            }
        }

        private Panel BuildPowersVisual()
        {
            WrapPanel panel = new WrapPanel();

            panel.Background = System.Windows.Media.Brushes.White;
            panel.Orientation = Orientation.Horizontal;
            panel.MaxWidth = 1200;

            List<Power> powers = new List<Power>();

            powers.AddRange(player.Powers.AtWillPowers);
            powers.AddRange(player.Powers.EncounterPowers);
            powers.AddRange(player.Powers.DailyPowers);

            foreach (Power power in powers)
            {
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();

                image.Source = Common.BuildBitmapImage(power.Picture);
                image.Stretch = Stretch.None;
                image.VerticalAlignment = VerticalAlignment.Top;

                panel.Children.Add(image);
            }

            return panel;
        }
    }
}
