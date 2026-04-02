namespace PokerGame.Presenters;

using PokerGame.Views;
using PokerGame.Services;
using PokerGame.Models;

public class MainPresenter
{
    private readonly IMainView _view;
    private readonly IDataService _dataService;

    public MainPresenter(IMainView view, IDataService dataService)
    {
        _view = view;
        _dataService = dataService;

        _view.LoadDataClicked += OnLoadDataClicked;
    }

    private async void OnLoadDataClicked(object? sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new()
        {
            Filter = "SM/T01 Files (*.SM;*.T01)|*.SM;*.T01|All Files (*.*)|*.*",
            //Title = "Select Poker Game Data File"
        };

        if (openFileDialog.ShowDialog((IWin32Window)_view) == DialogResult.OK)
        {
            var (rounds, randomNumbers) = await _dataService.ReadDataAsync(openFileDialog.FileName);
            
            var results = new List<RoundResult>();
            var usedCards = new HashSet<int>();
            int randIdx = 0;

            for (int r = 1; r <= rounds; r++)
            {
                // Deal to Player
                int playerCardNum = -1;
                while (randIdx < randomNumbers.Count)
                {
                    playerCardNum = (int)(randomNumbers[randIdx++] * 52);
                    if (!usedCards.Contains(playerCardNum))
                    {
                        usedCards.Add(playerCardNum);
                        break;
                    }
                    playerCardNum = -1;
                }

                // Deal to Banker
                int bankerCardNum = -1;
                while (randIdx < randomNumbers.Count)
                {
                    bankerCardNum = (int)(randomNumbers[randIdx++] * 52);
                    if (!usedCards.Contains(bankerCardNum))
                    {
                        usedCards.Add(bankerCardNum);
                        break;
                    }
                    bankerCardNum = -1;
                }

                if (playerCardNum != -1 && bankerCardNum != -1)
                {
                    var playerCard = new Card(playerCardNum);
                    var bankerCard = new Card(bankerCardNum);
                    
                    string res = "平手";
                    if (playerCard.PowerValue > bankerCard.PowerValue) res = "玩家贏";
                    else if (playerCard.PowerValue < bankerCard.PowerValue) res = "莊家贏";

                    results.Add(new RoundResult(r, playerCard, bankerCard, res));
                }
                else
                {
                    // Not enough random numbers
                    break;
                }
            }

            _view.ShowResults(results);
        }
    }
}
