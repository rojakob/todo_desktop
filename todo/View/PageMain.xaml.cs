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
using System.Windows.Navigation;
using System.Windows.Shapes;
using todo.ApplicationData;
using todo.Components;
using todo.Repository;
using todo.Task;

namespace todo.View
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public PageMain()
        {
            InitializeComponent();
            LoadTasks();
        }

       

       

        //private async void LoadTasks()
        //{

        //    // Очищаем StackPanel перед добавлением новых элементов
        //    scroll_tasks.Content = new StackPanel();
        //    var stackPanel = (StackPanel)scroll_tasks.Content;

        //    var tasks = await TaskRepository.GetAllUserTasks();
        //    // Проходимся по списку задач и создаем TaskControl для каждой
        //    foreach (var task in tasks)
        //    {
        //        if (tasks != null)
        //        {
        //            if (task.isCompleted == false)
        //            {
        //                // Создаем новый TaskControl
        //                TaskControl taskControl = new TaskControl();
        //                // Устанавливаем значения для task_title и task_time
        //                taskControl.task_title.Text = task.title;
        //                taskControl.task_time.Text = task.category;
        //                taskControl.MouseDoubleClick += TaskControl_MouseDoubleClick;

        //                // Добавляем эффект тени
        //                taskControl.Effect = new DropShadowEffect { BlurRadius = 10, ShadowDepth = 5, Opacity = 0.2 };
        //                taskControl.Margin = new Thickness(5);
        //                // Добавляем TaskControl в StackPanel
        //                stackPanel.Children.Add(taskControl);
        //            }

        //        }
        //    }



        //}

        

        //private async void TaskControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //   var tasks = await TaskRepository.GetAllUserTasks();
            
        //        TaskControl currentTaskControl = sender as TaskControl;

        //        string name = currentTaskControl.task_title.Text;

        //        foreach (var task in tasks)
        //        {
        //            if (name == task.title)
        //            {
        //                title_task_label.Text = task.title;
        //                time_task_label.Text = task.category;
        //                date_task_label.Text = task.date.ToString();
        //                description_task_label.Text = task.description;
        //            }
        //        }
            
        //}

        private void nameLabel_Initialized(object sender, EventArgs e)
        {
            string nameofUser = UserRepository.currentUser.Name;
            nameLabel.Content = nameofUser;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageCreateTask());
        }

        private void btn_history_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageHistory());
        }

        private void done_btn_Click(object sender, RoutedEventArgs e)
        {
            string name2 = title_task_label.Text;

            using (var context = new todoEntities())
            {

                TaskRepository.TaskIsDoneAPI(name2);

                foreach (var task in context.TaskModel)
                {
                    if (name2 == task.Name)
                    {

                        task.Completed = true;
                    }

                }
                context.SaveChanges();
            }

                
            
            LoadTasks();

            title_task_label.Text = "Выберите задачу";
            time_task_label.Text = "";
            date_task_label.Text = "";
            description_task_label.Text = "";

        }

        private  void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            string name3 = title_task_label.Text;
           
            try 
            {
                
                TaskRepository.DeleteTaskByName(name3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadTasks();

            title_task_label.Text = "Выберите задачу";
            time_task_label.Text = "";
            date_task_label.Text = "";
            description_task_label.Text = "";
        }

        private void LoadTasks()
        {

            // Очищаем StackPanel перед добавлением новых элементов
            scroll_tasks.Content = new StackPanel();
            var stackPanel = (StackPanel)scroll_tasks.Content;

            using (var context = new todoEntities())
            {
                // Проходимся по списку задач и создаем TaskControl для каждой
                foreach (var task in context.TaskModel)
                {
                    if (task.Completed == false)
                    {
                        // Создаем новый TaskControl
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
        }


        private void TaskControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var context = new todoEntities())
            {
                TaskControl currentTaskControl = sender as TaskControl;

                string name = currentTaskControl.task_title.Text;

                foreach (var task in context.TaskModel)
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
        }
    }
}
