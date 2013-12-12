namespace HotKeyControl
{
    partial class HotKeyControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.checkBoxWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxShift = new System.Windows.Forms.CheckBox();
            this.checkBoxAlt = new System.Windows.Forms.CheckBox();
            this.checkBoxCtrl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxKey
            // 
            this.textBoxKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKey.Location = new System.Drawing.Point(162, 3);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.Size = new System.Drawing.Size(109, 20);
            this.textBoxKey.TabIndex = 4;
            this.textBoxKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxKey_KeyDown);
            this.textBoxKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxKey_KeyUp);
            // 
            // checkBoxWindows
            // 
            this.checkBoxWindows.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxWindows.AutoSize = true;
            this.checkBoxWindows.Location = new System.Drawing.Point(120, 1);
            this.checkBoxWindows.Name = "checkBoxWindows";
            this.checkBoxWindows.Size = new System.Drawing.Size(36, 23);
            this.checkBoxWindows.TabIndex = 32;
            this.checkBoxWindows.Text = "Win";
            this.checkBoxWindows.UseVisualStyleBackColor = true;
            // 
            // checkBoxShift
            // 
            this.checkBoxShift.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxShift.AutoSize = true;
            this.checkBoxShift.Location = new System.Drawing.Point(76, 1);
            this.checkBoxShift.Name = "checkBoxShift";
            this.checkBoxShift.Size = new System.Drawing.Size(38, 23);
            this.checkBoxShift.TabIndex = 31;
            this.checkBoxShift.Text = "Shift";
            this.checkBoxShift.UseVisualStyleBackColor = true;
            // 
            // checkBoxAlt
            // 
            this.checkBoxAlt.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxAlt.AutoSize = true;
            this.checkBoxAlt.Location = new System.Drawing.Point(41, 1);
            this.checkBoxAlt.Name = "checkBoxAlt";
            this.checkBoxAlt.Size = new System.Drawing.Size(29, 23);
            this.checkBoxAlt.TabIndex = 30;
            this.checkBoxAlt.Text = "Alt";
            this.checkBoxAlt.UseVisualStyleBackColor = true;
            // 
            // checkBoxCtrl
            // 
            this.checkBoxCtrl.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCtrl.AutoSize = true;
            this.checkBoxCtrl.Location = new System.Drawing.Point(3, 1);
            this.checkBoxCtrl.Name = "checkBoxCtrl";
            this.checkBoxCtrl.Size = new System.Drawing.Size(32, 23);
            this.checkBoxCtrl.TabIndex = 29;
            this.checkBoxCtrl.Text = "Ctrl";
            this.checkBoxCtrl.UseVisualStyleBackColor = true;
            // 
            // HotKeyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxWindows);
            this.Controls.Add(this.checkBoxShift);
            this.Controls.Add(this.checkBoxAlt);
            this.Controls.Add(this.checkBoxCtrl);
            this.Controls.Add(this.textBoxKey);
            this.Name = "HotKeyControl";
            this.Size = new System.Drawing.Size(274, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.CheckBox checkBoxWindows;
        private System.Windows.Forms.CheckBox checkBoxShift;
        private System.Windows.Forms.CheckBox checkBoxAlt;
        private System.Windows.Forms.CheckBox checkBoxCtrl;
    }
}
