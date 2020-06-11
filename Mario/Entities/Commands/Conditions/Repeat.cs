using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.Entities.Commands
{
    class Repeat : CommandCondition
    {
        public Repeat(int repeat)
        {
            Count = repeat;
        }

        public int Count { get; set; }
        public override void Update()
        {
            Count--;
        }

        public override bool EvaluateCondition(Entity entity)
        {
            return Count == 0;
        }
    }
}
