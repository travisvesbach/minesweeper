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

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int[,] difficulty = { { 8,10 }, { 16, 40 }, { 24, 99 } };
        private string img_mine = "image/mine.png";
        private string img_flag = "image/flag.png";
        private string img_timer = "image/timer.png";
        private string img_smiley_face = "image/smiley_face.png";
        private string img_big_smiley_face = "image/big_smiley_face.png";
        private string img_frowney_face = "image/frowny_face.png";

        private int timeCount = 0;
        private int mineCount = 0;
        private int mapSize = 0;
        private int totalMines = 0;
        public ObservableCollection<MineSquare> mineSquares = new ObservableCollection<MineSquare>();



        public MainWindow()
        {
            InitializeComponent();

            string difficultySetting = "beginner";
            mapSize = difficulty[0,0];
            totalMines = difficulty[0,1];
            mineCount = totalMines;
            DrawMineField();

            DataContext = this;
        }

        public ObservableCollection<MineSquare> MineSquares
        {
            get { return mineSquares;  }
        }

        public void DrawMineField()
        {
            mineSquares = new ObservableCollection<MineSquare>();
            foreach (int x in Enumerable.Range(0, mapSize))
            {
                foreach(int y in Enumerable.Range(0, mapSize))
                {
                    MineSquare square = new MineSquare()
                    {
                        X = x,
                        Y = y,
                        Content = x.ToString() + "," + y.ToString(),
                    };
                    mineSquares.Add(square);
                }
            }
            //CreateGridRowsAndColumns();
        }


    }
}
