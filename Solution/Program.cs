using System;
using System.Linq;
using System.Collections.Generic;

namespace Kysect
{
    class Program
    {
        public static void Main()
        {
            string input = Console.ReadLine();
            List<Task> tasks = new List<Task>();
            List<Group> groups = new List<Group>();
            bool belongsToGroup, isUnique, isDeleted, isFound, isExist, isAdded;
            string taskName, groupName;
            int counter = 1;
            
            while (input != "/exit" && input != null)
            {
                List<string> command = input.Split(' ').ToList();
                switch (command[0])
                {
                    case "/add":
                        command.Remove("/add");
                        taskName = string.Join(" ", command.ToArray());
                        isUnique = true;
                        if (command.Count >= 1)
                        {
                            foreach (var task in tasks)
                            {
                                if (task.Status && task.Name == taskName)
                                {
                                    Console.WriteLine("Такая задача уже добавлена!");
                                    isUnique = false;
                                }
                            }

                            if (isUnique)
                            {
                                tasks.Add(new Task(counter, taskName));
                                ++counter;
                            }
                        }

                        else
                        {
                            Console.WriteLine("Не введено название задачи.");
                        }
                        
                        break;
                    
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
                            DateTime sample = new DateTime(1999, 12, 31);
                            List <Task> notGrouped = new List<Task>();
                            foreach (var task in tasks)
                            {
                                if (task.Status)
                                {
                                    belongsToGroup = false;
                                    foreach (var group in groups)
                                    {
                                        if (group.Members.Contains(task.ID) && group.Status)
                                        {
                                            belongsToGroup = true;
                                        }
                                    }

                                    if (!belongsToGroup)
                                    {
                                        notGrouped.Add(task);
                                    }
                                }
                            }
                            
                            foreach (var group in groups)
                            {
                                if (group.Status)
                                {
                                    Console.WriteLine("Группа {0}{1}{2}:", "\"", group.Name, "\"");
                                    foreach (var ID in group.Members)
                                    {
                                        foreach (var task in tasks)
                                        {
                                            if (task.Status && task.ID == ID)
                                            {
                                                Console.Write("\t- ");
                                                task.Print(false);
                                                if (task.Deadline != sample)
                                                {
                                                    Console.WriteLine("\t  (!)  Дедлайн для задачи {0}: {1}.", task.ID, task.Deadline.ToShortDateString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            Console.WriteLine("Задачи без групп:");
                            foreach (var task in notGrouped)
                            {
                                task.Print(true);
                                if (task.Deadline != sample)
                                {
                                    Console.WriteLine("(!)  Дедлайн для задачи {0}: {1}.", task.ID, task.Deadline.ToShortDateString());
                                }
                            }
                        }

                        break;
                    
                    case "/delete":
                        if (command.Count > 2)
                        {
                            Console.WriteLine("Неизвестная команда.");
                            break;
                        }
                        
                        isDeleted = false;
                        foreach (var task in tasks)
                        {
                            if (Convert.ToString(task.ID) == command[1])
                            {
                                if (task.Status)
                                {
                                    task.Status = false;
                                    isDeleted = true;
                                }

                                else
                                {
                                    Console.WriteLine("Задача уже удалена.");
                                    isDeleted = true;
                                }
                            }
                        }

                        if (!isDeleted)
                        {
                            Console.WriteLine("Задачи с таким идентификатором не существует.");
                        }
                        
                        break;
                    
                    case "/save":
                        command.Remove("/save");
                        string fileName = string.Join(" ", command.ToArray());
                        try
                        {
                            FileIO.Save(tasks, groups, fileName);
                        }

                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                            break;
                        }
                        
                        Console.WriteLine("{0} {1}.\n", "Задачи успешно сохранены в", fileName);
                        break;
                    
                    case "/load":
                        command.Remove("/load");
                        fileName = string.Join(" ", command.ToArray());
                        
                        try
                        {
                            FileIO.Load(tasks, groups, fileName, counter);
                        }

                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                            break;
                        }
                        
                        Console.WriteLine("{0} {1}.\n", "Задачи успешно загружены из", fileName);
                        break;
                    
                    case "/complete":
                        if (command.Count > 2)
                        {
                            Console.WriteLine("Неизвестная команда.");
                            break;
                        }
                        
                        isFound = false;
                        foreach (var task in tasks)
                        {
                            if (Convert.ToString(task.ID) == command[1])
                            {
                                if (task.Status && task.IsDone)
                                {
                                    Console.WriteLine("Эта задача уже выполнена.");
                                    isFound = true;
                                    break;
                                }

                                if (task.Status && !task.IsDone)
                                {
                                    task.IsDone = true;
                                    isFound = true;
                                }
                            }

                            else
                            {
                                foreach (var subtask in task.Subtasks)
                                {
                                    if (Convert.ToString(subtask.ID) == command[1] && task.Status)
                                    {
                                        if (subtask.IsDone)
                                        {
                                            Console.WriteLine("Эта подзадача уже выполнена.");
                                            isFound = true;
                                            break;
                                        }

                                        if (!subtask.IsDone)
                                        {
                                            subtask.IsDone = true;
                                            isFound = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (!isFound)
                        {
                            Console.WriteLine("Задачи с таким идентификатором не существует.");
                        }
                        
                        break;
                    
                    case "/completed":
                        if (tasks.Count == 0)
                        {
                            Console.WriteLine("Список задач пуст!");
                            break;
                        }

                        if (command.Count == 1)
                        {
                            Console.WriteLine("Выполненные задачи:");
                            foreach (var task in tasks)
                            {
                                if (task.IsDone && task.Status)
                                {
                                    Console.WriteLine("{0} {1}{2}{3} {4} {5}.", "Задача", "\"", task.Name, "\"", "с идентификатором", task.ID);
                                }
                            }
                        }

                        else
                        {
                            command.Remove("/completed");
                            string group = string.Join(" ", command.ToArray());
                            Console.WriteLine("Выполненные задачи группы \"{0}\":", group);
                            isFound = false;
                            
                            foreach (var item in groups)
                            {
                                if (item.Status && item.Name == group)
                                {
                                    isFound = true;
                                    foreach (var ID in item.Members)
                                    {
                                        foreach (var task in tasks)
                                        {
                                            if (task.Status && task.IsDone && task.ID == ID)
                                            {
                                                Console.WriteLine("{0} {1}{2}{3} {4} {5}.", "Задача", "\"", task.Name, "\"", "с идентификатором", task.ID);
                                            }
                                        }
                                    }
                                }
                            }

                            if (!isFound)
                            {
                                Console.WriteLine("Группы с таким именем не существует.");
                            }
                        }
                        
                        break;
                    
                    case "/set-deadline":
                        try
                        {
                            isFound = false;
                            if (command.Count == 3)
                            {
                                foreach (var task in tasks)
                                {
                                    if (Convert.ToString(task.ID) == command[1])
                                    {
                                        if (task.Status)
                                        {
                                            task.Deadline = DateTime.Parse(command[2]);
                                            isFound = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                Console.WriteLine("Неизвестная команда.");
                                break;
                            }

                            if (!isFound)
                            {
                                Console.WriteLine("Задачи с таким идентификатором не существует.");
                            }
                        }
                        
                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                        }

                        break;
                    
                    case "/today":
                        try
                        {
                            isFound = false;
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
                            
                            Console.WriteLine("Задачи, которые нужно сделать сегодня:");
                            if (tasks.Count > 0)
                            {
                                foreach (var task in tasks)
                                {
                                    if (task.Status && !task.IsDone && (task.Deadline == DateTime.Today))
                                    {
                                        task.Print(true);
                                        isFound = true;
                                    }
                                }
                            }

                            if (!isFound)
                            {
                                Console.WriteLine("Все задачи на сегодня выполнены! Ты - молодец :3");
                            }
                        }
                        
                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                        }

                        break;
                    
                    case "/create-group":
                        GroupsHandler.CreateGroup(command, groups);
                        break;
                    
                    case "/delete-group":
                        GroupsHandler.DeleteGroup(command, groups);
                        break;
                    
                    case "/add-to-group":
                        try
                        {
                            command.Remove("/add-to-group");
                            var taskToAdd = command[0];
                            command.Remove(taskToAdd);
                            groupName = string.Join(" ", command.ToArray());
                            isExist = false; 
                            isAdded = false;
                            
                            foreach (var task in tasks)
                            {
                                if (task.ID == Convert.ToInt32(taskToAdd) && task.Status)
                                {
                                    isExist = true;
                                }
                            }

                            if (!isExist)
                            {
                                Console.WriteLine("Такой задачи не существует.");
                                break;
                            }
                            
                            foreach (var group in groups)
                            {
                                if (group.Name == groupName && group.Status && isExist)
                                {
                                    group.AddMember(Convert.ToInt32(taskToAdd));
                                    isAdded = true;
                                }

                                if (!group.Status)
                                {
                                    Console.WriteLine("Эта группа удалена.");
                                    break;
                                }
                            }

                            if (!isAdded)
                            {
                                Console.WriteLine("Такой группы не существует.");
                            }
                        }
                        
                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                        }
                        
                        break;
                    
                    case "/delete-from-group":
                        try
                        {
                            command.Remove("/delete-from-group");
                            var taskToDelete = command[0];
                            command.Remove(taskToDelete);
                            groupName = string.Join(" ", command.ToArray());
                            isDeleted = false;
                            isExist = false;

                            foreach (var task in tasks)
                            {
                                if (task.ID == Convert.ToInt32(taskToDelete) && task.Status)
                                {
                                    isExist = true;
                                }
                            }

                            if (!isExist)
                            {
                                Console.WriteLine("Такой задачи не существует.");
                                break;
                            }
                            
                            foreach (var group in groups)
                            {
                                if (group.Name == groupName && group.Status && isExist)
                                {
                                    group.RemoveMember(Convert.ToInt32(taskToDelete));
                                    isDeleted = true;
                                }

                                if (!group.Status)
                                {
                                    Console.WriteLine("Эта группа удалена.");
                                    break;
                                }
                            }

                            if (!isDeleted)
                            {
                                Console.WriteLine("Такой группы не существует, либо в эту группу не входит такое задание.");
                            }
                        }
                        
                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                        }

                        break;
                    
                    case "/add-subtask":
                        try
                        {
                            command.Remove("/add-subtask");
                            var parentTaskID = command[0];
                            command.Remove(parentTaskID);
                            string subtaskName = string.Join(" ", command.ToArray());
                            isUnique = true;
                            isFound = false;

                            foreach (var task in tasks)
                            {
                                if (task.ID == Convert.ToInt32(parentTaskID) && task.Status)
                                {
                                    isFound = true;
                                    foreach (var subtask in task.Subtasks)
                                    {
                                        if (subtask.Name == subtaskName)
                                        {
                                            isUnique = false;
                                            Console.WriteLine("Такая подзадача уже добавлена.");
                                            break;
                                        }
                                    }
                                }
                            }
                            
                            foreach (var task in tasks)
                            {
                                if (task.ID == Convert.ToInt32(parentTaskID) && task.Status && isFound && isUnique)
                                {
                                    task.Subtasks.Add(new Task.Subtask(counter, subtaskName));
                                    ++counter;
                                }
                            }
                        }
                        
                        catch (Exception failure)
                        {
                            Console.WriteLine("Кажется, что-то пошло не так...\n" + failure.Message);
                        }
                        
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
                
                input = Console.ReadLine();
            }
        }
    }
}