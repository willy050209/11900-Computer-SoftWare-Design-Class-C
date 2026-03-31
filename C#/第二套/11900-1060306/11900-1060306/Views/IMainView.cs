namespace IdCardChecker.Views;

using IdCardChecker.Models;

public interface IMainView
{
    string CandidateName { get; set; }
    string CandidateNumber { get; set; }
    string DeskNumber { get; set; }
    string TestDate { get; set; }

    void ShowRecords(IEnumerable<IdCardRecord> records);
    event EventHandler LoadDataClicked;
}
