using Lecture_8_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_8_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_8_Tests
{
    [TemplatedTestClass]
    public class Exercise_4_Tests_Template
    {
        #region Exercise 4B
        [TestMethod("a. ConsoleController.HandleInput(string input) is a public method"), TestCategory("Exercise 4B")]
        public void ConsoleViewRunIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ConsoleController, string>((c, s) => c.HandleInput(s));
            test.Execute();
        }

        [TemplatedTestMethod("b. ConsoleController.HandleInput(\"Echo Hello world\") echos"), TestCategory("Exercise 4B")]
        public void ConsoleControllerHandleInputEchos()
        {
            // MSTest Extended
            ConsoleController controller = new ConsoleController();
            ConsoleAssert.WritesOut(() => controller.HandleInput("Echo Hello World"), "Hello World");
        }

        [TemplatedTestMethod("c. ConsoleController.HandleInput(\"Reverse Hello world\") reverses"), TestCategory("Exercise 4B")]
        public void ConsoleControllerHandleInputReverses()
        {
            // MSTest Extended
            ConsoleController constroller = new ConsoleController();
            ConsoleAssert.WritesOut(() => constroller.HandleInput("Reverse Hello world"), "dlrow olleH");
        }

        [TemplatedTestMethod("d. ConsoleController.HandleInput(\"Greet world\") greets"), TestCategory("Exercise 4B")]
        public void ConsoleControllerHandleInputGreets()
        {
            // MSTest Extended
            ConsoleController controller = new ConsoleController();
            ConsoleAssert.WritesOut(() => controller.HandleInput("Greet World"), "Hello World");
        }

        [TemplatedTestMethod("e. ConsoleController.HandleInput(\"NonExistentCommand Hello World\") errors"), TestCategory("Exercise 4B")]
        public void ConsoleControllerHandleInputErrors()
        {
            ConsoleController controller = new ConsoleController();
            ConsoleAssert.WritesOut(
                () => controller.HandleInput("NonExistentCommand Hello World"),
                "Command NonExistentCommand not found");
        }

        [TemplatedTestMethod("f. ConsoleController.HandleInput(\"Greet\") errors"), TestCategory("Exercise 4B")]
        public void ConsoleControllerHandleInputErrors2()
        {
            // MSTest Extended
            ConsoleController controller = new ConsoleController();
            ConsoleAssert.WritesOut(
                () => controller.HandleInput("Greet"),
                "Please provide a command argument");
        }

        [TemplatedTestMethod("g. ConsoleController.HandleInput(\"\") errors"), TestCategory("Exercise 4B")]
        public void ConsoleControllerHandleInputErrors3()
        {
            // MSTest Extended
            ConsoleController controller = new ConsoleController();
            ConsoleAssert.WritesOut(
                () => controller.HandleInput(""),
                "Please provide a command");
        }
        #endregion

        #region exercise 4C
        [TestMethod("a. ConsoleController.AddCommand(string name, Action<string> action) is a public method"), TestCategory("Exercise 4C")]
        public void ConsoleControllerAddCommand()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ConsoleController, string, Action<string>>((c, s, a) => c.AddCommand(s, a));
            test.Execute();
        }

        [TemplatedTestMethod("b. ConsoleController.AddCommand(\"SayGoodbye\", (s) => ...) adds command"), TestCategory("Exercise 4C")]
        public void ConsoleConstrollerAddCommandAddsCommand()
        {
            // MSTest Extended
            ConsoleController controller = new ConsoleController();

            controller.AddCommand("SayGoodbye", s => Console.WriteLine("Goodbye " + s));

            ConsoleAssert.WritesOut(
                () => controller.HandleInput("SayGoodbye World"),
                "Goodbye World");
        }
        #endregion

        #region Exercise 4D
        public void ConsoleControllerRemoveCommand()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ConsoleController, string>((c, s) => c.RemoveCommand(s));
            test.Execute();
        }

        [TemplatedTestMethod("a. ConsoleController.RemoveCommand(\"SayGoodbye\") removes command again"), TestCategory("Exercise 4D")]
        public void ConsoleControllerRemoveCommandRemovesCommand()
        {
            // MSTest Extended
            ConsoleController controller = new ConsoleController();

            controller.AddCommand("SayGoodbye", s => Console.WriteLine("Goodbye " + s));
            controller.RemoveCommand("SayGoodbye");

            ConsoleAssert.WritesOut(
                () => controller.HandleInput("SayGoodbye World"),
                "Command SayGoodbye not found");
        }
        #endregion
    }
}
