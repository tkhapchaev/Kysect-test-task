namespace Kysect
{
    class Subtask
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
}