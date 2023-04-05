using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace botserver_standard
{
    /// <summary>
    /// Логика взаимодействия для AskingPassword.xaml
    /// </summary>
    public partial class AskingPassword : Window
    {
        public AskingPassword()
        {
            InitializeComponent();
        }
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EnterPwdBox.Password == Settings.pwd) //если введенный пароль корректен
            {
                this.DialogResult = true;
            }
        }

        public string Password
        {
            get { return EnterPwdBox.Password; }
        }
    }
}
