using Entities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    // Я с удовольствием бы унаследовал Bot от User и расширил, но protobuf не позволяет
    // т.к. Entities не может ссылаться на Server и явно указать в атрибуте ProtoInclude класса User
    // тип Bot не представляется возможным.
    class Bot
    {
        private Dictionary<string, string> answers = new Dictionary<string, string>()
        {
            {"how are you", "Fine, thanks!" }
            // Сюда можно докидывать простые диалоговые ответы бота без сбора каких либо данных.
        };
        private User entity;
        internal Bot(string name)
        {
            entity = new User(name);
        }

        internal User GetEntity()
        {
            return entity;
        }

        internal bool IsUnderstand(string message)
        {
            if (answers.Keys.Contains(message, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal string GetAnswer(string message)
        {
            return answers[message.ToLower()];
        }

        internal Message CreateMessageWithUsersList(Dialog dialog)
        {
            var text = new StringBuilder();
            foreach (var user in dialog.ConnectedUsers)
            {
                text.Append(user.Name + "\n");
            }
            return new Message(text.ToString(), entity);
        }
    }
}
