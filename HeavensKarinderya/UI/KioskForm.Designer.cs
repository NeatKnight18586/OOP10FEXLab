using System.Drawing;
using System.Windows.Forms;

namespace HeavensKarinderya.UI;

partial class KioskForm
{
    private System.ComponentModel.IContainer? components;
    private FlowLayoutPanel _sidebar = null!;
    private FlowLayoutPanel _meals = null!;
    private FlowLayoutPanel _cartLines = null!;
    private ComboBox _cboMeals = null!;
    private Button _addOrder = null!;
    private Button _placeOrder = null!;
    private Label _total = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            foreach (var photo in _photos.Values) photo?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        headerLabel = new Label();
        cartPanel = new TableLayoutPanel();
        cartTitleLabel = new Label();
        selectPanel = new TableLayoutPanel();
        selectedItemLabel = new Label();
        _cboMeals = new ComboBox();
        _addOrder = new Button();
        _cartLines = new FlowLayoutPanel();
        _total = new Label();
        _placeOrder = new Button();
        _sidebar = new FlowLayoutPanel();
        _meals = new FlowLayoutPanel();
        cartPanel.SuspendLayout();
        selectPanel.SuspendLayout();
        SuspendLayout();
        //
        // headerLabel
        //
        headerLabel.BackColor = Color.FromArgb(185, 56, 47);
        headerLabel.Dock = DockStyle.Top;
        headerLabel.Font = new Font("Georgia", 28F, FontStyle.Bold);
        headerLabel.ForeColor = Color.FromArgb(212, 160, 23);
        headerLabel.Height = 80;
        headerLabel.Name = "headerLabel";
        headerLabel.Text = "Heaven's Karinderya";
        headerLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // cartPanel
        //
        cartPanel.BackColor = Color.White;
        cartPanel.ColumnCount = 1;
        cartPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        cartPanel.Controls.Add(cartTitleLabel, 0, 0);
        cartPanel.Controls.Add(selectPanel, 0, 1);
        cartPanel.Controls.Add(_cartLines, 0, 2);
        cartPanel.Controls.Add(_total, 0, 3);
        cartPanel.Controls.Add(_placeOrder, 0, 4);
        cartPanel.Dock = DockStyle.Right;
        cartPanel.Name = "cartPanel";
        cartPanel.Padding = new Padding(12);
        cartPanel.RowCount = 5;
        cartPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        cartPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        cartPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        cartPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
        cartPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        cartPanel.Width = 360;
        //
        // cartTitleLabel
        //
        cartTitleLabel.Dock = DockStyle.Fill;
        cartTitleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        cartTitleLabel.ForeColor = Color.FromArgb(36, 35, 31);
        cartTitleLabel.Name = "cartTitleLabel";
        cartTitleLabel.Text = "YOUR ORDER";
        cartTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // selectPanel
        //
        selectPanel.ColumnCount = 1;
        selectPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        selectPanel.Controls.Add(selectedItemLabel, 0, 0);
        selectPanel.Controls.Add(_cboMeals, 0, 1);
        selectPanel.Controls.Add(_addOrder, 0, 2);
        selectPanel.Dock = DockStyle.Fill;
        selectPanel.Name = "selectPanel";
        selectPanel.Padding = new Padding(0, 0, 0, 8);
        selectPanel.RowCount = 3;
        selectPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
        selectPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        selectPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
        //
        // selectedItemLabel
        //
        selectedItemLabel.Dock = DockStyle.Fill;
        selectedItemLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        selectedItemLabel.ForeColor = Color.FromArgb(36, 35, 31);
        selectedItemLabel.Name = "selectedItemLabel";
        selectedItemLabel.Text = "CURRENT SELECTED ITEM";
        //
        // _cboMeals
        //
        _cboMeals.AccessibleName = "Selected meal";
        _cboMeals.DisplayMember = "Name";
        _cboMeals.Dock = DockStyle.Fill;
        _cboMeals.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboMeals.Font = new Font("Segoe UI", 11F);
        _cboMeals.Name = "cboMeals";
        //
        // _addOrder
        //
        _addOrder.BackColor = Color.FromArgb(185, 56, 47);
        _addOrder.Cursor = Cursors.Hand;
        _addOrder.Dock = DockStyle.Fill;
        _addOrder.Enabled = false;
        _addOrder.FlatAppearance.BorderSize = 0;
        _addOrder.FlatStyle = FlatStyle.Flat;
        _addOrder.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
        _addOrder.ForeColor = Color.White;
        _addOrder.Name = "btnAddOrder";
        _addOrder.Text = "ADD ORDER";
        _addOrder.UseVisualStyleBackColor = false;
        //
        // _cartLines
        //
        _cartLines.AutoScroll = true;
        _cartLines.Dock = DockStyle.Fill;
        _cartLines.FlowDirection = FlowDirection.TopDown;
        _cartLines.Name = "orderListPanel";
        _cartLines.WrapContents = false;
        //
        // _total
        //
        _total.Dock = DockStyle.Fill;
        _total.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        _total.ForeColor = Color.FromArgb(185, 56, 47);
        _total.Name = "totalLabel";
        _total.Text = "Total: ₱0.00";
        _total.TextAlign = ContentAlignment.MiddleCenter;
        //
        // _placeOrder
        //
        _placeOrder.BackColor = Color.FromArgb(82, 121, 90);
        _placeOrder.Cursor = Cursors.Hand;
        _placeOrder.Dock = DockStyle.Fill;
        _placeOrder.FlatAppearance.BorderSize = 0;
        _placeOrder.FlatStyle = FlatStyle.Flat;
        _placeOrder.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
        _placeOrder.ForeColor = Color.White;
        _placeOrder.Name = "btnPlaceOrder";
        _placeOrder.Text = "PLACE ORDER";
        _placeOrder.UseVisualStyleBackColor = false;
        //
        // _sidebar
        //
        _sidebar.AutoScroll = true;
        _sidebar.BackColor = Color.White;
        _sidebar.Dock = DockStyle.Left;
        _sidebar.FlowDirection = FlowDirection.TopDown;
        _sidebar.Name = "categoryPanel";
        _sidebar.Padding = new Padding(10);
        _sidebar.Width = 210;
        _sidebar.WrapContents = false;
        //
        // _meals
        //
        _meals.AutoScroll = true;
        _meals.Dock = DockStyle.Fill;
        _meals.Name = "mealGridPanel";
        _meals.Padding = new Padding(12);
        //
        // KioskForm
        //
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(255, 248, 236);
        ClientSize = new Size(1200, 800);
        Controls.Add(_meals);
        Controls.Add(_sidebar);
        Controls.Add(cartPanel);
        Controls.Add(headerLabel);
        Name = "KioskForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Heaven's Karinderya - Kiosk";
        cartPanel.ResumeLayout(false);
        selectPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Label headerLabel = null!;
    private TableLayoutPanel cartPanel = null!;
    private Label cartTitleLabel = null!;
    private TableLayoutPanel selectPanel = null!;
    private Label selectedItemLabel = null!;
}
