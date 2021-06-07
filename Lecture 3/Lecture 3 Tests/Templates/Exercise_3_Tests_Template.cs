using Lecture_3_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_3_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_3_Tests
{
    [TemplatedTestClass]
    public class Exercise_3_Tests_Template
    {
        #region Exercise 3A
        [TestMethod("a. Figure is abstract class"), TestCategory("Exercise 3A")]
        public void FigureIsAbstractClass()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertAbstractClass<Figure>();
            test.Execute();
        }

        [TestMethod("b. Figure.CalculateArea() is abstract method"), TestCategory("Exercise 3A")]
        public void FigureCalculateAreaIsAbstractMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertMethod<Figure, double>(
                f => f.CalculateArea(), 
                new MemberAccessLevelVerifier(AccessLevels.Public),
                new MemberIsAbstractVerifier());
            test.Execute();
        }

        [TestMethod("c. Figure.Contains(Point p) is abstract method"), TestCategory("Exercise 3A")]
        public void FigureContainsIsAbstractMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertMethod<Figure, Point, bool>(
                (f, p) => f.Contains(p), 
                new MemberAccessLevelVerifier(AccessLevels.Public),
                new MemberIsAbstractVerifier());
            test.Execute();
        }
        #endregion

        #region Exercise 3B
        [TestMethod("a. Circle is subclass of Figure"), TestCategory("Exercise 3B")]
        public void CircleIsSubclassOfFigure() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Circle>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeBaseClassVerifier(typeof(Figure)));
            test.Execute();
        }

        [TestMethod("b. Rectangle is subclass of Figure"), TestCategory("Exercise 3B")]
        public void RectangleIsSubclassOfFigure()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Rectangle>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeBaseClassVerifier(typeof(Figure)));
            test.Execute();
        }
        #endregion

        #region Exercise 3C
        [TestMethod("a. Circle.Center is public read-only Point property"), TestCategory("Exercise 3C")]
        public void CenterIsPublicPointProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Circle, Point>(c => c.Center);
            test.Execute();
        }

        [TestMethod("b. Circle.Radius is public read-only double property"), TestCategory("Exercise 3C")]
        public void RadiusIsPublicDoubleProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Circle, double>(c => c.Radius);
            test.Execute();
        }

        [TemplatedTestMethod("c. Circle(Point center, double radius) ignores center = null"), TestCategory("Exercise 3C")]
        public void CenterIgnoresAssigmentOfNull() 
        {
            Circle circle = new Circle(null, 1.0);
            Assert.IsNotNull(circle.Center);
        }

        [TemplatedTestMethod("d. Circle(Point center, double radius) ignores radius = -1.0"), TestCategory("Exercise 3C")]
        public void RadiusIgnoresAssigmentOfMinusOne() 
        {
            Circle circle = new Circle(new Point(0, 0), -1.0);
            Assert.AreNotEqual(-1.0, circle.Radius, 0.001);
        }
        #endregion

        #region Exercise 3D
        [TestMethod("a. Rectangle.P1 is public read-only Point property"), TestCategory("Exercise 3D")]
        public void P1IsPublicPointProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Rectangle, Point>(r => r.P1);
            test.Execute();
        }

        [TestMethod("b. Regtangle.P2 is public read-only Point property"), TestCategory("Exercise 3D")]
        public void P2IsPublicPointProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Rectangle, Point>(r => r.P2);
            test.Execute();
        }

        [TemplatedTestMethod("c. Rectangle(Point p1, Point p2) ignores p1 = null"), TestCategory("Exercise 3D")]
        public void RectangleConstructorIgnoresP1ValueNull()
        {
            Rectangle rectangle = new Rectangle(null, new Point(1, 1));
            Assert.IsNotNull(rectangle.P1);
        }

        [TemplatedTestMethod("d. Rectangle(Point p1, Point p2) ignores p2 = null"), TestCategory("Exercise 3D")]
        public void RegtangleConstructorIgnoresP2ValueNull() 
        {
            Rectangle rectangle = new Rectangle(new Point(0, 0), null);
            Assert.IsNotNull(rectangle.P2);
        }
        #endregion

        #region Exercise 3E
        [TemplatedTestMethod("a. Circle.CalculateArea() returns expected output"), TestCategory("Exercise 3E")]
        public void CircleCalculateAreaReturnsExpectedOutput()
        {
            double expectedValue = 42.3 * 42.3 * Math.PI;
            Circle circle = new Circle(new Point(0, 0), 42.3);

            Assert.AreEqual(expectedValue, circle.CalculateArea(), 0.001);
        }

        [TemplatedTestMethod("b. Circle.Contains(Point p) returns true for point within circle"), TestCategory("Exercise 3E")]
        public void CircleContainsReturnTrueForPointWithinCircle()
        {
            Circle circle = new Circle(new Point(2, 3), 1);
            Assert.IsTrue(circle.Contains(new Point(2.5, 3)));
        }

        [TemplatedTestMethod("c. Circle.Contains(Point p) returns true for point on perimeter of circle"), TestCategory("Exercise 3E")]
        public void CircleContainsReturnTrueForPointOnPerimeterOfCircle()
        {
            Circle circle = new Circle(new Point(2, 3), 1);
            Assert.IsTrue(circle.Contains(new Point(3, 3)));
        }
        
        [TemplatedTestMethod("d. Circle.Contains(Point p) returns false for point outside of circle"), TestCategory("Exercise 3E")]
        public void CircleContainsReturnFalseForPointOutsideOfCircle()
        {
            Circle circle = new Circle(new Point(2, 3), 1);
            Assert.IsFalse(circle.Contains(new Point(4, 3)));
        }

        [TemplatedTestMethod("e. Rectangle.CalculateArea() returns expected output"), TestCategory("Exercise 3E")]
        public void RectangleCalculateAreaReturnsExpectedOutput()
        {
            Rectangle rectangle = new Rectangle(new Point(0, 0), new Point(2, 3));
            Assert.AreEqual(6, rectangle.CalculateArea(), 0.001);
        }

        [TemplatedTestMethod("f. Rectangle.Contains(Point p) returns true for point within rectangle"), TestCategory("Exercise 3E")]
        public void RectangleContainsReturnTrueForPointWithinRectangle()
        {
            Rectangle rectangle = new Rectangle(new Point(2, 3), new Point(3, 5));
            Assert.IsTrue(rectangle.Contains(new Point(2.5, 3)));
        }

        [TemplatedTestMethod("g. Rectangle.Contains(Point p) returns true for point on perimeter of rectangle"), TestCategory("Exercise 3E")]
        public void RectangleContainsReturnTrueForPointOnPerimeterOfRectangle()
        {
            Rectangle rectangle = new Rectangle(new Point(2, 3), new Point(3, 5));
            Assert.IsTrue(rectangle.Contains(new Point(3, 3)));
        }

        [TemplatedTestMethod("h. Rectangle.Contains(Point p) returns false for point outside of circle"), TestCategory("Exercise 3E")]
        public void RectangleContainsReturnFalseForPointOutsideOfRectangle()
        {
            Rectangle rectangle = new Rectangle(new Point(2, 3), new Point(3, 5));
            Assert.IsFalse(rectangle.Contains(new Point(4, 3)));
        }
        #endregion
    }
}
