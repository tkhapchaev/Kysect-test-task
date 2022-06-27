using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Kysect
{
    class FileIO
    {
        public static void Save(List<Task> tasks, List<Group> groups, string fileName)
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

        public static void Load(List<Task> tasks, List<Group> groups, string fileName, int counter)
        {
            string currentLine;
            StreamReader stream = new StreamReader(fileName);
            while ((currentLine = stream.ReadLine()) != null)
            {
                if (currentLine.Contains("{\"Type\":\"Task\","))
                {
                    Task temp = JsonSerializer.Deserialize<Task>(currentLine);
                    if (temp != null && temp.ID >= counter)
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
    }
}