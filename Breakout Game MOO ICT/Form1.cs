using System;
using System.Collections.Generic;
using System.Data.SQLite; // Za uporabo SQLite
using System.Drawing;
using System.Media; // Za predvajanje zvoka
using System.Windows.Forms;

namespace Breakout_Game
{
    public partial class Form1 : Form
    {
        // Spremenljivke za nadzor gibanja igralca in stanja igre
        bool goLeft;
        bool goRight;
        bool GameOver;

        // Spremenljivke za shranjevanje rezultata, hitrosti žogice in igralca
        int score;
        int ballx;
        int bally;
        int playerSpeed;

        // Objekt za generiranje naključnih števil
        Random rnd = new Random();
        // Polje za shranjevanje blokov
        PictureBox[] blockArray;

        // Predvajalnik zvoka za ozadje
        SoundPlayer backgroundPlayer;

        // Connection string za SQLite bazo
        private string connectionString = "Data Source=highscore.db;Version=3;";

        // Konstruktor za inicializacijo komponente in postavitev blokov
        public Form1()
        {
            InitializeComponent();
            InitializeDatabase(); // Inicializiraj bazo podatkov ob ustvarjanju obrazca
        }

        // Metoda za predvajanje glasbe v ozadju
        private void PlayBackgroundMusic()
        {
            backgroundPlayer = new SoundPlayer("background.wav"); // Poskrbite, da je pot pravilna
            backgroundPlayer.PlayLooping(); // Predvajaj glasbo v zanki
        }

        // Metoda za inicializacijo baze podatkov
        private void InitializeDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open(); // Odpri povezavo z bazo podatkov
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS HighScores (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            PlayerName TEXT NOT NULL,
                                            Score INTEGER NOT NULL
                                          );";
                // Ustvari tabelo za shranjevanje rezultatov, če ta še ne obstaja
                SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn);
                cmd.ExecuteNonQuery(); // Izvedi SQL ukaz
            }
        }

        // Razred za prikaz okna za vnos imena igralca
        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 300,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
                Button confirmation = new Button() { Text = "Ok", Left = 150, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        // Metoda za vstavljanje rezultata v bazo podatkov
        private void InsertHighScore(string playerName, int score)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open(); // Odpri povezavo z bazo podatkov
                string insertQuery = "INSERT INTO HighScores (PlayerName, Score) VALUES (@PlayerName, @Score)";
                SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@PlayerName", playerName); // Dodaj parameter za ime igralca
                cmd.Parameters.AddWithValue("@Score", score); // Dodaj parameter za rezultat
                cmd.ExecuteNonQuery(); // Izvedi SQL ukaz
            }
        }

        // Metoda za pridobivanje top highscore rezultatov iz baze podatkov
        public List<(string PlayerName, int Score)> GetTopHighScores(int limit = 5)
        {
            List<(string PlayerName, int Score)> highScores = new List<(string PlayerName, int Score)>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open(); // Odpri povezavo z bazo podatkov
                string selectQuery = "SELECT PlayerName, Score FROM HighScores ORDER BY Score DESC LIMIT @Limit";
                SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn);
                cmd.Parameters.AddWithValue("@Limit", limit); // Dodaj parameter za omejitev števila rezultatov
                using (SQLiteDataReader reader = cmd.ExecuteReader()) // Izvedi poizvedbo in pridobi rezultate
                {
                    while (reader.Read())
                    {
                        highScores.Add((reader.GetString(0), reader.GetInt32(1))); // Dodaj rezultate v seznam
                    }
                }
            }
            return highScores;
        }

        // Metoda za postavitev blokov na igralno površino
        private void PlaceBlocks()
        {
            blockArray = new PictureBox[15]; // Inicializiraj polje za bloke
            int a = 0;
            int top = 50;
            int left = 100;

            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 32; // Nastavi višino bloka
                blockArray[i].Width = 100; // Nastavi širino bloka
                blockArray[i].Tag = "blocks";
                blockArray[i].BackColor = Color.White;
                this.Controls.Add(blockArray[i]);
                blockArray[i].Top = top;
                blockArray[i].Left = left;

                left += 130;

                a++;

                // Uredi bloke v vrste
                if (a == 5)
                {
                    top += 50;
                    left = 100;
                    a = 0;
                }

                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (string)x.Tag == "blocks")
                    {
                        x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)); // Nastavi naključno barvo
                    }
                }
            }
        }

        // Metoda za odstranitev blokov iz igralne površine
        private void removeBlocks()
        {
            foreach (PictureBox x in blockArray)
            {
                this.Controls.Remove(x); // Odstrani blok iz obrazca
            }
        }

        // Metoda za začetek igre
        public void StartGame()
        {
            PlaceBlocks(); // Postavi bloke
            setupGame(); // Nastavi začetne vrednosti igre
            PlayBackgroundMusic(); // Predvajaj glasbo v ozadju
        }

        // Metoda za nastavitev začetnih vrednosti igre
        private void setupGame()
        {
            GameOver = false; // Nastavi igro na aktivno
            score = 0; // Nastavi začetni rezultat
            ballx = 7; // Nastavi začetno hitrost žogice po x osi
            bally = 7; // Nastavi začetno hitrost žogice po y osi
            playerSpeed = 12; // Nastavi začetno hitrost igralca
            txtScore.Text = "Score: " + score; // Prikaži začetni rezultat

            // Začetna pozicija žogice
            ball.Left = 376;
            ball.Top = 328;

            // Začetna pozicija igralca
            player.Left = 347;

            gameTImer.Start(); // Zaženi časovnik igre
        }

        // Metoda za zaključek igre z določenim sporočilom
        private void gameOver(string message)
        {
            GameOver = true; // Nastavi igro na končano
            gameTImer.Stop(); // Ustavi časovnik igre
            txtScore.Text = $"Score: {score} {message}\nPritisni Space za ponoven poskus\nPritisni 'M' za vrnitev v glavni meni"; // Prikaži končni rezultat in sporočilo v dveh vrsticah

            // Prikaži dialog za vnos imena igralca
            string playerName = Prompt.ShowDialog("Enter your name:", "Game Over");
            if (!string.IsNullOrWhiteSpace(playerName))
            {
                InsertHighScore(playerName, score); // Vstavi rezultat v bazo podatkov
            }

            StopBackgroundMusic(); // Ustavi predvajanje glasbe
        }

        // Metoda za ponastavitev igre ob zmagi
        private void ResetGame()
        {
            ballx += 2; // Povečanje hitrosti žogice
            bally += 2; // Povečanje hitrosti žogice
            playerSpeed += 2; // Povečanje hitrosti igralca
            removeBlocks(); // Odstrani obstoječe bloke
            PlaceBlocks(); // Postavi nove bloke
        }


        // Glavni dogodek časovnika igre, ki se sproži vsako sekundo
        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score; // Posodobi rezultat na zaslonu

            // Premikanje igralca v levo ali desno, če so ustrezne tipke pritisnjene
            if (goLeft && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if (goRight && player.Left < 700)
            {
                player.Left += playerSpeed;
            }

            // Premikanje žogice
            ball.Left += ballx;
            ball.Top += bally;

            // Spreminjanje smeri žogice, če zadene robove okna
            if (ball.Left < 0 || ball.Left > 775)
            {
                ballx = -ballx;
            }

            if (ball.Top < 0)
            {
                bally = -bally;
            }

            // Spreminjanje smeri žogice, če zadene igralca
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                ball.Top = 463;
                bally = -bally; // Obrni smer žogice po y osi
            }

            // Odstranitev bloka in povečanje rezultata, če žogica zadene blok
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1; // Povečaj rezultat
                        bally = -bally; // Obrni smer žogice
                        this.Controls.Remove(x); // Odstrani zadeti blok
                    }
                }
            }

            // Preverjanje, ali je igralec zmagal
            if (score % 15 == 0 && score != 0)
            {
                ResetGame(); // Ponastavi igro za naslednji nivo
            }

            // Žogica pobegne mimo loparja
            if (ball.Top > 580)
            {
                gameOver("Izgubil si!!"); // Konec igre
            }
        }


        // Dogodek, ki se sproži, ko je tipka pritisnjena
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }

        // Metoda za prikaz glavnega menija
        private void ShowMainMenu()
        {
            this.Close(); // Zapri trenutno okno
            MainMenuForm mainMenu = new MainMenuForm(); // Ustvari nov primer glavnega menija
            mainMenu.Show(); // Pokaži glavni meni
        }

        // Dogodek, ki se sproži, ko je tipka spuščena
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Space && GameOver == true)
            {
                removeBlocks(); // Odstrani obstoječe bloke
                PlaceBlocks(); // Postavi nove bloke
                setupGame(); // Ponastavi igro
                PlayBackgroundMusic(); // Predvajaj glasbo v ozadju
            }

            if (e.KeyCode == Keys.M && GameOver == true)
            {
                ShowMainMenu(); // Pokaži glavni meni
            }
        }

        // Metoda za ustavitev predvajanja glasbe
        private void StopBackgroundMusic()
        {
            if (backgroundPlayer != null)
            {
                backgroundPlayer.Stop(); // Ustavi predvajanje glasbe
            }
        }

        // Dogodek, ki se sproži, ko se obrazec zapira
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopBackgroundMusic(); // Ustavi predvajanje glasbe
            base.OnFormClosing(e); // Kliči osnovno metodo
        }
    }

}
