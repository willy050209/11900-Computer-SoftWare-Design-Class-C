namespace PokerGame.Views;

using PokerGame.Models;

public interface IMainView
{
    string CandidateName { get; set; }
    string CandidateNumber { get; set; }
    string DeskNumber { get; set; }
    string TestDate { get; set; }

    void ShowResults(IEnumerable<RoundResult> results);
    event EventHandler LoadDataClicked;
}
