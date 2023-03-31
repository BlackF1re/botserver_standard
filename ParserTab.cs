using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        private void ParserPeparseBtn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Reparsing...\n";

            });
            Task.Factory.StartNew(() => CardParser()); //ok
        }
    }
}
