using System;
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
            int counter = 1;
            
            while (input != "/exit" && input != null)
            {
                var newID = Parser.Parse(input, tasks, groups, counter);
                counter = newID;
                input = Console.ReadLine();
            }
        }
    }
}