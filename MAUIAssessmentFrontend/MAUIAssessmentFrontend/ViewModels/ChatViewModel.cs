using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MAUIAssessmentFrontend.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged();
                }
            }
        }


        public ObservableCollection<ChatMessage> Messages { get; set; } = new();

        public ICommand SendCommand { get; }

        private readonly GeminiService _geminiService = new GeminiService();

        public ChatViewModel()
        {
            SendCommand = new Command(async () => await SendMessage());
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
                return;

            Messages.Add(new ChatMessage { Message = UserInput, IsUser = true });
           


            var botReply = await _geminiService.GetGeminiResponseAsync(UserInput);
            Messages.Add(new ChatMessage { Message = botReply, IsUser = false });

            UserInput = string.Empty;
        }
    }
}