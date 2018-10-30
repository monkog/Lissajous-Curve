using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using LissajousCurve.Converters;

namespace LissajousCurve
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
        private int _a
        {
            get { return (int)GetValue(_aProperty); }
            set { SetValue(_aProperty, value); }
        }

        /// <summary>
        /// b value
        /// </summary>
        private int _b
        {
            get { return (int)GetValue(_bProperty); }
            set { SetValue(_bProperty, value); }
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
        private double _milliseconds;

        /// <summary>
        /// Gets or sets current timespan
        /// </summary>
        public double Milliseconds
        {
            get { return _milliseconds; }
            set
            {
                _milliseconds = value;
                NotifyPropertyChanged("Milliseconds");
            }
        }
        /// <summary>
        /// Helper class for managing DispatcherTimer and binding to its Interval property
        /// </summary>
        private DispatcherTimer _timer;

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
        public static readonly DependencyProperty _aProperty
            = DependencyProperty.Register("_a", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static readonly DependencyProperty _bProperty
            = DependencyProperty.Register("_b", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        public static DependencyProperty FadeProperty
            = DependencyProperty.Register("Fade", typeof(int), typeof(MainWindow), new PropertyMetadata(0));
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            ASlider.ValueChanged += ClearPoints;
            BSlider.ValueChanged += ClearPoints;
        }

        /// <summary>
        /// Sets up timer and binding
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetAmplitudeBinding();
            SetSliderBindings();
            _timer = new DispatcherTimer();
            _timer.Tick += TimerTick;
            _timer.Start();
            Milliseconds = 10;

            SizeChanged += ClearPoints;
        }

        /// <summary>
        /// Updates the phase after timer tick
        /// </summary>
        private void TimerTick(object sender, EventArgs e)
        {
            Phase += 0.02;
            MoveEllipse();
        }

        /// <summary>
        /// Clears all previously drawn paths
        /// </summary>
        void ClearPoints(object sender, EventArgs e)
        {
            Line.Points.Clear();
        }

        /// <summary>
        /// Sets binding to A and B
        /// </summary>
        private void SetAmplitudeBinding()
        {
            Binding aBinding = new Binding("ActualWidth");
            aBinding.Source = Canvas;
            aBinding.Mode = BindingMode.OneWay;
            aBinding.Converter = new AmplitudeConverter();
            SetBinding(AProperty, aBinding);

            Binding bBinding = new Binding("ActualHeight");
            bBinding.Source = Canvas;
            bBinding.Mode = BindingMode.OneWay;
            bBinding.Converter = new AmplitudeConverter();
            SetBinding(BProperty, bBinding);
        }

        /// <summary>
        /// Sets a, b, fade and speed bindings
        /// </summary>
        private void SetSliderBindings()
        {
            SetSimpleBinding(_aProperty, "Value", ASlider);
            SetSimpleBinding(_bProperty, "Value", BSlider);
            SetSimpleBinding(FadeProperty, "Value", FadeSlider);
        }

        /// <summary>
        /// Universal function for setting a simple binding
        /// </summary>
        /// <param name="dp">Dependency property</param>
        /// <param name="path">Binding path</param>
        /// <param name="source">Binding source</param>
        private void SetSimpleBinding(DependencyProperty dp, string path, object source)
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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
			_timer.Interval = TimeSpan.FromMilliseconds(_milliseconds);
        }

        /// <summary>
        /// Draws the curve and moves the ellipse
        /// </summary>
        /// <remarks>
        /// Lissajous curve is a graph of parametric equations:
        /// x = Asin(at + delta), y = Bsin(bt)
        /// </remarks>
        private void MoveEllipse()
        {
            double delta = ((B - 1) / B) * (Math.PI / 2);
            double x, y;

            x = A * Math.Sin(_a * Phase + delta) + A;
            y = B * Math.Sin(_b * Phase) + B;

            Canvas.SetLeft(Ellipse, x - Ellipse.Width / 2);
            Canvas.SetTop(Ellipse, y - Ellipse.Width / 2);

            Line.Points.Add(new Point(x, y));

            while (Line.Points.Count > Fade)
                Line.Points.RemoveAt(0);
        }
    }
}
