using System;
using System.Collections.Generic;

namespace Kysect
{
    class GroupsHandler
    {
        public static void CreateGroup(List<string> command, List<Group> groups)
        {
            command.Remove("/create-group");
            string groupName = string.Join(" ", command.ToArray());
            bool isUnique = true;
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
        }

        public static void DeleteGroup(List<string> command, List<Group> groups)
        {
            bool isDeleted = false;
            command.Remove("/delete-group");
            string groupName = string.Join(" ", command.ToArray());
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
        }
    }
}