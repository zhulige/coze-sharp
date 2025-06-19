using CozeSharp;
using CozeSharp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozeSharp_MauiApp.Services
{
    public class CozeAgnetService: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
        private readonly CozeAgent _agent;
        public CozeAgent Agent => _agent;
        private string _questionMessae = "";
        public string QuestionMessae
        {
            get => _questionMessae;
            set
            {
                if (_questionMessae != value)
                {
                    _questionMessae = value;
                    OnPropertyChanged(nameof(QuestionMessae));
                }
            }
        }

        private string _answerMessae = "";
        public string AnswerMessae
        {
            get => _answerMessae;
            set
            {
                if (_answerMessae != value)
                {
                    _answerMessae = value;
                    OnPropertyChanged(nameof(AnswerMessae));
                }
            }
        }
        public CozeAgnetService()
        {
            //CozeSharp.Global.IsAudio = false;
            _agent = new CozeAgent();
            _agent.Token = "pat_YAnaVgTxedzRCG5dzLk7YU3II1TvpAfJpqqN0IKWZD16qbbHdENuep4FWpdgJaRX";
            _agent.BotId = "7483916119175430153";
            _agent.UserId = "ZhuLige";
            _agent.OnMessageEvent += Agent_OnMessageEvent;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                _agent.AudioService = new Services.AudioService();
            }
            _ = _agent.Start();
        }

        private Task Agent_OnMessageEvent(string type, string message)
        {
            switch (type.ToLower())
            {
                case "question":
                    //LogConsole.WriteLine(MessageType.Send, $"[{type}] {message}");
                    QuestionMessae = message;
                    break;
                case "answer":
                    //LogConsole.WriteLine(MessageType.Recv, $"[{type}] {message}");
                    AnswerMessae = message;
                    break;
                default:
                    //LogConsole.InfoLine($"[{type}] {message}");
                    break;
            }

            return Task.CompletedTask;
            // throw new NotImplementedException();
        }
    }
}
