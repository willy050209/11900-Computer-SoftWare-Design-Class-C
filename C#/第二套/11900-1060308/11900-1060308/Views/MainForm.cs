namespace FractionArithmetic.Views;

using FractionArithmetic.Models;
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

    public void ShowResults(IEnumerable<CalculationResult> results)
    {
        dgvResults.Rows.Clear();
        foreach (var res in results)
        {
            dgvResults.Rows.Add(res.Value1.ToString(), res.Op, res.Value2.ToString(), res.Answer.ToString());
        }
    }
}
