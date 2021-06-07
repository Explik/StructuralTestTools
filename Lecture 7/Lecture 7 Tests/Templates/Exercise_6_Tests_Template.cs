using Lecture_7_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_7_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_7_Tests
{
    [TemplatedTestClass]
    public class Exercise_6_Tests_Template
    {
        #region Exercise 6A
        [TestMethod("a. ILogger is a public interface"), TestCategory("Exercise 6A")]
        public void ILoggerIsAPublicInterface()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicInterface<ILogger>();
            test.Execute();
        }

        [TestMethod("b. ILogger.Log(string message) is a method"), TestCategory("Exercise 6A")]
        public void ILoggerIsAMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertMethod<ILogger, string>((l, s) => l.Log(s));
            test.Execute();
        }
        #endregion

        #region Exercise 6B
        [TestMethod("a. Logger is a public class"), TestCategory("Exercise 6A")]
        public void LoggerIsAPublicClass()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicClass<Logger>();
            test.Execute();
        }

        [TestMethod("b. Logger implements ILogger"), TestCategory("Exercise 6B")]
        public void LoggerImplementsILogger()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Logger>(new TypeIsSubclassOfVerifier(typeof(ILogger)));
            test.Execute();
        }

        [TestMethod("c. Logger.Log(string message) is a public method"), TestCategory("Exercise 6B")]
        public void LoggerLogIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Logger, string>((l, s) => l.Log(s));
            test.Execute();
        }

        [TestMethod("d. Logger.Logs is a public List<string> property"), TestCategory("Exercise 6B")]
        public void LoggerLogsIsAPublicListProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Logger, List<string>>(l => l.Logs);
            test.Execute();
        }

        [TemplatedTestMethod("e. Logger.Log(string message) adds message to Logs"), TestCategory("Exercise 6B")]
        public void LoggerLogAddsMessageToLogs()
        {
            Logger logger = new Logger();
            
            logger.Log("message1");
            logger.Log("message2");

            List<string> expected = new List<string>() { "message1", "message2" };
            CollectionAssert.AreEqual(expected, logger.Logs);
        }
        #endregion

        #region Exercise 6C
        [TestMethod("a. Repository<T> has a constructor that takes ILogger"), TestCategory("Exercise 6C")]
        public void RepositoryHasPublicDefaultConstructorThatTakesILogger()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<ILogger, Repository<ICloneable>>(l => new Repository<ICloneable>(l));
            test.Execute();
        }
        #endregion

        #region Exercise 6D
        [TemplatedTestMethod("a. Repository<T>.Add(T entity) logs expected message"), TestCategory("Exercise 6D")]
        public void RepositoryAddLogsExpectedMessage()
        {
            Logger logger = new Logger();
            Dog dog = new Dog() { ID = 103, Name = "Oscar" };
            Repository<Dog> repository = new Repository<Dog>(logger);

            repository.Add(dog);

            List<string> expected = new List<string>() { "Dog Oscar (103) was added" };
            CollectionAssert.AreEqual(expected, logger.Logs);
        }

        [TemplatedTestMethod("b. Repository<T>.Update(T entity) logs expected message"), TestCategory("Exercise 6D")]
        public void RepositoryUpdateLogsExpectedMessage()
        {
            Logger logger = new Logger();
            Dog dog = new Dog() { ID = 43, Name = "Max" };
            Repository<Dog> repository = new Repository<Dog>(logger);

            repository.Add(dog);
            repository.Update(dog);

            List<string> expected = new List<string>() { "Dog Max (43) was added", "Dog Max (43) was updated" };
            CollectionAssert.AreEqual(expected, logger.Logs);
        }

        [TemplatedTestMethod("b. Repository<T>.Delete(T entity) logs expected message"), TestCategory("Exercise 6D")]
        public void RepositoryDeleteLogsExpectedMessage()
        {
            Logger logger = new Logger();
            Dog dog = new Dog() { ID = 3, Name = "Bella" };
            Repository<Dog> repository = new Repository<Dog>(logger);

            repository.Add(dog);
            repository.Delete(dog);

            List<string> expected = new List<string>() { "Dog Bella (3) was added", "Dog Bella (3) was deleted" };
            CollectionAssert.AreEqual(expected, logger.Logs);
        }
        #endregion
    }
}
