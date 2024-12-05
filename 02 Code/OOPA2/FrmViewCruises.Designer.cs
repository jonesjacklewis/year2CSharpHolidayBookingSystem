namespace OOPA2
{
    partial class FrmViewCruises
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
            this.dgvCruises = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCruises)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCruises
            // 
            this.dgvCruises.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCruises.Location = new System.Drawing.Point(4, 59);
            this.dgvCruises.Name = "dgvCruises";
            this.dgvCruises.RowTemplate.Height = 25;
            this.dgvCruises.Size = new System.Drawing.Size(920, 355);
            this.dgvCruises.TabIndex = 18;
            this.dgvCruises.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCruises_CellContentClick);
            // 
            // FrmViewCruises
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(18F, 45F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 461);
            this.Controls.Add(this.dgvCruises);
            this.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MaximumSize = new System.Drawing.Size(950, 500);
            this.MinimumSize = new System.Drawing.Size(950, 500);
            this.Name = "FrmViewCruises";
            this.Text = "View Cruises";
            this.Load += new System.EventHandler(this.FrmViewCruises_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCruises)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dgvCruises;
    }
}