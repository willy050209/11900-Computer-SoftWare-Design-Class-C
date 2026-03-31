namespace FractionArithmetic.Views;

using FractionArithmetic.Models;

public interface IMainView
{
    string CandidateName { get; set; }
    string CandidateNumber { get; set; }
    string DeskNumber { get; set; }
    string TestDate { get; set; }

    void ShowResults(IEnumerable<CalculationResult> results);
    event EventHandler LoadDataClicked;
}
