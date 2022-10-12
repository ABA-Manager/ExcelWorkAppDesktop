using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ExcelGenLib
{
    public class ExcelResponse
    {
        public List<string> AnalystsIncluded;
        private ZipArchive compactFile;
        private FileStream stream;

        public ExcelResponse()
        {
            AnalystsIncluded = new List<string>();
        }
        public ExcelResponse(string filename)
        {
            AnalystsIncluded = new List<string>();
            stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            compactFile = new ZipArchive(stream, ZipArchiveMode.Create);
        }

        public void AddAnalyst(ExcelFileData data)
        {
            AddAnalyst(data.Name, data.fileInfo);
        }
        public void AddAnalyst(string Analyst, FileInfo ExcelFile)
        {
            if (ExcelFile != null)
            {
                AnalystsIncluded.Add(Analyst); //, ExcelFile.Name);
                compactFile.CreateEntryFromFile(ExcelFile.FullName, ExcelFile.Name);
            }
            else
            {
                AnalystsIncluded.Add(Analyst);
            }
        }

        public void SavePack()
        {
            compactFile.Dispose();
        }
    }
}
