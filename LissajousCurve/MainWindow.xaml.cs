using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LissajousCurve
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private Curve _curve;

		private readonly DispatcherTimer _timer;

		private double _milliseconds;

		private double _phase;

		/// <summary>
		/// Gets or sets current timespan
		/// </summary>
		public double Milliseconds
		{
			get { return _milliseconds; }
			set
			{
				_milliseconds = value;
				OnPropertyChanged();
				_timer.Interval = TimeSpan.FromMilliseconds(_milliseconds);
			}
		}

		/// <summary>
		/// Gets or sets current timespan
		/// </summary>
		public Curve Curve
		{
			get { return _curve; }
			set
			{
				_curve = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public MainWindow()
		{
			InitializeComponent();

			_timer = new DispatcherTimer();
			_timer.Tick += DrawNextFrame;
			_timer.Start();
			Milliseconds = 10;
			Curve = new Curve();
		}

		private void DrawNextFrame(object sender, EventArgs e)
		{
			_phase += 0.02;
			var position = Curve.Move(_phase);

			Canvas.SetLeft(Ellipse, position.X - Ellipse.Width / 2);
			Canvas.SetTop(Ellipse, position.Y - Ellipse.Width / 2);
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (!(sender is Canvas)) return;
			var canvas = (Canvas) sender;
			Curve.SetAmplitude(canvas.ActualWidth, canvas.ActualHeight);
		}
	}
}