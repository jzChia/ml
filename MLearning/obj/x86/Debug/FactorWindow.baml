   M S B A M L   `   `   `  ����  	MLearning    MLearning.FactorWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  FactorWindow��5   	   $	��326��6   $	��604��6+   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��120 �6    � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��35 �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��datag��5   '   $��2��6      AutoGenerateColumns .     	RowHeight$ 30Q�6P      RowHeaderWidth$ 30Q�6_      HeadersVisibility$ Column=�6s      GridLinesVisibility$	 All=�6�      Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 操作��6)   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 40 65      CellTemplate 5      �� 5      .R�5      �� 6*   5      5      5  /System.Windows.Controls.DataGridHyperlinkColumn  5      $	 名称��6*   $	
 130 66   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *name+  5      $	 	创建人��6)   $
 80 66    5      �� 5      .R�5      �� $��Center=�6%   $��Center=�6B   ._�5   "   �� ��6,   �� *author+5      5      5         5!      $	 创建时间��6)   $	
 120 67    5"      �� 5#      .R�5$      �� $��Center=�6%   $��Center=�6B   ._�5%   "   �� ��6,   �� *
createTime+5&      5'      5)        5*      $	 修改日期��6)   $	
 120 67    5+      �� 5,      .R�5-      �� $��Center=�6%   $��Center=�6B   ._�5.   "   �� ��6,   �� *
updateTime+5/      50      52        53      $	 修改日期��6)   $	
 120 67    54      �� 55      .R�56      �� $��Center=�6%   $��Center=�6B   ._�57   "   �� $��cuozuo��6,   58      59      5;         RowStyle 5>      �� 5?       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6    �� Triggers 5@      P� 5A       P� Property �@J�IsMouseOver P� Value . . �5B      ��  �� Property � �� �� Value
 � ���6L   �� 5C       � ��
 ��  �6;   �� 5D       � ��$ 15(�69   6F   5E      5G      5I      � 5J   
   $��1��6   .+�5K      �� $��名称��6   $��Center=�6"   5M      5N      5O      5Q      