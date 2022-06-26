using System;
using System.IO;
using System.Linq;
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
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" + Convert.ToString(Subtasks.Count) + ")";
                        Console.WriteLine("[{0}]. {1} {2}{3}{4} {5}. {6}", ID, "Задача", "\"", Name, "\"", "выполнена", doneSubtasksStr);
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
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" + Convert.ToString(Subtasks.Count) + ")";
                        Console.WriteLine("[{0}]. {1} {2}{3}{4} {5}. {6}", ID, "Задача", "\"", Name, "\"", "не выполнена", doneSubtasksStr);
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
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" + Convert.ToString(Subtasks.Count) + ")";
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}. {6}\n", ID, "Задача", "\"", Name, "\"", "выполнена", doneSubtasksStr);
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
                        string doneSubtasksStr = "(" + Convert.ToString(numOfDoneSubtasks) + "/" + Convert.ToString(Subtasks.Count) + ")";
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}. {6}\n", ID, "Задача", "\"", Name, "\"", "не выполнена", doneSubtasksStr);
                    }

                    else
                    {
                        Console.Write("[{0}]. {1} {2}{3}{4} {5}.\n", ID, "Задача", "\"", Name, "\"", "не выполнена");
                    }
                }
            }
        }
    }

    class Group
    {
        public string Type { get; set; } = "Group";
        public string Name { get; set; }
        public bool Status { get; set; } = true;
        public List<int> Members { get; set; } = new List<int>();

        public Group(string name)
        {
            Name = name;
        }

        public void AddMember(int ID)
        {
            Members.Add(ID);
        }

        public void RemoveMember(int ID)
        {
            Members.Remove(ID);
        }
        
        public void Serialize(string fileName)
        {
            string contents = JsonSerializer.Serialize(this) + "\n";
            File.AppendAllText(fileName, contents);
        }
    }
    
    class Program
    {
        public static void Main()
        {
            string input = Console.ReadLine();
            List<Task> tasks = new List<Task>();
            List<Group> groups = new List<Group>();
            bool belongsToGroup, isUnique, isDeleted, isFound, isExist, isAdded;
            int counter = 1;
            
            while (input != "/exit" && input != null)
            {
                List<string> command = input.Split(' ').ToList();
                switch (command[0])
                {
                    case "/add":
                        command.Remove("/add");
                        string taskName = string.Join(" ", command.ToArray());
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
                            foreach (var task in tasks)
                            {
                                if (task.Status)
                                {
                                    task.Serialize(fileName);
                                }
                            }

                            foreach (var group in groups)
                            {
                                if (group.Status)
                                {
                                    group.Serialize(fileName);
                                }
                            }
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
                        string currentLine;

                        try
                        {
                            StreamReader stream = new StreamReader(fileName);
                            while ((currentLine = stream.ReadLine()) != null)
                            {
                                if (currentLine.Contains("{\"Type\":\"Task\","))
                                {
                                    Task temp = JsonSerializer.Deserialize<Task>(currentLine);
                                    if (temp.ID >= counter)
                                    {
                                        counter = temp.ID + 1;
                                    }
                                    
                                    tasks.Add(temp);
                                }

                                if (currentLine.Contains("{\"Type\":\"Group\","))
                                {
                                    groups.Add(JsonSerializer.Deserialize<Group>(currentLine));
                                }
                            }

                            stream.Close();
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
                        command.Remove("/create-group");
                        string groupName = string.Join(" ", command.ToArray());
                        isUnique = true;
                        if (command.Count >= 1)
                        {
                            foreach (var group in groups)
                            {
                                if (group.Status && group.Name == groupName)
                                {
                                    Console.WriteLine("Такая группа уже создана!");
                                    isUnique = false;
                                }
                            }

                            if (isUnique)
                            {
                                groups.Add(new Group(groupName));
                            }
                        }

                        else
                        {
                            Console.WriteLine("Не введено название группы.");
                        }
                        
                        break;
                    
                    case "/delete-group":
                        isDeleted = false;
                        command.Remove("/delete-group");
                        groupName = string.Join(" ", command.ToArray());
                        foreach (var group in groups)
                        {
                            if (group.Name == groupName)
                            {
                                if (group.Status)
                                {
                                    group.Status = false;
                                    isDeleted = true;
                                }

                                else
                                {
                                    Console.WriteLine("Группа уже удалена.");
                                    isDeleted = true;
                                }
                            }
                        }

                        if (!isDeleted)
                        {
                            Console.WriteLine("Группы с таким именем не существует.");
                        }
                        
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
                        
                        isFound = false;
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
                                            Console.WriteLine("- [{0}]. {1} {2}{3}{4} {5}.", subtask.ID, "Подзадача", "\"", subtask.Name, "\"", "выполнена");
                                        }

                                        else
                                        {
                                            Console.WriteLine("- [{0}]. {1} {2}{3}{4} {5}.", subtask.ID, "Подзадача", "\"", subtask.Name, "\"", "не выполнена");
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
                    
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }               
                
                input = Console.ReadLine();
            }
        }
    }
}