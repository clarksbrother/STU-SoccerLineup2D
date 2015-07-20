using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STU_SoccerLineup2D
{
    /// <summary>
    /// Interaction logic for ReservePlayerCard.xaml
    /// </summary>
    public partial class ReservePlayerCard : UserControl
    {
        public bool isSelected = false;
        public int playerId;
        public bool isHomeTeam;
        public bool hasResume;
        public bool hasVideo;
        public string clipPath;

        public bool resumeButtonPressed = false;
        public bool videoButtonPressed = false;

        public ReservePlayerCard()
        {
            InitializeComponent();
        }

        private void reserveResumeButton_TouchDown(object sender, TouchEventArgs e)
        {
            MainWindow theMainWindow = (MainWindow)Application.Current.MainWindow;
            resumeButtonPressed = true;
        }

        private void reserveVideoButton_TouchDown(object sender, TouchEventArgs e)
        {
            MainWindow theMainWindow = (MainWindow)Application.Current.MainWindow;
            videoButtonPressed = true;
        }

        public void animate()
        {

        }
    }
}