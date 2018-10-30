using LissajousCurve;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LissajousCurveTests
{
	[TestClass]
	public class CurveTests
	{
		[TestMethod]
		public void Ctor_NoParams_DefaultValuesSet()
		{
			var unitUnderTest = new Curve();

			Assert.AreEqual(2, unitUnderTest.A);
			Assert.AreEqual(5, unitUnderTest.B);
			Assert.AreEqual(500, unitUnderTest.Fade);
		}

		[TestMethod]
		public void Ctor_Params_PropertiesSet()
		{
			var unitUnderTest = new Curve(3, 9, 77);

			Assert.AreEqual(3, unitUnderTest.A);
			Assert.AreEqual(9, unitUnderTest.B);
			Assert.AreEqual(77, unitUnderTest.Fade);
		}
	}
}