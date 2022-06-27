using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Kysect
{
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
}