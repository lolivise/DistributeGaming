   M S B A M L   `   `   `  ����K  GDistributGameGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DistributGameGUIDistributGameGUI  5   	   &     DistributGameGUI.PortalSelection   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:DistributGameGUI     5   	   -   5          � Title$  PortalSelection��5   	   $	��450��6!   $	��800��6.   .��5	      � .+�5
   
   �� $��Portal Selection��6   $
��Left=�6+   $��
317,10,0,0q�6F   $	��Top=�6Z    J� RenderTransformOrigin$ 0.505,0.531&�6r   $��22(�6�   $
��Bold"�6�   $��Times New Roman)�6�   �� 5   
   -   5         ��Name$ btn_Server1��5      $��Enter��6&   $
��Left=�66   $��229,333,0,0q�6Q   $	��Top=�6f   $	��100��6~   $��30��6�   $��Times New Roman)�6�   $��24(�6�   $
��Bold"�6�   �� 5   
   $��Friends��6   $
��Left=�6"   $��596,132,0,0q�6=   $	��Top=�6R   �� 5   
   -   5         ��Name$ welcome��5      $��Label��6!   $
��Left=�61   $��
525,83,0,0q�6L   $	��Top=�6`   �� 5   
   $��Server Info:��6   $
��Left=�6'   $��
64,124,0,0q�6B   $	��Top=�6V   $ -5.705,-0.305&�6n   �� 5   
   -   5        $ btn_Server1_View��5      $
��View��6+   $
��Left=�6:   $��
64,333,0,0q�6U   $	��Top=�6i   $	��100��6�   $��30��6�   $��Times New Roman)�6�   $��24(�6�   $
��Bold"�6�   6�   �� 5   
   -   5         ��Name$ lvFriendState��5      $
��Left=�6*   $	��170��6E   $��550,158,0,0q�6R   $	��Top=�6g   $	��145��6    �� View 5      �� 5      .P�5      �� $
��Name��6%    �� Width$ 70��63    �� DisplayMemberBinding 5   %   �� *name+�� 5      $��State��6%   $ 70��64    5   %   �� *state+5      5      �� 5   
   -   5        $ lvServerState��5      $
��Left=�6*   $	��133��6E   $��
64,155,0,0q�6R   $	��Top=�6f   $	��265��6~    5      �� 5      .P�5      �� $
��Name��6%   $ 70��63    5   %   �� *name+�� 5      $��State��6%   $ 70��64    5    %   �� *state+�� 5!      $��Number of player��6%   $	 120��6?    5"   %   �� *numberClient+5#      5%      5'      5(      