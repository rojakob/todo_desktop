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
using todo.Task;

namespace todo.View
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private async void loginbut1_Click(object sender, RoutedEventArgs e)
        {
            var email = mailbox1.Text;
            var pass = passbox1.Password;

            

            try
            { 
                UserRepository.GetInstance().Login(email, pass);
                UserRepository.GetUserInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isLoginSuccessful = await UserRepository.GetInstance().LoginAPI(email, pass);

            if (isLoginSuccessful)
            {
                if (UserRepository.UserHasTasks(UserRepository.currentUser.Id_user))
                {
                    AppFrame.frameMain.Navigate(new PageMain());
                }
                else
                {
                    AppFrame.frameMain.Navigate(new PageMainEmpty());
                }
            }


        }

        private void regbut1_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageRegistration());
        }
    }
}



