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
    public class Card
    {
        Image imageQuestion;
        Image imageAnswer;
        public string question;
        public string answer;

        public void AddAnswerImage(Image image)
        {
            imageAnswer = image;
        }

        public void AddQuestionImage(Image image)
        {
            imageAnswer = image;
        }
    }
}
