   M S B A M L   `   `   `  ����G  CRWCS_Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:RWCS_DesktopRWCS_Desktop  5   	   !    RWCS_Desktop.SettingsWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:RWCS_Desktop     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ SettingsWindow��5	   	   $	��450��6    $	��400��6-   .��5
      � ���#DBDFEA.+�5   
   �� -   5         ��Name$ idLabel��5      $C��=Інтервал знімків екрану(секунди):��6!   $
��Left=�6M   $��
19,107,0,0q�6h   $	��Top=�6|   �� 5   
   -   5         ��Name$ 
saveButton��5      $��Зберегти��6%   $
��Left=�68   $��
72,255,0,0q�6S   $	��Top=�6g   $��26��6   $��66��6�   ���#4C4C6D
��� ���$
��Bold"�6�   �� 5   
   -   5        $ closeButton��5      $��
Вийти��6&   $
��Left=�66   $��218,255,0,0q�6Q   $	��Top=�6f   $��26��6~   $��66��6�   ���#4C4C6D
��� ���$
��Bold"�6�   �� 5   
   -   5         ��Name$ makeScreenshotsCheckBox��5      $G��AРобити знімки екрану під час роботи��64   $
��Left=�6b   $��	72,69,0,0q�6}   $	��Top=�6�   �� 5   
   -   5         ��Name$ intervalSlider��5      $
��Left=�6)   $��219,111,0,0q�6D   $	��Top=�6Y   $	��120��6q    � Maximum$	 295Q�6}    � Minimum$ 15Q�6�    � Value$ 15Q�6�   	 � SmallChange$	 5Q�6�   
 �� TickFrequency$
 5Q�6�    � LargeChange$ 5Q�6�   ���	#FF5853536�   �� 5   
   -   5        $ currentIntervalLabel��5      $�� ��6.   $
��Left=�69   $��344,107,0,0q�6T   $	��Top=�6i   $
��Bold"�6�   5      5      