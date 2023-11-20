using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.Threading;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Windows.Shapes;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace Obuchanik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window, IReadSave
    {
        List<Test> listTest = new List<Test>();
        int count = 0;
        int countOfCards;
        Button btnNewTest;
        Label NameTestLabel;
        TextBox textBoxForName;
        Button BtnNextStep;
        Test test;
        string imagePath;
        string imageAnsPath;
        string imageQuestPath;

        Dictionary<string, Action<string>> callerDict = new Dictionary<string, Action<string>>();

        public MainWindow()
        {
            listTest = GetData("DataSerialize");
            InitializeComponent();
            foreach (var item in listTest)
            {
                AddTestOnStPn(item);
            }
        }

        public void AddTestOnStPn(Test test)
        {
            count++;//считаем количество созданных тестов

            btnNewTest = new Button()
            {
                Content = test.nameTest,
                Name = "Test" + $"{count}",
                Style = (Style)FindResource("TestButton")
            };

            callerDict.Add(btnNewTest.Name, OpenSelectTest);
            btnNewTest.Click += new RoutedEventHandler(BtnTest_Clic);
            StPnTests.Children.Add(btnNewTest);
        }

        public void OpenSelectTest(string name)
        {
            //вывод тестов сохраненных в классе Test из List<Test>
            //по ключу Name теста
        }

        //обработчик кнопки для добавления новых тестов
        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            countOfCards = 0;

            count++;//считаем количество созданных тестов
            btnNewTest = new Button()
            {
                Content = "Новый тест " + $"{count}",
                Name = "Test" + $"{count}",
                Style = (Style)FindResource("TestButton")
            };

            callerDict.Add(btnNewTest.Name, OpenSelectTest);
            btnNewTest.Click += new RoutedEventHandler(BtnTest_Clic);
            StPnTests.Children.Add(btnNewTest);

            //создание строк в цикле
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

            //создаем объект класса Test
            test = new Test();
        }

        //обработчик нажатия на кнопку созданную в стек панеле
        //для открытия теста
        private void BtnTest_Clic(object sender, RoutedEventArgs e)
        {
            Button current = (Button)sender;

            callerDict[current.Name].Invoke(current.Name);

            OpenSelectTest(current.Name);
        }

        //обработчик на btnNextStep
        //создается при создании нового теста

        // нужно сделать метод по созданию карточки
        // его нужно сделать отдельно, иначе в неверном порядке идет создание карточки - текст боксы
        // еще не заполнены, а мы уже создаем новую карточку - по отладке понятно о чем речь

        private void BtnNextStep_Clic(object sender, RoutedEventArgs e)
        {
            test.nameTest = textBoxForName.Text;

            mainGrid.ShowGridLines = false;
            imageAnsPath = null;
            imageQuestPath = null;

            //переносим значение введенного названия теста в отображение в кнопке и поле
            btnNewTest.Content = textBoxForName.Text;
            NameTestLabel.Content = textBoxForName.Text;

            //очищаем grid для выводов полей по заполнению вопросов и ответов
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
            imageQuestPath = imagePath;
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
            imageAnsPath = imagePath;
            mainGrid.Children.Add(chooseAnswerImageButton);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("plus.png", UriKind.Relative));

            //кнопка по добавлению новой карточки к тесту
            //с добавлением к ней обработчика событий
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
            //addCardButton.Click += new RoutedEventHandler(addCardButton_Click);

            addCardButton.Click += new RoutedEventHandler(BtnNextStep_Clic);
            Grid.SetRow(addCardButton, 10);
            mainGrid.Children.Add(addCardButton);

            img = new Image();
            img.Source = new BitmapImage(new Uri("next.png", UriKind.Relative));
            Button BtnEndOfNewTestCreation = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                Margin = new Thickness(0, 5, 95, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            BtnEndOfNewTestCreation.Click += new RoutedEventHandler(BtnEndOfNewTestCreation_Click);

            Grid.SetRow(BtnEndOfNewTestCreation, 10);
            mainGrid.Children.Add(BtnEndOfNewTestCreation);

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


            Card newCard = new Card(textBoxForQuestion.Text, textBoxForAnswer.Text);

            if (imageQuestPath != null)
            {
                img = new Image();
                img.Source = new BitmapImage(new Uri(imageQuestPath, UriKind.Relative));
                newCard.AddQuestionImage(img);
            }

            if (imageAnsPath != null)
            {
                img = new Image();
                img.Source = new BitmapImage(new Uri(imageAnsPath, UriKind.Relative));
                newCard.AddAnswerImage(img);
            }

            test.AddCard(newCard);
        }

        private void BtnEndOfNewTestCreation_Click(object sender, RoutedEventArgs e)
        {
            listTest.Add(test);
            SaveData("DataSerialize", listTest);
            mainGrid.Children.Clear();
        }

        //private void addCardButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Card newCard = new Card();
        //}

        //обработчик для выбора изображения
        private void chooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // фильтр для выбора только картинки - файлы соответсвующих форматов
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
            }
        }

        public List<Test> GetData(string path)
        {
            XmlSerializer xmlDeserilizer = new XmlSerializer(typeof(List<Test>));

            List<Test> tests = new List<Test>();

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                tests = xmlDeserilizer.Deserialize(fs) as List<Test>;
            }
            
            return tests;
        }

        public void SaveData(string path, List<Test> listTest)
        {
            XmlSerializer xmlSerialaze = new XmlSerializer(typeof(List<Test>));

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xmlSerialaze.Serialize(fs, listTest);
            }

            MessageBox.Show("Данные сохранены");
        }
    }
}