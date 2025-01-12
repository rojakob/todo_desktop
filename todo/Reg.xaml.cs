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
using todo.Repository;

namespace todo
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void regbut2_Click(object sender, RoutedEventArgs e)
        { 
            var name = name_textbox.Text;
            var email = email_textbox.Text;
            var pass1 = pass1_textbox.Text;
            var pass2 = pass2_textbox.Text;

            try
            {
                UserRepository.GetInstance().Register(new Entities.UserModel() { Email = email, Name = name, Pass = pass1 }, pass2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new MainEmpty().Show();
            Close();
        }

        private void backbut1_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
