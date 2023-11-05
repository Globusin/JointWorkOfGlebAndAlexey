using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
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
        Button btnNewTest;
        Label newTestLabel;
        Label nameNewTest;
        TextBox textBoxForName;
        Button BtnNextStep;
        Label WriteQueAnsLabel;

        Dictionary<string, Action<string>> callerDict = new Dictionary<string, Action<string>>();

        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();

            count++;//считаем количество созданных тестов 

            //создаем кнопки
            btnNewTest = new Button()
            {
                Content = "Новый тест " + $"{count}",
                Name = "Test" + $"{count}",
                Style = (Style)FindResource("TestButton")
            };
            
            StPnTests.Children.Add(btnNewTest);

            //вариант как можно подключить для кнопки обработчик событий
            btnNewTest.Click += new RoutedEventHandler(BtnTest_Clic);

            RowDefinition rowDefinition1 = new RowDefinition();
            RowDefinition rowDefinition2 = new RowDefinition();
            RowDefinition rowDefinition3 = new RowDefinition();
            RowDefinition rowDefinition4 = new RowDefinition();

            mainGrid.RowDefinitions.Add(rowDefinition1);
            mainGrid.RowDefinitions.Add(rowDefinition2);
            mainGrid.RowDefinitions.Add(rowDefinition3);
            mainGrid.RowDefinitions.Add(rowDefinition4);

            //характеристика Label с помощью {}
            newTestLabel = new Label()
            {
                Content = "Новый тест " + $"{count}",
                FontSize = 35,
                Margin = new Thickness(300, 5, 200, 10)
            };
            
            Grid.SetRow(newTestLabel, 0);
            mainGrid.Children.Add(newTestLabel);

            //характеристика Label с помощью {}
            nameNewTest = new Label()
            {
                Content = "Введите название: ",
                FontSize = 30,
                Margin = new Thickness(80, 50, 50, 10)
            };

            Grid.SetRow(nameNewTest, 1);
            mainGrid.Children.Add(nameNewTest);

            //характеристика TextBox с помощью {}
            textBoxForName = new TextBox()
            {
                Height = 40,
                Width = 500,
                FontSize = 22,
                Margin = new Thickness(50, 0, 100, 10)
            };

            Grid.SetRow(textBoxForName, 2);
            mainGrid.Children.Add(textBoxForName);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("next.png", UriKind.Relative));
            //характеристика Button с помощью {}
            BtnNextStep = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                Margin = new Thickness(160, 100, 100, 10)
            };
            BtnNextStep.Click += new RoutedEventHandler(BtnNextStep_Clic);

            Grid.SetRow(BtnNextStep, 3);
            mainGrid.Children.Add(BtnNextStep);
        }

        //самодельно написанный обработчик нажатия на кнопку созданную в стек панеле
        private void BtnTest_Clic(object sender, RoutedEventArgs e)
        {
            Button current = (Button)sender;
            
        }

        //обработчик на btnNextStep
        private void BtnNextStep_Clic(object sender, RoutedEventArgs e)
        {
            //переносим значение введенного названия теста в отображение в кнопке и поле
            btnNewTest.Content = textBoxForName.Text;
            newTestLabel.Content = textBoxForName.Text;

            //очищаем grid для выводв полей по заполнению вопросов и ответов
            mainGrid.Children.Clear();

            WriteQueAnsLabel = new Label()
            {
                Content = "Введите вопрос",
                FontSize = 40,
                Margin = new Thickness(200, 60, 80, 80)
            };

            mainGrid.Children.Add(WriteQueAnsLabel);
        }
    }
}