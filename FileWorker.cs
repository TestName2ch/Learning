using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CaesarCipher
{
    public static class FileWorker
    {
        private static readonly List<string> Extensions;
        public static readonly string CurrentDirectory;

        static FileWorker()
        {
            Extensions = new List<string> { ".txt" };//When support additional extension it will be added here
            CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        internal static IEnumerable<FileInfo> EnlistFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(CurrentDirectory);

            return directory.EnumerateFiles().Where(x => Extensions.Contains(x.Extension.ToLower()));
        }

        /// <summary>
        /// Check the given filename
        /// Filename may have the extension and may be not
        /// </summary>
        internal static string CheckFileName(string filename)
        {
            //Filename has extension so just check if file even exist
            //And return filename with extension.ToLower() because sometimes files called like 'somefile.TXT'
            if (Path.HasExtension(filename))
            {
                if (File.Exists(CurrentDirectory + filename))
                    return CurrentDirectory+filename.ToLower();

                Console.WriteLine("File " + filename + " doesnt exist");
                return string.Empty;
            }
                
            //Filename doesnt have extension so enlist all files that match the rules (extensions)
            //Check if the file or files with given filename exists
            //and check for 3 possible situations:
            //Exactly 1 file with given filename
            //No files
            //Multiple file with same filename, but different extensions

            IEnumerable<FileInfo> allPossibleFiles = EnlistFiles();

            //Dont use LINQ or Lambda here because need to count files
            //And trying to avoid using multiply enumerations like .Count() before 'foreach'
            List<string> neededFiles = new List<string>();
            int fileCount = 0;
            foreach (var file in allPossibleFiles)
            {
                if (!Path.GetFileNameWithoutExtension(file.Name).ToLower().Equals(filename)) continue;

                neededFiles.Add(file.Name);
                fileCount++;
            }

            switch (fileCount)
            {
                case 1:
                    return CurrentDirectory + neededFiles[0].ToLower();
                case 0:
                    Console.WriteLine("Cant find '" + filename + "' maybe extension of that file is doesnt support yet, check possible extensions");
                    return string.Empty;
                default:
                    Console.WriteLine("There is multiply files with the same filename, please add the extension to the filename next time");
                    foreach (var file in neededFiles)
                        Console.WriteLine(file);
                    return string.Empty;
            }
        }

        internal static void ShowTxtFile(string filename)
        {
            //No need to check if file exist because this method is called after 'CheckFileName'
            //Check file size, if it <5mb then its safe for show in console window
            //Otherwise it could be long waiting until all content is presented
            filename = CheckFileName(filename);

            if (string.IsNullOrWhiteSpace(filename))
                return;

            FileInfo fileInfo = new FileInfo(filename);
            const int possibleTxtSize = 5 * 1024 * 1024;

            if (fileInfo.Length > possibleTxtSize)
            {
                Console.WriteLine("This .txt file is too big for show in console '>5mb'");
                return;
            }


            var fileContent = File.ReadAllLines(filename, Encoding.GetEncoding("Windows-1251"));

            foreach (var line in fileContent)
            {
                Console.WriteLine(line);
            }
        }
    }
}