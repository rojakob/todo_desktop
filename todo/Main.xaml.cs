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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Entities;
using todo.Components;
using todo.Repository;
using todo.Task;

namespace todo
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        

        private void LoadTasks()
        {
           
            // Очищаем StackPanel перед добавлением новых элементов
            scroll_tasks.Content = new StackPanel();
            var stackPanel = (StackPanel)scroll_tasks.Content;
            
            

            // Проходимся по списку задач и создаем TaskControl для каждой
            foreach (var task in TaskRepository.TaskRepo.tasks)
            {
                // Создаем новый TaskControl

                TaskControl taskControl = new TaskControl();
                // Устанавливаем значения для task_title и task_time
                taskControl.task_title.Text = task.Name;
                taskControl.task_time.Text = task.Time;

                // Добавляем эффект тени
                taskControl.Effect = new DropShadowEffect { BlurRadius = 10, ShadowDepth = 5, Opacity = 0.2 };
                taskControl.Margin = new Thickness (5);
                // Добавляем TaskControl в StackPanel
                stackPanel.Children.Add(taskControl);
            }
        }

        

        public Main()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void nameLabel_Initialized(object sender, EventArgs e)
        { 
            nameLabel.Content = UserRepository.currentUser.Name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Create_task().Show();
            this.Close();
        }

        private void btn_history_Click(object sender, RoutedEventArgs e)
        {
            new History().Show();
            this.Close();
        }
    }
}
