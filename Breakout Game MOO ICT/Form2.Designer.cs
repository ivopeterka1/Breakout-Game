namespace Breakout_Game
{
    partial class MainMenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button button1;

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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            this.button1 = new System.Windows.Forms.Button();
            this.HighScoresButton = new System.Windows.Forms.Button();
            this.rulesPictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.rulesPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(363, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HighScoresButton
            // 
            this.HighScoresButton.Location = new System.Drawing.Point(363, 242);
            this.HighScoresButton.Name = "HighScoresButton";
            this.HighScoresButton.Size = new System.Drawing.Size(75, 23);
            this.HighScoresButton.TabIndex = 1;
            this.HighScoresButton.Text = "High Scores";
            this.HighScoresButton.UseVisualStyleBackColor = true;
            this.HighScoresButton.Click += new System.EventHandler(this.HighScoresButton_Click);
            // 
            // rulesPictureBox1
            // 
            this.rulesPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("rulesPictureBox1.Image")));
            this.rulesPictureBox1.Location = new System.Drawing.Point(350, 296);
            this.rulesPictureBox1.Name = "rulesPictureBox1";
            this.rulesPictureBox1.Size = new System.Drawing.Size(97, 63);
            this.rulesPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rulesPictureBox1.TabIndex = 2;
            this.rulesPictureBox1.TabStop = false;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rulesPictureBox1);
            this.Controls.Add(this.HighScoresButton);
            this.Controls.Add(this.button1);
            this.Name = "MainMenuForm";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rulesPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button HighScoresButton;
        private System.Windows.Forms.PictureBox rulesPictureBox1;
    }
}