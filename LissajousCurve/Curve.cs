using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace LissajousCurve
{
	/// <summary>
	/// Represents a Lissajous curve.
	/// </summary>
	public class Curve : INotifyPropertyChanged
	{
		private int _a;

		private int _b;

		private int _fade;

		private PointCollection _points;

		private int _horizontalAmplitude;

		private int _verticalAmplitude;

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets or sets the length of the path that stays displayed on the screen.
		/// </summary>
		public int Fade
		{
			get { return _fade; }
			set
			{
				_fade = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the a parameter of the curve.
		/// </summary>
		public int A
		{
			get { return _a; }
			set
			{
				_a = value;
				OnPropertyChanged();
				Points.Clear();
			}
		}

		/// <summary>
		/// Gets or sets the b parameter of the curve.
		/// </summary>
		public int B
		{
			get { return _b; }
			set
			{
				_b = value;
				OnPropertyChanged();
				Points.Clear();
			}
		}

		/// <summary>
		/// Gets or sets the Collection of points of this curve being displayed.
		/// </summary>
		public PointCollection Points
		{
			get { return _points; }
			set
			{
				_points = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Curve"/> class.
		/// </summary>
		/// <param name="a">A parameter of the curve.</param>
		/// <param name="b">B parameter of the curve.</param>
		/// <param name="fade">The length of the curve being displayed.</param>
		public Curve(int a = 2, int b = 6, int fade = 500)
		{
			_a = a;
			_b = b;
			Fade = fade;
			Points = new PointCollection();
		}

		/// <summary>
		/// Moves the curve.
		/// </summary>
		/// <remarks>
		/// x = A * sin(a * t + delta), y = B * sin(b * t)
		/// A - horizontal amplitude
		/// B - vertical amplitude
		/// </remarks>
		/// <param name="phase">Current phase.</param>
		/// <returns>Current position.</returns>
		public Point Move(double phase)
		{
			var delta = ((_verticalAmplitude - 1.0) / _verticalAmplitude) * (Math.PI / 2);

			var x = _horizontalAmplitude * Math.Sin(A * phase + delta) + _horizontalAmplitude;
			var y = _verticalAmplitude * Math.Sin(B * phase) + _verticalAmplitude;

			Points.Add(new Point(x, y));

			while (Points.Count > Fade)
				Points.RemoveAt(0);

			// For the sake of binding notification.
			Points = new PointCollection(Points);

			return new Point(x, y);
		}

		/// <summary>
		/// Sets the vertical and horizontal amplitude values.
		/// </summary>
		/// <param name="horizontalAmplitude">The new value of horizontal amplitude.</param>
		/// <param name="verticalAmplitude">The new value of vertical amplitude.</param>
		public void SetAmplitude(double horizontalAmplitude, double verticalAmplitude)
		{
			_verticalAmplitude = (int)verticalAmplitude / 2;
			_horizontalAmplitude = (int)horizontalAmplitude / 2;

			Points.Clear();
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}