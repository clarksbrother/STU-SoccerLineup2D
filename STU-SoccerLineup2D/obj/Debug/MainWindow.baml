   M S B A M L   `   `   `  ����M  ISTU-SoccerLineup2D, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:STU_SoccerLineup2DSTU_SoccerLineup2D  5      I EVideoPlayerWPF, Version=1.4.4.2, Culture=neutral, PublicKeyToken=nullG4clr-namespace:VideoPlayerWPF;assembly=VideoPlayerWPFVideoPlayerWPF 5      J FTelestrationWPF, Version=1.9.1.1, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:TelestrationWPF;assembly=TelestrationWPFTelestrationWPF 5      F BWPFMediaKit, Version=1.4.0.2, Culture=neutral, PublicKeyToken=nullfBclr-namespace:WPFMediaKit.DirectShow.Controls;assembly=WPFMediaKitWPFMediaKit.DirectShow.Controls 5      #    STU_SoccerLineup2D.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      2local clr-namespace:STU_SoccerLineup2D     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6?   OVideoPlayerWPF4clr-namespace:VideoPlayerWPF;assembly=VideoPlayerWPF     5      RTelestrationWPF6clr-namespace:TelestrationWPF;assembly=TelestrationWPF     5      aDirectShowControlsBclr-namespace:WPFMediaKit.DirectShow.Controls;assembly=WPFMediaKit     5      -   5          �Name$  Window��5
       � Title$ 
MainWindow��5      $
��1920��5      $
��1080��6    � WindowStyle$
 None=�6   ��6G   Z� ��6]   Z� .��5      �� -   5         ��Name$ visibleCanvas��5      $
��1920��6$   $
��1080��61   $
��Left=�6?   $	��Top=�6Z    J� RenderTransformOrigin$ 0.5,0.5&�6r   ��5   
   �� 5       �� ImageSource$  Images/Main/background.jpg��6   .+�5   
   �� -   5        $ fieldPlayersRotate��5      $��11040��6-    �� Left$ -3044��6<   
 �� Top$ -4960��6P   $
��8000��6c   $ 0.5,0.5&�6p   .+�5      �� -   5        $ fieldPlayers��5      $��11040��6(   $ 0��67   $ 0��6G   $
��8000��6V   $ 0.5,0.5&�6c   .+�5      �� -   5        $ field��5      $��11040��6"   $ 0��61   $ 0��6A   $
��8000��6P   $	 0,0&�6]   .+�5      �� -   5         ��Name$ grass��5      $��31040��6&   $ -10000��65   
���   �$��28000��6Y   $ -10000��6g   $��0��6{   ��5      �� 5      $! Images/SoccerFieldGrass.png��6   	 j� Stretch$
	 None=�6D   
 j� TileMode$

 Tile=�6S    j� Viewport$ 0, 0, 2251, 1598�6c    j� ViewportUnits$ Absolute=�6   5      �� 5      -   5         ��Name$ lines��5      $
��7459��6"   $
 7388��60   $!��Images/SoccerFieldLines.png��6C   $	 256��6h   $��11184��6y    J� IsManipulationEnabled . z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 90Q�6    T� 5      5       5"      #  TelestrationWPF.DrawingCanvas  5#      -   5          Name$ myCanvas��5#   +      CanvasWidth$
 8000Q�6=      CanvasHeight$ 11040Q�6P    J� IsHitTestVisible .  $ 0��6~   $ 0��6�   $��0q�6�   $��11040��6�   $
��8000��6�   $y�Visible=�6�   �� 5$      -	   5        $ 
playerDots��5$      $��11040��6(   $ 0��67   $ 0��6G   $
��8000��6V   $	 0,0&�6c   $   STU_SoccerLineup2D.FieldPlayer  5%      $ 0��6   $ 0��6/    . $ 0.5,0.5&�6[   z�5&      V� 5'      .~�5(      �� �� 5)      �� 5*      T� 5+       T� X$
 4000Q�6#    T� Y$
 4000Q�6,   5,      5.        5/      $ 0��6   $ 0��6/    . $ 0.5,0.5&�6[   z�50      V� 51      .~�52      �� �� 53      �� 54      T� 55      $
 4000Q�6#   $
 5520Q�6,   56       � LayoutTransform 58      V� 59      .~�5:      ��  �� CenterX$	 0.5Q�6    �� CenterY$	 0.5Q�6-   �� 5;       �� CenterX$	 0.5Q�6    �� CenterY$	 0.5Q�6,   �� 5<       �� CenterX$	 0.5Q�6     �� CenterY$	 0.5Q�6.   T� 5=      5>      5@      5A      5B      5C      %   STU_SoccerLineup2D.ControlPanel  5D   
   $
 1854��6   $	 175��60   �� 5E   
   -
   5        $ 	rightMenu��5E      $	��905��6$   $
 1526��61   $	��394��6D   $	 175��6P   $ 0.5,0.5&�6a   {�.  $y�	Collapsed=�6�    .  z�5F      V� 5G      .~�5H      �� �� 5I      �� 5J      T� 5K      $	 328Q�6)   5L      .+�5N      �� -   5        $ 	menuBarBG��5N      $	��905��6'   $!��Images/Reserves/menuBar.png��64   $
��Fill=�6Y   $��66��6h   �� 5O      -   5        $ trayIn��5O      $��31��6(   $ 11.92��64   
���   �$
 18.5��6W   $��43��6i   $��0��6t   ��5P      �� 5Q      $$ Images/Reserves/btnCloseOn.png��6!   $
	 None=�6N   5S      �� 5T      -   5        $ trayOut��5T      $��31��6)   $ 11.92��65   
���   �$
 18.5��6X   $��43��6j   $��0��6u   ��5U      �� 5V      $# Images/Reserves/btnOpenOn.png��6!   $
	 None=�6M   5X      �� 5Y      -   5        $ fieldOff��5Y      $��31��6*   $ 11.92��66   
���   �$
 68.5��6Y   $��43��6k   $��0��6v   ��5Z      �� 5[      $% Images/Reserves/btnFieldOff.png��6!   $
	 None=�6O   5]      �� 5^      -   5        $ fieldOn��5^      $��31��6)   $ 11.92��65   
���   �$
 68.5��6X   $��43��6j   $��0��6u   ��5_      �� 5`      $$ Images/Reserves/btnFieldOn.png��6!   $
	 None=�6N   5b      �� 5c      -   5        $ 	playerOff��5c      $��31��6+   $ 11.92��67   
���   �$	 119��6Z   $��43��6k   $��0��6v   ��5d      �� 5e      $&  Images/Reserves/btnPlayerOff.png��6!   $
	 None=�6P   5g      �� 5h      -   5        $ playerOn��5h      $��31��6*   $ 11.92��66   
���   �$	 119��6Y   $��43��6j   $��0��6u   ��5i      �� 5j      $% Images/Reserves/btnPlayerOn.png��6!   $
	 None=�6O   5l      $  TelestrationWPF.DynamicPalette  5m      -   5          Name$ 	myPalette��5m   -   $ 0��6@   $	 853��6P    . $y�Visible=�6y    K� ZIndex$
 1001��6�   5n      5p      5q      