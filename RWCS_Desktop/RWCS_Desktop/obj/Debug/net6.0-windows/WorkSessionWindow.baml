   M S B A M L   `   `   `  ����G  CRWCS_Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:RWCS_DesktopRWCS_Desktop  5   	   $    RWCS_Desktop.WorkSessionWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:RWCS_Desktop     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ WorkSessionWindow��5	   	   $	��450��6#   $
��1000��60   .��5
      � ���#DBDFEA.+�5   
   �� -   5         ��Name$ closeButton��5      $��Закрити��6&   $
��Left=�68   $��
626,10,0,0q�6S   $	��Top=�6g   $��33��6   $��66��6�   6�   � 5   
   .+�5      �� -   5         ��Name$ screenshotImage��5      $��294,54,35,54q�6-   6E   �� 5      $��Name:��6   $
��Left=�6$   $��	38,94,0,0q�6?   $	��Top=�6R   �� 5      $��ProjectMemberId:��6   $
��Left=�6/   $��	38,53,0,0q�6J   $	��Top=�6]   �� 5      -   5         ��Name$ projectMemberIdLabel��5      $�� ��62   $
��Left=�6=   $��
154,53,0,0q�6X   $	��Top=�6l   �� 5      $��Date: ��6   $
��Left=�6%   $��
38,219,0,0q�6@   $	��Top=�6T   �� 5      -   5        $ 	dateLabel��5      $�� ��6'   $
��Left=�62   $��
91,219,0,0q�6M   $	��Top=�6a   �� 5      $��Sprint:��6   $
��Left=�6&   $��
38,133,0,0q�6A   $	��Top=�6U   �� 5      $��Task:��6   $
��Left=�6$   $��
38,174,0,0q�6?   $	��Top=�6S   �� 5      $��WorkSessionId:��6   $
��Left=�6-   $��	38,10,0,0q�6H   $	��Top=�6[   �� 5      $��	WorkTime:��6   $
��Left=�6(   $��
38,265,0,0q�6C   $	��Top=�6W    J� RenderTransformOrigin$ 0.452,0.587&�6o   �� 5      -   5        $ workTimeLabel��5      $�� ��6+   $
��Left=�66   $��116,265,0,0q�6Q   $	��Top=�6f   �� 5      -   5        $ 	taskLabel��5      $�� ��6'   $
��Left=�62   $��
91,174,0,0q�6M   $	��Top=�6a   �� 5      -   5        $ sprintLabel��5      $�� ��6)   $
��Left=�64   $��
97,133,0,0q�6O   $	��Top=�6c   �� 5      -   5        $ 	nameLabel��5      $�� ��6'   $
��Left=�62   $��	97,94,0,0q�6M   $	��Top=�6`   �� 5      -	   5        $ workSessionIdLabel��5      $��Label��60   $
��Left=�6@   $��
147,10,0,0q�6[   $	��Top=�6o   �� 5      -
   5        $ screenshotStatus��5      $�� ��6.   $
��Left=�69   $��
320,53,0,0q�6T   $	��Top=�6h   �� 5      -   5        $ loadScreenshotsButton��5      $6��0Завантажити знімки екрану��64   $
��Left=�6X   $��
350,12,0,0q�6s   $	��Top=�6�   $��33��6�   $	��161��6�   6�   �� 5      -   5        $ prevScreenshotButton��5      $	��<<<��63   $
��Left=�6J   $��
409,0,0,16q�6e   $��62��6y   $��28��6�   $��Bottom=�6�   �� 5       -   5        $ nextScreenshotButton��5       $	��>>>��63   $
��Left=�6J   $��
501,0,0,16q�6e   $��63��6y   $��28��6�   $��Bottom=�6�   �� 5!      -   5        $  currentScreenShotDataLabel��5!      $�� ��68   $
��Left=�6C   $��642,397,0,0q�6^   $	��Top=�6s   5"      5#      5$      