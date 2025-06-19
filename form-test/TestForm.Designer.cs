using form_test.Layouts;

namespace form_test
{
    partial class TestForm
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
            panel1 = new LayerLayout();
            panel3 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel4);
            panel1.EnableAlter = false;
            panel1.Location = new System.Drawing.Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(403, 313);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            panel3.Location = new System.Drawing.Point(20, 20);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(40, 40);
            panel3.TabIndex = 1;
            panel3.Tag = "0";
            panel3.MouseDown += panel3_MouseHover;
            // 
            // panel2
            // 
            panel2.BackColor = System.Drawing.SystemColors.Info;
            panel2.Controls.Add(panel4);
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(403, 313);
            panel2.TabIndex = 0;
            panel2.Tag = "1";
            // 
            // panel4
            // 
            panel4.BackColor = System.Drawing.SystemColors.Highlight;
            panel4.Location = new System.Drawing.Point(181, 97);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(200, 100);
            panel4.TabIndex = 0;
            panel4.Tag = "2";
            // 
            // TestForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(654, 337);
            Controls.Add(panel1);
            Name = "TestForm";
            Text = "TestForm";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private LayerLayout panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
    }
}