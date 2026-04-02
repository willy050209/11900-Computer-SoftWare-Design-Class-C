Imports System.Windows.Forms

Module Program
    <STAThread()>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim view As New Views.MainForm()
        Dim fraction As New Services.FractionService()
        Dim data As New Services.DataService()
        
        Dim presenter As New Presenters.MainPresenter(view, fraction, data)
        
        Application.Run(view)
    End Sub
End Module
