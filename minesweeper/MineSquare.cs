using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;

namespace minesweeper
{
    public class MineSquare : INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Content { get; set; }
        public SolidColorBrush BackgroundColor { get; set; } = new SolidColorBrush(Colors.SandyBrown);
        public bool IsRevealed { get; set; } = false;
        public bool IsMine { get; set; } = false;
        public bool IsFlagged { get; set; } = false;
        public bool IsClickable { get; set; } = true;
        public int adjacentCount = 0;
        private string img_mine = "images/mine.png";
        private string img_flag = "images/flag.png";
        private string img_one = "images/1.png";
        private string img_two = "images/2.png";
        private string img_three = "images/3.png";
        private string img_four = "images/4.png";
        private string img_five = "images/5.png";
        private string img_six = "images/6.png";
        private string img_seven = "images/7.png";
        private string img_eight = "images/8.png";
        private string img_nine = "images/9.png";
        private MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        private ICommand _clickSquare;
        private ICommand _flagSquare;

        public MineSquare()
        {
            //NotifyPropertyChanged("Content");
            //NotifyPropertyChanged("IsClickable");
            //NotifyPropertyChanged("BackgroundColor");
        }

        public ICommand ClickSquare
        {
            get
            {
                return _clickSquare ?? (_clickSquare = new CommandHandler(() => Click(), () => CanExecute));
            }
        }

        public ICommand FlagSquare
        {
            get
            {
                return _flagSquare ?? (_flagSquare = new CommandHandler(() => Flag(), () => CanExecute));
            }
        }

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }

        public void Click()
        {
            if (!IsFlagged && !IsRevealed)
            {
                if (IsMine)
                {
                    wnd.GameOver();
                }
                else
                {
                    Reveal();
                }
            }
        }

        public void Reveal(bool win = false)
        {
            if (win && IsMine)
            {
                Flag(true);
            }
            else if (!IsFlagged && !IsRevealed)
            {
                IsClickable = false;
                IsRevealed = true;
                BackgroundColor = new SolidColorBrush(Colors.White);
                SetContent();
                if (adjacentCount == 0)
                {
                    wnd.ExpandReveal(X, Y);
                }
                wnd.CheckWin(true);
            }

            NotifyPropertyChanged("IsClickable");
            NotifyPropertyChanged("BackgroundColor");
        }

        public void Flag(bool win = false)
        {
            if(IsFlagged && !win)
            {
                Content = null;
                IsFlagged = false;
            }
            else
            {
                IsFlagged = true;
                Content = img_flag;
            }
            NotifyPropertyChanged("Content");
            wnd.CheckWin();
        }

        public void SetContent()
        {
            if (IsMine)
            {
                Content = img_mine;
            }
            else
            {
                switch (adjacentCount)
                {
                    case 1:
                        Content = img_one;
                        break;
                    case 2:
                        Content = img_two;
                        break;
                    case 3:
                        Content = img_three;
                        break;
                    case 4:
                        Content = img_four;
                        break;
                    case 5:
                        Content = img_five;
                        break;
                    case 6:
                        Content = img_six;
                        break;
                    case 7:
                        Content = img_seven;
                        break;
                    case 8:
                        Content = img_eight;
                        break;
                    case 9:
                        Content = img_nine;
                        break;
                }
            }
            NotifyPropertyChanged("Content");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
