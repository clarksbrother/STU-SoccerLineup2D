   M S B A M L   `   `   `  ����  STU-SoccerLineup2D*    $STU_SoccerLineup2D.ReservePlayerCard   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5	      �� -   5          ��Name$  reservePlayerCardCanvas��5	      $	��107��6.   $	��326��6;   .+�5
   
   �� -   5         ��Name$ darkBG��5
      $	��107��6$   $	��326��61   ��5      �� 5       �� Color$ #9C9B9B��6"   5      �� 5   
   -   5        $ swatch��5      $	��107��6$   $	��326��61   ��5      �� 5       j� 
AlignmentX$
 Left=�6    j� 
AlignmentY$	 Top=�6/   -   5        swatchBrush�� �� ImageSource$/ )Images/Sample/GREENBAY_PACKERS_SWATCH.PNG��5   U    j� Stretch$
 None=�6�    �� RelativeTransform 5      V� 5      .~�5      ��  �� ScaleY$
 1000Q�6-   	 �� ScaleX$
	 1000Q�6;   �� 5      �� 5      T� 5      
 T� Y$

 -100Q�61   5      5      5      �� 5   
   -   5        $ swatchOverlay��5      $	��107��6+   $	��326��68   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5       �  � Color$ 	#88000000��6#    � Offset$ 1Q�65   � 5!      $ 	#00000000��6#   5"      5$      �� 5%   
   -   5        $  headshot��5%      $	��107��6#   $	��326��60    �� Left$ 0��6<   
 �� Top$ 0��6L    J� RenderTransformOrigin$ 0.5,0.5&�6[   $��Center=�6{   $��Center=�6�   ��5&      �� 5'      -   5        headshotBrush��$  Images/Sample/headshot.png��5'   4   $ Uniform=�6]    5(      V� 5)      .~�5*      ��  �� CenterY$	 0.5Q�6-    �� CenterX$	 0.5Q�6;   $		 1.9Q�6I   $	 1.9Q�6V   �� 5+       �� CenterY$	 0.5Q�6,    �� CenterX$	 0.5Q�6:   �� 5,       �� CenterY$	 0.5Q�6.    �� CenterX$	 0.5Q�6<   T� 5-      $

 0.38Q�61    T� X$ -0.36Q�6:   5.      50      52      (� 53   
   -   5         (�Name$ firstNameShadowViewbox��53      $
��Left=�62    (� StretchDirection$ DownOnly=�6M    (� Stretch$
 Fill=�6i   $��56��6x   $ 88.468��6�   $ 13.371��6�   $��187.532��6�   .��54      �� -	   5         ��Name$ firstNameShadow��54      $
��Wrap=�61   $��
First Name��6E   $N��H/STU-SoccerLineup2D;component/Fonts/#Touchscreen  Swiss721 BoldCondensed)�6W   $��40(�6�   
���   �6�   55      (� 56   
   -
   5        $ firstNameViewbox��56      $
��Left=�6,   $ DownOnly=�6G   $
 Fill=�6c   $��56��6r   $ 88.468��6~   $ 14.371��6�   $��187.532��6�   .��57      �� -   5        $ firstNameText��57      $
��Wrap=�6/   $��
First Name��6C   $N��H/STU-SoccerLineup2D;component/Fonts/#Touchscreen  Swiss721 BoldCondensed)�6U   $��40(�6�   ��58      �� 59      $ 0.5,1&�6*   $ 0.5,0&�6;   .��5:      � $ 	#FFE3E1E2��6'   $ 1Q�69   � 5;      $ White��6'   5<      5>      5?      (� 5@   
   -   5        $ lastNameShadowViewbox��5@      $
��Left=�61   $ DownOnly=�6L   $
 Fill=�6h   $��56��6w   $ 88.468��6�   $ 51��6�   $��187.532��6�   .��5A      �� -   5        $ lastNameShadow��5A      $
��Wrap=�60   $��	LAST NAME��6D   $N��H/STU-SoccerLineup2D;component/Fonts/#Touchscreen  Swiss721 BoldCondensed)�6U   $��40(�6�   
���   �6�   5B      (� 5C   
   -   5        $ lastNameViewbox��5C      $
��Left=�6+   $ DownOnly=�6F   $
 Fill=�6b   $��56��6q   $ 88.468��6}   $ 52��6�   $��187.532��6�   .��5D      �� -   5        $ lastNameText��5D      $
��Wrap=�6.   $��	LAST NAME��6B   $N��H/STU-SoccerLineup2D;component/Fonts/#Touchscreen  Swiss721 BoldCondensed)�6S   $��40(�6�   ��5E      �� 5F      $ 0.5,1&�6*   $ 0.5,0&�6;   .��5G      � $ 	#FFE3E1E2��6'   $ 1Q�69   � 5H      $ White��6'   5I      5K      5L      5N      5P      