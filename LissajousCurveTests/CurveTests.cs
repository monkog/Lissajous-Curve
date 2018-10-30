using System.Windows;
using LissajousCurve;
using NUnit.Framework;

namespace LissajousCurveTests
{
	[TestFixture]
	public class CurveTests
	{
		private Curve _unitUnderTest;

		[SetUp]
		public void Initialize()
		{
			_unitUnderTest = new Curve();
		}

		[Test]
		public void Ctor_NoParams_DefaultValuesSet()
		{
			var unitUnderTest = new Curve();

			Assert.AreEqual(2, unitUnderTest.A);
			Assert.AreEqual(6, unitUnderTest.B);
			Assert.AreEqual(500, unitUnderTest.Fade);
			Assert.AreEqual(0, unitUnderTest.Delta);
			Assert.IsNotNull(unitUnderTest.Points);
		}

		[Test]
		public void Ctor_Params_PropertiesSet()
		{
			var unitUnderTest = new Curve(3, 9, 77, 2);

			Assert.AreEqual(3, unitUnderTest.A);
			Assert.AreEqual(9, unitUnderTest.B);
			Assert.AreEqual(77, unitUnderTest.Fade);
			Assert.AreEqual(2, unitUnderTest.Delta);
			Assert.IsNotNull(unitUnderTest.Points);
		}

		[Test]
		public void SetA_Always_PointsCollectionCleared()
		{
			_unitUnderTest.Points.Add(new Point(10, 10));

			_unitUnderTest.A = 10;

			Assert.AreEqual(0, _unitUnderTest.Points.Count);
		}

		[Test]
		public void SetB_Always_PointsCollectionCleared()
		{
			_unitUnderTest.Points.Add(new Point(10, 10));

			_unitUnderTest.B = 10;

			Assert.AreEqual(0, _unitUnderTest.Points.Count);
		}

		[Test]
		public void SetDelta_Always_PointsCollectionCleared()
		{
			_unitUnderTest.Points.Add(new Point(10, 10));

			_unitUnderTest.Delta = 10;

			Assert.AreEqual(0, _unitUnderTest.Points.Count);
		}

		[Test]
		public void SetAmplitude_Always_PointsCollectionCleared()
		{
			_unitUnderTest.Points.Add(new Point(10, 10));

			_unitUnderTest.SetAmplitude(10, 2);

			Assert.AreEqual(0, _unitUnderTest.Points.Count);
		}

		[Test]
		public void Move_Always_PointAdded()
		{
			_unitUnderTest.Move(0.1);

			Assert.AreEqual(1, _unitUnderTest.Points.Count);
		}

		[Test]
		public void Move_Always_ReturnsLatestPoint()
		{
			_unitUnderTest.SetAmplitude(2, 2);

			var result = _unitUnderTest.Move(0);

			Assert.AreEqual(1, result.X);
			Assert.AreEqual(1, result.Y);
		}
	}
}