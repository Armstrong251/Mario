using System.Collections.Generic;
using System.Linq;

namespace Mario.Entities.Commands
{
    public class CommandComponent
    {
        private int index = 0;
        private Dictionary<int, List<Command>> commands;
        private Entity entity;
        public bool Running { get; private set; } = false;
        public IEnumerable<Command> Peek => commands.Select(p => p.Value[0]);
        public CommandComponent(Entity e)
        {
            entity = e;
            commands = new Dictionary<int, List<Command>>();
        }

        public int Add(Command c) { return Add(c, false); }
        public int Add(Command c, bool start)
        {
            int i = index;
            if (!commands.ContainsKey(i))
            {
                commands[i] = new List<Command>();
                index++;
            }
            if (start) commands[i].Insert(0, c);
            else commands[i].Add(c);
            return i;
        }
        public int Add(Command[] c) { return Add(c, -1, false); }
        public int Add(Command[] c, int i, bool start)
        {
            if (i < 0 || i == index)
            {
                i = index++;
                commands[i] = new List<Command>();
            }
            if (start) commands[i].InsertRange(0, c);
            else commands[i].AddRange(c);
            return i;
        }

        public void Clear()
        {
            commands.Clear();
        }

        public void Update()
        {
            foreach (var p in commands.ToArray())
            {
                Command command = p.Value[0];
                Running = true;
                command.Condition.Update();
                if(command.Invoke(entity))
                {
                    p.Value.RemoveAt(0);
                }
                Running = false;
                if (p.Value.Count == 0) commands.Remove(p.Key);
            }
        }
        public bool IsNext<T>()
            where T : Command
        {
            return commands.Any(p => p.Value.Count > 0 && p.Value[0] is T);
        }
        public T GetNext<T>()
            where T : Command
        {
            return (T)commands.Select(p => p.Value[0]).First(v => v is T);
        }
        public int GetQueue(Command c)
        {
            return commands.Where(p => p.Value.Contains(c)).First().Key;
        }
        public static CommandComponent operator +(CommandComponent c, Command command)
        {
            c.Add(command);
            return c;
        }
        public static CommandComponent operator +(CommandComponent c, Command[] command)
        {
            c.Add(command);
            return c;
        }
    }
}