namespace MLearningDemo
{
    partial class SelectFactorForm
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
            this.selectFactorControl21 = new userControls.SelectFactorControl2();
            this.SuspendLayout();
            // 
            // selectFactorControl21
            // 
            this.selectFactorControl21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectFactorControl21.Location = new System.Drawing.Point(0, 0);
            this.selectFactorControl21.Name = "selectFactorControl21";
            this.selectFactorControl21.Size = new System.Drawing.Size(817, 403);
            this.selectFactorControl21.TabIndex = 0;
            this.selectFactorControl21.savebtnClick += new System.EventHandler(this.selectFactorControl21_savebtnClick);
            // 
            // SelectFactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 403);
            this.Controls.Add(this.selectFactorControl21);
            this.Name = "SelectFactorForm";
            this.Text = "选择控矿要素";
            this.ResumeLayout(false);

        }

        #endregion

        private userControls.SelectFactorControl2 selectFactorControl21;
    }
}