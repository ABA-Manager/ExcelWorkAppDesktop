using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGenLib
{
    public class ExcelFileData
    {
        public string Name;
        public FileInfo fileInfo;

        public ExcelFileData(string vName, FileInfo vFileInfo)
        {
            Name = vName;
            fileInfo = vFileInfo;
        }
    }
}
