namespace OOPA2
{
    partial class FrmCreateBooking
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblTelephone = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblCruise = new System.Windows.Forms.Label();
            this.lblRoom = new System.Windows.Forms.Label();
            this.lblSingleOccupancy = new System.Windows.Forms.Label();
            this.lblPriceLabel = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblBypassCardValidation = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.cmbCruise = new System.Windows.Forms.ComboBox();
            this.cmbRoom = new System.Windows.Forms.ComboBox();
            this.cbSingleOccupancy = new System.Windows.Forms.CheckBox();
            this.lblPriceValue = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.cbBypassCardValidation = new System.Windows.Forms.CheckBox();
            this.btnBook = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(65, 25);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(164, 45);
            this.lblFullName.TabIndex = 0;
            this.lblFullName.Text = "Full Name";
            // 
            // lblTelephone
            // 
            this.lblTelephone.AutoSize = true;
            this.lblTelephone.Location = new System.Drawing.Point(65, 100);
            this.lblTelephone.Name = "lblTelephone";
            this.lblTelephone.Size = new System.Drawing.Size(167, 45);
            this.lblTelephone.TabIndex = 1;
            this.lblTelephone.Text = "Telephone";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(65, 175);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(96, 45);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // lblCruise
            // 
            this.lblCruise.AutoSize = true;
            this.lblCruise.Location = new System.Drawing.Point(65, 250);
            this.lblCruise.Name = "lblCruise";
            this.lblCruise.Size = new System.Drawing.Size(108, 45);
            this.lblCruise.TabIndex = 3;
            this.lblCruise.Text = "Cruise";
            // 
            // lblRoom
            // 
            this.lblRoom.AutoSize = true;
            this.lblRoom.Location = new System.Drawing.Point(65, 325);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(104, 45);
            this.lblRoom.TabIndex = 4;
            this.lblRoom.Text = "Room";
            // 
            // lblSingleOccupancy
            // 
            this.lblSingleOccupancy.AutoSize = true;
            this.lblSingleOccupancy.Location = new System.Drawing.Point(65, 400);
            this.lblSingleOccupancy.Name = "lblSingleOccupancy";
            this.lblSingleOccupancy.Size = new System.Drawing.Size(285, 45);
            this.lblSingleOccupancy.TabIndex = 5;
            this.lblSingleOccupancy.Text = "Single Occupancy?";
            // 
            // lblPriceLabel
            // 
            this.lblPriceLabel.AutoSize = true;
            this.lblPriceLabel.Location = new System.Drawing.Point(65, 475);
            this.lblPriceLabel.Name = "lblPriceLabel";
            this.lblPriceLabel.Size = new System.Drawing.Size(89, 45);
            this.lblPriceLabel.TabIndex = 6;
            this.lblPriceLabel.Text = "Price";
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Location = new System.Drawing.Point(65, 550);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(212, 45);
            this.lblCardNumber.TabIndex = 7;
            this.lblCardNumber.Text = "Card Number";
            // 
            // lblBypassCardValidation
            // 
            this.lblBypassCardValidation.AutoSize = true;
            this.lblBypassCardValidation.Location = new System.Drawing.Point(65, 625);
            this.lblBypassCardValidation.Name = "lblBypassCardValidation";
            this.lblBypassCardValidation.Size = new System.Drawing.Size(341, 45);
            this.lblBypassCardValidation.TabIndex = 8;
            this.lblBypassCardValidation.Text = "Bypass Card Validation";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(65, 700);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 75);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(450, 25);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(365, 50);
            this.txtFullName.TabIndex = 12;
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(450, 100);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(365, 50);
            this.txtTelephone.TabIndex = 13;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(450, 175);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(365, 50);
            this.txtEmail.TabIndex = 14;
            // 
            // cmbCruise
            // 
            this.cmbCruise.FormattingEnabled = true;
            this.cmbCruise.Location = new System.Drawing.Point(450, 250);
            this.cmbCruise.Name = "cmbCruise";
            this.cmbCruise.Size = new System.Drawing.Size(365, 53);
            this.cmbCruise.TabIndex = 15;
            this.cmbCruise.SelectedIndexChanged += new System.EventHandler(this.cmbCruise_SelectedIndexChanged);
            // 
            // cmbRoom
            // 
            this.cmbRoom.FormattingEnabled = true;
            this.cmbRoom.Location = new System.Drawing.Point(450, 325);
            this.cmbRoom.Name = "cmbRoom";
            this.cmbRoom.Size = new System.Drawing.Size(365, 53);
            this.cmbRoom.TabIndex = 16;
            this.cmbRoom.SelectedIndexChanged += new System.EventHandler(this.cmbRoom_SelectedIndexChanged);
            // 
            // cbSingleOccupancy
            // 
            this.cbSingleOccupancy.AutoSize = true;
            this.cbSingleOccupancy.Location = new System.Drawing.Point(450, 400);
            this.cbSingleOccupancy.Name = "cbSingleOccupancy";
            this.cbSingleOccupancy.Size = new System.Drawing.Size(189, 49);
            this.cbSingleOccupancy.TabIndex = 17;
            this.cbSingleOccupancy.Text = "Confirmed";
            this.cbSingleOccupancy.UseVisualStyleBackColor = true;
            this.cbSingleOccupancy.CheckedChanged += new System.EventHandler(this.cbSingleOccupancy_CheckedChanged);
            // 
            // lblPriceValue
            // 
            this.lblPriceValue.AutoSize = true;
            this.lblPriceValue.Location = new System.Drawing.Point(450, 475);
            this.lblPriceValue.Name = "lblPriceValue";
            this.lblPriceValue.Size = new System.Drawing.Size(135, 45);
            this.lblPriceValue.TabIndex = 18;
            this.lblPriceValue.Text = "£ {Price}";
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Location = new System.Drawing.Point(450, 550);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(365, 50);
            this.txtCardNumber.TabIndex = 19;
            // 
            // cbBypassCardValidation
            // 
            this.cbBypassCardValidation.AutoSize = true;
            this.cbBypassCardValidation.Location = new System.Drawing.Point(450, 625);
            this.cbBypassCardValidation.Name = "cbBypassCardValidation";
            this.cbBypassCardValidation.Size = new System.Drawing.Size(189, 49);
            this.cbBypassCardValidation.TabIndex = 20;
            this.cbBypassCardValidation.Text = "Confirmed";
            this.cbBypassCardValidation.UseVisualStyleBackColor = true;
            // 
            // btnBook
            // 
            this.btnBook.Location = new System.Drawing.Point(685, 700);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(150, 75);
            this.btnBook.TabIndex = 21;
            this.btnBook.Text = "Book";
            this.btnBook.UseVisualStyleBackColor = true;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // FrmCreateBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(18F, 45F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 861);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.cbBypassCardValidation);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblPriceValue);
            this.Controls.Add(this.cbSingleOccupancy);
            this.Controls.Add(this.cmbRoom);
            this.Controls.Add(this.cmbCruise);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtTelephone);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblBypassCardValidation);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.lblPriceLabel);
            this.Controls.Add(this.lblSingleOccupancy);
            this.Controls.Add(this.lblRoom);
            this.Controls.Add(this.lblCruise);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblTelephone);
            this.Controls.Add(this.lblFullName);
            this.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MaximumSize = new System.Drawing.Size(900, 900);
            this.MinimumSize = new System.Drawing.Size(900, 900);
            this.Name = "FrmCreateBooking";
            this.Text = "Create Booking";
            this.Load += new System.EventHandler(this.FrmCreateBooking_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblFullName;
        private Label lblTelephone;
        private Label lblEmail;
        private Label lblCruise;
        private Label lblRoom;
        private Label lblSingleOccupancy;
        private Label lblPriceLabel;
        private Label lblCardNumber;
        private Label lblBypassCardValidation;
        private Button btnBack;
        private TextBox txtFullName;
        private TextBox txtTelephone;
        private TextBox txtEmail;
        private ComboBox cmbCruise;
        private ComboBox cmbRoom;
        private CheckBox cbSingleOccupancy;
        private Label lblPriceValue;
        private TextBox txtCardNumber;
        private CheckBox cbBypassCardValidation;
        private Button btnBook;
    }
}