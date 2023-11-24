using System;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace Obuchanik
{
    [Serializable]
    public class Card
    {
        public Image imageQuestion;
        public Image imageAnswer;
        public string question = "";
        public string answer = "";

        public Card() { }

        public Card(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }

        public void AddAnswerImage(Image image)
        {
            this.imageAnswer = image;
        }

        public void AddQuestionImage(Image image)
        {
            this.imageQuestion = image;
        }
    }
}