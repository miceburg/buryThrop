namespace LastAccess
{
    partial class Config
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Config));
            this.lbNetFiles = new System.Windows.Forms.Label();
            this.lbLocFiles = new System.Windows.Forms.Label();
            this.tbNetFiles = new System.Windows.Forms.TextBox();
            this.tbLocFiles = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.rbNetwork = new System.Windows.Forms.RadioButton();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.rbBoth = new System.Windows.Forms.RadioButton();
            this.tbExt = new System.Windows.Forms.TextBox();
            this.lbExt = new System.Windows.Forms.Label();
            this.tbCopyDest = new System.Windows.Forms.TextBox();
            this.lbCopyDest = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbNetFiles
            // 
            this.lbNetFiles.AutoSize = true;
            this.lbNetFiles.Location = new System.Drawing.Point(13, 13);
            this.lbNetFiles.Name = "lbNetFiles";
            this.lbNetFiles.Size = new System.Drawing.Size(127, 13);
            this.lbNetFiles.TabIndex = 0;
            this.lbNetFiles.Text = "Location of Network Files";
            // 
            // lbLocFiles
            // 
            this.lbLocFiles.AutoSize = true;
            this.lbLocFiles.Location = new System.Drawing.Point(27, 46);
            this.lbLocFiles.Name = "lbLocFiles";
            this.lbLocFiles.Size = new System.Drawing.Size(113, 13);
            this.lbLocFiles.TabIndex = 1;
            this.lbLocFiles.Text = "Location of Local Files";
            // 
            // tbNetFiles
            // 
            this.tbNetFiles.Location = new System.Drawing.Point(146, 10);
            this.tbNetFiles.Name = "tbNetFiles";
            this.tbNetFiles.Size = new System.Drawing.Size(379, 20);
            this.tbNetFiles.TabIndex = 2;
            // 
            // tbLocFiles
            // 
            this.tbLocFiles.Location = new System.Drawing.Point(146, 43);
            this.tbLocFiles.Name = "tbLocFiles";
            this.tbLocFiles.Size = new System.Drawing.Size(379, 20);
            this.tbLocFiles.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 212);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(450, 212);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // rbNetwork
            // 
            this.rbNetwork.AutoSize = true;
            this.rbNetwork.Location = new System.Drawing.Point(155, 111);
            this.rbNetwork.Name = "rbNetwork";
            this.rbNetwork.Size = new System.Drawing.Size(65, 17);
            this.rbNetwork.TabIndex = 6;
            this.rbNetwork.TabStop = true;
            this.rbNetwork.Text = "Network";
            this.rbNetwork.UseVisualStyleBackColor = true;
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Location = new System.Drawing.Point(226, 111);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(51, 17);
            this.rbLocal.TabIndex = 7;
            this.rbLocal.TabStop = true;
            this.rbLocal.Text = "Local";
            this.rbLocal.UseVisualStyleBackColor = true;
            // 
            // rbBoth
            // 
            this.rbBoth.AutoSize = true;
            this.rbBoth.Location = new System.Drawing.Point(283, 111);
            this.rbBoth.Name = "rbBoth";
            this.rbBoth.Size = new System.Drawing.Size(47, 17);
            this.rbBoth.TabIndex = 8;
            this.rbBoth.TabStop = true;
            this.rbBoth.Text = "Both";
            this.rbBoth.UseVisualStyleBackColor = true;
            // 
            // tbExt
            // 
            this.tbExt.Location = new System.Drawing.Point(146, 76);
            this.tbExt.Name = "tbExt";
            this.tbExt.Size = new System.Drawing.Size(379, 20);
            this.tbExt.TabIndex = 9;
            // 
            // lbExt
            // 
            this.lbExt.AutoSize = true;
            this.lbExt.Location = new System.Drawing.Point(15, 80);
            this.lbExt.Name = "lbExt";
            this.lbExt.Size = new System.Drawing.Size(125, 13);
            this.lbExt.TabIndex = 10;
            this.lbExt.Text = "Extensions to Search For";
            // 
            // tbCopyDest
            // 
            this.tbCopyDest.Location = new System.Drawing.Point(146, 142);
            this.tbCopyDest.Name = "tbCopyDest";
            this.tbCopyDest.Size = new System.Drawing.Size(379, 20);
            this.tbCopyDest.TabIndex = 11;
            // 
            // lbCopyDest
            // 
            this.lbCopyDest.AutoSize = true;
            this.lbCopyDest.Location = new System.Drawing.Point(53, 145);
            this.lbCopyDest.Name = "lbCopyDest";
            this.lbCopyDest.Size = new System.Drawing.Size(87, 13);
            this.lbCopyDest.TabIndex = 12;
            this.lbCopyDest.Text = "Copy Destination";
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 247);
            this.Controls.Add(this.lbCopyDest);
            this.Controls.Add(this.tbCopyDest);
            this.Controls.Add(this.lbExt);
            this.Controls.Add(this.tbExt);
            this.Controls.Add(this.rbBoth);
            this.Controls.Add(this.rbLocal);
            this.Controls.Add(this.rbNetwork);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbLocFiles);
            this.Controls.Add(this.tbNetFiles);
            this.Controls.Add(this.lbLocFiles);
            this.Controls.Add(this.lbNetFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Config";
            this.Text = "Config";
            this.Load += new System.EventHandler(this.Config_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbNetFiles;
        private System.Windows.Forms.Label lbLocFiles;
        private System.Windows.Forms.TextBox tbNetFiles;
        private System.Windows.Forms.TextBox tbLocFiles;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton rbNetwork;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.RadioButton rbBoth;
        private System.Windows.Forms.TextBox tbExt;
        private System.Windows.Forms.Label lbExt;
        private System.Windows.Forms.TextBox tbCopyDest;
        private System.Windows.Forms.Label lbCopyDest;
    }
}