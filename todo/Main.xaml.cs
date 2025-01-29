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
                if (task.Completed == false)
                {
                    TaskControl taskControl = new TaskControl();
                    // Устанавливаем значения для task_title и task_time
                    taskControl.task_title.Text = task.Name;
                    taskControl.task_time.Text = task.Time;
                    taskControl.MouseDoubleClick += TaskControl_MouseDoubleClick;

                    // Добавляем эффект тени
                    taskControl.Effect = new DropShadowEffect { BlurRadius = 10, ShadowDepth = 5, Opacity = 0.2 };
                    taskControl.Margin = new Thickness(5);
                    // Добавляем TaskControl в StackPanel
                    stackPanel.Children.Add(taskControl);
                }
                
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

        private void TaskControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TaskControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaskControl currentTaskControl = sender as TaskControl;

            string name = currentTaskControl.task_title.Text;

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

        private void done_btn_Click(object sender, RoutedEventArgs e)
        {
            string name2 = title_task_label.Text;
            
            foreach (var task in TaskRepository.TaskRepo.tasks)
            {
                if (name2 == task.Name)
                {
                    task.Completed = true;
                }
                
            }

            // Очищаем StackPanel перед добавлением новых элементов
            scroll_tasks.Content = new StackPanel();
            var stackPanel = (StackPanel)scroll_tasks.Content;



            // Проходимся по списку задач и создаем TaskControl для каждой
            foreach (var task in TaskRepository.TaskRepo.tasks)
            {
                // Создаем новый TaskControl
                if (task.Completed == false)
                {
                    TaskControl taskControl = new TaskControl();
                    // Устанавливаем значения для task_title и task_time
                    taskControl.task_title.Text = task.Name;
                    taskControl.task_time.Text = task.Time;
                    taskControl.MouseDoubleClick += TaskControl_MouseDoubleClick;

                    // Добавляем эффект тени
                    taskControl.Effect = new DropShadowEffect { BlurRadius = 10, ShadowDepth = 5, Opacity = 0.2 };
                    taskControl.Margin = new Thickness(5);
                    // Добавляем TaskControl в StackPanel
                    stackPanel.Children.Add(taskControl);
                }
            }
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            string name3 = title_task_label.Text;

            // 1. Создаем копию списка задач
            var tasksToRemove = TaskRepository.TaskRepo.tasks.Where(task => task.Name == name3).ToList();


            // 2. Удаляем элементы из оригинального списка
            foreach (var task in tasksToRemove)
            {
                TaskRepository.TaskRepo.tasks.Remove(task);
            }

            LoadTasks();
        }
    }
}
