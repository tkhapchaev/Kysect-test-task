using System;

namespace test_task
{
    class Parser
    {
        private string path = null;

        public void SetPath(string pathInput)
        {
            path = pathInput;
        }
        
        public void ReadTasks()
        {
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

            else
            {
                Console.WriteLine("Ошибка: невозможно получить доступ к файлу.");
                Environment.Exit(0);
            }
        }

        public void WriteTask(string task)
        {
            if (path != null)
            {
                System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(path, true);
                fileWriter.Write(task);
                fileWriter.WriteLine(" [0]");
                fileWriter.Close();
            }
            
            else
            {
                Console.WriteLine("Ошибка: невозможно получить доступ к файлу.");
                Environment.Exit(0);
            }
        }
    } 
        
    class Program
    {
        static void Main()
        {
            Parser cur_parser = new Parser();
            cur_parser.SetPath(Console.ReadLine());
            cur_parser.ReadTasks();
            cur_parser.WriteTask(Console.ReadLine());
        }
    }
}
