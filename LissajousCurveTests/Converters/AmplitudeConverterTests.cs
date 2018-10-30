using LissajousCurve.Converters;
using NUnit.Framework;

namespace LissajousCurveTests.Converters
{
	[TestFixture]
	public class AmplitudeConverterTests
	{
		private AmplitudeConverter _unitUnderTest;

		[SetUp]
		public void Initialize()
		{
			_unitUnderTest = new AmplitudeConverter();
		}

		[Test]
		[TestCase(10, 5)]
		[TestCase(10.5, 5)]
		[TestCase(0, 0)]
		[TestCase(-3.5, -1)]
		public void Convert_Size_Amplitude(double value, int amplitude)
		{
			var result = _unitUnderTest.Convert(value, null, null, null);

			Assert.AreEqual(amplitude, result);
		}

		[Test]
		[TestCase(10, 20)]
		[TestCase(0, 0)]
		[TestCase(-3, -6)]
		public void ConvertBack_Amplitude_Size(int amplitude, double value)
		{
			var result = _unitUnderTest.ConvertBack(amplitude, null, null, null);

			Assert.AreEqual(value, result);
		}
	}
}