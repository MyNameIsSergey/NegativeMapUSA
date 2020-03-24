using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class StateCollection
    {
        public event EventHandler<StateEventArgs> OnNewData;
        public event EventHandler<StateEventArgs> OnEnd;
        public int Update = 300;
        public int ProcessedMessages { get; private set; } 
        public State[] States { get; private set; }

        public StateCollection(State[] states)
        {
            States = states;
        }
        public State FindState(PointF point)
        {
            foreach (var s in States)
            {
                if (s.Belong(point))
                {
                    return s;
                }
            }
            return null;
        }
        public void CalculatePositiveLevel(Dictionary dictionary, MessageReader messageReader)
        {
            Message message;
            while (!messageReader.EmptyBuff)
            {
                if ((message = messageReader.GetNextMessage()) == null)
                    continue;

                State s = FindState(message.Position);
                if (s == null)
                    continue;
                s.CorrectPositiveLevel(dictionary.CheckSentence(message.Text));
                if (++ProcessedMessages % Update == 0)
                {
                    Task.Run(() => OnNewData(this, new StateEventArgs() { States = States, ProcessedMessage = ProcessedMessages }));
                }
            }
            OnEnd(this, new StateEventArgs() { States = States, ProcessedMessage = ProcessedMessages });
        }

    }
    class StateEventArgs : EventArgs
    {
        public State[] States { get; set; }
        public int ProcessedMessage { get; set; }
    }
}
