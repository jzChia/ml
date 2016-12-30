namespace MLearningDemo
{
    partial class FactorForm
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
            this.factorControl1 = new userControls.FactorControl();
            this.SuspendLayout();
            // 
            // factorControl1
            // 
            this.factorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.factorControl1.Location = new System.Drawing.Point(0, 0);
            this.factorControl1.Name = "factorControl1";
            this.factorControl1.Size = new System.Drawing.Size(743, 415);
            this.factorControl1.TabIndex = 0;
            this.factorControl1.factorRowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.factorControl1_factorRowCellClick);
            this.factorControl1.addFactorClick += new System.EventHandler(this.factorControl1_addFactorClick);
            // 
            // FactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 415);
            this.Controls.Add(this.factorControl1);
            this.Name = "FactorForm";
            this.Text = "控矿要素";
            this.ResumeLayout(false);

        }

        #endregion

        private userControls.FactorControl factorControl1;
    }
}