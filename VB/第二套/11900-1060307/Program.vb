Imports System.Windows.Forms

Module Program
    <STAThread()>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim view As New Views.MainForm()
        Dim poker As New Services.PokerService()
        Dim data As New Services.DataService()
        
        Dim presenter As New Presenters.MainPresenter(view, poker, data)
        
        Application.Run(view)
    End Sub
End Module
