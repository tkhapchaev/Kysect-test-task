using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Kysect
{
    class Task
    {
        public class Subtask
        {
            public int ID { get; }
            public string Name { get; }
            public bool IsDone { get; set; }

            public Subtask(int id, string name)
            {
                ID = id;
                Name = name;
            }
        }

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

        public int CountDoneSubtasks()
        {
            int counter = 0;
            foreach (var subtask in Subtasks)
            {
                if (subtask.IsDone)
                {
                    ++counter;
                }
            }

            return counter;
        }

        public void Print(bool printWithNewLine)
        {
            if (printWithNewLine)
            {
                if (IsDone)
                {
                    if (Subtasks.Count != 0)
                    {
                        int numOfDoneSubtasks = CountDoneSubtasks();
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" +
                                                 Convert.ToString(Subtasks.Count) + ")";
                        Console.WriteLine("[{0}]. {1} {2}{3}{4} {5}. {6}", ID, "Задача", "\"", Name, "\"", "выполнена",
                            doneSubtasksStr);
                    }

                    else
                    {
                        Console.WriteLine("[{0}]. {1} {2}{3}{4} {5}.", ID, "Задача", "\"", Name, "\"", "выполнена");
                    }
                }

                else
                {
                    if (Subtasks.Count != 0)
                    {
                        int numOfDoneSubtasks = CountDoneSubtasks();
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" +
                                                 Convert.ToString(Subtasks.Count) + ")";
                        Console.WriteLine("[{0}]. {1} {2}{3}{4} {5}. {6}", ID, "Задача", "\"", Name, "\"",
                            "не выполнена", doneSubtasksStr);
                    }

                    else
                    {
                        Console.WriteLine("[{0}]. {1} {2}{3}{4} {5}.", ID, "Задача", "\"", Name, "\"", "не выполнена");
                    }
                }
            }

            else
            {
                if (IsDone)
                {
                    if (Subtasks.Count != 0)
                    {
                        int numOfDoneSubtasks = CountDoneSubtasks();
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" +
                                                 Convert.ToString(Subtasks.Count) + ")";
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}. {6}\n", ID, "Задача", "\"", Name, "\"", "выполнена",
                            doneSubtasksStr);
                    }

                    else
                    {
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}.\n", ID, "Задача", "\"", Name, "\"", "выполнена");
                    }
                }

                else
                {
                    if (Subtasks.Count != 0)
                    {
                        int numOfDoneSubtasks = CountDoneSubtasks();
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" +
                                                 Convert.ToString(Subtasks.Count) + ")";
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}. {6}\n", ID, "Задача", "\"", Name, "\"", "не выполнена",
                            doneSubtasksStr);
                    }

                    else
                    {
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}.\n", ID, "Задача", "\"", Name, "\"", "не выполнена");
                    }
                }
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
                            if (subtask.IsDone)
                            {
                                Console.WriteLine("- [{0}]. {1} {2}{3}{4} {5}.", subtask.ID, "Подзадача", "\"",
                                    subtask.Name, "\"", "выполнена");
                            }

                            else
                            {
                                Console.WriteLine("- [{0}]. {1} {2}{3}{4} {5}.", subtask.ID, "Подзадача", "\"",
                                    subtask.Name, "\"", "не выполнена");
                            }
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