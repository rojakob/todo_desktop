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
    /// Логика взаимодействия для PageCreateTask.xaml
    /// </summary>
    public partial class PageCreateTask : Page
    {
        public PageCreateTask()
        {
            InitializeComponent();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void btn_task_crate_Click(object sender, RoutedEventArgs e)
        {
            string theDate = dp_date.SelectedDate.Value.ToString();
            int nomer = UserRepository.currentUser.Id_user; 

            TaskRepository.AddTask(txb_title.Text, txb_description.Text, txb_category.Text, theDate, txb_timepicker.Text, false, nomer);

            AppFrame.frameMain.Navigate(new PageMain());
        }
    }
}
