using System;

namespace CaesarCipher
{
    public static class Program
    {
        /// <summary>
        /// <TODO>
        /// 1)Add decryption
        /// 2)Add another encryption methods
        /// 3)Add other text file extension support
        /// 4)Add check if file even a text-based file
        /// </TODO>
        /// </summary>
        private static void Main()
        {
            //Set the current directory and text file extension
            string userAnswer = string.Empty;

            Console.WriteLine("Caesar cipher made by Me, Year 2017");
            Console.WriteLine();
            Console.WriteLine("Hello there, what are you trying to do? Write '?' for aviable commands");
            Console.WriteLine();
            

            while (userAnswer != null && !userAnswer.ToLower().Equals("exit"))
            {
                userAnswer = Console.ReadLine();
                if (userAnswer != null) ConsoleWorker.UserInputHandler(userAnswer);
            }

            //FileWorker.EnlistFiles(currentDirectory, extension);

            Console.ReadLine();
        }
    }
}