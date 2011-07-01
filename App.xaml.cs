using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // This causes textbox to select all text when tabbing into them.
            EventManager.RegisterClassHandler(typeof(TextBox),
                    TextBox.GotFocusEvent,
                    new RoutedEventHandler((sender, args) => { (sender as TextBox).SelectAll(); }));

            base.OnStartup(e);
        }
    }
}
