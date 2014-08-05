using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lissajous
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Width amplitude
        /// </summary>
        private int A
        {
            get { return (int)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }
        /// <summary>
        /// Height amplitude
        /// </summary>
        private int B
        {
            get { return (int)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        /// <summary>
        /// Timer to keep track of moving Ellipse
        /// </summary>
        private DispatcherTimer m_timer;
        /// <summary>
        /// Time phase 
        /// </summary>
        private double Phase { get; set; }

        public static readonly DependencyProperty AProperty
            = DependencyProperty.Register("A", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static readonly DependencyProperty BProperty
            = DependencyProperty.Register("B", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Sets up timer and binding
        /// </summary>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            setBinding();
            m_timer = new DispatcherTimer();
            m_timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            m_timer.Tick += m_timer_Tick;
            m_timer.Start();

            SizeChanged += MainWindow_SizeChanged;
        }

        /// <summary>
        /// Updates the phase after timer tick
        /// </summary>
        void m_timer_Tick(object sender, EventArgs e)
        {
            Phase += 0.01;
            moveEllipse();
        }

        /// <summary>
        /// Clears all previously drawn paths
        /// </summary>
        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            m_line.Points.Clear();
        }

        /// <summary>
        /// Sets binding to A and B
        /// </summary>
        private void setBinding()
        {
            Binding aBinding = new Binding("ActualWidth");
            aBinding.Source = m_canvas;
            aBinding.Mode = BindingMode.OneWay;
            aBinding.Converter = new AmplitudeConverter();
            SetBinding(AProperty, aBinding);

            Binding bBinding = new Binding("ActualHeight");
            bBinding.Source = m_canvas;
            bBinding.Mode = BindingMode.OneWay;
            bBinding.Converter = new AmplitudeConverter();
            SetBinding(BProperty, bBinding);
        }

        /// <summary>
        /// Draws the curve and moves the ellipse
        /// </summary>
        /// <remarks>
        /// Lissajous curve is a graph of parametric equations:
        /// x = Asin(at + delta), y = Bsin(bt)
        /// </remarks>
        private void moveEllipse()
        {
            double delta = ((B - 1) / B) * (Math.PI / 2);
            double x, y;

            x = A * Math.Sin(2 * Phase + delta) + A;
            y = B * Math.Sin(5 * Phase) + B;

            Canvas.SetLeft(m_ellipse, x - m_ellipse.Width / 2);
            Canvas.SetTop(m_ellipse, y - m_ellipse.Width / 2);

            m_line.Points.Add(new Point(x, y));
        }

        /// <summary>
        /// Width and height to amplitude converter
        /// </summary>
        [ValueConversion(typeof(double), typeof(int))]
        public class AmplitudeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (int)((double)value / 2);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (double)value * 2;
            }
        }

    }
}
