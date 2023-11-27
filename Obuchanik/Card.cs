using System;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace Obuchanik
{
    [Serializable]
    public class Card
    {
        public string imageQuestionPath;
        public string imageAnswerPath;
        public string question = "";
        public string answer = "";
        public Status statusCard = Status.Incomplete;

        public Card() { }

        public Card(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }

        public void AddAnswerImage(string path)
        {
            imageAnswerPath = path;
        }

        public void AddQuestionImage(string path)
        {
            imageQuestionPath = path;
        }
    }
}