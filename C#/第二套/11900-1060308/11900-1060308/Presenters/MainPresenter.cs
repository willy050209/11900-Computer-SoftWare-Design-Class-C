namespace FractionArithmetic.Presenters;

using FractionArithmetic.Views;
using FractionArithmetic.Services;
using FractionArithmetic.Models;

public class MainPresenter
{
    private readonly IMainView _view;
    private readonly IDataService _dataService;
    private readonly IFractionService _fractionService;

    public MainPresenter(IMainView view, IDataService dataService, IFractionService fractionService)
    {
        _view = view;
        _dataService = dataService;
        _fractionService = fractionService;

        _view.LoadDataClicked += OnLoadDataClicked;
    }

    private async void OnLoadDataClicked(object? sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new()
        {
            Filter = "SM/T01 Files (*.SM;*.T01)|*.SM;*.T01|All Files (*.*)|*.*",
            Title = "Select Fraction Data File"
        };

        if (openFileDialog.ShowDialog((IWin32Window)_view) == DialogResult.OK)
        {
            var data = await _dataService.ReadDataAsync(openFileDialog.FileName);
            
            var results = data.Select(item => 
            {
                // Note: The input fractions themselves might need simplification?
                // Requirements say "運算結果如果仍為一分數，則必須將之簡化(約分)"
                // But sample shows 3/2 * 6/9 = 1. Let's simplify inputs too just in case.
                var f1 = _fractionService.Simplify(item.f1.Numerator, item.f1.Denominator);
                var f2 = _fractionService.Simplify(item.f2.Numerator, item.f2.Denominator);
                var answer = _fractionService.Calculate(f1, item.op, f2);
                return new CalculationResult(f1, item.op, f2, answer);
            }).ToList();

            _view.ShowResults(results);
        }
    }
}
