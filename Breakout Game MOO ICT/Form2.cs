using Breakout_Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game
{
    public partial class MainMenuForm : Form
    {

        // Nova spremenljivka za PictureBox
        private PictureBox rulesPictureBox;

        // Konstruktor za inicializacijo glavnega menija
        public MainMenuForm()
        {
            InitializeComponent(); // Inicializiraj komponente obrazca
            string imagePath = System.IO.Path.Combine(Application.StartupPath, "slike", "ozadje.jpg");
            this.BackgroundImage = Image.FromFile(imagePath);
            this.BackgroundImageLayout = ImageLayout.Stretch; // Uporabite ustrezno nastavitev

        }

        // Metoda za prikaz top 5 rezultatov
        private void ShowHighScores()
        {
            Form1 gameForm = new Form1(); // Ustvari nov primer Form1, da pridobi rezultate
            List<(string PlayerName, int Score)> highScores = gameForm.GetTopHighScores(); // Pridobi top 5 rezultatov iz igre

            // Sestavi besedilo za prikaz rezultatov
            string highScoreText = "Top 5 High Scores:\n";
            foreach (var score in highScores)
            {
                highScoreText += $"{score.PlayerName}: {score.Score}\n"; // Dodaj rezultat v besedilo
            }

            MessageBox.Show(highScoreText, "High Scores"); // Prikaži rezultat v sporočilnem oknu
        }

        // Dogodek za klik na gumb za začetek igre
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 gameForm = new Form1(); // Ustvari nov primer igre
            gameForm.Show(); // Pokaži obrazec igre
            gameForm.StartGame(); // Začni igro, ko je obrazec prikazan
            this.Hide(); // Skrij glavni meni, ko se začne igra
        }

        // Dogodek, ki se sproži, ko se glavni meni naloži
        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            //ShowHighScores(); // Prikaže top 5 rezultatov
            // Nastavite lokacijo PictureBox, da bo centriran pod gumbom
            rulesPictureBox1.Location = new Point(
                (this.ClientSize.Width - rulesPictureBox1.Width) / 2, // Centriraj po širini
                button1.Bottom + 40 // Razdalja od gumba
            );
        }

        private void HighScoresButton_Click(object sender, EventArgs e)
        {
            ShowHighScores(); // Prikaži rezultate
        }
    }
}
