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
using todo.Components;
using todo.Task;

namespace todo
{
    /// <summary>
    /// Логика взаимодействия для History.xaml
    /// </summary>
    public partial class History : Window
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
                if (task.Completed == true)
                {
                    TaskControl taskControl = new TaskControl();
                    // Устанавливаем значения для task_title и task_time
                    taskControl.task_title.Text = task.Name;
                    taskControl.task_time.Text = task.Time;
                    taskControl.task_title.TextDecorations = TextDecorations.Strikethrough;
                    taskControl.task_time.TextDecorations = TextDecorations.Strikethrough;
                    taskControl.task_title.Opacity = 0.4;
                    taskControl.task_time.Opacity = 0.4;
                    // Добавляем эффект тени
                    taskControl.Effect = new DropShadowEffect { BlurRadius = 10, ShadowDepth = 5, Opacity = 0.2 };
                    taskControl.MouseDoubleClick += TaskControl_MouseDoubleClick;
                    taskControl.Margin = new Thickness(5);
                    taskControl.check.Visibility = Visibility.Visible;
                    // Добавляем TaskControl в StackPanel
                    stackPanel.Children.Add(taskControl);
                }
            }
        }

        private void TaskControl_MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public History()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void add_new_task_Click(object sender, RoutedEventArgs e)
        {
            new Main().Show();
            this.Close();
        }

        private void TaskControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaskControl currentTaskControl = sender as TaskControl;

            string name = currentTaskControl.task_title.Text;

            currentTaskControl.fon.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABF294"));

            foreach (var task in TaskRepository.TaskRepo.tasks)
            {
                if (name == task.Name)
                {
                    title_task_label.Text = task.Name;
                    time_task_label.Text = task.Time;
                    date_task_label.Text = task.Date;
                    description_task_label.Text = task.Description;
                }
            }
        }

        private void btn_tasks_Click(object sender, RoutedEventArgs e)
        {
            new Main().Show();
            this.Close();
        }

        private void btn_history_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }
    }
}
