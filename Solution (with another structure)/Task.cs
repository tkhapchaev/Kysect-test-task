using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Kysect
{
    class Task
    {
        public string Type { get; set; } = "Task";
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDone { get; set; }
        public DateTime Deadline { get; set; } = new DateTime(1999, 12, 31);
        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();

        public Task(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public void Serialize(string fileName)
        {
            string contents = JsonSerializer.Serialize(this) + "\n";
            File.AppendAllText(fileName, contents);
        }

        public void AddSubtask(Subtask newSubtask)
        {
            Subtasks.Add(newSubtask);
        }
        
        public void Print(bool printWithNewLine)
        {
            string isDoneStr = IsDone ? ("выполнена") : ("не выполнена"), newLine = printWithNewLine ? ("\n") : ("");

            if (Subtasks.Count != 0)
            {
                int numOfDoneSubtasks = 0;
                
                foreach (var subtask in Subtasks)
                {
                    if (subtask.IsDone)
                    {
                        ++numOfDoneSubtasks;
                    }
                }
                
                string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" + Convert.ToString(Subtasks.Count) + ")";
                Console.Write("{0}[{1}]. {2} {3}{4}{5} {6}. {7}\n", newLine, ID, "Задача", "\"", Name, "\"", isDoneStr, doneSubtasksStr);
            }

            else
            {
                Console.Write("{0}[{1}]. {2} {3}{4}{5} {6}.\n", newLine, ID, "Задача", "\"", Name, "\"", isDoneStr);
            }
        }

        public static void PrintSubtasks(List<string> command, List<Task> tasks)
        {
            bool isFound = false;
            foreach (var task in tasks)
            {
                if (Convert.ToString(task.ID) == command[1])
                {
                    if (task.Status)
                    {
                        isFound = true;
                        foreach (var subtask in task.Subtasks)
                        {
                            string isDoneStr = subtask.IsDone ? ("выполнена") : ("не выполнена");
                            Console.WriteLine("- [{0}]. {1} {2}{3}{4} {5}.", subtask.ID, "Подзадача", "\"", subtask.Name, "\"", isDoneStr);
                        }
                    }
                }
            }

            if (!isFound)
            {
                Console.WriteLine("Задачи с таким идентификатором не существует.");
            }
        }
    }
}