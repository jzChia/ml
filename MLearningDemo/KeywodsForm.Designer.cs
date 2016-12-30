namespace MLearningDemo
{
    partial class KeywodsForm
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
            this.keywordsControl1 = new userControls.keywordsControl();
            this.SuspendLayout();
            // 
            // keywordsControl1
            // 
            this.keywordsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keywordsControl1.Location = new System.Drawing.Point(0, 0);
            this.keywordsControl1.Name = "keywordsControl1";
            this.keywordsControl1.Size = new System.Drawing.Size(836, 404);
            this.keywordsControl1.TabIndex = 0;
            this.keywordsControl1.savekeywordsClick += new System.EventHandler(this.keywordsControl1_savekeywordsClick);
            // 
            // KeywodsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 404);
            this.Controls.Add(this.keywordsControl1);
            this.Name = "KeywodsForm";
            this.Text = "关键词";
            this.ResumeLayout(false);

        }

        #endregion

        private userControls.keywordsControl keywordsControl1;
    }
}