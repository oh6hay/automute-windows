
namespace AutoMuteWF
{
    partial class Form1
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
            this.muteStatus = new System.Windows.Forms.Label();
            this.ToggleBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // muteStatus
            // 
            this.muteStatus.AutoSize = true;
            this.muteStatus.Location = new System.Drawing.Point(13, 9);
            this.muteStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.muteStatus.Name = "muteStatus";
            this.muteStatus.Size = new System.Drawing.Size(50, 16);
            this.muteStatus.TabIndex = 2;
            this.muteStatus.Text = "[status]";
            // 
            // ToggleBox
            // 
            this.ToggleBox.Checked = true;
            this.ToggleBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleBox.Location = new System.Drawing.Point(13, 28);
            this.ToggleBox.Name = "ToggleBox";
            this.ToggleBox.Size = new System.Drawing.Size(210, 24);
            this.ToggleBox.TabIndex = 3;
            this.ToggleBox.Text = "Enable auto mute";
            this.ToggleBox.UseVisualStyleBackColor = true;
            this.ToggleBox.CheckedChanged += new System.EventHandler(this.checkBoxToggled);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 63);
            this.Controls.Add(this.ToggleBox);
            this.Controls.Add(this.muteStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0.8D;
            this.Text = "automute";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox ToggleBox;

        #endregion
        private System.Windows.Forms.Label muteStatus;
    }
}

