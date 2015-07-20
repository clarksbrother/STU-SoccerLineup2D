using System;
using System.Collections.Generic;
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
	/// Interaction logic for FieldPlayer.xaml
	/// </summary>
	public partial class FieldPlayer : UserControl
	{
        public bool playerTrailEnabled = false;
		public FieldPlayer()
		{
			this.InitializeComponent();
		}

		private void dotsEnabled(object sender, System.Windows.Input.TouchEventArgs e)
		{
            playerTrailEnabled = true;
            
		}

		private void dotsDisabled(object sender, System.Windows.Input.TouchEventArgs e)
		{
            playerTrailEnabled = false;
            MessageBox.Show("DISABLED");
		}
	}
}