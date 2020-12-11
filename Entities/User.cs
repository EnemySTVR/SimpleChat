using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    [Serializable, ProtoContract]
    public class User
    {
        [ProtoMember(1132)]
        public string Name { get; set; }
        public User(string name)
        {
            Name = name;
        }

        public User()
        {

        }
    }
}
