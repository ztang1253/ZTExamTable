using System;
using System.IO;


namespace ExamTable.Controllers.Utils
{
    public class GenerateFolder
    {
        public void CreateFolderIfMissing(string path)
        {
            // Determine whether the directory exists.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}