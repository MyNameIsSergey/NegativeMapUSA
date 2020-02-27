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
        public bool EmptyBuff { get => reader.EndOfStream; }
        public int BuffSize { get => buffSize; set { buffSize = value > 0 ? value : buffSize;  } }
        private int buffSize = 200;

        private MessageParser parser = new MessageParser();
        private List<Message> messages = new List<Message>();

        System.IO.StreamReader reader;
        System.Threading.Thread thread;
        public MessageReader(string file)
        {
            OpenNewFile(file);
            ReadNext(buffSize / 2);

            thread = new System.Threading.Thread(ReadNext);
            thread.Priority = System.Threading.ThreadPriority.Normal;
            thread.Start();
        }
        public void OpenNewFile(string file)
        {
            if (reader != null)
            {
                reader.Close();
            }
            reader = new System.IO.StreamReader(file);
            //TryReadNext();
        }
        public Message GetNextMessage()
        {
            Message message;
            lock (messages)
            {
                message = messages[0];
                messages.RemoveAt(0);
            }
            
            return message;
        }


        private void ReadNext(int qt)
        {
            for (int i = 0; i < qt && !reader.EndOfStream; i++)
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
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                ReadNext(buffSize);
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
            List<string> array = s.ToLower().Split(' ', '\t', '[', ']', ',').ToList();
            try
            {
                message.Position = new PointF(float.Parse(array[1].Replace('.', ',')), float.Parse(array[3].Replace('.', ',')));
                //for (int i = 8; i < array.Length; i++)

                string l = "";
                for(int i = 8; i < array.Count - 8; i++)
                {
                    l += array[i] + ' ';
                }
                array = l.Split(',', ' ', '.', '%', '@', '}', '{', ')', '(').ToList();
                array.RemoveAll((k) => k == string.Empty);
                message.Text = new string[array.Count];
                Array.Copy(array.ToArray(), 0, message.Text, 0, array.Count);
                //message.Text += array[i] + ' ';
            }
            catch
            {
                return null;
            }
            return message;

        }
    }
}


//public Message GetNextMessage(IEnumerable<State> states)
//{
//    Message message = new Message();
//    if (buff.Count == 0)
//    {
//        if (TryReadNext())
//            return GetNextMessage(states);
//        return null;
//    }
//    string[] array = buff.First().Split(' ', '\t', '[', ']', ',');
//    try
//    {
//        message.Position = new PointF(float.Parse(array[1].Replace('.', ',')), float.Parse(array[3].Replace('.', ',')));
//        //for (int i = 8; i < array.Length; i++)
//        message.Text = new string[array.Length - 8];
//        Array.Copy(array, 8, message.Text, 0, array.Length - 8);
//        //message.Text += array[i] + ' ';
//    }
//    catch
//    {
//        buff.RemoveAt(0);
//        return GetNextMessage(states);
//    }

//    buff.RemoveAt(0);
//    return message;
//}
