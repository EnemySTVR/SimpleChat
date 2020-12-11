using Entities;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static Dialog dialog;
        static Bot serverBot;

        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 8080;
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverBot = new Bot("Bot");

            dialog = new Dialog();
            dialog.ConnectedUsers.Add(serverBot.GetEntity());

            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10);

            while(true)
            {
                var listener = tcpSocket.Accept();
                var buffer = new byte[256];
                var size = listener.Receive(buffer);
                var data = new byte[size];
                Array.Copy(buffer, data, size);

                var tryEncoding = Encoding.UTF8.GetString(data).ToString();
                if (tryEncoding.Equals("GetDialog"))
                {
                    listener.Send(Serializator.Serialization(dialog));
                }else if (tryEncoding.Contains("Disconnect"))
                {
                    // Похоже на hardcode... но это же тестовое задание)
                    // В идеале передавать Dictionary с коммандой и значением.
                    int index = tryEncoding.IndexOf(" ");
                    var userName = tryEncoding.Substring(index + 1);
                    var targetUser = dialog.ConnectedUsers.Where(x => x.Name.Equals(userName)).FirstOrDefault();
                    dialog.ConnectedUsers.Remove(targetUser);
                }
                else
                {
                    var message = Serializator.Deserialization(data, typeof(Message)) as Message;
                    if (message != null && 
                        message.Sender != null && 
                        message.Text != null && 
                        message.TimeOfSend != DateTime.MinValue)
                    {
                        dialog.Messages.Add(message);
                        switch (message.Text)
                        {
                            case "GetUsersList":
                                var messageWithUsersList = serverBot.CreateMessageWithUsersList(dialog);
                                dialog.Messages.Add(messageWithUsersList);
                                break;
                                // Сюда можно докидывать кейсами новые команды по формированию данных.
                        }
                        if (serverBot.IsUnderstand(message.Text))
                        {
                            var answerText = serverBot.GetAnswer(message.Text);
                            var answerMessage = new Message(answerText, serverBot.GetEntity());
                            dialog.Messages.Add(answerMessage);
                        }
                    }

                    var appendUser = Serializator.Deserialization(data, typeof(User)) as User;
                    if (appendUser != null && 
                        appendUser.Name != null)
                    {
                        dialog.ConnectedUsers.Add(appendUser);
                    }
                }
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
