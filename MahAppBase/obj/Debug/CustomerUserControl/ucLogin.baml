   M S B A M L   `   `   `  ����E  AMahAppBase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MahAppBase
MahAppBase  5   	       MahAppBase.ucLogin   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:MahAppBase     5   	     � Title$  登入��5   	   $	��300��6   $	��600��6!   .��5	      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   �� -   5         ��Name$ 	stWelcome��5   ,    �� Orientation$ Vertical=�6   .+�5      �� $3��-歡迎使用英語口語朗讀與診斷系統��6   $��34(�6/    �� TextAlignment$ Center=�6=   $��0,30,0,0q�6T   �� 5      -   5         ��Name$ btNext��5   $   $��進入系統��6   $��30,30, 30, 0q�64   $��15(�6J   $	��150��6X   6d   5      � 5   
   -   5         �Name$ gdLogin��5   3   $��0��6   $y�	Collapsed=�6    5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    .+�5      �� $ ��輸入課程資料以及ID��6   $��25(�6*   $ Center=�68   �� 5      $��1��6   $ 
Horizontal=�6&   $��Center=�6?   $��0,15,0,0q�6\   .+�5      �� $��書本名稱��6   $	��100��6(   �� 5      -   5         ��Name$ tbBook��5   >   $��Reading_Voyage_1��6   $	��200��62   6>   5       �� 5!      $��2��6   $ 
Horizontal=�6&   $��Center=�6?   $��0,15,0,0q�6\   .+�5"      �� $��單　　元��6   $	��100��6(   �� 5#      -   5        $ tbLesson��5#   4   $��Unit15��6   $	��200��6(   64   5$      �� 5%      $��3��6   $ 
Horizontal=�6&   $��Center=�6?   $��0,15,0,0q�6\   .+�5&      �� $��登入使用者名稱��6   $	��100��6+   �� 5'      -   5        $ 	tbStudent��5'   2   $
��TEST��6   $	��200��6&   62   5(      �� 5)      -   5        $ btLogin��5)   ]   $��確認登入��6   $��4��6$   $��0,20,0,0q�61   $	��150��6C   $��15(�6O   6n   5*      5,      5-      