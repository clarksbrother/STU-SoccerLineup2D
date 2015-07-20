using Artefact.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TelestrationWPF;

namespace STU_SoccerLineup2D
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        MainWindow theMainWindow = (MainWindow)Application.Current.MainWindow;

        public bool controlPanelOpen = false;
        public bool rightSide = true;

        public int currentTeam = 1; // 1=home, 2=visitors

        bool isBenchTap = false;
        Timer benchTouchOrTapTimer = new System.Timers.Timer();
        double homeBenchScrollOffsetY = 0;
        double visitorsBenchScrollOffsetY = 0;
        int reserveCardHeight = 107;

        string controlPanelPaletteButton;
        Color controlPanelColorButton;
        public static TelestrationWPF.AvailableColors availColors = new TelestrationWPF.AvailableColors();
        private const string PALETTE_COLOR_FILE = "colorpalette.xml";
        Double btnHeight = 50.0;
        Double btnWidth = 60.0;
        Double btnImageHeight = 45.0;
        Double btnImageWidth = 55.0;
        Color teleBGColor = new Color();
        SolidColorBrush scb = new SolidColorBrush();

        public BitmapImage showBenchOffBitmap, showBenchOnBitmap;
        public BitmapImage hideBenchOffBitmap, hideBenchOnBitmap;
        public BitmapImage resetCardsOffBitmap, resetCardsOnBitmap;
        public BitmapImage showHomeOnBitmap, showVisitorsOnBitmap, showBothOnBitmap;
        public BitmapImage showHomeOffBitmap, showVisitorsOffBitmap, showBothOffBitmap;
        public BitmapImage lineupOffBitmap, lineupOnBitmap;
        System.Windows.Threading.DispatcherTimer showBenchTimer = new System.Windows.Threading.DispatcherTimer();  // A UI timer
        System.Windows.Threading.DispatcherTimer hideBenchTimer = new System.Windows.Threading.DispatcherTimer();  // A UI timer
        System.Windows.Threading.DispatcherTimer resetButtonTimer = new System.Windows.Threading.DispatcherTimer();  // A UI timer
        System.Windows.Threading.DispatcherTimer lineupButtonTimer = new System.Windows.Threading.DispatcherTimer();  // A UI timer



        public ControlPanel()
        {
            InitializeComponent();

            btnShowBench.Visibility = Visibility.Visible;
            btnHideBench.Visibility = Visibility.Hidden;


            btnResetCards.TouchDown += btnResetCards_TouchDown;

            btnShowHome.TouchDown += btnShowHome_TouchDown;
            btnShowVisitors.TouchDown += btnShowVisitors_TouchDown;
           

            btnHomeTeam.TouchDown += btnHomeTeam_TouchDown;
            btnAwayTeam.TouchDown += btnAwayTeam_TouchDown;

            benchTouchOrTapTimer.Interval = 200;
            benchTouchOrTapTimer.Elapsed += new ElapsedEventHandler(benchTouchOrTapTimerElapsed);


            // Preload button states
            showBenchOffBitmap = new BitmapImage();
            showBenchOffBitmap.BeginInit();
            showBenchOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnShowBench.png");
            showBenchOffBitmap.EndInit();

            showBenchOnBitmap = new BitmapImage();
            showBenchOnBitmap.BeginInit();
            showBenchOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnShowBenchOn.png");
            showBenchOnBitmap.EndInit();

            hideBenchOffBitmap = new BitmapImage();
            hideBenchOffBitmap.BeginInit();
            hideBenchOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnHideBench.png");
            hideBenchOffBitmap.EndInit();

            hideBenchOnBitmap = new BitmapImage();
            hideBenchOnBitmap.BeginInit();
            hideBenchOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnHideBenchOn.png");
            hideBenchOnBitmap.EndInit();

            resetCardsOffBitmap = new BitmapImage();
            resetCardsOffBitmap.BeginInit();
            resetCardsOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnResetCards.png");
            resetCardsOffBitmap.EndInit();

            resetCardsOnBitmap = new BitmapImage();
            resetCardsOnBitmap.BeginInit();
            resetCardsOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnResetCardsOn.png");
            resetCardsOnBitmap.EndInit();

            showHomeOnBitmap = new BitmapImage();
            showHomeOnBitmap.BeginInit();
            showHomeOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnHomeOn.png");
            showHomeOnBitmap.EndInit();

            showHomeOffBitmap = new BitmapImage();
            showHomeOffBitmap.BeginInit();
            showHomeOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnHomeOff.png");
            showHomeOffBitmap.EndInit();

            showVisitorsOnBitmap = new BitmapImage();
            showVisitorsOnBitmap.BeginInit();
            showVisitorsOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnVisitorOn.png");
            showVisitorsOnBitmap.EndInit();

            showVisitorsOffBitmap = new BitmapImage();
            showVisitorsOffBitmap.BeginInit();
            showVisitorsOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnVisitorOff.png");
            showVisitorsOffBitmap.EndInit();

            showBothOnBitmap = new BitmapImage();
            showBothOnBitmap.BeginInit();
            showBothOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnBothOn.png");
            showBothOnBitmap.EndInit();

            showBothOffBitmap = new BitmapImage();
            showBothOffBitmap.BeginInit();
            showBothOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnBothOff.png");
            showBothOffBitmap.EndInit();

            lineupOffBitmap = new BitmapImage();
            lineupOffBitmap.BeginInit();
            lineupOffBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnLineUp_Off.png");
            lineupOffBitmap.EndInit();

            lineupOnBitmap = new BitmapImage();
            lineupOnBitmap.BeginInit();
            lineupOnBitmap.UriSource = new Uri("pack://application:,,,/STU-SoccerLineup2D;component/Images/ControlPanel/btnLineUp_On.png");
            lineupOnBitmap.EndInit();



            myPalette.SelectToolButton("DRAW");
            Color teleBGColor = new Color();
            teleBGColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#D7D4D5");
            scb = new SolidColorBrush(teleBGColor);

            myPalette.AddToolButton(PaletteButton.TYPE_DRAW_DESC, btnWidth, btnHeight, scb, btnImageWidth, btnImageHeight, true, false, 1, 1);
            
            this.myPalette.AddColorButton(availColors.GetColorByName("red"), btnWidth, btnHeight, scb, btnImageWidth, btnImageHeight, true, 2, 1);

            this.myPalette.OpenDirection = "UP";
            this.myPalette.InitializePalette();
            this.myPalette.SelectDefaultButtons();
            this.myPalette.CloseMe(true);

            AddEventHandlers();
        }

        private void InitializePalette()
        {
            try
            {


                this.myPalette.PaletteRows = 10;
                this.myPalette.PaletteColumns = 1;
                //this.myPalette.ClearButtonList();

                //this.myPalette2.ClearButtonList();

                this.myPalette.CloseMe(true);

                this.myPalette.OpenMe(true);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
            }
        }



        private String GetLocalDirectory()
        {
            String strReturn = "";
            try
            {
                // get the path from the current Assembly
                Assembly assem = Assembly.GetExecutingAssembly();
                string exeDir = System.IO.Path.GetDirectoryName(assem.Location);
                // get the full path to return
                if (exeDir.EndsWith("\\") == false)
                {
                    strReturn = exeDir + "\\";
                }
                else
                {
                    strReturn = exeDir;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
                strReturn = "";
            }
            return strReturn;
        }

        public void LoadColorsFromFile()
        {
            try
            {
                String localDir = GetLocalDirectory();
                if (localDir.Length > 0)
                {
                    string colorXMLFile = localDir + PALETTE_COLOR_FILE;
                    if (File.Exists(colorXMLFile) == true)
                    {
                        // parse the XML data for its components
                        XDocument xDoc = XDocument.Load(colorXMLFile);
                        // try to get the root element
                        XElement xRoot = xDoc.Root;
                        // now go through and get values for each color
                        foreach (XElement child in xRoot.Descendants())
                        {
                            // get the name of the color
                            String colorName = child.Name.LocalName;
                            // get the Red, Green and Blue values for the color
                            byte tempRed = Convert.ToByte(child.Attribute("red").Value);
                            byte tempGreen = Convert.ToByte(child.Attribute("green").Value);
                            byte tempBlue = Convert.ToByte(child.Attribute("blue").Value);
                            // add or update the color value in the list of available colors
                            availColors.AddColor(colorName, tempRed, tempGreen, tempBlue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
            }
        }

        #region ** Routed Event Handlers

        private void AddEventHandlers()
        {
            try
            {


                myPalette.AddHandler(PaletteButton.PaletteButtonPressedEvent, new RoutedEventHandler(PaletteButtonPressedHandler));
                myPalette.AddHandler(DynamicPalette.PalettePressedEvent, new RoutedEventHandler(PalettePressedEventHandler));


            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
            }
        }

        private void PaletteButtonPressedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.OriginalSource != null)
                {
                    // code here to perform button press actions for the palette
                    // get a reference to the Palette button
                    PaletteButton pb = e.OriginalSource as PaletteButton;

                    if (pb.ButtonType == PaletteButton.TYPE_PALETTE_DESC && sender == myPalette)
                    {
                        switch (myPalette.PaletteIsOpen)
                        {
                            case false:
                                myPalette.OpenMe(true);
                                break;
                            case true:
                                myPalette.CloseMe(true);

                                break;
                        }
                    }

                    else
                    {
                        // set the properties of the drawing canvas based on the button type
                        switch (pb.ButtonType)
                        {

                            case PaletteButton.TYPE_COLOR_DESC:
                                theMainWindow.globalColorButton = pb.ButtonColor;
                                theMainWindow.myCanvas.CurrentDrawColor = pb.ButtonColor;
                                myPalette.SelectColorButton(pb.ButtonColor);

                                break;
                            case PaletteButton.TYPE_UNDO_DESC:
                                theMainWindow.myCanvas.ClearLastChange();
                                break;
                            case PaletteButton.TYPE_UNDOALL_DESC:
                                theMainWindow.myCanvas.ClearAllChanges();
                                break;
                            default:

                                theMainWindow.globalPaletteButton = pb.ButtonType;
                                theMainWindow.myCanvas.CurrentButtonType = pb.ButtonType;

                                myPalette.SelectToolButton(pb.ButtonType);
                                break;
                        }
                    }
                }
                // we are done with this bubbled event
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
            }
        }

        private void PalettePressedEventHandler(object sender, RoutedEventArgs e)
        {

            if (myPalette.PaletteIsOpen == true)
            {
                //CLOSED
                try
                {
                    // clear any changes on the drawing canvas
                    //myCanvas.ClearAllChanges();
                    // reset properties of the drawing canvas
                    theMainWindow.myCanvas.CurrentButtonType = PaletteButton.TYPE_DEFAULT_DESC;
                    theMainWindow.myCanvas.CurrentDrawColor = Colors.Transparent;
                    theMainWindow.myCanvas.IsHitTestVisible = false;
                    //myCanvas.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
                }
            }
            else
            {


                //OPENED
                try
                {
                    // make sure the defaults are showing in the palette
                    myPalette.SelectDefaultButtons();
                    theMainWindow.myCanvas.IsHitTestVisible = true;
                    theMainWindow.myCanvas.Visibility = Visibility.Visible;
                    // set properties of the drawing canvas
                    if (myPalette.CurrentTool != null)
                    {
                        theMainWindow.myCanvas.CurrentButtonType = myPalette.CurrentTool.ButtonType;
                    }
                    else
                    {
                        theMainWindow.myCanvas.CurrentButtonType = PaletteButton.TYPE_DEFAULT_DESC;
                    }
                    if (myPalette.CurrentColor != null)
                    {
                        theMainWindow.myCanvas.CurrentDrawColor = myPalette.CurrentColor.ButtonColor;
                    }
                    else
                    {
                        theMainWindow.myCanvas.CurrentDrawColor = Colors.Transparent;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
                }
            }

        }

        private void PaletteOpenedHandler(object sender, RoutedEventArgs e)
        {

        }

        #endregion




        private void AddPaletteToolButton(PaletteToolInfo btnInfo, Int32 toolRow, Int32 toolColumn)
        {
            try
            {
                // see if we are adding a Tool or a Color button
                if (btnInfo.ButtonType == PaletteButton.TYPE_COLOR_DESC)
                {
                    this.myPalette.AddColorButton(btnInfo.ButtonColor, btnWidth, btnHeight, scb,
                        btnImageWidth, btnImageHeight, btnInfo.IsDefault, toolRow, toolColumn);
                }
                else
                {
                    this.myPalette.AddToolButton(btnInfo.ButtonType, btnWidth, btnHeight, scb,
                        btnImageWidth, btnImageHeight, btnInfo.IsDefault, btnInfo.AlwaysOpen,
                        toolRow, toolColumn, 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR ** " + AppUtils.GetMethodName() + " ** " + ex.Message);
            }
        }

        private void bench_TouchDown(object sender, TouchEventArgs e)
        {
            bench.IsManipulationEnabled = true;
            isBenchTap = true;
            benchTouchOrTapTimer.Stop();
            benchTouchOrTapTimer.Enabled = true;
            benchTouchOrTapTimer.Start();
            isBenchTap = true;
        }

        private void bench_TouchUp(object sender, TouchEventArgs e)
        {
            if (isBenchTap == true)
            {
                benchTap(sender, e);
                isBenchTap = false;
            }
            bench.IsManipulationEnabled = false;
        }

        public void benchTap(object sender, TouchEventArgs e)
        {
           
        }

        private void bench_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        private void bench_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Canvas currentBench = sender as Canvas;
            Canvas rectToMove = (Canvas)currentBench.Children[0];
            rectToMove.NormalizeTransformGroup();

            MainWindow theMainWindow = (MainWindow)Application.Current.MainWindow;
            if (rectToMove.RenderTransform.Value.OffsetY + (e.DeltaManipulation.Translation.Y / theMainWindow.scalingValue) < 0 && rectToMove.RenderTransform.Value.OffsetY + (e.DeltaManipulation.Translation.Y / theMainWindow.scalingValue) > ((rectToMove.Children.Count - 6.5) * -reserveCardHeight))
            {
                if (currentBench.Name == "bench")   // home bench
                {
                    homeBenchScrollOffsetY = (e.DeltaManipulation.Translation.Y / theMainWindow.scalingValue) + rectToMove.RenderTransform.Value.OffsetY;
                }
                else // visitors bench
                {
                    visitorsBenchScrollOffsetY = (e.DeltaManipulation.Translation.Y / theMainWindow.scalingValue) + rectToMove.RenderTransform.Value.OffsetY;
                }

                rectToMove.OffsetTo(0.0, (e.DeltaManipulation.Translation.Y / theMainWindow.scalingValue) + rectToMove.RenderTransform.Value.OffsetY);
            }
        }

        private void benchTouchOrTapTimerElapsed(object sender, ElapsedEventArgs e)
        {
            benchTouchOrTapTimer.Stop();
            benchTouchOrTapTimer.Enabled = false;
            isBenchTap = false;
        }

        private void btnShowBench_TouchDown(object sender, TouchEventArgs e)
        {
         
        }


 
   

        public void showBench()
        {
          
        }

        public void hideBench()
        {
            MainWindow theMainWindow = (MainWindow)Application.Current.MainWindow;

            // Close the bench if it is not already
            if (controlPanelOpen == true)
            {
                int xPosOpen, camControlsX;

                // Set defaults
                if (this.rightSide)
                {
                    xPosOpen = 1854;
                    camControlsX = 340;
                    
                }
                else
                {
                    xPosOpen = -326;
                    camControlsX = -340;
                }

                // Slide the bench back
                this.OffsetTo(xPosOpen, 0, 0.5, AnimationTransitions.QuadEaseInOut, 0.0);
                controlPanelOpen = false;

            }
        }

        private void btnResetCards_TouchDown(object sender, TouchEventArgs e)
        {

        }


        private void lineupButtonTimer_Tick(object sender, EventArgs e)
        {
            lineupButtonTimer.Stop();

            // Reset buttons to their off state images
           // showLineupCardBrush.ImageSource = lineupOffBitmap;
        }

        private void btnShowHome_TouchDown(object sender, TouchEventArgs e)
        {

            
        }

        private void btnShowVisitors_TouchDown(object sender, TouchEventArgs e)
        {
 
        }

        private void btnShowLineupCard_TouchDown(object sender, TouchEventArgs e)
        {
           
        }

        private void btnHomeTeam_TouchDown(object sender, TouchEventArgs e)
        {
            showHomeBench();
        }

        private void btnAwayTeam_TouchDown(object sender, TouchEventArgs e)
        {
            showVisitorsBench();
        }

        public void showHomeBench()
        {
        }

        public void showVisitorsBench()
        {
        
        }

        public void deselectReserves(List<ReservePlayerCard> reserveList)
        {
            foreach (ReservePlayerCard reserve in reserveList)
            {
                if (reserve.isSelected == true)
                {
                    reserve.isSelected = false;
                    reserve.swatch.Visibility = Visibility.Visible;
                    break;  // there can only be one max selected
                }
            }
        }

        private void rotateField_buttonUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
         //   Debugger.Break();
        	theMainWindow.rotateField();
        }

 
    }
}
