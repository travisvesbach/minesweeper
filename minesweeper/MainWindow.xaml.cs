using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int[,] difficulty = { { 8,10 }, { 16, 40 }, { 24, 99 } };
        //private string img_mine = "image/mine.png";
        //private string img_flag = "image/flag.png";
        //private string img_timer = "image/timer.png";
        private string img_smiley_face = "/images/smiley_face.png";
        private string img_big_smiley_face = "/images/big_smiley_face.png";
        private string img_frowney_face = "/images/frowny_face.png";

        private string difficultySetting { get; set; }
        private int timeCount { get; set; }
        private int mineCount { get; set; }
        private int mapSize { get; set; }
        private int totalMines { get; set; }
        private string status { get; set; }
        private int revealedCount { get; set; }
        private int revealedWinCount { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        private DateTime timerStart { get; set; }
        public ObservableCollection<MineSquare> mineSquares = new ObservableCollection<MineSquare>();



        public MainWindow()
        {
            InitializeComponent();

            difficultySetting = "beginner";
            mapSize = difficulty[0,0];
            totalMines = difficulty[0,1];

            ResetMineField();

            DataContext = this;
        }

        public ObservableCollection<MineSquare> MineSquares
        {
            get { return mineSquares;  }
        }

        public void ResetMineField()
        {
            UpdateStatus("ready");
            mineCount = totalMines;
            revealedCount = 0;
            revealedWinCount = (mapSize * mapSize) - totalMines;
            DrawMineField();
            SetMines();
            SetAdjacentCount();
            tbTimeCount.Text = "000";
            tbMineCount.Text = mineCount.ToString("D3");

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateTimer;

        }

        public void DrawMineField()
        {
            mineSquares.Clear();
            foreach (int x in Enumerable.Range(0, mapSize))
            {
                foreach(int y in Enumerable.Range(0, mapSize))
                {
                    MineSquare square = new MineSquare()
                    {
                        X = x,
                        Y = y,
                    };
                    mineSquares.Add(square);
                }
            }
        }

        public void SetMines()
        {
            List<int[]> positions = new List<int[]>();
            Random rand = new Random();
            while (positions.Count() < totalMines)
            {
                int x = rand.Next(mapSize);
                int y = rand.Next(mapSize);
                int[] position = { x, y };
                if (!positions.Any(posList => Enumerable.SequenceEqual(position, posList)))
                {
                    mineSquares.Where(ms => ms.X == x && ms.Y == y).FirstOrDefault().IsMine = true;
                    positions.Add(position);
                }

            }
        }

        public void SetAdjacentCount()
        {
            foreach (MineSquare mineSquare in mineSquares)
            {
                int x = mineSquare.X;
                int y = mineSquare.Y;
                int adjacentMineCount = 0;
                for (int x_position = Math.Max(0, x - 1); x_position < Math.Min(x + 2, mapSize); x_position++)
                {
                    for (int y_position = Math.Max(0, y - 1); y_position < Math.Min(y + 2, mapSize); y_position++)
                    {
                        if (mineSquares.Any(ms => ms.X == x_position && ms.Y == y_position && ms.IsMine))
                        {
                            adjacentMineCount++;
                        }
                        
                    }
                }
                mineSquare.adjacentCount = adjacentMineCount;
            }
        }

        public void ExpandReveal(int x, int y)
        {
            for (int x_position = Math.Max(0, x - 1); x_position < Math.Min(x + 2, mapSize); x_position++)
            {
                for (int y_position = Math.Max(0, y - 1); y_position < Math.Min(y + 2, mapSize); y_position++)
                {
                    MineSquare mineSquare = mineSquares.Where(ms => ms.X == x_position && ms.Y == y_position).FirstOrDefault();
                    if (mineSquare != null && !mineSquare.IsMine)
                    {
                        mineSquare.Reveal();
                    }
                }
            }
        }

        public void RevealMap()
        {
            foreach (MineSquare mineSquare in mineSquares) {
                if (status == "win")
                {
                    mineSquare.Reveal(true);
                }
                else
                {
                    mineSquare.Reveal();
                }
            }
        }

        public void UpdateStatus(string newStatus)
        {
            status = newStatus;
            if (status == "win")
            {
                tbButtonImage.Source = new BitmapImage(
                                    new Uri(img_big_smiley_face, UriKind.RelativeOrAbsolute));
            }
            else if (status == "failed")
            {
                tbButtonImage.Source = new BitmapImage(
                                    new Uri(img_frowney_face, UriKind.RelativeOrAbsolute));
            }
            else
            {
                tbButtonImage.Source = new BitmapImage(
                                    new Uri(img_smiley_face, UriKind.RelativeOrAbsolute));
            }
        }

        public void UpdateButtonImage()
        {

            tbButtonImage.Source = new BitmapImage(
                new Uri("pack://application:,,,/AssemblyName;component/Resources/logo.png"));
        }

        public void CheckWin(bool revealed = false)
        {
            if (status == "ready")
            {
                GameStart();
            }
            if (status == "playing")
            {
                if (revealed)
                {
                    revealedCount++;
                }
                if (revealedCount == revealedWinCount)
                {
                    GameWin();
                }
            }
        }

        public void GameStart()
        {
            UpdateStatus("playing");
            //start timer
            timer.Start();
            timerStart = DateTime.Now;
        }

        public void GameWin()
        {
            UpdateStatus("win");
            timer.Stop();
            RevealMap();
            string message = "Congratulations! You beat " + difficultySetting + " in " + (DateTime.Now - timerStart).Seconds.ToString() + " seconds!";
            MessageBox.Show(message);
        }

        public void GameOver()
        {
            UpdateStatus("failed");
            timer.Stop();
            RevealMap();
        }

        public void UpdateTimer(object sender, EventArgs e)
        {
            tbTimeCount.Text = (DateTime.Now - timerStart).Seconds.ToString("D3");
        }

        public void ClickButton(object sender, EventArgs e)
        {
            if (status == "playing")
            {
                UpdateStatus("failed");
                RevealMap();
            }
            else if (status == "failed" || status == "win")
            {
                ResetMineField();
            }
        }

    }
}
