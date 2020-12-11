using Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Easy_chat.Models
{
    class Connector
    {
        private readonly string ip = "127.0.0.1";
        private readonly int port = 8080;
        private IPEndPoint tcpEndPoint;
        private Socket tcpSocket;

        internal Dialog GetDialog()
        {
            tcpSocket = CreateNewTCPSocket();

            string query = "GetDialog";
            var sendData = Encoding.UTF8.GetBytes(query);
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(sendData);

            var buffer = new byte[65536];
            var size = tcpSocket.Receive(buffer);
            var receivedData = new byte[size];
            Array.Copy(buffer, receivedData, size);
            
            var dialog = (Dialog)Serializator.Deserialization(receivedData, typeof(Dialog));

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
            return dialog;
        }

        internal void SendMessage(Message message)
        {
            tcpSocket = CreateNewTCPSocket();

            var data = Serializator.Serialization(message);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        internal void Connect(User user)
        {
            tcpSocket = CreateNewTCPSocket();

            var data = Serializator.Serialization(user);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        internal void Disconnect(User user)
        {
            var query = "Disconnect " + user.Name;

            tcpSocket = CreateNewTCPSocket();
            var sendData = Encoding.UTF8.GetBytes(query);
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(sendData);
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        private Socket CreateNewTCPSocket()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return tcpSocket;
        }
    }
}
