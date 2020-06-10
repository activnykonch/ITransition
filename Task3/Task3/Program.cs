using System;
using System.Security.Cryptography;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length % 2 == 0 || args.Length < 2 || CheckForRepetition(args))
            {
                Console.WriteLine("Incorrect arguments");
            }
            else
            {
                RandomNumberGenerator generator = RandomNumberGenerator.Create();
                HMACSHA512 hash = new HMACSHA512();
                bool isGameOn = true;
                while(isGameOn)
                {
                    Menu(args);
                    byte[] key = new byte[32];
                    generator.GetBytes(key);
                    int botChoice = RandomNumberGenerator.GetInt32(1, args.Length);
                    hash.Key = key;
                    Console.WriteLine($"HMAC is {BitConverter.ToString(hash.ComputeHash(BitConverter.GetBytes(botChoice)))}");
                    int choice, size = args.Length;
                    while (!Int32.TryParse(Console.ReadLine(), out choice))
                    {
                        Menu(args);
                        Console.WriteLine($"HMAC is {hash.ComputeHash(BitConverter.GetBytes(botChoice))}\n");
                    }
                    if (choice < 1 || choice > args.Length)
                    {
                        if (choice == 0) break;
                        Console.WriteLine("Wrong input!");
                        continue;
                    }
                    Console.WriteLine($"Your choice is {args[choice - 1]}\n");
                    if (choice > 0 || choice <= size)
                    {
                        if (choice == botChoice)
                        {
                            Console.WriteLine("Draw!\n");
                            continue;
                        }
                        if ((botChoice > choice && botChoice <= (choice + (size - 1) / 2))
                            || ((botChoice < choice) && (botChoice <= (choice + ((size - 1)) / 2) % size)))
                        {
                            Console.WriteLine("Win!\n");
                        }
                        else
                        {
                            Console.WriteLine("Lose!\n");
                        }
                    }
                    Console.WriteLine($"Bot choice is {args[botChoice - 1]}\n");
                    Console.WriteLine($"HMAC key: {BitConverter.ToString(key)}\n");
                    Console.WriteLine("\n1 - continue\nAny button - exit");
                    int play;
                    Int32.TryParse(Console.ReadLine(), out play);
                    if (play == 1) isGameOn = true;
                    else isGameOn = false;
                }
            }
        }

        static bool CheckForRepetition(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
                for (int j = 0; j < args.Length; j++)
                    if (i != j)
                        if (args[i] == args[j])
                            return true;
            return false;
        }

        static void Menu(string[] args)
        {
            Console.Clear();
            int i = 1;
            foreach(var s in args)
            {
                Console.WriteLine($"{i++}. {s}");
            }
            Console.WriteLine("0 - exit");
        }
    }
}
