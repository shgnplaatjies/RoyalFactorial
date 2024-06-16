using RoyalFactorial.Core.Commands;
using RoyalFactorial.MVVM.Model;
using RoyalFactorial.Services;
using RoyalFactorial.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoyalFactorial.MVVM.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICardGame _cardGame;
        private int _cardsPerPlayer = 5;
        private int _numberOfDecks = 2;
        private string _newPlayerName = string.Empty;

        public MainViewModel() : this(new CardGame()) { }

        public MainViewModel(ICardGame cardGame)
        {
            _cardGame = cardGame;
            PlayerNames = [];
            _shuffleAndDealCommand = new DelegateCommand(ShuffleAndDeal);
            _addPlayerCommand = new DelegateCommand(AddPlayer);
        }


        public int CardsPerPlayer
        {
            get => _cardsPerPlayer;
            set
            {
                _cardsPerPlayer = value;
                OnPropertyChanged(nameof(CardsPerPlayer));
            }
        }

        public int NumberOfDecks
        {
            get => _numberOfDecks;
            set
            {
                _numberOfDecks = value;
                OnPropertyChanged(nameof(NumberOfDecks));
            }
        }

        public string NewPlayerName
        {
            get => _newPlayerName;
            set
            {
                _newPlayerName = value;
                OnPropertyChanged(nameof(NewPlayerName));
            }
        }

        public ObservableCollection<string> PlayerNames { get; }
        public ObservableCollection<Player> Players { get; private set; } = [];
        public Leaderboard Leaderboard { get; private set; } = new([]);

        private void ShuffleAndDeal()
        {
            _cardGame.InitGame(new (PlayerNames), CardsPerPlayer, NumberOfDecks);
            Players = new ObservableCollection<Player>(_cardGame.Players);
            Leaderboard = _cardGame.Leaderboard;

            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(Leaderboard));
        }

        private void AddPlayer()
        {
            bool isValidName = !string.IsNullOrWhiteSpace(NewPlayerName) && !PlayerNames.Contains(NewPlayerName);

            if (isValidName)
            {
                PlayerNames.Add(NewPlayerName);
                NewPlayerName = string.Empty;
                OnPropertyChanged(nameof(PlayerNames));
            }
        }

        private readonly DelegateCommand _shuffleAndDealCommand;
        private readonly DelegateCommand _addPlayerCommand;

        public ICommand ShuffleAndDealCommand => _shuffleAndDealCommand;
        public ICommand AddPlayerCommand => _addPlayerCommand;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new(propertyName));
    }
}
