﻿using System;
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
        string NamesTest;

        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();

            count++;//считаем количество созданных тестов 


            //создаем кнопки
            Button btnNewTest = new Button();
            btnNewTest.Content = "Новый тест " + $"{count}";
            btnNewTest.Name = "Test" + $"{count}";
            btnNewTest.Style = (Style)FindResource("TestButton");
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

            Label newTestLabel = new Label();
            newTestLabel.Content = "Новый тест " + $"{count}";
            newTestLabel.FontSize = 25;
            newTestLabel.Margin = new Thickness(300, 5, 200, 10);

            Grid.SetRow(newTestLabel, 0);
            mainGrid.Children.Add(newTestLabel);

            Label nameNewTest = new Label();
            nameNewTest.Content = "Введите название: ";
            nameNewTest.FontSize = 22;
            nameNewTest.Margin = new Thickness(50, 50, 50, 10);

            Grid.SetRow(nameNewTest, 1);
            mainGrid.Children.Add(nameNewTest);

            TextBox textBoxForName = new TextBox()
            {
                Height = 40,
                Width = 500,
                FontSize = 22,
                Margin = new Thickness(50, 0, 100, 10)
            };

            NamesTest = textBoxForName.Text;

            Grid.SetRow(textBoxForName, 2);
            mainGrid.Children.Add(textBoxForName);

            Button nextStep = new Button();
            nextStep.Style = (Style)FindResource("RoundButton");
            //Image img = new Image();
            //img.Source = new BitmapImage(new Uri(@"next.png"));
            //nextStep.Content = img;
            Grid.SetRow(nextStep, 3);
            mainGrid.Children.Add(nextStep);
        }

        //самодельно написанный обработчик нажатия на кнопку созданную в стек панеле
        private void BtnTest_Clic(object sender, RoutedEventArgs e)
        {
            Button current = (Button)sender;
            MessageBox.Show($"{NamesTest}");
        }
    }
}
