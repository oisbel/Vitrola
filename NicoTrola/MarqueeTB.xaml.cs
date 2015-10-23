using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para MarqueeTB.xaml
    /// </summary>
    public partial class MarqueeTB : UserControl
    {
        public MarqueeTB()
        {
            InitializeComponent();
           
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        //    var gradientStop = new GradientStop();
        //    gradientStop.Offset = 0;
        //    gradientStop.Color = Colors.Black;
        //    var gradientStop1 = new GradientStop();
        //    gradientStop1.Offset = 1;
        //    gradientStop1.Color = Colors.Red;
        //    var GradientStops = new GradientStopCollection(new List<GradientStop>() { gradientStop, gradientStop1 });
        //    lblText.Background = new RadialGradientBrush(GradientStops);


            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0 ;
            doubleAnimation.To = OuterPanel.ActualWidth/2;
            doubleAnimation.AutoReverse = true;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));

            lblText.BeginAnimation(Canvas.RightProperty, doubleAnimation);

           

        
            //lblText.Background = Brushes.Transparent;
        }
        
    }
}
