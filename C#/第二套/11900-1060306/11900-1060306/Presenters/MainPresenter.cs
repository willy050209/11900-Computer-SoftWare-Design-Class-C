namespace IdCardChecker.Presenters;

using IdCardChecker.Views;
using IdCardChecker.Services;
using IdCardChecker.Models;

public class MainPresenter
{
    private readonly IMainView _view;
    private readonly IIdCardValidatorService _validator;
    private readonly IDataService _dataService;

    public MainPresenter(IMainView view, IIdCardValidatorService validator, IDataService dataService)
    {
        _view = view;
        _validator = validator;
        _dataService = dataService;

        _view.LoadDataClicked += OnLoadDataClicked;
    }

    private async void OnLoadDataClicked(object? sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new()
        {
            Filter = "SM/T01 Files (*.SM;*.T01)|*.SM;*.T01|All Files (*.*)|*.*",
            Title = "Select ID Card Data File"
        };

        if (openFileDialog.ShowDialog((IWin32Window)_view) == DialogResult.OK)
        {
            var records = await _dataService.ReadRecordsAsync(openFileDialog.FileName);
            
            var validatedRecords = records
                .Select(r => _validator.Validate(r))
                .OrderBy(r => r.Id)
                .ToList();

            _view.ShowRecords(validatedRecords);
        }
    }
}
