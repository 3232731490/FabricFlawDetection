   M S B A M L   `   `   `  ����J  FFabricDetection, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FabricDetectionFabricDetection  5   	        FabricDetection.MainWindow   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FabricDetection     5   	   _ [MaterialDesignThemes.Wpf, Version=4.2.1.2, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	      MaterialDesignBody#���   5	   	   $��Regular"�5
   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	    � Title$ 瑕疵检测��5   	   $
��auto��6   $	��960��6$   $
��1000��60   $	��620��6@   .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   6*   �� 5      $��1* �6   6*   �� 5      $	��10* �6   6+    � ColumnDefinitions 5   
   �� 5      6!   .+�5   
   #  MaterialDesignThemes.Wpf.Card   * &MaterialDesignLightSeparatorBackground#���  5       ! MaterialDesignLightForeground#���  5      $
��auto��5      $��8q�5       .��5!      �� $��20(�5"      $��布 料 瑕 疵 检 测��5#      $��Center=�5$      $��Center=�5%      $��STCAIYUN)�5&      $
��Bold"�5'      5)      � 5*   
   $��1��6   
������� 5,      �� 5-      $��2* �6#   60   �� 5.      $��3* �6#   60   �� 5/      $	��200 �6#   62   .+�51      �  52      �� 53      $
��auto �6'   66   �� 54      $��1* �6'   64   .+�56      �� -   5        $��FindFile��58      $��0��57       + 'MaterialDesignPaperSecondaryLightButton#��[ 59       � ToolTip$" MaterialDesignOutlinedButton��5:      $��2q�5;      $��5q�5<      $
��auto��5=      $��24��5>      {�5?      �� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+ �� AncestorType
 Window.��5A      �� $
��Bold"�5B       # MaterialDesignOverlineTextBlock#��[ 5C      $��选择目录��5D      5E      �� 5F      -   5        $��filePath��5F   5   $��24��6   $��1��6%   $��1q�6E   ���	#FF5E7A14$��2q�6q   ._�5G      �� -   5        $��CurFilePath��5G      $�� ��61   $��5q�69   $��24��6F   $��Center=�6S   ��6p   ��  �� Path$ ActualWidth�	 �� ElementName$	 filePath��5H      5J      � 5K      $��1��6    5L      �� 5M      $
��auto �6'   66   �� 5N      $��1* �6'   64   .+�5P      �� $!��当前正在检测样本 : ��6   $��Center=�60   
 �� Padding$
 20q�6K   $��Heavy"�6X   $��16(�6k   ���	#FFA4D18B5Q      �� 5R      -   5         ��Name$ fileName��5R      $��1��6,   $��text.jpg��6<   $��Center=�6L   $��Center=�6g   $��Heavy"�6�   $��16(�6�   ���	#FFA4D18B6�   5S      � 5U      $��2��6   .+�5V      �  5W      �� 5X      $��1* �6(   66   �� 5Y      $��14 �6(   66   .+�5[      �� -   5        $
��Exit��5^      $��30��5\      $��30��5]       7 3MaterialDesignFloatingActionMiniSecondaryDarkButton#��[ 5_      $, &MaterialDesignFloatingActionMiniButton��5`      {�5a      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5b      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	ExitToApp=�5c      
�������5d      5f      �� 5g      $��1��6    $��10(�6-   $
��Bold"�6;   
���"�"�$��Center=�6f   退出系统6�   5h      5j      5k      � 5l   
   $��2��6   .+�5m      �  5n      �� 5o      $
��1.5* �6'   66   �� 5p      $��4* �6'   64   �� 5q      $��1* �6'   64   .+�5s      �� -   5         ��Name$ 	textBlock��5s      $��1��6/   $��10q�6?   �� 5t      -   5        $��ImageParent��5t      .+�5u      �� -   5         ��Name$	 Img��5u       $)��#C:\Users\32327\Pictures\photo_1.jpg��6-   $
��Fill=�6  ��6�   �� *ActualWidth+$	 	textBlock�� �� Mode$ OneWay=���6�   �� *ActualHeight+$	 	textBlock��$ OneWay=�6  5v      5w      �� 5x      $��2��6   $��5q�6)   
����İ�$��2,2,2,2q�6Q   ._�5y      �  5z      �� 5{      $��40 �6,   6:   �� 5|      $��40 �6,   6:   �� 5}      $��1* �6,   6:   �� 5~      $��60 �6,   6:   .+�5�      � .+�5�      �� $��Center=�6(   $��Center=�6C   $
��Bold"�6`   $��16(�6r   瑕疵区域列表6�   5�      �� 5�      -	   5        $��IsAll��5�   #   $��Center=�6D   $
��Left=�6_   $��10q�6z   $
��Bold"�6�   $��16(�6�   $��1��6�   $��全选��6�   6�   � 5�      $��2��6   .+�5�      �� -
   5        $��
FabricList��5�   3   $��2��6&   $��Stretch=�6E   ��5�   "   �� 5�   &   .R�5�   *   �� $��Fabric_Stack��6Y   $��3q�65    �� Orientation$ 
Horizontal=�6@   .+�5�   .   �� -   5        $
��10,0q�5�   n    d� 	IsChecked 67   �� *	IsChecked+6n   �� 5�   .   $��
FabricItem��6{   -   5        $
��Bold"�5�   �   $��Center=�6�   $
��Left=�6  ��68   �� *ActualWidth+$	 
FabricList��$ OneWay=���6�   �� *
KindName+68  5�   +   5�   '   5�      5�      � 5�      $��3��6   .+�5�      �� -   5        $��Delete��5�   )   $��1��5�   )   $
��auto��5�      $5 /Resource name: MaterialDesignRaisedAccentButton��5�      +  %MaterialDesignThemes.Wpf.RippleAssist   
IsDisabled$
 True��5�      $��删除区域��5�        MaterialDesignRaisedButton#���  5�   #   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth3=�5�      $��10q�5�   !   $��Center=�5�      ���	#FF7FBB36���	#FFA4D18B � Cursor$
 Hand��5�   P   {�5�      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window5�   ^   5�      5�      5�      � 5�      $��0��6   $��5q�6'   .+�5�      �  5�      �� 5�      $��3* �6,   6:   �� 5�      $��3* �6,   6:   �� 5�      $��5* �6,   6:    5�      �� 5�      61   �� 5�      61   .+�5�      � $��10q�6   $��0��6+   $��2��68   .+�5�      �  5�   "   �� 5�   &   $��1* �67   6D   �� 5�   &   $��1* �67   6D    5�   "   �� 5�   &   $��3* �64   6B   �� 5�   &   $��1* �64   6B   .+�5�   "   �� $��Center=�6,   $��0��6I   $��1��6Y   $��Center=�6f   $��5q�6�   $
��Bold"�6�   框选瑕疵6�   �� 5�   "   -   5        $��
SelectFlaw��5�   %   
���2͚� *	 &MaterialDesignFloatingActionMiniButton#��[	 5�      $��Center=�5�      $, &MaterialDesignFloatingActionMiniButton��5�      {�5�      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5�   &     $ Allergy=�5�   !   5�   #   �� 5�   "   -   5        $��
DeleteDraw��5�   %   $��1��5�   !   
���2͚�#��[	 5�      $��Center=�5�      $, &MaterialDesignFloatingActionMiniButton��5�      {�5�      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5�   &     $ MarkerCancel=�5�   !   5�   #   �� 5�   "   $��1��6,   $��1��69   $��Center=�6I   $��Center=�6f   $��5q�6�   $
��Bold"�6�   撤销瑕疵6�   5�      5�      � 5�      $��10q�6   $��1��6+   $��2��68   .+�5�      �  5�   "   �� 5�   &   $��1* �67   6D   �� 5�   &   $��1* �67   6D    5�   "   �� 5�   &   $��3* �64   6B   �� 5�   &   $��1* �64   6B   .+�5�   "   �� $��Center=�6,   $��0��6I   $��1��6Y   $��Center=�6f   $��5q�6�   $
��Bold"�6�   结束框选6�   �� 5�   "   -   5        $��	Completed��5�   %   $��0��5�   %   
���2͚�#��[	 5�      $��Center=�5�      $, &MaterialDesignFloatingActionMiniButton��5�      {�5�      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5�   &     $ 
StoreCheck=�5�   !   5�   #   �� 5�   "   $��Center=�6-   $��1��6J   $��1��6Z   $��Center=�6g   $��5q�6�   $
��Bold"�6�   保存样本6�   �� 5   "   -   5        $��saveXML��5  %   $��1��5  !   
���2͚�#��[	 5     $��Center=�5     $, &MaterialDesignFloatingActionMiniButton��5     {�5     �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5  &     $ ContentSave=�5	  !   5  #   5     5     � 5     $��2��6   $��2��6,    5     �� 5  "   $��2* �60   6>   �� 5  "   $��1* �60   6>   .+�5     �� $��0��6%   
���"�"�$��1,1,1,1q�6L   ._�5  "   �  5  &   �� 5  *   $��1* �68   6F   �� 5  *   $��1* �68   6F   .+�5  &   �  5  *   �� 5  .   $��1* �6?   6L   �� 5  .   $��2* �6?   6L   .+�5  *   �� $��12(�64   $
��Bold"�6B   $��Center=�6T   $��Center=�6q   样本数量5   +   �� 5!  *   -   5        $ Gene_num��5!  �   $��1��62    �� IsInputMethodEnabled$ False��6B    J� Opacity$ 1Q�6k   $��12(�6w   ���#b6ea7b���	#FF898585$��Center=�6�    �� TextWrapping$
 Wrap=�6�   $��3��6�   $��49��6�   $��Center=�6	   �� 	MaxLength$ 5��6B  $��24��6P  $��5q�6\  ��6+  Z� 6h  5"  '   �� 5#  &   -   5        $��
Generation��5$  )   $��1��5%  )   $
��auto��5&     $5 /Resource name: MaterialDesignRaisedAccentButton��5'     $
 True��5(     $��生成随机样本��5)     #���  5+  #   $ Depth3=�5,     $��10q�5-  !   $��Center=�5.     ���	#FF7FBB36���	#FFA4D18B$
 Hand��5/  P   {�5*     �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window5/  ^   51  #   52     � 53     $��1��6#   $��5q�60    54  "   �� 55  &   $��1* �67   6D   �� 56  &   $��1* �67   6D    58  "   �� 59  &   $��1* �64   6B   �� 5:  &   $��20 �64   6B   .+�5<  "   �� -   5        $	��pre��5=  %   $��0��5>  !   $��25��5?  %   $��25��5@  %   ���	#FF7FBB36#��[	 5B     $��Center=�5C     $, &MaterialDesignFloatingActionMiniButton��5D     {�5E     �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5F  &     $ 	ArrowLeft=�5G  !   5I  #   �� 5J  "   -   5        $
��next��5K  %   $��25��5L  %   $��25��5M  %   $��1��5N  !   ���	#FF7FBB36#��[	 5P     $��Center=�5Q     $, &MaterialDesignFloatingActionMiniButton��5R     {�5S     �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5T  &     $ 
ArrowRight=�5U  !   5W  #   �� 5X  "   $��Center=�6,   $��10(�6I   $��Center=�6W   $
��Bold"�6s   $��1��6�   	上一个6�   �� 5Y  "   $��Center=�6,   $��10(�6I   $��Center=�6W   $
��Bold"�6s   $��1��6�   $��1��6�   	下一个6�   5Z     5[     5\     5]     5^     5_     5`     5a     