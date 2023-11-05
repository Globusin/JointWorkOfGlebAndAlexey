using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obuchanik
{
    interface IReadSave
    {
        List<Test> GetData(string path);
        void SaveData(string path, List<Test> listTest);
    }
}