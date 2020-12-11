using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Entities
{
    [Serializable, ProtoContract]
    public class Dialog
    {
        [ProtoMember(1)]
        private ICollection<Message> messages;
        [ProtoMember(2)]
        public ObservableCollection<Message> Messages
        {
            get
            {
                if (messages == null)
                {
                    messages = new ObservableCollection<Message>();
                }
                return messages as ObservableCollection<Message>;
            }
            set
            {
                messages = value;
            }
        }

        [ProtoMember(3)]
        private ICollection<User> connectedUsers;
        [ProtoMember(4)]
        public ObservableCollection<User> ConnectedUsers
        {
            get
            {
                if (connectedUsers == null)
                {
                    connectedUsers = new ObservableCollection<User>();
                }
                return connectedUsers as ObservableCollection<User>;
            }
            set
            {
                connectedUsers = value;
            }
        }

    }
}
