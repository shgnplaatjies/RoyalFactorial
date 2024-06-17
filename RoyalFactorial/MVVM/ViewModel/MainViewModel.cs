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
        private readonly int MIN_CARDS_PER_PLAYER = 1;
        private readonly int MAX_CARDS_PER_PLAYER = 100;
        private readonly int MIN_NUMBER_OF_DECKS = 1;
        private readonly int MAX_NUMBER_OF_DECKS = 100;
        private readonly int NAME_CHARACTER_LIMIT = 50;
        private readonly int MAX_PLAYERS_PER_GAME = 10;
        private readonly int MIN_PLAYERS_PER_GAME = 1;

        private readonly ICardGame _cardGame;
        private int _cardsPerPlayer = 5;
        private int _numberOfDecks = 2;
        private string _newPlayerName = string.Empty;
        private string _alertMessage = string.Empty;

        public MainViewModel() : this(new CardGame()) { }

        public MainViewModel(ICardGame cardGame)
        {
            _cardGame = cardGame;
            PlayerNames = [];
            _shuffleAndDealCommand = new DelegateCommand(ShuffleAndDeal);
            _addPlayerCommand = new DelegateCommand(AddPlayer);
            _removeLeaderboardPlayerCommand = new DelegateCommand<Player>(RemoveLeaderboardPlayer);
        }


        public int CardsPerPlayer
        {
            get => _cardsPerPlayer;
            set
            {
                if (value < MIN_CARDS_PER_PLAYER || value > MAX_CARDS_PER_PLAYER)
                { AlertMessage = $"Cards per player must be between {MIN_CARDS_PER_PLAYER} and {MAX_CARDS_PER_PLAYER}."; return; }

                _cardsPerPlayer = value;
                OnPropertyChanged(nameof(CardsPerPlayer));
            }
        }

        public int NumberOfDecks
        {
            get => _numberOfDecks;
            set
            {
                if (value < MIN_NUMBER_OF_DECKS || value > MAX_NUMBER_OF_DECKS)
                { AlertMessage = $"Number of decks must be between {MIN_NUMBER_OF_DECKS} and {MAX_NUMBER_OF_DECKS}."; return; }

                _numberOfDecks = value;
                OnPropertyChanged(nameof(NumberOfDecks));
            }
        }

        public string NewPlayerName
        {
            get => _newPlayerName;
            set
            {
                if (value.Length > NAME_CHARACTER_LIMIT)
                { AlertMessage = $"Player name must be less than {NAME_CHARACTER_LIMIT} characters."; return; }

                AlertMessage = string.Empty;

                _newPlayerName = value;
                OnPropertyChanged(nameof(NewPlayerName));
            }
        }

        public ObservableCollection<string> PlayerNames { get; }
        public ObservableCollection<Player> Players { get; private set; } = [];
        public Leaderboard Leaderboard { get; private set; } = new([]);

        private void ShuffleAndDeal()
        {
            if (PlayerNames.Count < MIN_PLAYERS_PER_GAME)
            { AlertMessage = $"Please add at least {MIN_PLAYERS_PER_GAME} players."; return; }
            else if (PlayerNames.Count > MAX_PLAYERS_PER_GAME)
            { AlertMessage = $"Cannot add more than {MAX_PLAYERS_PER_GAME} players."; return; }

            _cardGame.InitGame(new(PlayerNames), CardsPerPlayer, NumberOfDecks);
            Players = new ObservableCollection<Player>(_cardGame.Players);
            Leaderboard = _cardGame.Leaderboard;

            AlertMessage = string.Empty;

            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(Leaderboard));
        }

        private void AddPlayer()
        {
            if (string.IsNullOrWhiteSpace(NewPlayerName))
            { AlertMessage = "Player name cannot be empty."; return; }
            else if (PlayerNames.Contains(NewPlayerName))
            { AlertMessage = "Player name already exists."; return; }

            AlertMessage = string.Empty;

            PlayerNames.Add(NewPlayerName);
            NewPlayerName = string.Empty;

            OnPropertyChanged(nameof(PlayerNames));
            OnPropertyChanged(nameof(Leaderboard));

        }

        private void RemoveLeaderboardPlayer(Player player)
        {
            if (!PlayerNames.Contains(player.Name))
            { AlertMessage = "Player does not exist."; return; }

            AlertMessage = $"Removed {player.Name} from next shuffle.";


            var isLeaderboardPlayerRemoved = PlayerNames.Remove(player.Name);

            if (!isLeaderboardPlayerRemoved)
            { AlertMessage = "Unexpected error - player not removed."; }

            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(PlayerNames));
        }

        public string AlertMessage
        {
            get => _alertMessage;
            set
            {
                _alertMessage = value;
                OnPropertyChanged(nameof(AlertMessage));
            }
        }

        private readonly DelegateCommand _shuffleAndDealCommand;
        private readonly DelegateCommand _addPlayerCommand;
        private readonly DelegateCommand<Player> _removeLeaderboardPlayerCommand;

        public ICommand ShuffleAndDealCommand => _shuffleAndDealCommand;
        public ICommand AddPlayerCommand => _addPlayerCommand;
        public ICommand RemoveLeaderboardPlayerCommand => _removeLeaderboardPlayerCommand;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new(propertyName));
    }
}
