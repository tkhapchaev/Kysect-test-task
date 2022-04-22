using System;

namespace test_task
{
    class Program
    {
        static void Main()
        {
            string path = Console.ReadLine();
            if (path != null)
            {
                System.IO.StreamReader fileReader = new System.IO.StreamReader(path);
                string line = fileReader.ReadLine();

                while (line != null)
                {
                    bool isCompleted;
                    if (line.Contains("[0]"))
                    {
                        isCompleted = false;
                    }

                    else if (line.Contains("[1]"))
                    {
                        isCompleted = true;
                    }

                    else isCompleted = false;

                    int bracketPosition = line.LastIndexOf('[');
                    line = line.Substring(0, bracketPosition);

                    Console.Write(line);
                    if (isCompleted) Console.WriteLine(": выполнено.");
                    else Console.WriteLine(": не выполнено.");

                    line = fileReader.ReadLine();
                }

                fileReader.Close();
            }
        }
    }
}
