   M S B A M L   `   `   `  ����   	NicoTrola    NicoTrola.Configuration   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$   �� � 
ResizeMode$ NoResize=�$	��380��$	��680�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=�   TabControlContentBrush#���    � 	Resources   EstiloControlGlass �� 
TargetType �� Property �� Value �� EndPoint	 �� 
StartPoint
 � Color � Offset3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Opacity  OutsideFontColor �� Color  DisabledBackgroundBrush  ListItemSelectedBrush  TabControlHeaderBrush   TabControlContentBorderBrush  TabItemHoverBrush 
� GradientOrigin �� RelativeTransform �� CenterX �� CenterY �� ScaleX �� ScaleY �� CenterX �� CenterY �� CenterX �� CenterY �� Angle T� X T� Y  TabItemHoverBorderBrush 	 TabItemHoverHighlightBrush 
 TabItemSelectedBrush  TabItemSelectedBorderBrush  TabItemBorderTopBrush  TabItemHighlightTopBrush  TabItemBorderTop2Brush  TabItemFocusBrush  SliderBackgroundBrush  
WhiteColor  DisabledForegroundBrush  TextControlBorderBrush �� 
TargetType J� SnapsToDevicePixels-  'System.Windows.Input.KeyboardNavigation   TabNavigation  � ColumnDefinitions! ��Name" � RowDefinitions# ��Name$ ��Name% �� CornerRadius& K� ZIndex' ��Name( K� IsItemsHost)   TabIndex* J� RenderTransformOrigin+ � LayoutTransform, �� AngleX- �� AngleY. ��Name/ �� Triggers0 P� Property1 P� Value2 �� 
TargetName3 � 	Resources  Key4 g� 	BeginTime5 �� 
TargetName6 �� TargetProperty7 O� KeyTime8 O� Value9 �Name: J� Opacity; J� IsHitTestVisible< N� ExitActions= ��Name  
FocusedOff> N� EnterActions  	FocusedOn  HoverOff  HoverOn? ]� 
Conditions@ �� PropertyA �� Value  SelectedOff  
SelectedOnB �� 
SourceNameC �� Triggers  GenreItemTemplateD �� BindsDirectlyToSourceE �� ModeF �� FallbackValue  	MainColor  ArtistItemTemplate  FilterItemTemplateG d� 	IsChecked  CustomerDetail�� %>"  &	       &	 �    &	 �    &	     &	 �    &	      &	     &	 4    &	 B    &		 W    &	
 l    &	 �    &	 �    &	 <    &	 �    &	 �    &	     &	 c    &	 v    &	 �    '�� �    '�� �    7  7  7  7  7  7  &	 �    &	 1    &	 �    &	      �� " ��.���  � ��$ 2q���  � ��$ 5q���  � �� �� $ 0.75,1&�$	 0.25,0&�.��� $
 	#33FFFFFF��$ 0Q�� $
 	#C0FFFFFF��$ 0.287Q�� $
 	#4011322D��$ 0.683Q�� $
 	#33FFFFFF��$ 1Q���  � �� �� $ 0.5,1&�$	 0.5,0&�.��� $
 	#5811322D��$ 0Q�� $
 	#3EFFFFFF��$
 0.25Q�� $
 	#FFFFFFFF��$	 0.5Q�� $
 	#3EFFFFFF��$
 0.75Q�� $
 	#BFFFFFFF��$ 1Q���  �@J�Effect   $ 3Q�$ 3Q�$	 0.5Q��� $ 	#FF000000���� $ 	#A5FFFFFF���� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FFFFFFFF��$ 0.046Q�� $
 	#FFD7E0EA��$ 0.194Q�� $
 	#FFBCC5D5��$ 0.507Q�� $
 	#FFA4ADBB��$ 0.521Q�� $
 	#FFBAC1CF��$ 0.811Q�� $
 	#FFE3E4E6��$ 0.982Q��� $ 	CadetBlue���� $ Brown���� $ Transparent��
� $ 
0.503,1.06&� V� .~��� $	 0.5Q�$	 0.5Q�$
 2.06Q�$ 2.418Q��� $	 0.5Q�$	 0.5Q��� $	 0.5Q�$	 0.5Q�$
 -180Q�T� $ 0Q�$ 0.315Q�.��� $
 	#00FFFFFF��$ 0.362Q�� $
 	#AEE8E6E6��$ 0.496Q�� $
 	#F2E7D39C��$ 0.629Q��� $ 	#FFDDEFFF���� $ 	#FF9FBCF0���� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FFEFF6FE��$ 0Q�� $
 	#FFE0EAF6��$ 0.384Q��� $ 	#FFF3F8FF���� $ 0.5,1&�$	 0.5,-0.5&�.��� $
 	#FFECF9FA��$ 0Q�� $
 	#FFB4E0F4��$ 1Q��� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FFF3F8FF��$ 0Q�� $
 	#D8ECF8FC��$ 1Q��� $ 	#FF9ABBE6���� $ 0.5,1&�$	 0.5,0&�.��� $
 	#99F7C53B��$ 0Q�� $
 	#99F2B93F��$ 1Q��� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FF324D70��$ 0.493Q�� $
 	#FF6A8BB6��$ 1Q���	#FFFFFFFF���� $
 #888���� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FFABAEB3��� $
 	#FFE2E8EE��$ 1Q��� " ��.���  � �� �	#FFFFFFFF��  � ��$ 1q���  � ��$ Trebuchet MS)���  � ��$ 12(���  � ��$ 5q���  � ��$ Stretch=���  � ��$ Stretch=���  � �� �� " ��.R�� }�.  . $ Local=�  �� $! ColumnDefinition0���� $! ColumnDefinition1��$��0 �" �� $# RowDefinition0��$
��Auto ��� $# RowDefinition1��$��* �.+��� $$ border��#���  $% 2,2,0,0��$��0,0,0,-2q�$	& 100��._��� $' HeaderPanel��( . $& 1��$) 1��$* 0.5,0.5&�$
��Auto��$
��Auto��$��4,0,0,0q�+ V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� $ 0Q�T� $ 0Q�$ 0Q�z�V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� T� $ 0Q�$ 0Q��� $$ ContentPanel��$��1��#���   #���  #��z��$% 3,3,1,1��._��� $. PART_SelectedContentHost��$��4q�$��SelectedContent��/ P� 0 �@��TabStripPlacement$1 Bottom=�. ��� $2 ContentPanel�� � ��$ 0���� $2 RowDefinition0�� � ��$ * ��� $2 RowDefinition1�� � ��$
 Auto ��� $2 border�� � ��$ 1���� $2 ContentPanel�� �@��CornerRadius$ 2,2,0,0���� $2 border�� �@��CornerRadius$ 0,0,2,2���� $2 HeaderPanel�� � ��$ 
4,-2.5,0,0q��� $2 border�� � ��$ 0,0,0,0q�P� 0 �@��TabStripPlacement$
1 Left=�. ��� $2 HeaderPanel�� � ��$ 0���� $2 ContentPanel�� � ��$ 0���� $2 ContentPanel�� � ��$ 1���� $2 ColumnDefinition0�� � ��$
 Auto ��� $2 ColumnDefinition1�� � ��$ * ��� $2 RowDefinition0�� � ��$ * ��� $2 RowDefinition1�� � ��$ 0 ��� $2 border�� � ��$ 0���� $2 border�� � ��$ 0,0,0,0q��� $2 ContentPanel�� � ��$ 0,0,0,0q��� $2 border�� �@��CornerRadius$ 0,2,2,0���� $2 ContentPanel�� �@��CornerRadius$ 0,2,2,0���� $2 HeaderPanel�� � ��$ 
4,0,-2.5,0q�P� 0 �@��TabStripPlacement$1 Right=�. ��� $2 HeaderPanel�� � ��$ 0���� $2 ContentPanel�� � ��$ 0���� $2 ContentPanel�� � ��$ 0���� $2 ColumnDefinition0�� � ��$ * ��� $2 ColumnDefinition1�� � ��$
 Auto ��� $2 RowDefinition0�� � ��$ * ��� $2 RowDefinition1�� � ��$ 0 ��� $2 border�� � ��$ 1���� $2 ContentPanel�� �@��CornerRadius$ 2,0,0,2���� $2 border�� �@��CornerRadius$ 0,2,2,0���� $2 HeaderPanel�� � ��$ -3,0,0,0q�P� 0 � {�1 .  . ��� # � �� � ���� " ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ 	10,6,10,6q���  � ��$ 5����  � ��$ 5����  � �� �� " ��3 ���� 
SelectedOn .��U� $4 00:00:00X�$5 SelectedState��$6 (UIElement.Opacity)�.\��� $7 00:00:00.1000000��$8 1Q�U� $4 00:00:00X�$5 UnSelectedState��$6 (UIElement.Opacity)�.\��� $7 00:00:00.1000000��$8 0Q��� SelectedOff .��U� $4 00:00:00X�$5 UnSelectedState��$6 (UIElement.Opacity)�.\��� $7 00:00:00.3000000��$8 1Q�U� $4 00:00:00X�$5 SelectedState��$6 (UIElement.Opacity)�.\��� $7 00:00:00.3000000��$8 0Q��� HoverOn .��U� $4 00:00:00X�$5 TopUnselectedOver��$6 (UIElement.Opacity)�.\��� $7 00:00:00.1000000��$8 1Q�U� $4 00:00:00X�$5 TopUnselectedBorderOver��$6 (UIElement.Opacity)�.\��� $7 00:00:00.1000000��$8 1Q��� HoverOff .��U� $4 00:00:00X�$5 TopUnselectedOver��$6 (UIElement.Opacity)�.\��� $7 00:00:00.3000000��$8 0Q�U� $4 00:00:00X�$5 TopUnselectedBorderOver��$6 (UIElement.Opacity)�.\��� $7 00:00:00.3000000��$8 0Q��� 	FocusedOn .��U� $4 00:00:00X�$5 FocusVisualTop��$6 (UIElement.Opacity)�.\��� $7 00:00:00.1000000��$8 1Q��� 
FocusedOff .��U� $4 00:00:00X�$5 FocusVisualTop��$6 (UIElement.Opacity)�.\��� $7 00:00:00.3000000��$8 0Q�.R�� $
9 grid��$��2,1,2,0q�+ V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� $ 0Q�T� $ 0Q�$ 0Q�.+�� .+�� $9 UnSelectedState��$��1,0,1,0q�.+��� $$ TopUnselectedOver��#���  $��1,1,1,0q�$% 2.21,2.21,.5,.5��$: 0Q�#���  �� $$ TopUnselectedBorderOver��$��-1,-1,-1,0.5q�#��� 	 $��1,1,1,0q�$% 3.5,3.5,0,0��$: 0Q�� $9 SelectedState��$��1,0,1,-1q�$: 0Q�.+��� $$ BackgroundTop��#���  $��2,0,2,0q�$% 3,3,0,0��#��� 
 �� $$ 	BorderTop��$��2,0,2,0q�$% 3,3,0,0��#���  �� $$ HighlightTop��$��2,0,2,0q�$��1,3,1,0q�$% 1.25,1.25,0,0��#���  �� $$ 
BorderTop2��$��	-1,0,-1,0q�#���  $��1,1,1,0q�$% 3,3,0,0��; .  �� $$ FocusVisualTop��$��	-1,0,-1,0q�$��3,2,3,0q�$% 3,3,0,0��$: 0Q�#���  �� $$ DisabledVisualTopSelected��$��1,0,1,0q�#���  $% 3,3,0,0��; .  $y�	Collapsed=��� $. ContentSite��$��Center=�#��z��$��Center=���. $��Header��/ P� 0 �@J�	IsFocused1 . < �� $ = FocusedOff_BeginStoryboard��8��  > �� $= FocusedOn_BeginStoryboard��8�� P� 0 �@��TabStripPlacement$
1 Left=�. ��� $
2 grid�� �@�LayoutTransform V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� $	 -90Q�T� $ 0Q�$ 0Q�P� 0 �@��TabStripPlacement$1 Right=�. ��� $
2 grid�� �@�LayoutTransform V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� $ 90Q�T� $ 0Q�$ 0Q�P� 0 �@��TabStripPlacement$1 Bottom=�. ��� $2 ContentSite�� �@�LayoutTransform V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� $	 180Q�T� $ 0Q�$ 0Q��� $
2 grid�� �@�LayoutTransform V� .~��� $ 1Q�$ 1Q��� $, 0Q�$- 0Q��� $	 180Q�T� $ 0Q�$ 0Q�]� < �� $= HoverOff_BeginStoryboard��8�� > �� $= HoverOn_BeginStoryboard��8�� ? �� @ �@J�IsMouseOverA . �� @ �@��
IsSelectedA .  P� 0 �@��
IsSelected1 . < �� $!= SelectedOff_BeginStoryboard��8�� > �� 8�� . ���  �@K�ZIndex$	 100��]� ? �� $
B grid��@ �@J�IsMouseOverA . �� @ �@��
IsSelectedA . P� 0 � {�1 .  . ��� # �   � ����  � �� �� # �  C P� 0 �@��
IsSelected1 . . ���  � ��# �  �� .R��� $��20(�$��Black"�$��5,5,5,5q�$��Segoe UI)����� D . $E OneWay=�$F NoName������ # �  �� .R��� $��16(�$��Black"�$��5,5,5,5q�$��Segoe UI)����� D . $E OneWay=�$F NoName������ # �  �� .R��� $��checkBoxFilter��-   $��0,5,0,0q�$��0��$��14(�$
G TrueY����� D . $E OneWay=��� .R�� 
�������" �� �� .+�]� $��0��.+��� $��16(�$��Temas: ��$	��Top=��� $��12(�
���p�$	��Top=����� *
TracksOk+]� $��1��.+��� $��16(�$��Fallos: ��$	��Top=��� $��12(�
���p�$	��Top=����� *
TracksFail+.��� $��0q�" �� �� $��40 �.+�� $��0��.+��� -   $��tabControl1��$
��Left=�$��5,5,0,0q�$	��Top=�.@��� -   $��tabItem1��$��Géneros-Artistas��.��� $	��300��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0��$��0,10,0,5q�" �� �� �� �� �� .+��� -   $	��100��$��50��$��0��$��0,3,0,3q�.��]� .+��� $
��Wrap=�$��16(�H �� TextAlignment$H Center=�$
��Bold"�
����� �Actualizar Colección�� -   $	��100��$��50��$��1��$��0,3,0,3q�.��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Cargar Colección�� -   $	��100��$��50��$��2��$��0,3,0,3q�.��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Agregar Género�� -   $	��100��$��50��$��3��$��0,3,0,3q�#��[ .��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Eliminar Género�� -	   $	��100��$��50��$��4��$��0,3,0,3q�#��[ .��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Eliminar Todo�� -
   $��genres��$��1��$��	20,0,10,0q�$	��200������ I �� Path$I 
CountGenre�.���� -   $��listBoxGenre��#���  $��5q� . #���  $	��150��$	��240��
����������� $I Genres�J �� SelectedValueJ �� $I SelectedGenre�$E TwoWay=�]� $��2��$	��201��$��0,0,0,20q�.+��� -   $	��100��$��50��$	��Top=�$��0,10,0,5q�.��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Agregar Artista�� -   $	��100��$��50��$	��Top=�$��0,10,0,5q�#��[ .��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Eliminar Artista�� -   $	��100��$��50��#��[ .��]� .+��� $
��Wrap=�$��16(�$H Center=�$
��Bold"�
����� �Eliminar Todo�� -   $��artists��$��3��$	��200��$��	20,0,10,0q����� $I CountArtist�.���� -   $��listBoxArtist��#���  $��5q� . #���  $	��150��$	��240��
����������� $I Artists�J �� $I SelectedArtist�$E TwoWay=��� -   $��tabItem2��$��General��.��� " �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* �  �� $
��Auto ��� $
��Auto �.+��� $��Segoe UI)�$��16(�$��0��$��Right=�.��Valor para tocar video:�� $��Segoe UI)�$��16(�$��1��.��*(Nivel de volumen al iniciar el programa:�� $��Segoe UI)�$��16(�$��2��.��-+Habilitar / Deshabilitar Vista en Miniatura�� $��Segoe UI)�$��16(�$��3��.��,*Habilitar / Deshabilitar ScreenSaver Video�� $��Segoe UI)�$��16(�$��4��.��,*Tamaño de letra de las listas(Géneros..)�� -   $��valuePlayVideoTB��$��1��$
��Left=�$��5,5,0,5q�$��31���� $��0��$��1��$��	32,2,62,3q�$��16��$��26��.��$�� -   $��slider1��$��1��$��1��$
��Left=�$��0,10,0,0q�$	��Top=�$	��100��K � Value$K 50Q�#���  L � Maximum$	L 100Q�M �� AutoToolTipPrecision$M 1��N � SmallChange$N 2Q��� -   $��viewMiniatureCB��$��2��$��2��$��10q��� -   $��viewScreenSaverVideoCB��$��3��$��2��$��10q��� -   $��fontsizeLListBox��$��1��$
��Left=�$��5,5,0,5q�$��90��$��4��]� $��5��.+��� $��Segoe UI)�$��16(�.��Retrasar la Vitrola:�� -   $��delayMin��$��20��$
��Left=�$��30���� $��Segoe UI)�$��16(�$
��Left=�.��	minutos�� -   $��delaySeg��$��20��$
��Left=�$��30���� $��Segoe UI)�$��16(�$
��Left=�.��
segundos�� -   $��tabItem3��$��Activación��.���� .��Este programa está activado�� -   $��tabItem4��$��Filtros��.���   �� �� .+��� -   $��listBoxFilters��$��0��$��0q�$��10(�#���  $	��120��
����������� $I Filters�]� $��1��$��Bottom=�.+��� -   $��60��$��Right=�$��Bottom=�$��5q�.��	Agregar�� -   $��textBoxFilter��$��5q�$��20��$��60��$
��Left=��� -   $��tabItem5��$��Temas��.���   �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $��listBoxBackGound��$��0�� . $��20(�$��0,20,0,0q�$	��150��$	��240��
����������� $I Genres�J �� $I SelectedGenreImage�$E TwoWay=��� $��1��$��10q�.+��� $��10q�
���   �$��14(�$��Asociar WallPapers��.���   �� �� .+��� $��0��$��0,10,0,0q�.��Buscar imagen:�� -    $��seachPicture��$��0,10,0,0q�$��30��$��1��.��...�� $��20q�
���   �$��14(�$��Asociar Colores de Fondo��.��� " �� $
��Auto ��� $��30 �.+�� -!   $��0��" �� $��20 ��� $��20 �  �� �� �� �� .+��� -"   $��MediumOrchid��$��1q�$��0��$��0��$��2q�$% 1��._��� 
����U���� -#   $��Black��$��1q�$��0��$��1��$��2q�$% 1��._��� 
���   ��� -$   $��	MistyRose��$��1q�$��0��$��2��$��2q�._��� 
��������� -%   $��DeepPink��$��1q�$��0��$��3��$��2q�$% 1��._��� 
�������� -&   $��YellowGreen��$��1q�$��1��$��0��$��2q�$% 1��._��� 
���2͚��� -'   $��MediumPurple��$��1q�$��1��$��1��$��2q�$% 1��._��� 
����p���� -(   $��
MediumBlue��$��1q�$��1��$��2��$��2q�$% 1��._��� 
����  ��� -)   $��Tomato��$��1q�$��1��$��3��$��2q�$% 1��._��� 
���Gc���� -*   $��40��$��Right=�$��0,8,5,2q�$��1��.��..mas� -+   $��groupBoxImage��$��2��$	��160��$	��150������ $I SelectColor�.+��� -,   $��imageBackGround���� $��Reportes��.��� " �� �� $��30 �.+��� .��&   System.Windows.Controls.DataGrid  --   $��dataGrid��$��1��$��1��$	��650��
�������O   RowDetailsVisibilityMode$O 	Collapsed=�P   RowBackground
P �����Q   AlternatingRowBackground
Q �����$	��Top=�$��14(�
�������$��5q�R   
IsReadOnlyR . S   AutoGenerateColumnsS .  T   RowDetailsTemplate#T [ U   RowHeaderTemplateU �� .R�9� -.   V   ColumnsV 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumnW   Header$W Fecha��1  +System.Windows.Controls.DataGridBoundColumnX   BindingX �� *	DateShort+  $W Número��X �� *Number+  $W 	Canciones��X �� *CountC+  $W Monedas��X �� *CountM+  $W Dinero��X �� *
CountMoney+  $W 	Erróneas��X �� *CountE+]� $��1��.+��� -/   $��Copperplate Gothic)�$��16(�.��Eliminar ultimas�� -0   $��countTrazasDel��$��Copperplate Gothic)�$��16(�$��5,2,0,0q�$��25��$��Center=�.��7�� $��Copperplate Gothic)�$��16(�$��7,5,0,0q�días.� $��1��  �� �� �� .+��� -1   $��accept��$��0��$	: 0.5Q�
�������$��5,5,5,5q�.��]� $��80��+ �� $, 20Q�.+��� $��Arial)�$��16(�$
��Bold"�
���   �	Aceptar�� -2   $��cancel��$��1��$	: 0.5Q�
�������$��5,5,5,5q�.��]� $��80��+ �� $, 20Q�.+��� $��Arial)�$��16(�$
��Bold"�
���   �$��90��
Cancelar�� -3   $
��help��$��2��$	: 0.5Q�
�������$��5,5,5,5q�.��]� $��80��+ �� $, 20Q�.+��� $��Arial)�$��16(�$
��Bold"�
���   �$��90��Ayuda