// filepath: Views/IMainView.cs
using TestLauncher.Models;

namespace TestLauncher.Views;

public interface IMainView
{
    // Task 1 Inputs
    string Task1CodePath { get; set; }
    string Task1UserPdfPath { get; set; }
    string Task1AnsPdfPath { get; set; }
    string CandidateName { get; set; }
    string CandidateTestNo { get; set; }
    string CandidateSeatNo { get; set; }
    string LoopType { get; set; }

    // Task 2 Inputs
    string Task06ExePath { get; set; }
    string Task06DataPath { get; set; }
    string Task07ExePath { get; set; }
    string Task07DataPath { get; set; }
    string Task08ExePath { get; set; }
    string Task08DataPath { get; set; }

    // UI Feedback
    void AppendLog(string text, System.Drawing.Color? color = null);
    void ClearLog();
    void SetBusy(bool busy);

    // Dialogs
    string? ShowFileOpenDialog(string filter);

    // Localization
    void UpdateLocalizedText(Func<string, string> getStr);
}
