using Lecture_8_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static OleVanSanten.TestTools.Expressions.TestExpression;
using static Lecture_8_Tests.TestHelper;
using static OleVanSanten.TestTools.Helpers.StructureHelper;

namespace Lecture_8_Tests
{
    [TemplatedTestClass]
    public class Exercise_3_Tests_Template
    {
        #region Exercise 3A
        [TestMethod("a. ConsoleView.Run() is a public method"), TestCategory("Exercise 3A")]
        public void ConsoleViewRunIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ConsoleView>(v => v.Run());
            test.Execute();
        }

        [TemplatedTestMethod("b. ConsoleView.Run() returns on empty line input"), TestCategory("Exercise 3A")]
        public void ConsoleViewRunReturnsOnEmptyLineInput()
        {
            // MSTest Extended
            ConsoleView view = new ConsoleView();

            ConsoleInputter.Clear();
            ConsoleInputter.WriteLine();
            view.Run();
        }
        #endregion

        #region exercise 3B
        [TestMethod("a. InputHandler is public delegate"), TestCategory("Exercise 3B")]
        public void InputHandlerIsPublicDelegate()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicDelegate<InputHandler, Action<string>>();
            test.Execute();
        }
        #endregion

        #region Exercise 3C
        [TestMethod("a. ConsoleView.Input is public event"), TestCategory("Exercise 3C")]
        public void ConsoleViewInputIsPublicEvent()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertEvent(
                typeof(ConsoleView).GetEvent("Input"),
                new MemberAccessLevelVerifier(AccessLevels.Public),
                new EventHandlerTypeVerifier(typeof(InputHandler)));
            test.Execute();
        }

        [TemplatedTestMethod("b. ConsoleView.Run emits Input on non-empty-line input"), TestCategory("Exercise 3C")]
        public void ConsoleViewRunEmitsInputOnNonEmptyLineInput()
        {
            bool isCalled = false;
            ConsoleView view = new ConsoleView();
            view.Input += (input) => isCalled = true;

            ConsoleInputter.Clear();
            ConsoleInputter.WriteLine("User input");
            ConsoleInputter.WriteLine();
            view.Run();

            Assert.IsTrue(isCalled, "The ConsoleView.Run event was never emitted");
        }

        [TemplatedTestMethod("c. ConsoleView.Run does not emit Input on empty-line input"), TestCategory("Exercise 3C")]
        public void ConsoleViewRunDoesNotEmitInputOnEmptyLineInput()
        {
            bool isCalled = false;
            ConsoleView view = new ConsoleView();
            view.Input += (input) => isCalled = true;

            ConsoleInputter.Clear();
            ConsoleInputter.WriteLine();
            view.Run();

            Assert.IsFalse(isCalled, "The ConsoleView.Run event was emitted");
        }
        #endregion
    }
}
