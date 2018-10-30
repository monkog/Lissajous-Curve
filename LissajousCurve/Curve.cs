using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LissajousCurve
{
	/// <summary>
	/// Represents a Lissajous curve.
	/// </summary>
	/// <remarks>
	/// x = A * sin(a * t + delta), y = B * sin(b * t)
	/// A - horizontal amplitude
	/// B - vertical amplitude
	/// </remarks>
	public class Curve: INotifyPropertyChanged
	{
		private int _a;

		private int _b;

		private int _fade;

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
		/// Gets or sets the length of the path that stays displayed on the screen.
		/// </summary>
		public int A
		{
			get { return _a; }
			set
			{
				_a = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the length of the path that stays displayed on the screen.
		/// </summary>
		public int B
		{
			get { return _b; }
			set
			{
				_b = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Curve"/> class.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="fade"></param>
		public Curve(int a  = 2, int b = 5, int fade = 500)
		{
			A = a;
			B = b;
			Fade = fade;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}