using System;
using System.Collections.Generic;

public class UI
{
    private struct App
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        public App(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }

    private List<App> apps = new List<App>();
    public UI()
    {
        SetApplicationNames();
    }

    public string[] PromptLogin()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Please log in using your TD login credentials...");
        Console.ResetColor();

        Console.Write("Enter username: ");
        Console.ForegroundColor = ConsoleColor.White;
        string username = Console.ReadLine();
        Console.ResetColor();
        Console.Write("Enter password: ");
        Console.ForegroundColor = ConsoleColor.White;
        string password = getPassword();
        Console.ResetColor();

        return new string[] { username, password };
    }

    /*
     * getPassword() grabs the entered key and writes a '*' to the screen so password is hidden.
     * Password is then returned.
     */
    private string getPassword()
    {
        string sPassword = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                sPassword += key.KeyChar;
                Console.Write("*");
            }
            else
            {
                if (key.Key == ConsoleKey.Backspace && sPassword.Length > 0)
                {
                    sPassword = sPassword.Substring(0, (sPassword.Length - 1));
                    Console.Write("\b \b");
                }
            }
        }
        while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return sPassword;
    }

    public string PromptForFilename()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("I need you to give me a text file that has a list of all the ticket IDs in it.");
        Console.ResetColor();
        Console.Write("Use the default file path? (y/n) ");
        Console.ForegroundColor = ConsoleColor.White;
        string choice = Console.ReadLine();
        Console.ResetColor();

        switch (choice)
        {
            case "y":
                return @"C:\Users\loganes1\BYU-Idaho\Business Solutions - Documents\R2S2\Logan\Clean Up\cleanUp.txt";
            case "n":
            default:
                Console.Write("Enter the file path: ");
                Console.ForegroundColor = ConsoleColor.White;
                string filePath = Console.ReadLine();
                Console.ResetColor();
                return filePath;
        }
    }

    public int PromptForReportID()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("What is the ID of the report you would like to me reference? ");
        Console.ForegroundColor = ConsoleColor.White;
        string ID = Console.ReadLine();
        Console.ResetColor();
        int x = 0;
        int.TryParse(ID, out x);
        return x;
    }

    public int[] GetFileContents(string filePath)
    {
        try
        {
            string[] fileContents = System.IO.File.ReadAllLines(@filePath);
            return Array.ConvertAll(fileContents, int.Parse);
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("There's something wrong with the file path you gave me...");
            Console.ResetColor();
            return null;
        }
    }

    private void SetApplicationNames()
    {
        // if you need to add an application that is not already there, add it here and the table will automatically adjust
        apps.Add(new App("Accounting", 54));
        apps.Add(new App("Admissions", 52));
        apps.Add(new App("BYUI Tickets", 40));
        apps.Add(new App("Employment", 30));
        apps.Add(new App("Financial Aid", 53));
        apps.Add(new App("Lost and Found", 60));
        apps.Add(new App("Operations", 42));
        apps.Add(new App("Outreach", 39));
        apps.Add(new App("Systems and Depts", 31));
        apps.Add(new App("SRR", 55));
    }

    public int PromptApplication()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Enter application to clean:");
        Console.ResetColor();
        WriteApplicationNamesToConsole();
        string applicationName = Console.ReadLine();
        foreach (var app in apps)
            if (app.Name == applicationName)
                return app.Id;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nCheck to make sure the name is correct?\n");
        Console.ResetColor();
        return PromptApplication();
    }

    private void WriteApplicationNamesToConsole()
    {
        const int cellWidth = 20;
        const int columns = 5;

        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int i = 0; i < columns; i++)
        {
            Console.Write("+");
            for (int j = 0; j < cellWidth; j++)
                Console.Write("-");
        }
        Console.WriteLine("+");
        for (int i = 0; i < apps.Count; i += columns)
        {
            for (int j = i; j < i + columns; j++) // 5 columns
            {
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.Cyan;
                if (j < apps.Count) // if there's an application
                {
                    // append spaces before and after the string; center it
                    int whiteSpace = (cellWidth - apps[j].Name.Length) / 2;
                    for (int k = 0; k < whiteSpace; k++)
                        Console.Write(" ");
                    Console.Write(apps[j].Name);
                    for (int k = 0; k < whiteSpace; k++)
                        Console.Write(" ");
                    if (apps[j].Name.Length % 2 == 1) // it's odd
                        Console.Write(" "); // add an extra space to keep things even
                }
                else
                    for (int k = 0; k < cellWidth; k++)
                        Console.Write(" ");

                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.WriteLine("|");
            for (int j = 0; j < columns; j++)
            {
                Console.Write("+");
                for (int k = 0; k < cellWidth; k++)
                    Console.Write("-");
            }
            Console.WriteLine("+");
        }
        Console.ResetColor();
    }
}
