using Lecture_6_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_6_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_6_Tests
{
    [TemplatedTestClass]
    public class Exercise_6_Tests_Template
    {
        #region Exercise 6A
        [TestMethod("a.ILogger is an interface"), TestCategory("Exercise 6A")]
        public void ILoggerIsAnInterface()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertInterface<ILogger>();
            test.Execute();
        }

        [TestMethod("b.ILogger.Log(string message) is a method"), TestCategory("Exercise 6A")]
        public void ILoggerLogIsAMehthod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ILogger, string>((l, s) => l.Log(s));
            test.Execute();
        }
        #endregion

        #region Exercise 6B
        [TestMethod("a. FileLogger's constructor takes string"), TestCategory("Exercise 6B")]
        public void DieConstructorTakesIRandomAndInt()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<string, ILogger>(s => new FileLogger(s));
            test.Execute();
        }
        #endregion

        #region Exercise 6C
        [TestMethod("a. FileLogger implements ILogger"), TestCategory("Exercise 6C")]
        public void FileLoggerImplementILogger()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<FileLogger>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeIsSubclassOfVerifier(typeof(ILogger)));
            test.Execute();
        }
        #endregion

        #region Exercise 6D
        [TestMethod("a. FileLogger implements IDisposable"), TestCategory("Exercise 6D")]
        public void FileLoggerImplementIDisposable()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<FileLogger>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeIsSubclassOfVerifier(typeof(IDisposable)));
            test.Execute();
        }
        #endregion

        #region Exercise 6E
        private void FileLoggerAppendsFileSetup()
        {
            string path = "./log.txt";
            
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, "Customer Ryan Johnson was created" + Environment.NewLine);
        }

        [TemplatedTestMethod("a. FileLogger.Log(string message) appends file"), TestCategory("Exercise 6E")]
        public void FileLoggerAppendsFile()
        {
            FileLoggerAppendsFileSetup();
            
            FileLogger logger = new FileLogger("./log.txt");
            
            logger.Log("Customer Ryan Johnson was deleted");
            logger.Dispose();

            string expectedContent = string.Join(
                Environment.NewLine, 
                "Customer Ryan Johnson was created", 
                "Customer Ryan Johnson was deleted");
            Assert.AreEqual(expectedContent, File.ReadAllText("./log.txt"));
        }
        #endregion

        #region Exercise 6F
        [TemplatedTestMethod("a. ConsoleLogger.Log(string message) writes to console"), TestCategory("Exercise 6F")]
        public void ConsoleLoggerWritesToConsole()
        {
            ConsoleLogger logger = new ConsoleLogger();

            string expectedWriteout = "Customer Ryan Johnson was deleted";
            ConsoleAssert.WritesOut(() => logger.Log("Customer Ryan Johnson was deleted"), expectedWriteout);
        }
        #endregion
    }
}
