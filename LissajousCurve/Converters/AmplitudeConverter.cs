using System;
using System.Globalization;
using System.Windows.Data;

namespace LissajousCurve.Converters
{
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
			return (int)value * 2.0;
		}
	}
}