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
        private void SetTokenBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetLogPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetDbPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OutputPauseBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImportSettingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetPwdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SetPwdBox.Password == SetRepeatedPwdBox.Password)
            {
                Settings.pwd = SetRepeatedPwdBox.Password;
                MessageBox.Show("Password setted", "Notice");
            }
            else
            {
                MessageBox.Show("Passwords are not the same", "Error");
                SetPwdBox.Clear();
                SetRepeatedPwdBox.Clear();
            }
        }

        private void UseThisPwdCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pwdIsSetted = true;
        }

        private void UseThisPwdCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pwdIsSetted = false;
        }
    }
}
