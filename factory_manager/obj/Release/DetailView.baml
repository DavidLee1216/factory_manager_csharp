   M S B A M L   `   `   `  ���� J  Ffactory_manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:factory_managerfactory_manager       factory_manager.DetailView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:factory_manager       � Title$  查看详情��$	��610��$
��1110��$��16(� � 
ResizeMode$ NoResize=�.��� ���	#FFEEF3F7.+��� $��10q�$
��Left=�$	��Top=�$
��1070��$	��400�����	#FF3C4347����  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ label_place��$
��Bold"�摆放资料.��&   System.Windows.Controls.DataGrid  -     Name$ 	dataGrid1��   
IsReadOnly .    AutoGenerateColumns .     SelectionMode$ Single=�   SelectionUnit$ FullRow=�	   CanUserAddRows	 .  $
��Left=�$	��370��$	��Top=�$
��1050��
   AlternatingRowBackground
 �	#FFEEF3F7 �� AlternationCount$ 2�����	#FFEEF3F7��. ! �� IsDeferredScrollingEnabled . $��Visible=�$��Visible=�$��3,0,0,0q� � 	Resources  �� Color��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ #FFFF00��   Columns 0  *System.Windows.Controls.DataGridTextColumn  -   grid1_label_asset_no��,  &System.Windows.Controls.DataGridColumn   Header$ 资产No��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 70 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
asset_id+  -   grid1_label_place_no��$ 摆放No��$	 100  �� *id+  -   grid1_label_chinese_name��$ 品名��$	 150  �� *chinese_name+  -   grid1_label_korean_name��$ 品名(朝鲜语)��$	 150  �� *korean_name+  -   grid1_label_place��$ 摆放工程��$	 100  �� *place+  -   grid1_label_place_date��$ 摆放日期��$	 150  �� *date+  -	   grid1_label_model��$ 规格/型号��$	 150  �� *model+  -
   grid1_label_count��$ 数量��$	 100  �� *count+  -   grid1_label_remark��$ 备注��$	 100  �� *remark+�� $��
10 410 0 0q�$
��Left=�$	��Top=�$	��530��$	��150�����	#FF3C4347���� $ 
Horizontal=�.+��� -   $ 
label_back��$
��Bold"�遣返资料.��  -   $ dataGrid_back�� .  .  $ Single=�$ FullRow=�	 .  $
��Left=�$	��120��$	��Top=�$	��510�����	#FFEEF3F7��.  . $��Visible=�$��Visible=�$��3,0,0,0q� ��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ #FFFF00��   -   grid_back_label_no��$ No��$	 100  �� *id+  $ back_No��$	 100    
Visibility$ Hidden=� �� *	back_id+  -   grid_back_label_back_place��$ 遣返工程��$	 120  �� *place+  -   grid_back_label_back_date��$ 遣返日期��$	 120  �� *date+  -   grid_back_label_back_count��$ 数量��$ 70  �� *count+�� $��552,410,0,0q�$
��Left=�$	��Top=�$	��530��$	��150�����	#FF3C4347���� $ 
Horizontal=�.+��� -   $ label_exhaust��$
��Bold"��� $�处置资料��.��  -   $ dataGrid_exhaust�� .  .  $ Single=�$ FullRow=�	 .  $
��Left=�$	��120��$	��Top=�$	��510�����	#FFEEF3F7��.  . $��Visible=�$��Visible=�$��3,0,0,0q� ��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ #FFFF00��   -   grid_exhaust_no��$ No��$	 100  �� *id+  $ 
exhaust_No��$	 100 $ Hidden=� �� *	back_id+  -   grid_exhaust_exhaust_place��$ 处置工程��$	 120  �� *place+  -   grid_exhaust_exhaust_date��$ 处置日期��$	 120  �� *date+  -   grid_exhaust_exhaust_count��$ 数量��$ 70  �� *count+