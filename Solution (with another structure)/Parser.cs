using System;
using System.Linq;
using System.Collections.Generic;

namespace Kysect
{
    class Parser
    { 
        public static int Parse(string input, List<Task> tasks, List<Group> groups, int counter)
        {
            List<string> command = input.Split(' ').ToList();
            var result = 0;
            
            switch (command[0])
            {
                case "/all":
                    if (command.Count > 1)
                    {
                        Console.WriteLine("Неизвестная команда.");
                        break;
                    }
                        
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Список задач пуст!");
                        break;
                    }

                    if (tasks.Count > 0)
                    {
                        Executor.All(tasks, groups);
                    }
                    
                    break;
                
                case "/add":
                    command.Remove("/add");
                    if (command.Count == 0)
                    {
                        Console.WriteLine("Не введено название задачи.");
                        break;
                    }

                    result = Executor.Add(tasks, command, counter);
                    counter = result;
                    break;

                case "/delete":
                    if (command.Count > 2)
                    {
                        Console.WriteLine("Неизвестная команда.");
                        break;
                    }

                    Executor.Delete(tasks, command);
                    break;
                
                case "/save":
                    command.Remove("/save");
                    Executor.Save(tasks, groups, command);
                    break;
                
                case "/load":
                    command.Remove("/load");
                    result = Executor.Load(tasks, groups, command, counter);
                    counter = result;
                    break;
                
                case "/complete":
                    if (command.Count > 2)
                    {
                        Console.WriteLine("Неизвестная команда.");
                        break;
                    }

                    Executor.Complete(tasks, command);
                    break;
                
                case "/completed":
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Список задач пуст!");
                        break;
                    }

                    Executor.Completed(tasks, groups, command);
                    break;
                
                case "/set-deadline":
                    Executor.SetDeadline(tasks, command);
                    break;
                
                case "/today":
                    if (command.Count > 1)
                    {
                        Console.WriteLine("Неизвестная команда.");
                        break;
                    }

                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Список задач пуст!");
                        break;
                    }

                    Executor.Today(tasks);
                    break;
                
                case "/create-group":
                    GroupHandler.CreateGroup(command, groups);
                    break;
                
                case "/delete-group":
                    GroupHandler.DeleteGroup(command, groups);
                    break;
                
                case "/add-to-group":
                    command.Remove("/add-to-group");
                    Executor.AddToGroup(tasks, groups, command);
                    break;
                
                case "/delete-from-group":
                    command.Remove("/delete-from-group");
                    Executor.DeleteFromGroup(tasks, groups, command);
                    break;
                
                case "/add-subtask":
                    command.Remove("/add-subtask");
                    result = Executor.AddSubtask(tasks, command, counter);
                    counter = result;
                    break;
                
                case "/subtasks":
                    if (command.Count > 2)
                    {
                        Console.WriteLine("Неизвестная команда.");
                        break;
                    }

                    Task.PrintSubtasks(command, tasks);
                    break;
                
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }

            return counter;
        }
    }
}