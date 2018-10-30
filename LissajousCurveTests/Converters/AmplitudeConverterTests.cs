using LissajousCurve.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LissajousCurveTests.Converters
{
	[TestClass]
	public class AmplitudeConverterTests
	{
		private AmplitudeConverter _unitUnderTest;

		[TestInitialize]
		public void Initialize()
		{
			_unitUnderTest = new AmplitudeConverter();
		}

		[TestMethod]
		[DataRow(10, 5)]
		[DataRow(10.5, 5)]
		[DataRow(0, 0)]
		[DataRow(-3.5, -1)]
		public void Convert_Size_Amplitude(double value, int amplitude)
		{
			var result = _unitUnderTest.Convert(value, null, null, null);

			Assert.AreEqual(amplitude, result);
		}

		[TestMethod]
		[DataRow(10, 20)]
		[DataRow(0, 0)]
		[DataRow(-3, -6)]
		public void ConvertBack_Amplitude_Size(int amplitude, double value)
		{
			var result = _unitUnderTest.ConvertBack(amplitude, null, null, null);

			Assert.AreEqual(value, result);
		}
	}
}