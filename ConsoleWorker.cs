using System;
using System.Collections;
using System.Linq;

namespace CaesarCipher
{
    internal static class ConsoleWorker
    {
        private static readonly string[] FileCommands;

        static ConsoleWorker()
        {
            FileCommands = new[] {"show", "encrypt","encryptthis","setstrength"};
        }

        internal static void UserInputHandler(string userAnswer)
        {
            //Парсинг введенного текста
            //Текст разбивается на команду и параметр
            string[] correctedInput = CorrectedInput(userAnswer);
            string command = correctedInput[0];
            string parameter = correctedInput[1];

            if (string.IsNullOrWhiteSpace(parameter))
                ParameterLessCommand(command);
            else
                ParameterCommand(command, parameter);
        }

        private static string[] CorrectedInput(string userAnswer)
        {        
            //Удалим все пробелы до и после введенного текста,
            //на случай, если пользователь случайно ввел один или несколько пробелов перед коммандой.
            //Находим первый пробел во введенном тексте и извлекаем подстроку до этого пробела
            //После пробелов нет текста, значит команда без параметров
            //Это и будет потенциальная комманда
            string command;
            string parameter;
            userAnswer = userAnswer.Trim();

            int endCommandIndex = userAnswer.Trim().IndexOf(' ');

            //Команда может быть двух видов
            if (endCommandIndex > 0)
            {
                //с параметрами
                command = userAnswer.Substring(0, endCommandIndex).ToLower();
                parameter = userAnswer.Substring(endCommandIndex + 1).Trim();
            }
            else
            {
                //без параметров
                command = userAnswer.ToLower();
                parameter = string.Empty;
            }

            return new[] {command, parameter};
        }

        private static void ParameterLessCommand(string command)
        {
            switch (command)
            {
                case "?":
                    ListAvailableCommands();
                    break;
                case "list":
                    ListCommand();
                    break;
                case "showencryption":
                    Encryption.ShowEncryption();
                    break;
                default:
                    NotResolveInput(command);
                    break;
            }
        }

        private static void ParameterCommand(string command, string parameter)
        {

            if (!FileCommands.Contains(command))
            {
                NotResolveInput(command);
                return;
            }

            switch (command)
            {
                case "show":
                    FileWorker.ShowTxtFile(parameter.ToLower());
                    break;
                case "encrypt":
                    Encryption.EncryptTxtFile(parameter.ToLower());
                    break;
                case "encryptthis":
                    Encryption.EncryptText(parameter);
                    break;
                case "setstrength":
                    Encryption.SetStrength(parameter);
                    break;
            }
        }
        private static void ListAvailableCommands()
        {
            Console.WriteLine();
            Console.WriteLine("Available commands: ");
            Console.WriteLine("..................");
            Console.WriteLine("'list' - List all files in the current directory");
            Console.WriteLine("'show xxx.txt' - Show the content of xxx file");
            Console.WriteLine("'encrypt xxx.txt' - Encrypt the xxx file");
            Console.WriteLine("'encryptthis sometext' - Encrypt the 'sometext'");
            Console.WriteLine("'setstrength number ' - set the strength of encryption");
            Console.WriteLine("'showencryption' - show information about the current encryption'");
            Console.WriteLine("..................");
            Console.WriteLine("Where 'xxx' - name of file '.txt' are optional");
            Console.WriteLine();
        }
        private static void NotResolveInput(string userAnswer)
        {
            Console.WriteLine("Unknown command '"+userAnswer+"', try again or write ? for help");
        }

        private static void ListCommand()
        {
            Console.WriteLine();
            Console.WriteLine("Possible files in the current directory:");
            Console.WriteLine();

            IEnumerable fileList = FileWorker.EnlistFiles();

            foreach (var fileName in fileList)
            {
                Console.WriteLine(fileName);
            }
            Console.WriteLine();
        }

    }
}
