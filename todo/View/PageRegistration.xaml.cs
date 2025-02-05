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
using todo.ApplicationData;
using todo.Repository;

namespace todo.View
{
    /// <summary>
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
        }

        private void backbut1_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void regbut2_Click(object sender, RoutedEventArgs e)
        {
            var name = name_textbox.Text;
            var email = email_textbox.Text;
            var pass1 = pass1_textbox.Text;
            var pass2 = pass2_textbox.Text;

            try
            {
                UserRepository.GetInstance().Register(new UserModel() { Email = email, Name = name, Pass = pass1 }, pass2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AppFrame.frameMain.Navigate(new PageMainEmpty());
        }
    }
}
