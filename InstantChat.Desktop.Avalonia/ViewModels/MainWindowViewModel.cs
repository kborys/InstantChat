using Avalonia.Interactivity;
using Avalonia.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantChat.Desktop.Avalonia.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private HubConnection _chatConnection;
        private List<string> messages = new();
        private bool isConnected = false;

        private string message = String.Empty;
        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }

        public MainWindowViewModel()
        {
            _chatConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7094/chathub")
            .WithAutomaticReconnect()
            .Build();

            _chatConnection.Reconnecting += (sender) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var newMessage = "Attempting to reconect...";
                    messages.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            _chatConnection.Reconnected += (sender) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var newMessage = "Reconnected to the server";
                    messages.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            _chatConnection.Closed += (sender) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var newMessage = "Connection closed";
                    messages.Add(newMessage);
                    isConnected = false;
                });
                return Task.CompletedTask;
            };
        }

        private async void openConnection_Click()
        {
            _chatConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                // event is called on some thread, use dispatcher to ensure that it is called on the same thread as UI
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var formattedMessage = $"{user}: {message}";
                    messages.Add(formattedMessage);
                });
            });

            try
            {
                await _chatConnection.StartAsync();
                messages.Add("Connected");
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
            }
        }

        private async void sendMessage_Click()
        {
            try
            {
                await _chatConnection.InvokeAsync("SendMessage", "WPFClient", message);
                Message = String.Empty;
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
            }
        }
    }
}