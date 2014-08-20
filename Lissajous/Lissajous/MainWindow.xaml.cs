using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Lissajous
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Variables and Properties
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
        /// a value
        /// </summary>
        private int a
        {
            get { return (int)GetValue(aProperty); }
            set { SetValue(aProperty, value); }
        }
        /// <summary>
        /// b value
        /// </summary>
        private int b
        {
            get { return (int)GetValue(bProperty); }
            set { SetValue(bProperty, value); }
        }
        /// <summary>
        /// Fade value
        /// </summary>
        private int Fade
        {
            get { return (int)GetValue(FadeProperty); }
            set { SetValue(FadeProperty, value); }
        }
        /// <summary>
        /// Keeps track of bound slider value
        /// </summary>
        private double m_milliseconds;
        /// <summary>
        /// Gets or sets current timespan
        /// </summary>
        public double Milliseconds
        {
            get { return m_milliseconds; }
            set
            {
                m_milliseconds = value;
                NotifyPropertyChanged("Milliseconds");
            }
        }
        /// <summary>
        /// Helper class for managing DispatcherTimer and binding to its Interval property
        /// </summary>
        private DispatcherTimer m_timer;
        /// <summary>
        /// Time phase 
        /// </summary>
        private double Phase { get; set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region DependencyProperties
        public static readonly DependencyProperty AProperty
            = DependencyProperty.Register("A", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static readonly DependencyProperty BProperty
            = DependencyProperty.Register("B", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static readonly DependencyProperty aProperty
            = DependencyProperty.Register("a", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static readonly DependencyProperty bProperty
            = DependencyProperty.Register("b", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static DependencyProperty FadeProperty
            = DependencyProperty.Register("Fade", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            m_aSlider.ValueChanged += ClearPoints;
            m_bSlider.ValueChanged += ClearPoints;
        }

        /// <summary>
        /// Sets up timer and binding
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            setAmplitudeBinding();
            setSliderBindings();
            m_timer = new DispatcherTimer();
            m_timer.Tick += m_timer_Tick;
            m_timer.Start();
            Milliseconds = 10;

            SizeChanged += ClearPoints;
        }

        /// <summary>
        /// Updates the phase after timer tick
        /// </summary>
        private void m_timer_Tick(object sender, EventArgs e)
        {
            Phase += 0.02;
            moveEllipse();
        }

        /// <summary>
        /// Clears all previously drawn paths
        /// </summary>
        void ClearPoints(object sender, EventArgs e)
        {
            m_line.Points.Clear();
        }

        /// <summary>
        /// Sets binding to A and B
        /// </summary>
        private void setAmplitudeBinding()
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
        /// Sets a, b, fade and speed bindings
        /// </summary>
        private void setSliderBindings()
        {
            setSimpleBinding(aProperty, "Value", m_aSlider);
            setSimpleBinding(bProperty, "Value", m_bSlider);
            setSimpleBinding(FadeProperty, "Value", m_fadeSlider);
        }

        /// <summary>
        /// Universal function for setting a simple binding
        /// </summary>
        /// <param name="dp">Dependency property</param>
        /// <param name="path">Binding path</param>
        /// <param name="source">Binding source</param>
        private void setSimpleBinding(DependencyProperty dp, string path, object source)
        {
            Binding binding = new Binding(path);
            binding.Mode = BindingMode.OneWay;
            binding.Source = source;
            SetBinding(dp, binding);
        }

        /// <summary>
        /// Notifies that a property's value has changed
        /// </summary>
        /// <param name="property">Property name</param>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            m_timer.Interval = TimeSpan.FromMilliseconds(m_milliseconds);
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

            x = A * Math.Sin(a * Phase + delta) + A;
            y = B * Math.Sin(b * Phase) + B;

            Canvas.SetLeft(m_ellipse, x - m_ellipse.Width / 2);
            Canvas.SetTop(m_ellipse, y - m_ellipse.Width / 2);

            m_line.Points.Add(new Point(x, y));

            while (m_line.Points.Count > Fade)
                m_line.Points.RemoveAt(0);
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
