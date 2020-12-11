using Easy_chat.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Entities;
using System.Windows.Controls;
using Easy_chat.Views;

namespace Easy_chat.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly Page loginPage;
        private readonly Page chatPage;

        private readonly Connector connector = new Connector();

        private Page selectedPage;
        public Page SelectedPage
        {
            get
            {
                return selectedPage;
            }
            set
            {
                selectedPage = value;
                OnPropertyChanged("SelectedPage");
            }
        }

        private User CurrentUser { get; set; }

        private Dialog currentDialog;
        public Dialog CurrentDialog
        {
            get
            {
                if (currentDialog == null)
                {
                    currentDialog = connector.GetDialog();
                }
                return currentDialog;
            }
            set
            {
                currentDialog = value;
                OnPropertyChanged("CurrentDialog");
            }
        }

        private string loginFormText;
        public string LoginFormText
        {
            get
            {
                return loginFormText;
            }
            set
            {
                loginFormText = value;
                OnPropertyChanged("LoginFormText");
            }
        }

        private string messageText;
        public string MessageText
        {
            get
            {
                return messageText;
            }
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        private RelayCommand joinTheChat;
        public RelayCommand JoinTheChat
        {
            get
            {
                if (joinTheChat == null)
                {
                    joinTheChat = new RelayCommand(obj =>
                    {
                        CurrentUser = new User(LoginFormText);
                        LoginFormText = "";
                        SelectedPage = chatPage;
                        connector.Connect(CurrentUser);
                        CurrentDialog = connector.GetDialog();
                    });
                }
                return joinTheChat;
            }
        }

        private RelayCommand sendMessage;
        public RelayCommand SendMessage
        {
            get
            {
                if (sendMessage == null)
                {
                    sendMessage = new RelayCommand(obj => 
                    {
                        if (MessageText.Equals("By"))
                        {
                            connector.Disconnect(CurrentUser);
                            SelectedPage = loginPage;
                        }
                        else
                        {
                            var message = new Message(MessageText, CurrentUser);
                            connector.SendMessage(message);
                            MessageText = "";
                            CurrentDialog = connector.GetDialog();
                        }
                    });
                }
                return sendMessage;
            }
        }

        public MainViewModel()
        {
            loginPage = new LoginPage
            {
                DataContext = this
            };
            chatPage = new ChatPage
            {
                DataContext = this
            };
            SelectedPage = loginPage;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}