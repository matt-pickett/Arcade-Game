   M S B A M L   `   `   `  ����E  ASnake Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Pacman_GamePacman_Game  5   	       Pacman_Game.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Pacman_Game     5   	   -   5          � Title$  Snake Game - Score: 0��5   	    � SizeToContent$ WidthAndHeight=�6'    � 
ResizeMode$ NoResize=�6�    � WindowStyle$
 None=�6�   
���   �.��5	      ]� 
���   �.+�5
      � -   5        $��pnlTitleBar��5
      $	��Top=�6	    � ColumnDefinitions 5      �� 5      $��* �6   �� 5      $��* �6   �� 5      $
��Auto �6    � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       �� 5      " ��6   .�5      ��  � ��$ Consolas)�6$   �� 5       � ��
 �����6$   �� 5       � ��$ 24(�6"   �� 5       � ��$
 Bold"�6$   5      5      .+�5      � $��	10,0,10,0q�6   .+�5      �� Score:6   �� 5      -   5        $��tbStatusScore��5      06(   5      � 5      $��	10,0,10,0q�6   $��1��6"   $��Right=�62   .+�5      �� Lives:6   �� 5       -   5        $��tbStatusLives��5       $��Right=�6%   06D   5!      �� 5"      -   5        $��btnClose��5"   �   $��2��6   $��Right=�6   
������ 
�������$
��Bold"�6_   $��20(�6q   $��0q�6   $
��10,0q�6�   .��6�   X6�   5#      �� 5$      
���   �$��5q�6   ._�5%      �� -   5        $��GameArea��5%      $
}�True��6   $	��500��60   $	��300��6<   5&      5'      5(      5)      