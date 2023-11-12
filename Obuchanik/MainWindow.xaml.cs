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
using Microsoft.Win32;

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
        Label NameTestLabel;
        TextBox textBoxForName;
        Button BtnNextStep;

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

            //RowDefinition rowDefinition1 = new RowDefinition();
            //RowDefinition rowDefinition2 = new RowDefinition();
            //RowDefinition rowDefinition3 = new RowDefinition();
            //RowDefinition rowDefinition4 = new RowDefinition();
            //RowDefinition rowDefinition5 = new RowDefinition();
            //RowDefinition rowDefinition6 = new RowDefinition();
            //RowDefinition rowDefinition7 = new RowDefinition();
            //RowDefinition rowDefinition8 = new RowDefinition();
            //RowDefinition rowDefinition9 = new RowDefinition();
            //RowDefinition rowDefinition10 = new RowDefinition();
            //RowDefinition rowDefinition11 = new RowDefinition();
            //RowDefinition rowDefinition12 = new RowDefinition();

            //mainGrid.RowDefinitions.Add(rowDefinition1);
            //mainGrid.RowDefinitions.Add(rowDefinition2);
            //mainGrid.RowDefinitions.Add(rowDefinition3);
            //mainGrid.RowDefinitions.Add(rowDefinition4);
            //mainGrid.RowDefinitions.Add(rowDefinition5);
            //mainGrid.RowDefinitions.Add(rowDefinition6);
            //mainGrid.RowDefinitions.Add(rowDefinition7);
            //mainGrid.RowDefinitions.Add(rowDefinition8);
            //mainGrid.RowDefinitions.Add(rowDefinition9);
            //mainGrid.RowDefinitions.Add(rowDefinition10);
            //mainGrid.RowDefinitions.Add(rowDefinition11);
            //mainGrid.RowDefinitions.Add(rowDefinition12);

            for (int i = 0; i < 12; i++)
            {
                RowDefinition rowDifinition = new RowDefinition();
                mainGrid.RowDefinitions.Add(rowDifinition);
            }

            //характеристика Label с помощью {}
            NameTestLabel = new Label()
            {
                Content = "Новый тест " + $"{count}",
                FontSize = 35,
                Margin = new Thickness(300, 5, 200, 10)
            };
            Grid.SetRow(NameTestLabel, 0);
            mainGrid.Children.Add(NameTestLabel);

            //характеристика Label с помощью {}
            Label nameNewTest = new Label()
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

        int countOfCards = 0;

        //обработчик на btnNextStep
        private void BtnNextStep_Clic(object sender, RoutedEventArgs e)
        {
            mainGrid.ShowGridLines = false;
            //переносим значение введенного названия теста в отображение в кнопке и поле
            btnNewTest.Content = textBoxForName.Text;
            NameTestLabel.Content = textBoxForName.Text;

            //очищаем grid для выводв полей по заполнению вопросов и ответов
            mainGrid.Children.Clear();

            Label nameTest = new Label()
            {
                Content = NameTestLabel.Content,
                FontSize = 35,
                HorizontalAlignment = HorizontalAlignment.Center,
                //Margin = new Thickness(300, 5, 80, 0)
            };
            Grid.SetRow(nameTest, 0);
            mainGrid.Children.Add(nameTest);

            Label CardLabel = new Label()
            {
                Content = $"Карточка {++countOfCards}",
                FontSize = 32,
                Margin = new Thickness(280, 5, 80, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid.SetRow(CardLabel, 1);
            mainGrid.Children.Add(CardLabel);

            Label WriteQueLabel = new Label()
            {
                Content = "Вопрос:",
                FontSize = 30,
                Margin = new Thickness(50, 2, 10, 0)
            };
            Grid.SetRow(WriteQueLabel, 2);
            mainGrid.Children.Add(WriteQueLabel);

            TextBox textBoxForQuestion = new TextBox()
            {
                Height = 40,
                Width = 500,
                FontSize = 22,
                Margin = new Thickness(50, 0, 100, 0)
            };
            Grid.SetRow(textBoxForQuestion, 3);
            mainGrid.Children.Add(textBoxForQuestion);

            Label OrLabel = new Label()
            {
                Content = "Или выбирите картинку:",
                FontSize = 30,
                Margin = new Thickness(50, 2, 10, 0)
            };
            Grid.SetRow(OrLabel, 4);
            mainGrid.Children.Add(OrLabel);

            Button chooseQuestionImageButton = new Button()
            {
                Height = 40,
                Width = 120,
                //Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = "Вопрос картинка",
                Margin = new Thickness(0, 5, 400, 0)
            };
            Grid.SetRow(chooseQuestionImageButton, 5);
            chooseQuestionImageButton.Click += new RoutedEventHandler(chooseImageButton_Click);
            mainGrid.Children.Add(chooseQuestionImageButton);

            Label WriteAnsLabel = new Label()
            {
                Content = "Ответ:",
                FontSize = 30,
                Margin = new Thickness(50, 2, 10, 0)
            };
            Grid.SetRow(WriteAnsLabel, 6);
            mainGrid.Children.Add(WriteAnsLabel);

            TextBox textBoxForAnswer = new TextBox()
            {
                Height = 40,
                Width = 500,
                FontSize = 22,
                Margin = new Thickness(50, 0, 100, 0)
            };
            Grid.SetRow(textBoxForAnswer, 7);
            mainGrid.Children.Add(textBoxForAnswer);

            Label OrLabel2 = new Label()
            {
                Content = "Или выбирите картинку:",
                FontSize = 30,
                Margin = new Thickness(50, 2, 10, 0)
            };
            Grid.SetRow(OrLabel2, 8);
            mainGrid.Children.Add(OrLabel2);

            Button chooseAnswerImageButton = new Button()
            {
                Height = 40,
                Width = 120,
                //Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = "Ответ картинка",
                Margin = new Thickness(0, 5, 400, 0)
            };
            Grid.SetRow(chooseAnswerImageButton, 9);
            chooseAnswerImageButton.Click += new RoutedEventHandler(chooseImageButton_Click);
            mainGrid.Children.Add(chooseAnswerImageButton);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("plus.png", UriKind.Relative));
            Button addCardButton = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                Margin = new Thickness(90, 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            addCardButton.Click += new RoutedEventHandler(BtnNextStep_Clic);
            Grid.SetRow(addCardButton, 10);
            mainGrid.Children.Add(addCardButton);

            img = new Image();
            img.Source = new BitmapImage(new Uri("next.png", UriKind.Relative));
            Button endOfNewTestCreation= new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                Margin = new Thickness(0, 5, 95, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            endOfNewTestCreation.Click += new RoutedEventHandler(BtnNextStep_Clic);
            Grid.SetRow(endOfNewTestCreation, 10);
            mainGrid.Children.Add(endOfNewTestCreation);

            Label addNewCardForButtonLabel = new Label()
            {
                Content = "Добавить\n карточку",
                FontSize = 30,
                Margin = new Thickness(70, 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid.SetRow(addNewCardForButtonLabel, 11);
            mainGrid.Children.Add(addNewCardForButtonLabel);


            Label endOfNewTestCreationForButtonLabel = new Label()
            {
                Content = "Завершить\n создание",
                FontSize = 30,
                Margin = new Thickness(0, 5, 70, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            Grid.SetRow(endOfNewTestCreationForButtonLabel, 11);
            mainGrid.Children.Add(endOfNewTestCreationForButtonLabel);
        }

        private void chooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // фильтр для выбора только картинки - файлы соответсвующих форматов
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Обработка выбранного файла
            }
        }
    }
}