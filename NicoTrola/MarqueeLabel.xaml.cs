using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para MarqueeLabel.xaml
    /// </summary>
    public partial class MarqueeLabel : UserControl
    {
        
        public MarqueeLabel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "MarqueeText", typeof (string), typeof (MarqueeLabel),
                new FrameworkPropertyMetadata(String.Empty, new PropertyChangedCallback(MarqueeTextChangedCallback)));

        public string MarqueeText { get; set; }

        private static void MarqueeTextChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MarqueeLabel MarqueControl = (MarqueeLabel)obj;
            MarqueControl.lblText.Content = args.NewValue;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = OuterPanel.ActualWidth / 2 - lblText.ActualWidth; 
            doubleAnimation.To = OuterPanel.ActualWidth - 10 ;
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(14));
            lblText.BeginAnimation(Canvas.RightProperty, doubleAnimation);
        }
    }
}
