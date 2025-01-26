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
using todo.Task;

namespace todo
{
    /// <summary>
    /// Логика взаимодействия для Create_task.xaml
    /// </summary>
    public partial class Create_task : Window
    {
        public Create_task()
        {
            InitializeComponent();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            new MainEmpty().Show();
            this.Close();
        }

        private void btn_task_crate_Click(object sender, RoutedEventArgs e)
        {
            string theDate = dp_date.SelectedDate.Value.ToString();

            

            TaskRepository.TaskRepo.AddTask(txb_title.Text, txb_description.Text, txb_category.Text, theDate, txb_timepicker.Text );
            
            new Main().Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainEmpty().Show();
            this.Close();
        }
    }
}
