using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace STU_SoccerLineup2D
{
    public class PaletteToolInfo
    {
        private String myButtonType = "";
        private Color myButtonColor = Colors.Transparent;
        private Boolean myIsDefault = false;
        private Boolean myAlwaysOpen = false;

        public String ButtonType
        {
            get
            {
                return myButtonType;
            }
            set
            {
                myButtonType = value;
            }
        }

        public Color ButtonColor
        {
            get
            {
                return myButtonColor;
            }
            set
            {
                myButtonColor = value;
            }
        }

        public Boolean IsDefault
        {
            get
            {
                return myIsDefault;
            }
            set
            {
                myIsDefault = value;
            }
        }

        public Boolean AlwaysOpen
        {
            get
            {
                return myAlwaysOpen;
            }
            set
            {
                myAlwaysOpen = value;
            }
        }
    }
}
