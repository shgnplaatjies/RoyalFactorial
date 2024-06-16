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
    public class MainViewModel(ICardGame cardGame) : INotifyPropertyChanged
    {
        private static readonly int CARDS_PER_PLAYER = 5;
        private static readonly int NUMBER_OF_DECKS = 2;
        private static readonly List<string> PLAYER_NAMES = new([
                "Trevor",
                "Michael",
                "Franklin",
                "Niko",
                "Luis",
                "Johnny",
            ]);

        public MainViewModel() : this(new CardGame()) { }

        private readonly ICardGame _cardGame = cardGame;
        public ObservableCollection<Player> Players { get; private set; } = [];
        public Leaderboard Leaderboard { get; private set; } = new([]);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ShuffleAndDeal()
        {
            _cardGame.InitGame(PLAYER_NAMES, CARDS_PER_PLAYER, NUMBER_OF_DECKS);

            Players = new ObservableCollection<Player>(_cardGame.Players);
            Leaderboard = _cardGame.Leaderboard;

            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(Leaderboard));
        }

        private DelegateCommand _shuffleAndDealCommand = null!;
        public ICommand ShuffleAndDealCommand =>
            _shuffleAndDealCommand ??= new(() => ShuffleAndDeal());
    }
}
