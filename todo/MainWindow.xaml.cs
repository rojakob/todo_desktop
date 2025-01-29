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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using todo.Repository;

namespace todo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserRepository.GetInstance().Register(new Entities.UserModel() { Email = "jacob@ai.com", Name = "jacob", Pass = "123456" }, "123456");
        }

        private void loginbut1_Click(object sender, RoutedEventArgs e)
        {
            var email = mailbox1.Text;
            var pass = passbox1.Password;

            try 
            { 
                UserRepository.GetInstance().Login(email, pass); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new MainEmpty().Show();
            Close();
        }

        private void regbut1_Click(object sender, RoutedEventArgs e)
        {
            new Reg().Show();
            Close();
        }
    }
}
