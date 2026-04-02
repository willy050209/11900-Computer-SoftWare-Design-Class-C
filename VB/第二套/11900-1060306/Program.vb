Imports System.Windows.Forms

Module Program
    <STAThread()>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim view As New Views.MainForm()
        Dim validator As New Services.IdCardValidatorService()
        Dim dataService As New Services.DataService()
        
        Dim presenter As New Presenters.MainPresenter(view, validator, dataService)
        
        Application.Run(view)
    End Sub
End Module
