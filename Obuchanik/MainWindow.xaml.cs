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
using System.ComponentModel;

namespace Obuchanik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window, IReadSave
    {
        List<Test> listTest = new List<Test>();
        int countTests = 0;
        int countOfCards;
        Button btnNewTest;
        Label NameTestLabel;
        TextBox textBoxForName;
        TextBox textBoxForQuestion;
        TextBox textBoxForAnswer;
        Button BtnNextStep;
        Test test;
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

        //обработчик нажатия на кнопку созданную в стек панеле
        //для открытия теста
        private void BtnTest_Clic(object sender, RoutedEventArgs e)
        {
            Button current = (Button)sender;

            callerDict[current.Name].Invoke(current.Name);

            OpenSelectTest(current.Name);
        }

        //обработчик кнопки для добавления новых тестов
        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.ShowGridLines = true;
            countOfCards = 0;

            countTests++;
            btnNewTest = new Button()
            {
                Content = "Новый тест " + $"{countTests}",
                Name = "Test" + $"{countTests}",
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

            NameTestLabel = new Label()
            {
                Content = "Новый тест " + $"{countTests}",
                FontSize = 35,
                //Margin = new Thickness(300, 5, 200, 10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(NameTestLabel, 0);
            mainGrid.Children.Add(NameTestLabel);

            Label nameNewTest = new Label()
            {
                Content = "Введите название: ",
                FontSize = 30,
                //Margin = new Thickness(80, 50, 50, 10)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(nameNewTest, 1);
            mainGrid.Children.Add(nameNewTest);

            textBoxForName = new TextBox()
            {
                FontSize = 22,
                Height = 40,
                Width = 500,
                Margin = new Thickness(50, 0, 100, 10)
            };
            Grid.SetRow(textBoxForName, 2);
            mainGrid.Children.Add(textBoxForName);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("next.png", UriKind.Relative));
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

            test = new Test();
        }

        private void BtnNextStep_Clic(object sender, RoutedEventArgs e)
        {
            CreateNewCard();
        }

        private void BtnEndOfNewTestCreation_Click(object sender, RoutedEventArgs e)
        {
            addCard(); ////////////////////////////// Надо ли?
            listTest.Add(test);
            SaveData("DataSerialize", listTest);
            mainGrid.Children.Clear();
        }

        private void addCardButton_Clic(object sender, RoutedEventArgs e)
        {
            addCard();
            CreateNewCard();
        }

        private void chooseImageAnsButton_Click(object sender, RoutedEventArgs e)
        {
            imageAnsPath = LoadImage();
        }

        private void chooseImageQueButton_Click(object sender, RoutedEventArgs e)
        {
            imageQuestPath = LoadImage();
        }

        //////////////////////// Методы:

        public void AddTestOnStPn(Test test)
        {
            countTests++;

            btnNewTest = new Button()
            {
                Content = test.nameTest,
                Name = test.nameTest,
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

            Test curTest = listTest.FindAll(x => x.nameTest == name)[0];

            foreach (var item in curTest.cards)
            {
                mainGrid.Children.Clear();
                ShowCard(item);
            }
        }

        private void ShowCard(Card card)
        {
            Label questionLabel = new Label()
            {
                Content = card.question,
                FontSize = 35,
                HorizontalAlignment = HorizontalAlignment.Center,
                //Margin = new Thickness(300, 5, 80, 0)
            };
            Grid.SetRow(questionLabel, 0);
            mainGrid.Children.Add(questionLabel);
        }

        private void CreateNewCard()
        {
            test.nameTest = textBoxForName.Text;
            mainGrid.ShowGridLines = false;
            imageAnsPath = null;
            imageQuestPath = null;
            btnNewTest.Content = textBoxForName.Text;
            NameTestLabel.Content = textBoxForName.Text;
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

            textBoxForQuestion = new TextBox()
            {
                FontSize = 22,
                Height = 40,
                Width = 500,
                Margin = new Thickness(50, 0, 100, 10)
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
                Background = new SolidColorBrush(Color.FromRgb(223, 238, 132)),
                Content = "Вопрос картинка",
                Margin = new Thickness(0, 5, 400, 0),
                Style = (Style)FindResource("TestButton")
            };
            Grid.SetRow(chooseQuestionImageButton, 5);
            chooseQuestionImageButton.Click += new RoutedEventHandler(chooseImageQueButton_Click);
            mainGrid.Children.Add(chooseQuestionImageButton);

            Label WriteAnsLabel = new Label()
            {
                Content = "Ответ:",
                FontSize = 30,
                Margin = new Thickness(50, 2, 10, 0)
            };
            Grid.SetRow(WriteAnsLabel, 6);
            mainGrid.Children.Add(WriteAnsLabel);

            textBoxForAnswer = new TextBox()
            {
                FontSize = 22,
                Height = 40,
                Width = 500,
                Margin = new Thickness(50, 0, 100, 10)
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
                Background = new SolidColorBrush(Color.FromRgb(223, 238, 132)),
                Content = "Ответ картинка",
                Margin = new Thickness(0, 5, 400, 0),
                Style = (Style)FindResource("TestButton"),
            };
            Grid.SetRow(chooseAnswerImageButton, 9);
            chooseAnswerImageButton.Click += new RoutedEventHandler(chooseImageAnsButton_Click);
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
            addCardButton.Click += new RoutedEventHandler(addCardButton_Clic);
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
        }

        private void addCard()
        {
            Card newCard = new Card(textBoxForQuestion.Text, textBoxForAnswer.Text);

            Image img;
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

        private string LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // фильтр для выбора только картинки - файлы соответсвующих форматов
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
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