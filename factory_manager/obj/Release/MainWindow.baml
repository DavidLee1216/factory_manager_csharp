   M S B A M L   `   `   `  ���� J  Ffactory_manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:factory_managerfactory_manager       factory_manager.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:factory_manager       � Title$  工厂管理器��$	��900��$
��1500�� � 
ResizeMode$ CanMinimize=�.��� .+��� -    ��Name$ tab_control��.@��� -   $
��tab1��$��资产入库摆放��$��16(�$	��150��.��� 
�������.+��� $��10q�$
��Left=�$	��300��$��10q����	#FF3C4347����  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ label_input_data��$
��Bold"�输入资料.���� $��0,0,0,0q�.+��� -   $ label_chinese_name��品名�� -    ��Name$ asset_chinese_name���� -   $ label_korean_name��$��0 10 0 0q�品名(朝鲜语)�� -   $ asset_korean_name���� -   $ label_model��$��0 10 0 0q�规格/型号�� -	   $ model���� -
   $ label_owner��$��0 10 0 0q�	归属厂�� -   $ owner���� -   $ label_in_date��$��0 10 0 0q�入库日期(  "System.Windows.Controls.DatePicker  -     Name$ purchase_date��   SelectedDateFormat$ Short=��� -   $ label_count��$��0 10 0 0q�$
��Left=�数量�� -   $ count���� -   $ label_price��$��0 10 0 0q�$
��Left=�单价�� -   $ price���� -   $ 	pay_block��$��0 10 0 0q�$
��Left=�付款情况	� -    	�Name$ pay_yes��	 	� 	GroupName$	 Pay_State_Group��$��	30 10 0 0q�$	��是��	� -   $ pay_no��$	 Pay_State_Group��$��	30 10 0 0q�$	��不���� -   $��
30 20 30 0q�$��30��.���� -   $ 	label_add��添加�� -   $��
30 20 30 0q�$��30��.���� -   $ label_place��摆放�� -   $��
30 20 30 0q�$��30��.���� -   $ label_asset_modify��变更�� -   $��
30 20 30 0q�$��30��.���� -   $ label_asset_delete��删除�� -   $��
30 20 30 0q�$��30��.���� -   $ label_add_excel_data���� $�Excel资料添加��&   System.Windows.Controls.DataGrid  -   
  Name$
 	dataGrid1��   
IsReadOnly .    AutoGenerateColumns .     SelectionMode$ Single=�$��1��   SelectionUnit$ FullRow=�   CanUserAddRows .  $
��Left=�$	��795��$��
326,22,0,0q�$	��Top=�$
��1141��   AlternatingRowBackground �	#FFEEF3F7 �� AlternationCount$ 2��
���������. ! �� IsDeferredScrollingEnabled . $��Visible=�$��Visible=� � 	Resources  �� Color��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ #FFFF00��   Columns 0  *System.Windows.Controls.DataGridTextColumn  -    grid1_label_no��,  &System.Windows.Controls.DataGridColumn   Header$ No��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *id+  -!   grid1_label_chinese_name��$ 品名��$	 150  �� *chinese_name+  -"   grid1_label_korean_name��$ 品名(朝鲜语)��$	 150  �� *korean_name+  -#   grid1_label_in_date��$ 入库日期��$	 150  �� *	in_date+  -$   grid1_label_count��$ 数量��$	 100  �� *count+  -%   grid1_label_model��$ 规格/型号��$	 150  �� *model+  -&   grid1_label_price��$ 单价��$	 100  �� *price+  -'   grid1_label_owner��$ 	归属厂��$	 100  �� *owner+  -(   grid1_label_pay_state��$ 付款情况��$	 100  �� *	pay_state+�� -)   $
��tab2��$��摆放资产管理��$��16(�$	��150��.��� 
�������.+�  -*   $
 	dataGrid2�� .  .  $ Single=�$ FullRow=� .  $
��Left=�$	��571��$��	10,45,0,0q�$	��Top=�$
��1141�� �	#FFEEF3F7$ 2��
���������.  . $��Visible=�$��Visible=� ��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ #FFFF00��   -+   grid2_label_no��$ No��$	 100  �� *
asset_id+  -,   grid2_label_place_no��$ place_No��$	 100    
Visibility$ Hidden=� �� *id+  --   grid2_label_chinese_name��$ 品名��$	 150  �� *chinese_name+  -.   grid2_label_korean_name��$ 品名(朝鲜语)��$	 150  �� *korean_name+  -/   grid2_label_place��$ 摆放工程��$	 100  �� *place+  -0   grid2_label_in_date��$ 入库日期��$	 150  �� *	in_date+  -1   grid2_label_place_date��$ 摆放日期��$	 150  �� *date+  -2   grid2_label_model��$ 规格/型号��$	 150  �� *model+  -3   grid2_label_count��$ 数量��$	 100  �� *count+  -4   grid2_label_price��$ 单价��$	 100  �� *price+  -5   grid2_label_remark��$ 备注��$	 100  �� *remark+  -6   $
 dataGrid_back�� .  .  $ Single=�$ FullRow=� .  $
��Left=�$	��155��$	��Top=�$	��560��
���������.  . $��Visible=�$��Visible=�$��
10,660,0,0q� ��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ Beige��   -7   grid_back_label_no��$ No��$	 100  �� *id+  -8   grid_back_label_place_no��$ place_No��$	 100 $ Hidden=� �� *
place_id+  -9   grid_back_label_back_no��$ back_No��$	 100 $ Hidden=� �� *	back_id+  -:   grid_back_label_place��$ 返送工程��$	 150  �� *place+  -;   grid_back_label_date��$ 返送日期��$	 150  �� *date+  -<   grid_back_label_count��$ 数量��$	 100  �� *count+  -=   $
 dataGrid_exhaust�� .  .  $ Single=�$ FullRow=� .  $
��Left=�$	��155��$	��Top=�$	��560��
���������.  . $��Visible=�$��Visible=�$��590,660,0,0q� ��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ Beige��   ->   grid_exhaust_label_no��$ No��$	 100  �� *id+  -?   grid_exhaust_label_place_no��$ place_No��$	 100 $ Hidden=� �� *
place_id+  -@   !grid_exhaust_label_exhaust_no��$ 
exhaust_No��$	 100 $ Hidden=� �� *	back_id+  -A   grid_exhaust_label_place��$ 处置工程��$	 150  �� *place+  -B   grid_exhaust_label_date��$ 处置日期��$	 150  �� *date+  -C   grid_exhaust_label_count��$ 数量��$	 100  �� *count+�� $
��Left=�$	��124��$��1172,12,0,0q�$	��Top=�$	��293�����	#FF3C4347���� $ 
Horizontal=�.+��� -D   $ label_simple_search��$
��Bold"�简单查询.���� .+��� $ 
Horizontal=�$��50��.+��� $��No��$��	20 10 0 0q��� -E   $ simple_search_id��$	��192��$��20,0,0,0q�$��29���� -F    ��Name$ label_button_search��$��38��$	��100��.��查看�� $
��Left=�$	��366��$��1172,136,0,0q�$	��Top=�$	��293�����	#FF3C4347���� $ 
Horizontal=�.+��� -G   $$ label_place_modify_move_delete��$
��Bold"�" 摆放工程变更/移动/删除.���� $	��343��$	��Top=�$��0,0,0,-5q�.+��� -H   $" label_modify_move_place_name��$��变更/移动工程名��$��	20 10 0 0q��� -I   $ place2��$��	20 0 20 0q�$��29���� -J   $ label_modify_move_date��$��变更/移动日期��$��	20 10 0 0q�  -K   $��date2��$��	20 0 20 0q��� $ 
Horizontal=�.+��� -L   $ label_modify_move_count��$��变更/移动数量��$��	20 10 0 0q��� -M   $ count2��$��
20 10 20 0q�$��29��$	��120���� $ 
Horizontal=�.+��� -N   $ label_combo_remark��$��备注��$��	20 10 0 0q��� -O    ��Name$ remarkCombo2��$��
90 10 20 0q�$��29��$	��120�� �� SelectedIndex$ 1��.@��� $
��1楼���� $
��2楼���� $
��3楼���� $
��4楼���� -P   $ label_button_modify��$��30��$��
40 15 40 0q�$��变更���� -Q   $ label_button_delete��$��30��$��
40 15 40 0q�$��删除���� -R   $ label_button_move��$��30��$��
40 15 40 0q�$��移动���� $
��Left=�$	��157��$��1172,502,0,0q�$	��Top=�$	��293�����	#FF3C4347���� $ 
Horizontal=�.+��� -S   $ label_group_back��$
��Bold"�返送.���� .+��� $ 
Horizontal=�$��	10 10 0 0q�.+��� -T   $ label_group_back_count��数量�� -U   $ 
back_count��$��52 0 0 0q�$	��170��$��28���� $ 
Horizontal=�$��
10 15 10 0q�.+��� -V   $ label_group_back_date��返送日期  -W   $��	back_date��$��20 0 0 0q�$	��170���� $ 
Horizontal=�.+��� -X   $" label_group_back_button_back��$��28��$	��100��$��20 10 20 10q�.��返送�� -Y   $$ label_group_back_button_cancel��$��28��$	��100��$��20 10 20 10q�.��取消�� $
��Left=�$	��157��$��1172,660,0,0q�$	��Top=�$	��293�����	#FF3C4347���� $ 
Horizontal=�.+��� -Z   $ label_group_exhaust��$
��Bold"�处置.���� .+��� $ 
Horizontal=�$��	10 10 0 0q�.+��� -[   $ label_group_exhaust_count��数量�� -\   $ exhaust_count��$��52 0 0 0q�$	��170��$��28���� $ 
Horizontal=�$��
10 15 10 0q�.+��� -]   $ label_group_exhaust_date��处置日期  -^   $��exhaust_date��$��20 0 0 0q�$	��170���� $ 
Horizontal=�.+��� -_   $( "label_group_exhaust_button_exhaust��$��28��$	��100��$��20 10 20 10q�.��处置�� -`   $' !label_group_exhaust_button_cancel��$��28��$	��100��$��20 10 20 10q�.��取消�� -a    ��Name$ label_label_back_data��$��返送资料��$
��Left=�$��
10,629,0,0q�$	��Top=�$��74���� -b   $ label_label_exhaust_data��$��处置资料��$
��Left=�$��590,628,0,0q�$	��Top=��� -c   $ label_label_place_data��$��摆放资料��$
��Left=�$��	10,14,0,0q�$	��Top=��� -d   $
��tab3��$��资产材料查看��$��16(�$	��150��.��� 
�������.+��� -e    ��Name$ label_group_search_items��$��搜索项目��$
��Left=�$	��115��$��	10,10,0,0q�$	��Top=�$
��1454�����	#FF3C4347.��� $��0,0,10,0q�.+��� -f   $ label_search_area��$��搜索范围��$
��Left=�$��	10,14,0,0q�$	��Top=��� -g   $ search_area_combo��$
��Left=�$��
110,14,0,0q�$	��Top=�$	��118��.@��� -h   $��item1��$��入库资料���� -i   $��item2��$��摆放资料���� -j   $��item3��$��移动资料���� -k   $��item4��$��返送资料���� -l   $��item5��$��处置资料���� -m   $��item6��$��删除记录资料���� -n   $ search_label_chinese_name��$��品名��$
��Left=�$��
274,10,0,0q�$	��Top=��� -o   $ search_label_korean_name��$��品名(朝鲜语)��$
��Left=�$��
274,46,0,0q�$	��Top=��� -p   $ search_text_chinese_name��$
��Left=�$��23��$��
379,14,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��180���� -q   $ search_text_korean_name��$
��Left=�$��23��$��
379,50,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��180��  -r   $��search_date_from��$
��Left=�$��
680,14,0,0q�$	��Top=�$	��115���� -s   $ search_label_date��$��日期��$
��Left=�$��
593,14,0,0q�$	��Top=�$��93���� $��-��$
��Left=�$��
817,10,0,0q�$	��Top=�$��21��  -t   $��search_date_to��$
��Left=�$��
860,14,0,0q�$	��Top=�$	��115���� -u   $ search_label_place��$��工程��$
��Left=�$��
593,45,0,0q�$	��Top=�$��83���� -v   $ search_text_place��$
��Left=�$��23��$��
681,50,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��124���� -w   $ search_label_model��$��规格/型号��$
��Left=�$��1056,10,0,0q�$	��Top=��� -x   $ search_text_model��$
��Left=�$��23��$��1153,14,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��124���� -y   $ 
btn_search��$��查看��$
��Left=�$��1187,52,0,0q�$	��Top=�$��75��  -z   $
 dataGrid_search�� .  .  $ Single=�$ FullRow=� .  $
��Left=�$	��654��$	��Top=�$
��1454�� �	#FFEEF3F7$ 2��
���������.  . $��Visible=�$��Visible=�$��
10,174,0,0q� ��%�   (��       * SystemColors.HighlightBrushKey+)(��      *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� $ #0070F0���� $ Beige��   -{   grid_search_label_no��$ No��$	 100  �� *id+  -|   grid_search_label_place_no��$ 摆放No��$	 100  �� *
place_id+  -}   "grid_search_label_chinese_name��$ 品名��$	 100  �� *chinese_name+  -~   !grid_search_label_korean_name��$ 品名(朝鲜语)��$	 100  �� *korean_name+  -   grid_search_label_in_date��$ 入库日期��$	 100  �� *	in_date+  -�    grid_search_label_place_date��$ 摆放日期��$	 100  �� *date+  -�   grid_search_label_place��$ 摆放工程��$ 80  �� *place+  -�   grid_search_label_place_old��$ 	前工程��$ 80  �� *	old_place+  -�   grid_search_label_place_new��$ 	后工程��$ 80  �� *	new_place+  -�   grid_search_label_model��$ 规格/型号��$	 100  �� *model+  -�   grid_search_label_count��$ 数量��$	 100  �� *count+  -�   grid_search_label_price��$ 单价��$	 100  �� *price+  -�   grid_search_label_owner��$ 	归属厂��$	 100  �� *owner+  -�   grid_search_label_pay_state��$ 付款情况��$	 100  �� *	pay_state+  -�   grid_search_label_remark��$ 备注��$	 100  �� *remark+  -�    grid_search_label_back_count��$ 返送数量��$	 100  �� *
back_count+  -�   grid_search_label_back_date��$ 返送日期��$	 100  �� *	back_date+  -�    grid_search_label_back_place��$ 返送工程��$ 80  �� *
back_place+  -�   #grid_search_label_exhaust_count��$ 处置数量��$	 100  �� *exhaust_count+  -�   "grid_search_label_exhaust_date��$ 处置日期��$	 100  �� *exhaust_date+  -�   #grid_search_label_exhaust_place��$ 处置工程��$ 80  �� *exhaust_place+  -�   )%grid_search_label_delete_history_date��$ 删除日期��$	 100  �� *deleted_date+  -�   )%grid_search_label_delete_history_user��$ 用户��$	 100  �� *deleted_user+�� -�   $
��tab4��$��设定��$��16(�$	��150��.��� 
�������.+��� $
��Left=�$	��227��$��	36,35,0,0q�$	��Top=�$	��376�����	#FF3C4347���� $ 
Horizontal=�.+��� -�   $ label_lang_setting��$
��Bold"�设定语言.���� $
��Left=�$	��182��$��21,6,0,0q�$	��Top=�$	��329��.+��� $ 
Horizontal=�.+�	� -�   $ label_chinese��$	 lang_setting��$��
40 20 30 0q�$��汉语���� -�     ��Name$  check_chinese_png��$��50��$��60��$y�Visible=����� ! �� DecodePixelWidth$! 50��" �� DecodePixelHeight$" 60��# �� 	UriSource$# /image/check_mark.png?��� $ 
Horizontal=�.+�	� -�   $ label_korean��$	 lang_setting��$��
40 20 30 0q�$��	朝鲜语���� -�   $  check_korean_png��$��50��$��60��$y�Visible=����� $! 50��$" 60��$# /image/check_mark.png?��� -�   $ btn_set_default_lang��$	��150��$��30��$��0 10 0 0q�.��设定为标准�� -�   $ group_user_manage��$
��Left=�$	��251��$��
498,35,0,0q�$	��Top=�$	��529�����	#FF3C4347���� $ 
Horizontal=�.+��� -�   $ label_user_setting��$
��Bold"�用户管理.���� $ 
Horizontal=�.+��� $ 
Horizontal=�$
��Left=�$	��262��$��0,0,0,10q�.+��� -�   $ ��Name$$ username_list��$	��260���� $	��220��$��30 0 0 0q�.+��� -�   $��label_username_setting��$��	用户名���� -�   $ username_text_setting��$��28��$
 Wrap=�$�� ���� -�   $ username_add_button��$��添加��$��0 20 0 0q��� -�   $ username_modify_button��$��变更��$��0 20 0 0q��� -�   $ username_delete_button��$��删除��$��0 20 0 0q��� -�   $ group_pass_manage��$��密码管理��$
��Left=�$	��251��$��1111,35,0,0q�$	��Top=�$	��320�����	#FF3C4347.���� .+��� $
��Left=�$��43��$��	10,10,0,0q�$	��Top=�$	��286��$ 
Horizontal=�.+��� -�   $ label_old_pass��$��	旧密码��$��0 5 0 0q�G� -�   % G�Name$% old_pass��$	��210��$��28��$��5,8,0,7q��� $
��Left=�$��43��$��	10,10,0,0q�$	��Top=�$	��286��$ 
Horizontal=�.+��� -�   $ label_new_pass��$��	新密码��$��0 5 0 0q�G� -�   $% new_pass��$	��210��$��28��$��5,8,0,7q��� $
��Left=�$��43��$��	10,10,0,0q�$	��Top=�$	��286��$ 
Horizontal=�.+��� -�   $ label_retype_new_pass��$��	新密码��$��0 5 0 0q�G� -�   $% retype_new_pass��$	��210��$��28��$��5,8,0,7q��� -�   $ btn_pass_change��$��变更��$��
50 20 50 0q�