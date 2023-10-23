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

namespace Obuchanik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            Button btnAddNewTest = new Button();
            btnAddNewTest.Content = "Новый тест";
            StPnTests.Children.Add(btnAddNewTest);

            Label newTestLabel = new Label();
            BorderMain.Child = newTestLabel;
            newTestLabel.Content = "Новый тест";
            newTestLabel.FontSize = 25;
            newTestLabel.Margin = new Thickness(350, 5, 200, 10);

            Label nameNewTest = new Label();
            BorderMain.Child = nameNewTest;
            newTestLabel.Content = "Введите название:";
            newTestLabel.FontSize = 20;
            newTestLabel.Margin = new Thickness(350, 500, 200, 10);
        }
    }
}
