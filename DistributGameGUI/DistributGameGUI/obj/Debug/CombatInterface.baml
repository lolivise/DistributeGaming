   M S B A M L   `   `   `  ����K  GDistributGameGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DistributGameGUIDistributGameGUI  5   	   &     DistributGameGUI.CombatInterface   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:DistributGameGUI     5   	   -   5          � Title$  CombatInterface��5   	   $��495.6��6!   $	��800��60   .��5	      � .+�5
   
   �� $��Combat Interface��6   $
��Left=�6+   $��
254,10,0,0q�6F   $	��Top=�6Z   $	��309��6r   $��Times New Roman)�6~   $��36(�6�   $
��Bold"�6�   �� 5   
   $��Allies State :��6   $
��Left=�6)   $��
578,77,0,0q�6D   $	��Top=�6X   $��29��6p   �� 5   
   -   5         ��Name$ 	hero_type��5      $��Your State :��6#   $
��Left=�6:   $��
65,274,0,0q�6U   $	��Top=�6i   �� 5   
   $��Abilities :��6   $
��Left=�6&   $��
76,313,0,0q�6A   $	��Top=�6U   �� 5   
   $��Boss :��6   $
��Left=�6!   $��325,110,0,0q�6<   $	��Top=�6Q   �� 5   
   -   5        $ 	selfState��5      $��Label��6#   $
��Left=�63   $��192,273,0,0q�6N   $	��Top=�6c    J� RenderTransformOrigin$ 0.485,-0.258&�6{   �� 5   
   $��Decision Time:��6   $
��Left=�6)   $��
65,159,0,0q�6D   $	��Top=�6X   �� 5   
   -   5        $ decision_Time��5      $�� ��6'   $
��Left=�62   $��
86,190,0,0q�6M   $	��Top=�6a   $��16(�6y   $
��Bold"�6�   �� 5   
   $��seconds��6   $
��Left=�6"   $��148,194,0,0q�6=   $	��Top=�6R   �� 5   
   -   5        $ ability_for_next_round��5      $��Label��60   $
��Left=�6@   $��162,313,0,0q�6[   $	��Top=�6p   $ 0.485,-0.258&�6�   �� 5   
   -   5        $ 	boss_name��5      $��	Boss Name��6#   $
��Left=�67   $��325,141,0,0q�6R   $	��Top=�6g   �� 5   
   -   5        $ boss_hp��5      $
��HP :��6!   $
��Left=�60   $��325,172,0,0q�6K   $	��Top=�6`   �� 5   
   -   5        $ boss_last_target��5      $��Last Target: ��6*   $
��Left=�6B   $��325,204,0,0q�6]   $	��Top=�6r   �� 5   
   -	   5         ��Name$ lvAbilitySelection��5      $
��Left=�6/   $	��100��6J   $��
65,344,0,0q�6W   $	��Top=�6k   $	��505��6�    �� View 5      �� 5      .P�5      �� $��Ability��6%    �� Width$	 500��66    �� DisplayMemberBinding 5   %   �� *abilityDetail+5      5      �� 5   
   -
   5        $ lvAllies��5      $
��Left=�6%   $	��202��6@   $��530,106,0,0q�6M   $	��Top=�6b   $	��244��6z    5       �� 5!      .P�5"      �� $
��User��6%   $ 80��63    5#   %   �� *name+�� 5$      $
��Hero��6%   $ 80��63    5%   %   �� *hero+�� 5&      $��HP��6%   $ 80��61    5'   %   �� *hp+5(      5*      �� 5+   
   -   5        $ 	currentHP��5+      $��Label��6#   $
��Left=�63   $��162,273,0,0q�6N   $	��Top=�6c   $��30��6{   $ 
0.78,0.586&�6�   �� 5,   
   -   5         ��Name$ btn_decision��5,      $��Decide��6'   $
��Left=�68   $��630,391,0,0q�6S   $	��Top=�6h   $��75��6�   6�   5.      5/      