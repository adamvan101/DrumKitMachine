   M S B A M L   `   `   `  ����B  >DrumKit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DrumKitDrumKit  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       DrumKit.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   'localclr-namespace:DrumKit     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$  Drum Kit��5   	   $	��800��6   $	��725��6'    � DataContext 5        DrumKit.DrumViewModel  5   
    � 	Resources 5
        DrumKit.VolumeConverter   volConverter��%   &	          5   
   6    5      .��5      � .+�5   
   `�  `� IsLocked .  `� 
Background
 �����$��Stretch=�69   .�5      c� -   5        .@�5      o�  c� OverflowMode$ Never=�6   .@�5      m� -   5        $
��Save��5      6-   m� 5      -   5        $��
Save As...��5      63   m� 5      -   5        $
��Load��5      6-   m� 5      -   5        $��
Load As...��5      63   m� 5      -   5        $��Record��5      6/   m� 5      -   5        $
��Stop��5      6-   m� 5      -   5        $
��Play��5      6-   m� 5      -	   5        $��Export��5      6/   m� 5      -
   5        $��View Controls��5      66   5      5      5      �� 5   
   $��30q�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��15(�6   $"��Metronome Beats Per Minute: ��6&   �� 5       $��15(�6    �� TextAlignment$ Center=�6(    �� 
IsReadOnly . ��6?   �� 	 �� Mode$	 TwoWay=�
 �� Path$	
 Bpm�6f   5!      �� 5"      ��6   �� $	 TwoWay=�$
 Instruments���5#      �� 5$      .R�5%      �� 5&      ��5(      �� 5)      .R�5*      �� .+�5+      �� $ 
Horizontal=�6)   .+�5,   "   �� $��25(�6(   ��66   �� $	 TwoWay=�$

 Name��� 5-   "   $��Center=�6)    � Maximum$	 200Q�6D    � Minimum$ 0Q�6R     VolumeSlider#���  6�    � Value 6^   �� $	 TwoWay=�$
 Volume� �� 	Converter# [  6�   �� 5.   "   $��Center=�6(   $��15(�6C   $��Volume: ��6Q   �� 5/   "   $��Center=�6(   $��15(�6C   ��6Q   �� $	 TwoWay=�$
 Volume�# [  50      �� 51      $ 
Horizontal=�6)   .+�52   "   �� $��5q�6+   $	��200��66    �� DisplayMemberPath$
 Name��6�   ��6B   �� $	 OneWay=�$
 BrushA���6r   �� $	 TwoWay=�$
 	SoundList� �� SelectedItem 6�   �� $	 TwoWay=�$
 SoundA�6�   �� 53   "   $��5q�6+   $	��200��66   $
 Name��6�   ��6B   �� $	 OneWay=�$
 BrushB���6r   �� $	 TwoWay=�$
 	SoundList� 6�   �� $	 TwoWay=�$
 SoundB�6�   54      55      56      58      59      5:      5;      