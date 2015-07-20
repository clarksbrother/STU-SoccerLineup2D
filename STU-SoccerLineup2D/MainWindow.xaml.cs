using Artefact.Animation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TelestrationWPF;
using WPF_VCS_Common;
using WPFMediaKit;
using System.Windows.Automation.Peers;

namespace STU_SoccerLineup2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region VCS functionality, global variable declarations

        private WPF_VCS_Common.WPF_VCS_COMMON objVCSCommon = new WPF_VCS_COMMON();

        public static TelestrationWPF.AvailableColors availColors = new TelestrationWPF.AvailableColors();
        // create global variables to keep track of current color
        private Color startingColor = availColors.GetColorByName("White");
        private String startingChipType = PaletteButton.TYPE_DRAW_DESC;
        private const string PALETTE_COLOR_FILE = "colorpalette.xml";

        public string globalPaletteButton;
        string globalXMLString;
        string saveFileName = "";
        string sceneLoadData;
        public Color globalColorButton;
        public double scalingValue = 1;

        SolidColorBrush scb = new SolidColorBrush();
        Double btnHeight = 50.0;
        Double btnWidth = 60.0;
        Double btnImageHeight = 45.0;
        Double btnImageWidth = 55.0;

        Point lastPlayerPointDot = new Point(-99999, -99999);

        bool isFieldRotated = false;

        PaletteToolInfo[] requestedTools = new PaletteToolInfo[10];

        System.Collections.ObjectModel.ObservableCollection<STU_SoccerLineup2D.Classes.TeamData> TeamInfoData = new ObservableCollection<STU_SoccerLineup2D.Classes.TeamData>();


        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            

            scalingValue = Convert.ToDouble(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) / 1080;
            ScaleTransform myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleY = scalingValue;
            myScaleTransform.ScaleX = scalingValue;
            myScaleTransform.CenterX = .5;
            myScaleTransform.CenterY = .5;

            RotateTransform myRotateTransform = new RotateTransform();
            myRotateTransform.Angle = 0;

            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.X = 0;
            myTranslate.Y = 0;

            SkewTransform mySkew = new SkewTransform();
            mySkew.AngleX = 0;
            mySkew.AngleY = 0;

            // Create a TransformGroup to contain the transforms 
            // and add the transforms to it. 
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myScaleTransform);
            myTransformGroup.Children.Add(myRotateTransform);
            myTransformGroup.Children.Add(myTranslate);
            myTransformGroup.Children.Add(mySkew);

            // Associate the transforms to the object 
            //scaledElements.RenderTransform = myTransformGroup;
            //scaledVideoElements.RenderTransform = myTransformGroup;


            try
            {
                if (DesignerProperties.GetIsInDesignMode(this) == false)
                {
                    // not in Blend...
                    // see if we have a save file name specified
                    saveFileName = ConfigurationManager.AppSettings["saveFileName"];
                    if (saveFileName != null && saveFileName.Length > 0)
                    {
                        string savedData = objVCSCommon.GetXMLFromFile(saveFileName);
                        if (savedData.Length > 0)
                        {
                            LoadData(savedData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("ERROR ** " + GetMethodName() + " ** " + ex.Message);
            }



            objVCSCommon.DoCommonSceneInitialization(this, false, false, false);

            // Hook into VCS Proxy
            objVCSCommon.InitializeProxySocket(ConfigurationManager.AppSettings["WPFProxyPort"], ConfigurationManager.AppSettings["WPFProxyAddress"]);
            objVCSCommon.MessageReceived += new WPF_VCS_COMMON.MessageReceivedEventHandler(objVCSCommon_MessageReceived);

            //rightTouchOrTapTimer.Interval = 200;
            //rightTouchOrTapTimer.Elapsed += new ElapsedEventHandler(rightTouchOrTapTimerElapsed);




            // catch the routed events

            // start off by initializing the tool palette
            // now initialize the drawing canvas
            this.InitializeDrawingCanvas();
            // get the palette colors from the XML file
            LoadColorsFromFile();
            //String[] colrs = availColors.GetColorNames();

            myCanvas.ControlScale = 1.33;

            //myCanvas.CurrentDrawColor = startingColor;
            //myCanvas.CurrentButtonType = startingChipType;

            fieldOn.Visibility = Visibility.Collapsed;
            playerOn.Visibility = Visibility.Visible;

            //lines.ManipulationDelta -= field_ManipulationDelta;
            //lines.ManipulationStarting -= field_ManipulationStarting;
            //lines.ManipulationInertiaStarting -= field_InertiaStarting;
            //lines.IsManipulationEnabled = false;

            for (int i = 0; i < field.Children.Count; i++)
            {
                if (field.Children[i] is FieldPlayer)
                {
                    FieldPlayer tempFieldPlayer = (FieldPlayer)field.Children[i];

                    tempFieldPlayer.ManipulationDelta += player_ManipulationDelta;
                    tempFieldPlayer.ManipulationStarting += player_ManipulationStarting;
                    tempFieldPlayer.ManipulationInertiaStarting += player_ManipulationInertiaStarting;
                    tempFieldPlayer.ManipulationCompleted += player_ManipulationCompleted;
                    tempFieldPlayer.IsManipulationEnabled = true;
                }

            }

            TransformGroup rotateTransformGroup = new TransformGroup();
            RotateTransform defaultRotateTransform = new RotateTransform();
            defaultRotateTransform.Angle = 0;

            rotateTransformGroup.Children.Add(defaultRotateTransform);
            
            // Associate the transforms to the object 
            fieldPlayers.RenderTransform = rotateTransformGroup;
            


        }

         private void LoadData(string sender)
        {
            try
            {
                Debugger.Break();
                scalingValue = Convert.ToDouble(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) / 1080;



                XmlDocument theXmlDocument = new XmlDocument();
                theXmlDocument.LoadXml(sender);

                XmlNodeList elemListTeams = theXmlDocument.GetElementsByTagName("visitorteam");
                string TeamInfoXML = elemListTeams[0].InnerText;
                XmlSerializer xmlSerializerRounds = new XmlSerializer(TeamInfoData.GetType());
                TeamInfoData = (ObservableCollection<STU_SoccerLineup2D.Classes.TeamData>)xmlSerializerRounds.Deserialize(new StringReader(TeamInfoXML));
                Debugger.Break();
            }
            catch { }

        }

        private void objVCSCommon_MessageReceived(object sender, MessageReceivedEventArgs e)
        {

           
            try
            {

                string delims = "|";
                string[] msgParts = e.Message.Split(delims.ToCharArray());

                if (msgParts.Length > 0)
                {
                    switch (msgParts[0])
                    {
                        case WPF_COMMANDS_BASE.CMD_WPF_RESET:


                            Dispatcher uiDispatcherLogoReset = this.Dispatcher;
                            //uiDispatcherLogoReset.Invoke(new resetDelegate(reset));

                            break;

                        case WPF_COMMANDS_BASE.CMD_WPF_LOAD_DATA:
                            if (msgParts.Length > 1)
                            {

                                this.Dispatcher.Invoke(new Action<string, string>(objVCSCommon.SaveXMLToFile), msgParts[1], saveFileName);
                                this.Dispatcher.Invoke(new Action<string>(LoadData), new object[] { msgParts[1] });

                                sceneLoadData = msgParts[1];

                                Dispatcher uiDispatcherReset1 = this.Dispatcher;
                            }
                            break;

                        //case "WPF_PLAYER_UP":
                        //    Dispatcher uiDispatcherPreviewPlayerUp = this.Dispatcher;
                        //    uiDispatcherPreviewPlayerUp.Invoke(new previewPlayerUpDelegate(previewPlayerUp));

                        //    break;
                        //case "WPF_PLAYER_DOWN":
                        //    Dispatcher uiDispatcherPreviewPlayerDown = this.Dispatcher;
                        //    uiDispatcherPreviewPlayerDown.Invoke(new previewPlayerDownDelegate(previewPlayerDown));
                        //    break;

                        //case "WPF_TEAM_UP":
                        //    Dispatcher uiDispatcherPreviewTeamUp = this.Dispatcher;
                        //    uiDispatcherPreviewTeamUp.Invoke(new previewTeamUpDelegate(previewTeamUp));

                        //    break;
                        //case "WPF_TEAM_DOWN":
                        //    Dispatcher uiDispatcherPreviewTeamDown = this.Dispatcher;
                        //    uiDispatcherPreviewTeamDown.Invoke(new previewTeamDownDelegate(previewTeamDown));
                        //    break;
                        //case "WPF_SHOW_POSITION":
                        //    Dispatcher uiDispatcherPreviewPosition = this.Dispatcher;
                        //    uiDispatcherPreviewPosition.Invoke(new previewPositionDelegate(previewPosition), msgParts[1]);
                        //    break;
                        //case "WPF_SHOW_ROUND":
                        //    Dispatcher uiDispatcherPreviewRound = this.Dispatcher;
                        //    uiDispatcherPreviewRound.Invoke(new previewRoundDelegate(previewRound), Convert.ToInt32(msgParts[1]));
                        //    break;
                        //case "WPF_SPONSOR_POS":
                        //    if (msgParts.Length > 1)
                        //    {


                        //        string[] sponsorValues = msgParts[1].Split(' ');

                        //        Dispatcher uiDispatcherScaleSponsor = this.Dispatcher;
                        //        uiDispatcherScaleSponsor.Invoke(new scaleSponsorDelegate(scaleSponsor), sponsorValues[0], sponsorValues[1], sponsorValues[2]);

                        //    }
                        //    break;


                    }
                }
            }
            catch
            {

            }
        }

        private class _FakeWindowsPeer : WindowAutomationPeer
        {
            public _FakeWindowsPeer(Window window)
                : base(window)
            { }
            protected override List<AutomationPeer> GetChildrenCore()
            { return null; }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        { return new _FakeWindowsPeer(this); }



        private void InitializeDrawingCanvas()
        {
            try
            {
                // set the parent handle for the Magnifying Glass
                // the magnifying glass needs a reference to the Canvas
                // directly under the main  window

                myCanvas.ParentUIElement = field;
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



        private void field_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        private void field_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Get the Rectangle and its RenderTransform matrix.
            
            //Canvas rectToMove = e.OriginalSource as Canvas;
            Canvas rectToMove = (Canvas)field;

            Matrix rectsMatrix = ((MatrixTransform)rectToMove.RenderTransform).Matrix;


            // Rotate the Rectangle.
            //rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
            //                     e.ManipulationOrigin.X + 4000 - (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2),
            //                     e.ManipulationOrigin.Y + 5520 - (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2));

            // Resize the Rectangle.  Keep it square 
            // so use only the X value of Scale.
            rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
                                e.DeltaManipulation.Scale.X,
                                e.ManipulationOrigin.X + 4000 - (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2),
                                 e.ManipulationOrigin.Y + 5520 - (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2));

            myCanvas.ControlScale = (1 / field.RenderTransform.Value.M22);


            for (int i = 0; i < field.Children.Count; i++)
            {
                if (field.Children[i] is FieldPlayer)
                {
                    FieldPlayer tempFieldPlayer = (FieldPlayer)field.Children[i];
                    
                    //tempFieldPlayer.NormalizeTransformGroup();
                    //tempFieldPlayer.ScaleTo(1 / field.RenderTransform.Value.M22, 1 / field.RenderTransform.Value.M22);

                    ScaleTransform myScaleTransform = new ScaleTransform();
                    myScaleTransform.ScaleY = 1 / field.RenderTransform.Value.M22;
                    myScaleTransform.ScaleX = 1 / field.RenderTransform.Value.M22;

                    RotateTransform myRotateTransform = new RotateTransform();
                    myRotateTransform.Angle = 0;

                    //TranslateTransform myTranslate = new TranslateTransform();
                    //myTranslate.X = (tempFieldPlayer.RenderTransform.Value.OffsetX * field.RenderTransform.Value.M22);// -tempFieldPlayer.RenderTransform.Value.OffsetX;
                    //myTranslate.Y = (tempFieldPlayer.RenderTransform.Value.OffsetY * field.RenderTransform.Value.M22);// -tempFieldPlayer.RenderTransform.Value.OffsetY;

                    SkewTransform mySkew = new SkewTransform();
                    mySkew.AngleX = 0;
                    mySkew.AngleY = 0;

                    // Create a TransformGroup to contain the transforms 
                    // and add the transforms to it. 
                    TransformGroup myTransformGroup = new TransformGroup();
                    myTransformGroup.Children.Add(myScaleTransform);
                    myTransformGroup.Children.Add(myRotateTransform);
                   // myTransformGroup.Children.Add(myTranslate);
                    myTransformGroup.Children.Add(mySkew);

                    // Associate the transforms to the object 
                    tempFieldPlayer.playerCanvas.RenderTransform = myTransformGroup; 
                    
                }
            }

            // Move the Rectangle.
            if (isFieldRotated == false)
            {
                rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
                                      e.DeltaManipulation.Translation.Y);
            }
            else
            {
                rectsMatrix.Translate(e.DeltaManipulation.Translation.Y,
                                      -e.DeltaManipulation.Translation.X);
            }
            // Apply the changes to the Rectangle.
            rectToMove.RenderTransform = new MatrixTransform(rectsMatrix);









            Rect containingRect =
                new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

            Rect shapeBounds =
                rectToMove.RenderTransform.TransformBounds(
                    new Rect(rectToMove.RenderSize));

            // Check if the rectangle is completely in the window.
            // If it is not and intertia is occuring, stop the manipulation.
            if (e.IsInertial && !containingRect.Contains(shapeBounds))
            {
                e.Complete();
            }


            e.Handled = true;
        }

        private void field_InertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
        }

        private void player_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        private void player_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Get the Rectangle and its RenderTransform matrix.

            //Canvas rectToMove = e.OriginalSource as Canvas;
            FieldPlayer rectToMove = (FieldPlayer)sender;

            rectToMove.NormalizeTransformGroup();

            if (isFieldRotated == false)
            {
                rectToMove.OffsetTo((rectToMove.RenderTransform.Value.OffsetX + (e.DeltaManipulation.Translation.X / field.RenderTransform.Value.M22)), (rectToMove.RenderTransform.Value.OffsetY + (e.DeltaManipulation.Translation.Y / field.RenderTransform.Value.M22)));
            }
            else
            {
                rectToMove.OffsetTo((rectToMove.RenderTransform.Value.OffsetX + (e.DeltaManipulation.Translation.Y / field.RenderTransform.Value.M22)), (rectToMove.RenderTransform.Value.OffsetY + (-e.DeltaManipulation.Translation.X / field.RenderTransform.Value.M22)));
            }

            //Debugger.Break();

            double a = lastPlayerPointDot.X - (rectToMove.RenderTransform.Value.OffsetX + (e.DeltaManipulation.Translation.X / field.RenderTransform.Value.M22));
            double b = lastPlayerPointDot.Y - (rectToMove.RenderTransform.Value.OffsetY + (e.DeltaManipulation.Translation.Y / field.RenderTransform.Value.M22));
            double distance = Math.Sqrt(a * a + b * b);
            //lastPlayerPointDot = new Point(rectToMove.RenderTransform.Value.OffsetX, rectToMove.RenderTransform.Value.OffsetY);

            if (distance > (45 / field.RenderTransform.Value.M22))
            {
                lastPlayerPointDot = new Point(rectToMove.RenderTransform.Value.OffsetX, rectToMove.RenderTransform.Value.OffsetY);

                FieldPlayer tempFieldPlayer = (FieldPlayer)sender;

                Point locationFromWindow = tempFieldPlayer.TranslatePoint(new Point(0, 0), this);
                locationFromWindow.X = locationFromWindow.X / tempFieldPlayer.RenderTransform.Value.M22;
               
                Point locationFromScreen = tempFieldPlayer.PointToScreen(locationFromWindow);
                tempFieldPlayer.number.Text = (84 - (0.5 * ((1 / field.RenderTransform.Value.M22) - 1) * 168)).ToString();
                tempFieldPlayer.name.Text = (e.ManipulationOrigin.X - locationFromWindow.X).ToString();
                //if (e.ManipulationOrigin.X - locationFromWindow.X < 84 - (0.25 * ((1 / field.RenderTransform.Value.M22) - 1) * 168))
                if (tempFieldPlayer.playerTrailEnabled == true)
                {

                    Ellipse tempEllipse = new Ellipse();
                    tempEllipse.Height = 15 / field.RenderTransform.Value.M22;
                    tempEllipse.Width = 15 / field.RenderTransform.Value.M22;

                    tempEllipse.Fill = tempFieldPlayer.swatchBackground.Fill;
                    //tempEllipse.Fill = new SolidColorBrush(Colors.Red);

                    ScaleTransform ellipseScaleTransform = new ScaleTransform();
                    ellipseScaleTransform.ScaleY = 1;// / field.RenderTransform.Value.M22;
                    ellipseScaleTransform.ScaleX = 1;// / field.RenderTransform.Value.M22;
                    ellipseScaleTransform.CenterX = .5;
                    ellipseScaleTransform.CenterY = .5;

                    RotateTransform ellipseRotateTransform = new RotateTransform();
                    ellipseRotateTransform.Angle = 0;

                    TranslateTransform ellipseTranslate = new TranslateTransform();
                    ellipseTranslate.X = lastPlayerPointDot.X + rectToMove.Width / 2;// / field.RenderTransform.Value.M22;
                    ellipseTranslate.Y = lastPlayerPointDot.Y + rectToMove.Height / 2;// / field.RenderTransform.Value.M22;

                    SkewTransform ellipseSkew = new SkewTransform();
                    ellipseSkew.AngleX = 0;
                    ellipseSkew.AngleY = 0;

                    // Create a TransformGroup to contain the transforms 
                    // and add the transforms to it. 
                    TransformGroup ellipseTransformGroup = new TransformGroup();
                    ellipseTransformGroup.Children.Add(ellipseScaleTransform);
                    ellipseTransformGroup.Children.Add(ellipseRotateTransform);
                    ellipseTransformGroup.Children.Add(ellipseTranslate);
                    ellipseTransformGroup.Children.Add(ellipseSkew);

                    tempEllipse.RenderTransform = ellipseTransformGroup;

                    playerDots.Children.Add(tempEllipse);
                }
                //MessageBox.Show(playerDots.Children.Count.ToString());

                
            }
            //Matrix rectsMatrix = ((MatrixTransform)rectToMove.RenderTransform).Matrix;
                      
            //// Move the Rectangle.
            //rectsMatrix.Translate(e.DeltaManipulation.Translation.X / field.RenderTransform.Value.M22,
            //                      e.DeltaManipulation.Translation.Y / field.RenderTransform.Value.M22);

            //// Apply the changes to the Rectangle.
            //rectToMove.RenderTransform = new MatrixTransform(rectsMatrix);

            Rect containingRect =
                new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

            Rect shapeBounds =
                rectToMove.RenderTransform.TransformBounds(
                    new Rect(rectToMove.RenderSize));

            // Check if the rectangle is completely in the window.
            // If it is not and intertia is occuring, stop the manipulation.
            if (e.IsInertial && !containingRect.Contains(shapeBounds))
            {
                e.Complete();
            }


            e.Handled = true;
        }

        private void player_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
        }

        private void player_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

            FieldPlayer tempFieldPlayer = (FieldPlayer)sender;
            tempFieldPlayer.playerTrailEnabled = false;
            //tempFieldPlayer.NormalizeTransformGroup();
            //tempFieldPlayer.OffsetTo(tempFieldPlayer.RenderTransform.Value.OffsetX, tempFieldPlayer.RenderTransform.Value.OffsetY);
            //e.Handled = true;
        }

        private void fieldActivate(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
;
                fieldOn.Visibility = Visibility.Visible;
                playerOn.Visibility = Visibility.Collapsed;

                lines.ManipulationDelta += field_ManipulationDelta;
                lines.ManipulationStarting += field_ManipulationStarting;
                lines.ManipulationInertiaStarting += field_InertiaStarting;
                lines.IsManipulationEnabled = true;


                //for (int i = 0; i < field.Children.Count; i++)
                //{
                //    if (field.Children[i] is FieldPlayer)
                //    {
                //        FieldPlayer tempFieldPlayer = (FieldPlayer)field.Children[i];
                //        tempFieldPlayer.ManipulationDelta -= player_ManipulationDelta;
                //        tempFieldPlayer.ManipulationStarting -= player_ManipulationStarting;
                //        tempFieldPlayer.ManipulationInertiaStarting -= player_ManipulationInertiaStarting;
                //        tempFieldPlayer.ManipulationCompleted -= player_ManipulationCompleted;
                //    }

                //}
            }
            catch
            {

                MessageBox.Show("SOMETHINGS WRONG IN THE FIELD ACTIVATE");
            }

        }

        private void playerActivate(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
               
                fieldOn.Visibility = Visibility.Collapsed;
                playerOn.Visibility = Visibility.Visible;

                //lines.ManipulationDelta -= field_ManipulationDelta;
                //lines.ManipulationStarting -= field_ManipulationStarting;
                //lines.ManipulationInertiaStarting -= field_InertiaStarting;
                //lines.IsManipulationEnabled = false;

                for (int i = 0; i < field.Children.Count; i++)
                {
                    if (field.Children[i] is FieldPlayer)
                    {
                        FieldPlayer tempFieldPlayer = (FieldPlayer)field.Children[i];

                        tempFieldPlayer.ManipulationDelta += player_ManipulationDelta;
                        tempFieldPlayer.ManipulationStarting += player_ManipulationStarting;
                        tempFieldPlayer.ManipulationInertiaStarting += player_ManipulationInertiaStarting;
                        tempFieldPlayer.ManipulationCompleted += player_ManipulationCompleted;
                    }

                }
            }
            catch
            {
                MessageBox.Show("SOMETHINGS WRONG IN THE PLAYER ACTIVATE");
            }
        }
    
        public void rotateField()
        {

            fieldPlayersRotate.NormalizeTransformGroup();

            if (isFieldRotated == true)
            {
                isFieldRotated = false;
                fieldPlayersRotate.RotateTo(0, 0.5, AnimationTransitions.QuadEaseInOut, 0.0);
                
                for (int i = 4; i < field.Children.Count; i++)
                {
                    FieldPlayer theTempPlayer = (FieldPlayer)field.Children[i];
                    theTempPlayer.NormalizeTransformGroup();
                    theTempPlayer.RenderTransformOrigin = new Point(0.5, 0.5);
                    theTempPlayer.RotateTo(0, 0.5, AnimationTransitions.QuadEaseInOut, 0.0);
                }
            }
            else
            {
                isFieldRotated = true;
                fieldPlayersRotate.RotateTo(90, 0.5, AnimationTransitions.QuadEaseInOut, 0.0);

                for (int i = 4; i < field.Children.Count; i++)
                {
                    FieldPlayer theTempPlayer = (FieldPlayer)field.Children[i];
                    theTempPlayer.NormalizeTransformGroup();
                    theTempPlayer.RenderTransformOrigin = new Point(0.5, 0.5);
                    theTempPlayer.RotateTo(-90, 0.5, AnimationTransitions.QuadEaseInOut, 0.0);
                }
            }


        }
            
    }
}
