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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using todo.ApplicationData;
using todo.Repository;
using todo.View;

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
            AppConnect.todoModel = new todoEntities();
            AppFrame.frameMain = FrmMain;
            FrmMain.Navigate(new PageLogin());
            
        }

        private void FrmMain_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var animation = new ThicknessAnimation();
            animation.Duration = TimeSpan.FromSeconds(0.3);
            animation.DecelerationRatio = 0.7;
            animation.To = new Thickness(0, 0, 0, 0);

            if (e.NavigationMode == NavigationMode.New)
            {
                animation.From = new Thickness(500, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                animation.From = new Thickness(0, 0, 500, 0);
            }

            (e.Content as Page).BeginAnimation(MarginProperty, animation);
        }
    }
}
