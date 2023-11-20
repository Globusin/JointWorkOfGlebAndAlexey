using System;
using System.Collections.Generic;

namespace Obuchanik
{
    interface IReadSave
    {
        List<Test> GetData(string path);
        void SaveData(string path, List<Test> listTest);
    }
}