using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    class MessageReader : IDisposable
    {
        public bool EmptyBuff { get => reader.BaseStream.Position == reader.BaseStream.Length && messages.Count == 0; }
        public int BuffSize { get => buffSize; set { buffSize = value > 0 ? value : buffSize;  } }
        private int buffSize = 200;

        private MessageParser parser = new MessageParser();
        private List<Message> messages = new List<Message>();

        System.IO.StreamReader reader;
        System.Threading.Thread thread;
        public MessageReader(string file)
        {
            OpenNewFile(file);
            FillBuff();

            thread = new System.Threading.Thread(ReadNext);
            thread.Priority = System.Threading.ThreadPriority.Normal;
            thread.Start();
        }
        public void OpenNewFile(string file)
        {
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
            }

            reader = new System.IO.StreamReader(file);
        }
        public Message GetNextMessage()
        {
            Message message = null;
            lock (messages)
            {
                if (messages.Count != 0)
                {
                    message = messages[0];
                    messages.RemoveAt(0);
                }
            }
            
            return message;
        }


        private void FillBuff()
        {
            for (int i = 0; messages.Count < buffSize && !reader.EndOfStream; i++)
            {
                Message message;
                if ((message = parser.ParseMessage(reader.ReadLine())) != null)
                    messages.Add(message);
            }
        }
        private void ReadNext()
        {
            while (true)
            {
                if (messages.Count > buffSize / 2)
                {
                    //System.Threading.Thread.Sleep(300);
                    continue;
                }
                FillBuff();
            }
        }


        public void Dispose()
        {
            thread.Abort();
        }
    }

    class MessageParser
    {
        public Message ParseMessage(string s)
        {
            Message message = new Message();
            string[] array = s.ToLower().Split(' ', '\t', '[', ']', ',');
            float x, y;
            if (array.Length < 8)
                return null;
            if (!float.TryParse(array[1].Replace('.', ','), out x))
                return null;
            if (!float.TryParse(array[3].Replace('.', ','), out y))
                return null;
            message.Position = new PointF(x, y);

            List<string> temp = new List<string>();
            for(int i = 8; i < array.Length; i++)
            {
                temp.AddRange(array[i].Split(',', ' ', '.', '%', '@', '}', '{', ')', '('));
            }
            temp.RemoveAll((k) => k == string.Empty);
            if (temp.Count == 0)
                return null;
            message.Text = temp.ToArray();
            return message;
        }
    }
}

//public Message ParseMessage(string s)
//{
//    Message message = new Message();
//    string[] array = s.ToLower().Split(' ', '\t', '[', ']', ',');
//    float x, y;
//    if (array.Length < 8)
//        return null;
//    if (float.TryParse(array[1], out x))
//        return null;
//    if (float.TryParse(array[3], out y))
//        return null;
//    message.Position = new PointF(x, y);

//    List<string> temp = new List<string>();
//    for (int i = 8; i < array.Length; i++)
//    {
//        temp.AddRange(array[i].Split(',', ' ', '.', '%', '@', '}', '{', ')', '('));
//    }
//    temp.RemoveAll((k) => k == string.Empty);
//    if (temp.Count == 0)
//        return null;
//    message.Text = temp.ToArray();
//    return message;
//}
