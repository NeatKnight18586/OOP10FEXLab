#nullable disable
namespace HeavensKarinderya.UI
{
    partial class KioskForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flpMeals = new FlowLayoutPanel();
            flpCategories = new FlowLayoutPanel();
            pnlCart = new Panel();
            lstCart = new ListBox();
            pnlQty = new Panel();
            btnMinus = new Button();
            btnPlus = new Button();
            btnRemove = new Button();
            lblTotal = new Label();
            btnPlaceOrder = new Button();
            lblCartTitle = new Label();
            lblHeader = new Label();
            pnlCart.SuspendLayout();
            pnlQty.SuspendLayout();
            SuspendLayout();
            // 
            // flpMeals
            // 
            flpMeals.AutoScroll = true;
            flpMeals.Dock = DockStyle.Fill;
            flpMeals.Location = new Point(234, 107);
            flpMeals.Margin = new Padding(3, 4, 3, 4);
            flpMeals.Name = "flpMeals";
            flpMeals.Padding = new Padding(25);
            flpMeals.Size = new Size(561, 557);
            flpMeals.TabIndex = 0;
            // 
            // flpCategories
            // 
            flpCategories.BackColor = Color.White;
            flpCategories.Dock = DockStyle.Left;
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.Location = new Point(0, 107);
            flpCategories.Margin = new Padding(3, 4, 3, 4);
            flpCategories.Name = "flpCategories";
            flpCategories.Padding = new Padding(19);
            flpCategories.Size = new Size(234, 557);
            flpCategories.TabIndex = 1;
            flpCategories.WrapContents = false;
            // 
            // pnlCart
            // 
            pnlCart.BackColor = Color.White;
            pnlCart.Controls.Add(lstCart);
            pnlCart.Controls.Add(pnlQty);
            pnlCart.Controls.Add(lblTotal);
            pnlCart.Controls.Add(btnPlaceOrder);
            pnlCart.Controls.Add(lblCartTitle);
            pnlCart.Dock = DockStyle.Right;
            pnlCart.Location = new Point(795, 107);
            pnlCart.Margin = new Padding(3, 4, 3, 4);
            pnlCart.Name = "pnlCart";
            pnlCart.Padding = new Padding(19);
            pnlCart.Size = new Size(389, 557);
            pnlCart.TabIndex = 2;
            // 
            // lstCart
            // 
            lstCart.Dock = DockStyle.Fill;
            lstCart.Font = new Font("Segoe UI", 12F);
            lstCart.FormattingEnabled = true;
            lstCart.Location = new Point(19, 100);
            lstCart.Margin = new Padding(3, 4, 3, 4);
            lstCart.Name = "lstCart";
            lstCart.Size = new Size(351, 176);
            lstCart.TabIndex = 0;
            lstCart.SelectedIndexChanged += lstCart_SelectedIndexChanged;
            // 
            // pnlQty
            // 
            pnlQty.Controls.Add(btnMinus);
            pnlQty.Controls.Add(btnPlus);
            pnlQty.Controls.Add(btnRemove);
            pnlQty.Dock = DockStyle.Bottom;
            pnlQty.Location = new Point(19, 276);
            pnlQty.Margin = new Padding(3, 4, 3, 4);
            pnlQty.Name = "pnlQty";
            pnlQty.Size = new Size(351, 81);
            pnlQty.TabIndex = 1;
            // 
            // btnMinus
            // 
            btnMinus.Location = new Point(36, 15);
            btnMinus.Margin = new Padding(3, 4, 3, 4);
            btnMinus.Name = "btnMinus";
            btnMinus.Size = new Size(62, 50);
            btnMinus.TabIndex = 0;
            btnMinus.Text = "-";
            btnMinus.Click += btnMinus_Click;
            // 
            // btnPlus
            // 
            btnPlus.Location = new Point(121, 15);
            btnPlus.Margin = new Padding(3, 4, 3, 4);
            btnPlus.Name = "btnPlus";
            btnPlus.Size = new Size(62, 50);
            btnPlus.TabIndex = 1;
            btnPlus.Text = "+";
            btnPlus.Click += btnPlus_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(213, 15);
            btnRemove.Margin = new Padding(3, 4, 3, 4);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(125, 50);
            btnRemove.TabIndex = 2;
            btnRemove.Text = "Remove";
            btnRemove.Click += btnRemove_Click;
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(160, 28, 28);
            lblTotal.Location = new Point(19, 357);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(351, 94);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "Total: ₱0.00";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPlaceOrder
            // 
            btnPlaceOrder.BackColor = Color.FromArgb(80, 125, 90);
            btnPlaceOrder.Dock = DockStyle.Bottom;
            btnPlaceOrder.FlatStyle = FlatStyle.Flat;
            btnPlaceOrder.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPlaceOrder.ForeColor = Color.White;
            btnPlaceOrder.Location = new Point(19, 451);
            btnPlaceOrder.Margin = new Padding(3, 4, 3, 4);
            btnPlaceOrder.Name = "btnPlaceOrder";
            btnPlaceOrder.Size = new Size(351, 87);
            btnPlaceOrder.TabIndex = 3;
            btnPlaceOrder.Text = "PLACE ORDER";
            btnPlaceOrder.UseVisualStyleBackColor = false;
            btnPlaceOrder.Click += btnPlaceOrder_Click;
            // 
            // lblCartTitle
            // 
            lblCartTitle.Dock = DockStyle.Top;
            lblCartTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblCartTitle.Location = new Point(19, 19);
            lblCartTitle.Name = "lblCartTitle";
            lblCartTitle.Size = new Size(351, 81);
            lblCartTitle.TabIndex = 4;
            lblCartTitle.Text = "YOUR ORDER";
            lblCartTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            lblHeader.BackColor = Color.FromArgb(160, 28, 28);
            lblHeader.Dock = DockStyle.Top;
            lblHeader.Font = new Font("Georgia", 28F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(212, 160, 23);
            lblHeader.Location = new Point(0, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(1184, 107);
            lblHeader.TabIndex = 3;
            lblHeader.Text = "Heaven's Karinderya";
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // KioskForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 248, 232);
            ClientSize = new Size(1184, 664);
            Controls.Add(flpMeals);
            Controls.Add(flpCategories);
            Controls.Add(pnlCart);
            Controls.Add(lblHeader);
            Margin = new Padding(3, 4, 3, 4);
            Name = "KioskForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Heaven's Karinderya - Kiosk";
            Load += KioskForm_Load;
            pnlCart.ResumeLayout(false);
            pnlQty.ResumeLayout(false);
            ResumeLayout(false);
        }

        private FlowLayoutPanel flpMeals;
        private FlowLayoutPanel flpCategories;
        private Panel pnlCart;
        private ListBox lstCart;
        private Panel pnlQty;
        private Button btnMinus;
        private Button btnPlus;
        private Button btnRemove;
        private Label lblTotal;
        private Button btnPlaceOrder;
        private Label lblCartTitle;
        private Label lblHeader;
    }
}