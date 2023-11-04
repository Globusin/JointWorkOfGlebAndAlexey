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

        int count = 0;

        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();

            count++;//считаем количество созданных тестов 

            Button btnNewTest = new Button();
            btnNewTest.Content = "Новый тест " + $"{count}";
            btnNewTest.Name = "Test" + $"{count}";
            StPnTests.Children.Add(btnNewTest);

            //вариант как можно подключить для кнопки обработчик событий
            btnNewTest.Click += new RoutedEventHandler(BtnTest_Clic);

            RowDefinition rowDefinition1 = new RowDefinition();
            RowDefinition rowDefinition2 = new RowDefinition();
            RowDefinition rowDefinition3 = new RowDefinition();

            mainGrid.RowDefinitions.Add(rowDefinition1);
            mainGrid.RowDefinitions.Add(rowDefinition2);
            mainGrid.RowDefinitions.Add(rowDefinition3);

            Label newTestLabel = new Label();
            newTestLabel.Content = "Новый тест " + $"{count}";
            newTestLabel.FontSize = 25;
            newTestLabel.Margin = new Thickness(300, 5, 200, 10);

            Grid.SetRow(newTestLabel, 0);
            mainGrid.Children.Add(newTestLabel);

            //this.Show();

            //Label nameNewTest = new Label();
            //BorderMain.Child = nameNewTest;
            //newTestLabel.Content = "Введите название:";
            //newTestLabel.FontSize = 20;
            //newTestLabel.Margin = new Thickness(350, 500, 200, 10);
        }

        //самодельно написанный обработчик нажатия на кнопку созданную в стек панеле
        private void BtnTest_Clic(object sender, RoutedEventArgs e)
        {
            Button current = (Button)sender;
            
        }
    }
}
