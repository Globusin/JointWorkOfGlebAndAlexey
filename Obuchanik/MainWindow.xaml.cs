using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
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
        int countTests = 0;
        int countOfCards;
        Button btnNewTest;
        Label NameTestLabel;
        TextBox textBoxForName;
        TextBox textBoxForQuestion;
        TextBox textBoxForAnswer;
        Button BtnNextStep;
        Test test;
        Card curCard;
        string imageAnsPath;
        string imageQuestPath;
        Border borderForQuestion;
        Border borderForAnswer;
        string nameBtnForDelete = null;

        Dictionary<string, Action<string>> callerDict = new Dictionary<string, Action<string>>();

        public MainWindow()
        {
            try
            {
                listTest = GetData("DataSerialize");
            }
            catch (Exception)
            {
                SaveData("DataSerialize", listTest);
            }
            
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
            ColorStPn();
            if (!mainGrid.Children.Contains(mainImage))
                SaveData("DataSerialize", listTest);

            Button currentBtn = (Button)sender;
            currentBtn.Background = new SolidColorBrush(Color.FromRgb(114, 201, 238));

            callerDict[currentBtn.Content.ToString()].Invoke(currentBtn.Content.ToString());

            nameBtnForDelete = currentBtn.Content.ToString();

            OpenSelectTest(currentBtn.Content.ToString());
        }

        //обработчик кнопки для добавления новых тестов
        private void Btn_clic_plus(object sender, RoutedEventArgs e)
        {
            BtnPlus.IsEnabled = false;
            BtnBasket.IsEnabled = false;
            foreach (var item in StPnTests.Children)
            {
                Button btn = (Button)item;

                btn.IsEnabled = false;
            }

            ColorStPn();
            mainGrid.Children.Clear();
            mainGrid.VerticalAlignment = VerticalAlignment.Top;
            mainGrid.RowDefinitions.Clear();
            mainGrid.ShowGridLines = false;
            countOfCards = 0;

            btnNewTest = new Button()
            {
                Style = (Style)FindResource("TestButton")
            };

            //создание строк в цикле
            for (int i = 0; i < 5; i++)
            {
                RowDefinition rowDifinition = new RowDefinition();
                mainGrid.RowDefinitions.Add(rowDifinition);
            }

            NameTestLabel = new Label()
            {
                Content = "Новый тест " + $"{countTests + 1}",
                FontSize = 35,
                Margin = new Thickness(5, 5, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(NameTestLabel, 0);
            mainGrid.Children.Add(NameTestLabel);

            mainGrid.RowDefinitions[1].Height = new GridLength(100);

            Label nameNewTest = new Label()
            {
                Content = "Введите название: ",
                FontSize = 30,
                Margin = new Thickness(20, 5, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(nameNewTest, 2);
            mainGrid.Children.Add(nameNewTest);

            textBoxForName = new TextBox()
            {
                FontSize = 22,
                Height = 40,
                Width = 500,
                Margin = new Thickness(20, 5, 5, 5),
                HorizontalAlignment= HorizontalAlignment.Left,
                VerticalAlignment= VerticalAlignment.Center,
            };
            Grid.SetRow(textBoxForName, 3);
            mainGrid.Children.Add(textBoxForName);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("Source/next.png", UriKind.Relative));
            BtnNextStep = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                Margin = new Thickness(5, 5, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            BtnNextStep.Click += new RoutedEventHandler(BtnNextStep_Clic);
            Grid.SetRow(BtnNextStep, 4);
            mainGrid.Children.Add(BtnNextStep);

            test = new Test();
        }

        //обрабочик для кнопки стрелочки при создании нового теста
        private void BtnNextStep_Clic(object sender, RoutedEventArgs e)
        {
            if (textBoxForName.Text == "" || !Regex.IsMatch(textBoxForName.Text, "^[^\\/:*?\"<>|+]+$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Некорректное имя теста");
                ToStartScreen();
            }
            else
            {
                btnNewTest.Content = textBoxForName.Text.ToString();
                if (callerDict.ContainsKey(btnNewTest.Content.ToString()))
                {
                    MessageBox.Show("Тест с таким именем уже есть"); 

                    countTests--;
                    ToStartScreen();
                }
                else
                {
                    countTests++;
                    callerDict.Add(btnNewTest.Content.ToString(), OpenSelectTest);
                    btnNewTest.Click += new RoutedEventHandler(BtnTest_Clic);
                    btnNewTest.IsEnabled = false;
                    StPnTests.Children.Add(btnNewTest);

                    Path.Combine(Directory.GetCurrentDirectory(), btnNewTest.Content.ToString());
                    Directory.CreateDirectory(btnNewTest.Content.ToString());

                    CreateNewCard();
                }
            }
        }

        private void BtnEndOfNewTestCreation_Click(object sender, RoutedEventArgs e)
        {
            AddCard();
            listTest.Add(test);

            SaveData("DataSerialize", listTest);
            mainGrid.Children.Clear();

            BtnPlus.IsEnabled = true;
            BtnBasket.IsEnabled = true;
            foreach (var item in StPnTests.Children)
            {
                Button btn = (Button)item;

                btn.IsEnabled = true;
            }
            nameBtnForDelete = null;
        }

        private void addCardButton_Clic(object sender, RoutedEventArgs e)
        {
            AddCard();
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

        //обрабочтик кнопки Знаю
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            curCard.statusCard = Status.Passed;
            ShowCard(curCard);
        }

        //обработчик кнопки Следует повторить
        private void Btn_DontKnow_Click(object sender, RoutedEventArgs e)
        {
            curCard.statusCard = Status.Incomplete;
            ShowCard(curCard);
        }

        //обработчик кнопки Не знаю
        private void Btn_NotOK_Click(object sender, RoutedEventArgs e)
        {
            curCard.statusCard = Status.Failed;
            ShowCard(curCard);
        }

        //обработчик вывода правильного ответа карточки
        private void Btn_Coup_Click(object sender, RoutedEventArgs e)
        {
            if (borderForAnswer.Visibility == Visibility.Visible)
            {
                borderForAnswer.Visibility = Visibility.Hidden;
                borderForQuestion.Visibility = Visibility.Visible;
            }
            else
            {
                borderForAnswer.Visibility = Visibility.Visible;
                borderForQuestion.Visibility = Visibility.Hidden;
            }
        }

        private void BtnNextCard_Click(object sender, RoutedEventArgs e)
        {
            int index = test.cards.IndexOf(curCard);
            ShowCard(test.cards[(index + 1) % test.cards.Count]);
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (nameBtnForDelete != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы хотите удалить тест?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    mainGrid.Children.Clear();
                    countTests--;

                    foreach (var item in StPnTests.Children)
                    {
                        Button btn = (Button)item;
                        if (btn.Content.ToString() == nameBtnForDelete)
                        {
                            StPnTests.Children.Remove((Button)item);
                            break;
                        }
                    }

                    foreach (var items in listTest)
                    {
                        if (items.nameTest == nameBtnForDelete)
                        {
                            listTest.Remove(items);
                            callerDict.Remove(items.nameTest);
                            break;
                        }
                    }

                    using (FileStream fs = new FileStream("DataSerialize", FileMode.OpenOrCreate))
                    {
                        fs.SetLength(0);
                    }

                    try
                    {
                        Path.Combine(Directory.GetCurrentDirectory(), nameBtnForDelete);
                        Directory.Delete(nameBtnForDelete, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    SaveData("DataSerialize", listTest);
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveData("DataSerialize", listTest);
        }

        //////////////////////// Методы:

        public void ToStartScreen()
        {
            mainGrid.Children.Clear();

            BtnPlus.IsEnabled = true;
            BtnBasket.IsEnabled = true;
            foreach (var item in StPnTests.Children)
            {
                Button btn = (Button)item;

                btn.IsEnabled = true;
            }
            nameBtnForDelete = null;
        }

        public void ColorStPn()
        {
            foreach (var item in StPnTests.Children)
            {
                Button button = (Button)item;
                button.Background = new SolidColorBrush(Color.FromRgb(223, 238, 132));
            }
        }

        public void AddTestOnStPn(Test test)
        {
            countTests++;

            btnNewTest = new Button()
            {
                Content = test.nameTest,
                Style = (Style)FindResource("TestButton")
            };

            callerDict.Add(btnNewTest.Content.ToString(), OpenSelectTest);
            btnNewTest.Click += new RoutedEventHandler(BtnTest_Clic);
            StPnTests.Children.Add(btnNewTest);
        }

        public void OpenSelectTest(string name)
        {
            //вывод тестов сохраненных в классе Test из List<Test>
            //по ключу Name теста

            test = listTest.FindAll(x => x.nameTest == name)[0];
            mainGrid.Children.Clear();
            ShowCard(test.cards[0]);
        }

        private Border GetCardQuestion(Card card)
        {
            Grid grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i = 0; i < 2; i++)
            {
                RowDefinition rowDifinition = new RowDefinition();
                grid.RowDefinitions.Add(rowDifinition);
            }

            if (card.imageQuestionPath != null)
            {
                Image img = new Image();
                try
                {
                    img.Source = new BitmapImage(new Uri(card.imageQuestionPath));
                    img.HorizontalAlignment = HorizontalAlignment.Stretch;
                    img.VerticalAlignment = VerticalAlignment.Stretch;
                    Grid.SetRow(img, 0);
                    grid.Children.Add(img);
                }
                catch (Exception)
                {
                }
            }

            if (card.question != null)
            {
                Label label = new Label()
                {
                    Content = card.question,
                    FontSize = 30,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                Grid.SetRow(label, 1);
                grid.Children.Add(label);
            }

            Border borderForQuestion = new Border()
            {
                CornerRadius = new CornerRadius(20),
                Padding = new Thickness(10),
                Background = new SolidColorBrush(Colors.White),
                Child = grid,
            };
            Border bigBorderForQuestion = new Border()
            {
                Child = borderForQuestion,
                Padding = new Thickness(100, 10, 100, 10),
            };

            return bigBorderForQuestion;
        }

        private Border GetCardAnswer(Card card)
        {
            Grid grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i = 0; i < 2; i++)
            {
                RowDefinition rowDifinition = new RowDefinition();
                grid.RowDefinitions.Add(rowDifinition);
            }

            if (card.imageAnswerPath != null)
            {
                Image img = new Image();
                try
                {
                    img.Source = new BitmapImage(new Uri(card.imageAnswerPath));
                    img.HorizontalAlignment = HorizontalAlignment.Stretch;
                    img.VerticalAlignment = VerticalAlignment.Stretch;
                    Grid.SetRow(img, 0);
                    grid.Children.Add(img);
                }
                catch (Exception)
                {
                }
            }

            if (card.answer != null)
            {
                Label label = new Label()
                {
                    Content = card.answer,
                    FontSize = 30,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                Grid.SetRow(label, 1);
                grid.Children.Add(label);
            }

            Border borderForAnswer = new Border()
            {
                CornerRadius = new CornerRadius(20),
                Padding = new Thickness(10),
                Background = new SolidColorBrush(Colors.White),
                Child = grid,
            };
            Border bigBorderForAnswer = new Border()
            {
                Child = borderForAnswer,
                Padding = new Thickness(100, 10, 100, 10),
            };

            return bigBorderForAnswer;
        }

        private void ShowCard(Card card)
        {
            curCard = card;
            mainGrid.Children.Clear();
            mainGrid.VerticalAlignment = VerticalAlignment.Top;
            mainGrid.RowDefinitions.Clear();
            mainGrid.ShowGridLines = false;
            for (int i = 0; i < 4; i++)
            {
                RowDefinition rowDifinition = new RowDefinition();
                mainGrid.RowDefinitions.Add(rowDifinition);
            }
            mainGrid.RowDefinitions[2].Height = new GridLength(150);
            mainGrid.RowDefinitions[1].Height = new GridLength(100);
            mainGrid.RowDefinitions[0].Height = new GridLength(450);

            borderForQuestion = GetCardQuestion(card);
            borderForAnswer = GetCardAnswer(card);
            borderForAnswer.Visibility = Visibility.Hidden;

            Grid.SetRow(borderForQuestion, 0);
            Grid.SetRow(borderForAnswer, 0);
            mainGrid.Children.Add(borderForQuestion);
            mainGrid.Children.Add(borderForAnswer);
            mainGrid.Children[0].SetValue(Grid.ColumnProperty, 0);
            mainGrid.Children[0].SetValue(Grid.RowProperty, 0);

            Grid gridForInformationAboutCard = new Grid();
            gridForInformationAboutCard.HorizontalAlignment = HorizontalAlignment.Stretch;
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition colDifinition = new ColumnDefinition();
                gridForInformationAboutCard.ColumnDefinitions.Add(colDifinition);
            }

            int curCardNumber = test.cards.IndexOf(card);
            Label labelNumCard = new Label()
            {
                Content = $"Карточка: {curCardNumber + 1}/{test.cards.Count}",
                FontSize = 15,
                Margin = new Thickness(100, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(labelNumCard, 0);
            gridForInformationAboutCard.Children.Add(labelNumCard);

            Button showAnswerBtn = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Content = "Перевернуть карточку",
                FontSize = 20,
                Background = new SolidColorBrush(Color.FromRgb(223, 238, 132)),
                Height = 40,
                Width = 230,
            };
            showAnswerBtn.Click += new RoutedEventHandler(Btn_Coup_Click);
            Grid.SetColumn(showAnswerBtn, 1);
            gridForInformationAboutCard.Children.Add(showAnswerBtn);

            SolidColorBrush colorBrush = null;
            string status = null;
            if (card.statusCard == Status.Passed)
            {
                status = "Изучено";
                colorBrush = new SolidColorBrush(Color.FromRgb(162, 242, 160));
            }
            if (card.statusCard == Status.Incomplete)
            {
                status = "Повторим";
                colorBrush = new SolidColorBrush(Color.FromRgb(255, 207, 82));
            }
            if (card.statusCard == Status.Failed)
            {
                status = "Не изучено";
                colorBrush = new SolidColorBrush(Color.FromRgb(252, 110, 153));
            }

            Label labelStatus = new Label()
            {
                Content = $"{status}",
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Border borderForStatus = new Border()
            {
                CornerRadius = new CornerRadius(10),
                Background = colorBrush,
                Child = labelStatus,
                Height = 30,
                Width = 100,
                HorizontalAlignment= HorizontalAlignment.Stretch,
            };
            Grid.SetColumn(borderForStatus, 2);
            gridForInformationAboutCard.Children.Add(borderForStatus);

            Grid.SetRow(gridForInformationAboutCard, 1);
            mainGrid.Children.Add(gridForInformationAboutCard);

            //////////////////////////////////////////////////////

            Grid gridForBtnOkNotOkDontKnow = new Grid();
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition colDifinition = new ColumnDefinition();
                gridForBtnOkNotOkDontKnow.ColumnDefinitions.Add(colDifinition);
            }

            Image imgOK = new Image();
            imgOK.Source = new BitmapImage(new Uri("Source/galochka1.png", UriKind.Relative));
            Button btnOK = new Button()
            {
                Name = "BtnOK",
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)FindResource("RoundButton"),
                Content = imgOK,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Width = 110,
                Height = 110
            };
            btnOK.Click += new RoutedEventHandler(Btn_OK_Click);
            Grid.SetColumn(btnOK, 0);
            gridForBtnOkNotOkDontKnow.Children.Add(btnOK);

            Image imgNotOK = new Image();
            imgNotOK.Source = new BitmapImage(new Uri("Source/Krestik1.png", UriKind.Relative));
            Button btnNotOK = new Button()
            {
                Name = "BtnNotOK",
                HorizontalAlignment= HorizontalAlignment.Center,
                Style = (Style)FindResource("RoundButton"),
                Content = imgNotOK,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Width = 110,
                Height = 110
            };
            btnNotOK.Click += new RoutedEventHandler(Btn_NotOK_Click);
            Grid.SetColumn(btnOK, 2);
            gridForBtnOkNotOkDontKnow.Children.Add(btnNotOK);

            Image imgDontKnow = new Image();
            imgDontKnow.Source = new BitmapImage(new Uri("Source/VoprosZnak1.png", UriKind.Relative));
            Button btnDontKnow = new Button()
            {
                Name = "btnDontKnow",
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)FindResource("RoundButton"),
                Content = imgDontKnow,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Width = 110,
                Height = 110
            };
            btnDontKnow.Click += new RoutedEventHandler(Btn_DontKnow_Click);
            Grid.SetColumn(btnDontKnow, 1);
            gridForBtnOkNotOkDontKnow.Children.Add(btnDontKnow);

            Border borderForButtons = new Border()
            {
                Padding = new Thickness(20),
                Child = gridForBtnOkNotOkDontKnow,
            };
            Grid.SetRow(borderForButtons, 2);
            mainGrid.Children.Add(borderForButtons);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("Source/next.png", UriKind.Relative));
            Button buttonNextCard = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 50,
                Width = 50,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            buttonNextCard.Click += new RoutedEventHandler(BtnNextCard_Click);
            Grid.SetRow(buttonNextCard, 3);
            mainGrid.Children.Add(buttonNextCard);
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
            mainGrid.VerticalAlignment = VerticalAlignment.Top;
            mainGrid.RowDefinitions.Clear();

            for (int i = 0; i < 12; i++)
            {
                RowDefinition rowDifinition = new RowDefinition();
                mainGrid.RowDefinitions.Add(rowDifinition);
            }

            Label nameTest = new Label()
            {
                Content = NameTestLabel.Content,
                FontSize = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetRow(nameTest, 0);
            mainGrid.Children.Add(nameTest);

            Label CardLabel = new Label()
            {
                Content = $"Карточка {++countOfCards}",
                FontSize = 27,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(50, 0, 0, 0),
            };
            Grid.SetRow(CardLabel, 1);
            mainGrid.Children.Add(CardLabel);

            Label WriteQueLabel = new Label()
            {
                Content = "Вопрос:",
                FontSize = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(50, 0, 0, 0),
            };
            Grid.SetRow(WriteQueLabel, 2);
            mainGrid.Children.Add(WriteQueLabel);

            textBoxForQuestion = new TextBox()
            {
                FontSize = 22,
                Height = 40,
                Width = 500,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(50, 0, 0, 0),
            };
            Grid.SetRow(textBoxForQuestion, 3);
            mainGrid.Children.Add(textBoxForQuestion);

            Label OrLabel = new Label()
            {
                Content = "Или выбирите картинку:",
                FontSize = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(50, 0, 0, 0),
            };
            Grid.SetRow(OrLabel, 4);
            mainGrid.Children.Add(OrLabel);

            Button chooseQuestionImageButton = new Button()
            {
                Height = 40,
                Width = 120,
                Background = new SolidColorBrush(Color.FromRgb(223, 238, 132)),
                Content = "Вопрос картинка",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(50, 0, 0, 0),
                Style = (Style)FindResource("TestButton")
            };
            Grid.SetRow(chooseQuestionImageButton, 5);
            chooseQuestionImageButton.Click += new RoutedEventHandler(chooseImageQueButton_Click);
            mainGrid.Children.Add(chooseQuestionImageButton);

            Label WriteAnsLabel = new Label()
            {
                Content = "Ответ:",
                FontSize = 25,
                Margin = new Thickness(50, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid.SetRow(WriteAnsLabel, 6);
            mainGrid.Children.Add(WriteAnsLabel);

            textBoxForAnswer = new TextBox()
            {
                FontSize = 22,
                Height = 40,
                Width = 500,
                Margin = new Thickness(50, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid.SetRow(textBoxForAnswer, 7);
            mainGrid.Children.Add(textBoxForAnswer);

            Label OrLabel2 = new Label()
            {
                Content = "Или выбирите картинку:",
                FontSize = 25,
                Margin = new Thickness(50, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid.SetRow(OrLabel2, 8);
            mainGrid.Children.Add(OrLabel2);

            Button chooseAnswerImageButton = new Button()
            {
                Height = 40,
                Width = 120,
                Background = new SolidColorBrush(Color.FromRgb(223, 238, 132)),
                Content = "Ответ картинка",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(50, 0, 0, 0),
                Style = (Style)FindResource("TestButton"),
            };
            Grid.SetRow(chooseAnswerImageButton, 9);
            chooseAnswerImageButton.Click += new RoutedEventHandler(chooseImageAnsButton_Click);
            mainGrid.Children.Add(chooseAnswerImageButton);

            Grid subGrid1 = new Grid();
            Grid subGrid2 = new Grid();
            subGrid1.HorizontalAlignment = HorizontalAlignment.Stretch;
            subGrid2.HorizontalAlignment = HorizontalAlignment.Stretch;

            for (int i = 0; i < 2; i++)
            {
                ColumnDefinition colDifinition1 = new ColumnDefinition();
                ColumnDefinition colDifinition2 = new ColumnDefinition();
                subGrid1.ColumnDefinitions.Add(colDifinition1);
                subGrid2.ColumnDefinitions.Add(colDifinition2);
            }

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("Source/plus.png", UriKind.Relative));
            Button addCardButton = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                HorizontalAlignment = HorizontalAlignment.Center,
            };         
            addCardButton.Click += new RoutedEventHandler(addCardButton_Clic);
            Grid.SetColumn(addCardButton, 0);
            subGrid1.Children.Add(addCardButton);

            img = new Image();
            img.Source = new BitmapImage(new Uri("Source/next.png", UriKind.Relative));
            Button BtnEndOfNewTestCreation = new Button()
            {
                Style = (Style)FindResource("RoundButton"),
                Height = 100,
                Width = 100,
                Background = new SolidColorBrush(Color.FromRgb(244, 252, 196)),
                Content = img,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            BtnEndOfNewTestCreation.Click += new RoutedEventHandler(BtnEndOfNewTestCreation_Click);
            Grid.SetColumn(BtnEndOfNewTestCreation, 1);
            subGrid1.Children.Add(BtnEndOfNewTestCreation);

            Label addNewCardForButtonLabel = new Label()
            {
                Content = "Добавить\n карточку",
                FontSize = 25,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetColumn(addNewCardForButtonLabel, 0);
            subGrid2.Children.Add(addNewCardForButtonLabel);

            Label endOfNewTestCreationForButtonLabel = new Label()
            {
                Content = "Завершить\n создание",
                FontSize = 22,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetColumn(endOfNewTestCreationForButtonLabel, 1);
            subGrid2.Children.Add(endOfNewTestCreationForButtonLabel);

            mainGrid.RowDefinitions[10].Height = new GridLength(110);
            mainGrid.RowDefinitions[11].Height = new GridLength(90);
            
            
            Grid.SetRow(subGrid1, 10);
            mainGrid.Children.Add(subGrid1);

            Grid.SetRow(subGrid2, 11);
            mainGrid.Children.Add(subGrid2);
        }

        private void AddCard()
        {
            Card newCard = new Card(textBoxForQuestion.Text, textBoxForAnswer.Text);

            if (imageQuestPath != null)
                newCard.AddQuestionImage(imageQuestPath);

            if (imageAnsPath != null)
                newCard.AddAnswerImage(imageAnsPath);

            test.AddCard(newCard);
        }

        private string ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            return LoadImage();
        }

        private string LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // фильтр для выбора только картинки - файлы соответсвующих форматов
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), test.nameTest);
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);

                if (!File.Exists(folderPath + $@"\{fileName}"))
                    File.Copy(filePath, folderPath + $@"\{fileName}");
                return folderPath + $@"\{fileName}";
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
        }
    }
}