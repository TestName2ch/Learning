using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CaesarCipher
{
    internal enum Encryptions
    {
        Caesar = 1
    }

    internal static class Encryption
    {
        private static readonly string EnglishLetters;
        private static readonly string RussianLetters;
        private const byte EnglishCount = 26;
        private const byte RussianCount = 33;

        private static byte CaesarStrength { get; set; }

        public static void SetStrength(string parameter)
        {
            Console.WriteLine();
            byte expectedValue;
            byte.TryParse(parameter, out expectedValue);

            if (expectedValue > 0)
                CaesarStrength = expectedValue;
            else
                Console.WriteLine("This value is unsupported, please enter the correct value (1-255)");
        }

        public static Encryptions CurrentEncryption { get; set; }
        //A-Z - 65-90
        //a-z - 97-122
        //A-Я - 1040-1071 Ё in 1025 position
        //а-я - 1072-1103 same for ё
        //English alphabet - 26
        //Russian alphabet - 33, but letter Ё in 1025 position

        static Encryption()
        {
            CurrentEncryption = Encryptions.Caesar;
            CaesarStrength = 1;
            EnglishLetters = "abcdefghijklmnopqrstuvwxyz";
            RussianLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        }

        public static void ShowEncryption()
        {
            Console.WriteLine();
            Console.WriteLine("Current encryption is "+CurrentEncryption);
            Console.WriteLine("And its strength is "+CaesarStrength);
            Console.WriteLine();
        }

        internal static void EncryptTxtFile(string filename)
        {

            string fileName = FileWorker.CheckFileName(filename);

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            Console.WriteLine();
            string fileContent = File.ReadAllText(filename, Encoding.GetEncoding("Windows-1251"));

            if (fileContent.Length > 250000)
            {
                Console.WriteLine("There is too much symbols, please try smaller!");
                return;
            }
            Console.WriteLine("Encrypting is in process.......");
            fileContent = CaesarEncrypt(fileContent);
            string newFilename = FileWorker.CurrentDirectory +"Output_Caesar.txt";

            File.WriteAllText(newFilename, fileContent, Encoding.GetEncoding("Windows-1251"));
            Console.WriteLine("File " + newFilename + " is succesfully created!");
        }

        internal static void EncryptText(string text)
        {
            Console.WriteLine();
            text = CaesarEncrypt(text);

            Console.WriteLine("Encrypted text:");

            Console.WriteLine(text);
            Console.WriteLine();
        }

        private static string CaesarEncrypt(string content)
        {
            //Planned to do this with checking by the (char) number and char.IsLetter()
            //But letters Ё, ё in completely different possition
            //So check if character is english or russian letter
            //find index of the next letter by modulo
            //and add new letter to the content depending on case
            string newContent = string.Empty;

            foreach (var letter in content)
            {
                int index;
                if (EnglishLetters.Contains(char.ToLower(letter)))
                {
                    index = (EnglishLetters.IndexOf(char.ToLower(letter)) + CaesarStrength) % EnglishCount;
                    newContent += char.IsLower(letter)
                        ? EnglishLetters[index]
                        : char.ToUpper(EnglishLetters[index]);
                }
                else if (RussianLetters.Contains(char.ToLower(letter)))
                {
                    index = (RussianLetters.IndexOf(char.ToLower(letter)) + CaesarStrength) % RussianCount;
                    newContent += char.IsLower(letter)
                        ? RussianLetters[index]
                        : char.ToUpper(RussianLetters[index]);
                }
                else
                    newContent += letter;
            }

            return newContent;
        }

    }
}