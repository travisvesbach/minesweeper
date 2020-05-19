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
        private string img_mine = "images/mine.png";
        private string img_flag = "images/flag.png";
        private MainWindow wnd = (MainWindow)Application.Current.MainWindow;

        private ICommand _clickSquare;
        private ICommand _flagSquare;

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
                IsClickable = false;
                IsRevealed = true;
                BackgroundColor = new SolidColorBrush(Colors.White);
            }

            this.NotifyPropertyChanged("IsClickable");
            this.NotifyPropertyChanged("BackgroundColor");
        }

        public void Flag()
        {
            if(IsFlagged)
            {
                Content = null;
                IsFlagged = false;
            }
            else
            {
                IsFlagged = true;
                Content = img_flag;
            }
            this.NotifyPropertyChanged("Content");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
