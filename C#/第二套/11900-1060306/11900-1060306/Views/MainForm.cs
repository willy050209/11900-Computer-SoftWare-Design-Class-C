namespace IdCardChecker.Views;

using IdCardChecker.Models;
using System.Windows.Forms;
using System.ComponentModel;

public partial class MainForm : Form, IMainView
{
    public MainForm()
    {
        InitializeComponent();
        
        // 預填應檢人資料
        this.txtName.Text = "陳宇威";
        this.txtNumber.Text = "112590005";
        this.txtDesk.Text = "005";
        this.txtDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

        // 視圖載入後立即觸發讀取
        this.Load += (s, e) => LoadDataClicked?.Invoke(this, EventArgs.Empty);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CandidateName { get => txtName.Text; set => txtName.Text = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CandidateNumber { get => txtNumber.Text; set => txtNumber.Text = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string DeskNumber { get => txtDesk.Text; set => txtDesk.Text = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string TestDate { get => txtDate.Text; set => txtDate.Text = value; }

    public event EventHandler? LoadDataClicked;

    public void ShowRecords(IEnumerable<IdCardRecord> records)
    {
        dgvResults.Rows.Clear();
        foreach (var record in records)
        {
            dgvResults.Rows.Add(record.Id, record.Name, record.Sex, record.Error);
        }
    }
}
