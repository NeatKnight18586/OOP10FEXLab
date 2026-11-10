using System.Drawing;
using System.Windows.Forms;

namespace HeavensKarinderya.UI;

partial class QueueMonitorForm
{
    private System.ComponentModel.IContainer? components;
    private TableLayoutPanel _root = null!;
    private TableLayoutPanel _staffControls = null!;
    private TableLayoutPanel _preparingNumbers = null!;
    private TableLayoutPanel _servingNumbers = null!;
    private Label[] _preparingSlots = null!;
    private Label[] _servingSlots = null!;
    private Label preparingNumber1 = null!;
    private Label preparingNumber2 = null!;
    private Label preparingNumber3 = null!;
    private Label preparingNumber4 = null!;
    private Label preparingNumber5 = null!;
    private Label preparingNumber6 = null!;
    private Label preparingNumber7 = null!;
    private Label preparingNumber8 = null!;
    private Label servingNumber1 = null!;
    private Label servingNumber2 = null!;
    private Label _brand = null!;
    private Label _clock = null!;
    private Label _preparingTitle = null!;
    private Label _servingTitle = null!;
    private Label _pageInfo = null!;
    private Label _servingPageInfo = null!;
    private Label _footer = null!;
    private Label _staffStatus = null!;
    private Button _callNextButton = null!;
    private Button _completeButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _root = new TableLayoutPanel();
        headerPanel = new TableLayoutPanel();
        _brand = new Label();
        _clock = new Label();
        bodyPanel = new TableLayoutPanel();
        preparingPanel = new TableLayoutPanel();
        preparingHeading = new TableLayoutPanel();
        _preparingTitle = new Label();
        _pageInfo = new Label();
        preparingRule = new Panel();
        _preparingNumbers = new TableLayoutPanel();
        servingPanel = new TableLayoutPanel();
        servingHeading = new TableLayoutPanel();
        _servingTitle = new Label();
        _servingPageInfo = new Label();
        servingRule = new Panel();
        _servingNumbers = new TableLayoutPanel();
        _staffControls = new TableLayoutPanel();
        _staffStatus = new Label();
        _callNextButton = new Button();
        _completeButton = new Button();
        footerPanel = new Panel();
        _footer = new Label();

        preparingNumber1 = new Label();
        preparingNumber2 = new Label();
        preparingNumber3 = new Label();
        preparingNumber4 = new Label();
        preparingNumber5 = new Label();
        preparingNumber6 = new Label();
        preparingNumber7 = new Label();
        preparingNumber8 = new Label();
        servingNumber1 = new Label();
        servingNumber2 = new Label();
        _root.SuspendLayout();
        headerPanel.SuspendLayout();
        bodyPanel.SuspendLayout();
        preparingPanel.SuspendLayout();
        preparingHeading.SuspendLayout();
        servingPanel.SuspendLayout();
        servingHeading.SuspendLayout();
        _staffControls.SuspendLayout();
        footerPanel.SuspendLayout();
        SuspendLayout();
        //
        // _root
        //
        _root.ColumnCount = 1;
        _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _root.Controls.Add(headerPanel, 0, 0);
        _root.Controls.Add(bodyPanel, 0, 1);
        _root.Controls.Add(_staffControls, 0, 2);
        _root.Controls.Add(footerPanel, 0, 3);
        _root.Dock = DockStyle.Fill;
        _root.Margin = Padding.Empty;
        _root.Name = "mainLayout";
        _root.RowCount = 4;
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        //
        // headerPanel
        //
        headerPanel.BackColor = Color.FromArgb(106, 32, 58);
        headerPanel.ColumnCount = 2;
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        headerPanel.Controls.Add(_brand, 0, 0);
        headerPanel.Controls.Add(_clock, 1, 0);
        headerPanel.Dock = DockStyle.Fill;
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(20, 0, 20, 0);
        //
        // _brand
        //
        _brand.Dock = DockStyle.Fill;
        _brand.Font = new Font("Segoe UI", 29F, FontStyle.Bold);
        _brand.ForeColor = Color.FromArgb(255, 231, 208);
        _brand.Name = "brandLabel";
        _brand.Text = "HEAVEN'S KARINDERYA";
        _brand.TextAlign = ContentAlignment.MiddleLeft;
        //
        // _clock
        //
        _clock.Dock = DockStyle.Fill;
        _clock.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        _clock.ForeColor = Color.FromArgb(255, 231, 208);
        _clock.Name = "clockLabel";
        _clock.Text = "10:14 AM";
        _clock.TextAlign = ContentAlignment.MiddleRight;
        //
        // bodyPanel
        //
        bodyPanel.BackColor = Color.FromArgb(13, 13, 13);
        bodyPanel.ColumnCount = 2;
        bodyPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        bodyPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        bodyPanel.Controls.Add(preparingPanel, 0, 0);
        bodyPanel.Controls.Add(servingPanel, 1, 0);
        bodyPanel.Dock = DockStyle.Fill;
        bodyPanel.Margin = Padding.Empty;
        bodyPanel.Name = "bodyPanel";
        bodyPanel.Padding = new Padding(0, 6, 0, 0);
        //
        // preparingPanel
        //
        preparingPanel.BackColor = Color.FromArgb(248, 236, 213);
        preparingPanel.ColumnCount = 1;
        preparingPanel.Controls.Add(preparingHeading, 0, 0);
        preparingPanel.Controls.Add(preparingRule, 0, 1);
        preparingPanel.Controls.Add(_preparingNumbers, 0, 2);
        preparingPanel.Dock = DockStyle.Fill;
        preparingPanel.Margin = Padding.Empty;
        preparingPanel.Name = "preparingPanel";
        preparingPanel.Padding = new Padding(20, 12, 20, 12);
        preparingPanel.RowCount = 3;
        preparingPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        preparingPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 7F));
        preparingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // preparingHeading
        //
        preparingHeading.ColumnCount = 2;
        preparingHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
        preparingHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        preparingHeading.Controls.Add(_preparingTitle, 0, 0);
        preparingHeading.Controls.Add(_pageInfo, 1, 0);
        preparingHeading.Dock = DockStyle.Fill;
        preparingHeading.Name = "preparingHeading";
        //
        // _preparingTitle
        //
        _preparingTitle.Dock = DockStyle.Fill;
        _preparingTitle.Font = new Font("Segoe UI", 29F, FontStyle.Bold);
        _preparingTitle.ForeColor = Color.FromArgb(13, 13, 13);
        _preparingTitle.Name = "preparingTitle";
        _preparingTitle.Text = "PREPARING";
        _preparingTitle.TextAlign = ContentAlignment.MiddleLeft;
        //
        // _pageInfo
        //
        _pageInfo.Dock = DockStyle.Fill;
        _pageInfo.Font = new Font("Segoe UI", 10F);
        _pageInfo.Name = "preparingPageInfo";
        _pageInfo.TextAlign = ContentAlignment.BottomRight;
        //
        // preparingRule
        //
        preparingRule.BackColor = Color.FromArgb(13, 13, 13);
        preparingRule.Dock = DockStyle.Fill;
        preparingRule.Margin = Padding.Empty;
        preparingRule.Name = "preparingRule";
        //
        // _preparingNumbers
        //
        _preparingNumbers.BackColor = Color.FromArgb(248, 236, 213);
        _preparingNumbers.Dock = DockStyle.Fill;
        _preparingNumbers.Name = "preparingNumbers";
        _preparingNumbers.Padding = new Padding(0, 12, 0, 0);
        //
        // servingPanel
        //
        servingPanel.BackColor = Color.FromArgb(255, 129, 12);
        servingPanel.ColumnCount = 1;
        servingPanel.Controls.Add(servingHeading, 0, 0);
        servingPanel.Controls.Add(servingRule, 0, 1);
        servingPanel.Controls.Add(_servingNumbers, 0, 2);
        servingPanel.Dock = DockStyle.Fill;
        servingPanel.Margin = Padding.Empty;
        servingPanel.Name = "servingPanel";
        servingPanel.Padding = new Padding(20, 12, 20, 12);
        servingPanel.RowCount = 3;
        servingPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        servingPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 7F));
        servingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // servingHeading
        //
        servingHeading.ColumnCount = 2;
        servingHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
        servingHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        servingHeading.Controls.Add(_servingTitle, 0, 0);
        servingHeading.Controls.Add(_servingPageInfo, 1, 0);
        servingHeading.Dock = DockStyle.Fill;
        servingHeading.Name = "servingHeading";
        //
        // _servingTitle
        //
        _servingTitle.Dock = DockStyle.Fill;
        _servingTitle.Font = new Font("Segoe UI", 29F, FontStyle.Bold);
        _servingTitle.ForeColor = Color.FromArgb(13, 13, 13);
        _servingTitle.Name = "servingTitle";
        _servingTitle.Text = "NOW SERVING";
        _servingTitle.TextAlign = ContentAlignment.MiddleLeft;
        //
        // _servingPageInfo
        //
        _servingPageInfo.Dock = DockStyle.Fill;
        _servingPageInfo.Font = new Font("Segoe UI", 10F);
        _servingPageInfo.Name = "servingPageInfo";
        _servingPageInfo.TextAlign = ContentAlignment.BottomRight;
        //
        // servingRule
        //
        servingRule.BackColor = Color.FromArgb(13, 13, 13);
        servingRule.Dock = DockStyle.Fill;
        servingRule.Margin = Padding.Empty;
        servingRule.Name = "servingRule";
        //
        // _servingNumbers
        //
        _servingNumbers.BackColor = Color.FromArgb(255, 129, 12);
        _servingNumbers.Dock = DockStyle.Fill;
        _servingNumbers.Name = "servingNumbers";
        _servingNumbers.Padding = new Padding(0, 12, 0, 0);
        //
        // _staffControls
        //
        _staffControls.BackColor = Color.FromArgb(45, 45, 45);
        _staffControls.ColumnCount = 3;
        _staffControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        _staffControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        _staffControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        _staffControls.Controls.Add(_staffStatus, 0, 0);
        _staffControls.Controls.Add(_callNextButton, 1, 0);
        _staffControls.Controls.Add(_completeButton, 2, 0);
        _staffControls.Dock = DockStyle.Fill;
        _staffControls.Name = "staffControls";
        _staffControls.Padding = new Padding(12, 8, 12, 8);
        _staffControls.Visible = false;
        //
        // _staffStatus
        //
        _staffStatus.Dock = DockStyle.Fill;
        _staffStatus.Font = new Font("Segoe UI", 11F);
        _staffStatus.ForeColor = Color.White;
        _staffStatus.Name = "staffStatus";
        _staffStatus.Text = "F9 to hide staff controls";
        _staffStatus.TextAlign = ContentAlignment.MiddleLeft;
        //
        // _callNextButton
        //
        _callNextButton.BackColor = Color.FromArgb(54, 121, 73);
        _callNextButton.Cursor = Cursors.Hand;
        _callNextButton.Dock = DockStyle.Fill;
        _callNextButton.FlatAppearance.BorderSize = 0;
        _callNextButton.FlatStyle = FlatStyle.Flat;
        _callNextButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        _callNextButton.ForeColor = Color.White;
        _callNextButton.Margin = new Padding(5, 0, 5, 0);
        _callNextButton.Name = "btnCallNext";
        _callNextButton.Text = "CALL NEXT";
        _callNextButton.UseVisualStyleBackColor = false;
        //
        // _completeButton
        //
        _completeButton.BackColor = Color.FromArgb(106, 32, 58);
        _completeButton.Cursor = Cursors.Hand;
        _completeButton.Dock = DockStyle.Fill;
        _completeButton.FlatAppearance.BorderSize = 0;
        _completeButton.FlatStyle = FlatStyle.Flat;
        _completeButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        _completeButton.ForeColor = Color.White;
        _completeButton.Margin = new Padding(5, 0, 5, 0);
        _completeButton.Name = "btnCompleteOldest";
        _completeButton.Text = "COMPLETE OLDEST";
        _completeButton.UseVisualStyleBackColor = false;
        //
        // footerPanel
        //
        footerPanel.BackColor = Color.FromArgb(13, 13, 13);
        footerPanel.Controls.Add(_footer);
        footerPanel.Dock = DockStyle.Fill;
        footerPanel.Name = "footerPanel";
        //
        // _footer
        //
        _footer.Dock = DockStyle.Fill;
        _footer.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        _footer.ForeColor = Color.FromArgb(255, 231, 208);
        _footer.Name = "footerLabel";
        _footer.Text = "Pakihintay ang inyong numero sa Now Serving.";
        _footer.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.ColumnCount = 2;
        _preparingNumbers.RowCount = 4;
        _preparingNumbers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _preparingNumbers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _preparingNumbers.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        _preparingNumbers.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        _preparingNumbers.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        _preparingNumbers.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        _servingNumbers.ColumnCount = 1;
        _servingNumbers.RowCount = 2;
        _servingNumbers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _servingNumbers.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        _servingNumbers.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        preparingNumber1.Name = "preparingNumber1";
        preparingNumber1.Dock = DockStyle.Fill;
        preparingNumber1.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber1.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber1.Text = "001";
        preparingNumber1.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber1, 0, 0);
        preparingNumber2.Name = "preparingNumber2";
        preparingNumber2.Dock = DockStyle.Fill;
        preparingNumber2.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber2.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber2.Text = "002";
        preparingNumber2.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber2, 0, 1);
        preparingNumber3.Name = "preparingNumber3";
        preparingNumber3.Dock = DockStyle.Fill;
        preparingNumber3.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber3.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber3.Text = "003";
        preparingNumber3.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber3, 0, 2);
        preparingNumber4.Name = "preparingNumber4";
        preparingNumber4.Dock = DockStyle.Fill;
        preparingNumber4.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber4.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber4.Text = "004";
        preparingNumber4.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber4, 0, 3);
        preparingNumber5.Name = "preparingNumber5";
        preparingNumber5.Dock = DockStyle.Fill;
        preparingNumber5.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber5.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber5.Text = "005";
        preparingNumber5.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber5, 1, 0);
        preparingNumber6.Name = "preparingNumber6";
        preparingNumber6.Dock = DockStyle.Fill;
        preparingNumber6.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber6.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber6.Text = "006";
        preparingNumber6.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber6, 1, 1);
        preparingNumber7.Name = "preparingNumber7";
        preparingNumber7.Dock = DockStyle.Fill;
        preparingNumber7.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber7.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber7.Text = "007";
        preparingNumber7.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber7, 1, 2);
        preparingNumber8.Name = "preparingNumber8";
        preparingNumber8.Dock = DockStyle.Fill;
        preparingNumber8.Font = new Font("Segoe UI", 60F, FontStyle.Bold);
        preparingNumber8.ForeColor = Color.FromArgb(13, 13, 13);
        preparingNumber8.Text = "008";
        preparingNumber8.TextAlign = ContentAlignment.MiddleCenter;
        _preparingNumbers.Controls.Add(preparingNumber8, 1, 3);
        servingNumber1.Name = "servingNumber1";
        servingNumber1.Dock = DockStyle.Fill;
        servingNumber1.Font = new Font("Segoe UI", 95F, FontStyle.Bold);
        servingNumber1.ForeColor = Color.FromArgb(13, 13, 13);
        servingNumber1.Text = "001";
        servingNumber1.TextAlign = ContentAlignment.MiddleCenter;
        _servingNumbers.Controls.Add(servingNumber1, 0, 0);
        servingNumber2.Name = "servingNumber2";
        servingNumber2.Dock = DockStyle.Fill;
        servingNumber2.Font = new Font("Segoe UI", 95F, FontStyle.Bold);
        servingNumber2.ForeColor = Color.FromArgb(13, 13, 13);
        servingNumber2.Text = "002";
        servingNumber2.TextAlign = ContentAlignment.MiddleCenter;
        _servingNumbers.Controls.Add(servingNumber2, 0, 1);
        //
        // QueueMonitorForm
        //
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 236, 213);
        ClientSize = new Size(1366, 768);
        Controls.Add(_root);
        KeyPreview = true;
        MinimumSize = new Size(950, 580);
        Name = "QueueMonitorForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Heaven's Karinderya - Queue Monitor";
        WindowState = FormWindowState.Maximized;
        _root.ResumeLayout(false);
        headerPanel.ResumeLayout(false);
        bodyPanel.ResumeLayout(false);
        preparingPanel.ResumeLayout(false);
        preparingHeading.ResumeLayout(false);
        servingPanel.ResumeLayout(false);
        servingHeading.ResumeLayout(false);
        _staffControls.ResumeLayout(false);
        footerPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private TableLayoutPanel headerPanel = null!;
    private TableLayoutPanel bodyPanel = null!;
    private TableLayoutPanel preparingPanel = null!;
    private TableLayoutPanel preparingHeading = null!;
    private Panel preparingRule = null!;
    private TableLayoutPanel servingPanel = null!;
    private TableLayoutPanel servingHeading = null!;
    private Panel servingRule = null!;
    private Panel footerPanel = null!;
}
