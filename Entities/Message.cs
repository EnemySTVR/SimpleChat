using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    [Serializable, ProtoContract]
    public class Message
    {
        [ProtoMember(1)]
        public string Text { get; set; }

        [ProtoMember(2)]
        public DateTime TimeOfSend { get; private set; }

        [ProtoMember(3)]
        public User Sender { get; set; }

        public Message(string text, User sender)
        {
            Text = text;
            Sender = sender;
            TimeOfSend = DateTime.Now;
        }

        public Message()
        {

        }
    }
}
