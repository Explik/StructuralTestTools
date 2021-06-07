using Lecture_7_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_7_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_7_Tests
{
    [TemplatedTestClass]
    public class Exercise_4_Tests_Template
    {
        #region Exercise 4A
        [TestMethod("a. Repository<T> has a default constructor"), TestCategory("Exercise 4A")]
        public void RepositoryHasPublicDefaultConstructor()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<Repository<ICloneable>>(() => new Repository<ICloneable>());
            test.Execute();
        }
        #endregion

        #region Exercise 4B
        [TestMethod("a. Repositoty<T>.Add(T entity) is a public method"), TestCategory("Exercise 4B")]
        public void RepositoryAddIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Repository<ICloneable>, ICloneable>((r, e) => r.Add(e));
            test.AssertPublicMethod<Repository<Dog>, Dog>((r, e) => r.Add(e));
            test.Execute();
        }

        [TemplatedTestMethod("b. Repository<T>.Add(T entity) takes TEntity and adds it to list of entities"), TestCategory("Exercise 4B")]
        public void RepositoryAddTakesTEntityAndAddsToEntities()
        {
            Repository<Dog> repository = new Repository<Dog>();
            
            repository.Add(new Dog());

            Assert.AreEqual(1, repository.GetAll().Count);
        }

        [TemplatedTestMethod("c. Repository<T>.Add(T entity) adds entity, which cannot be affected"), TestCategory("Exercise 4B")]
        public void RepositoryAddAddsEntityWhichCannotBeAffected()
        {
            Dog dog = new Dog() { Name = "Name" };
            Repository<Dog> repository = new Repository<Dog>();
            repository.Add(dog);

            dog.Name = "NewName";

            Assert.AreEqual("Name", repository.GetAll()[0].Name);
        }
        #endregion

        #region Exercise 4C
        [TestMethod("a. Repositoty<T>.GetAll() is a public method"), TestCategory("Exercise 4C")]
        public void RepositoryGetAllIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Repository<ICloneable>, List<ICloneable>>(r => r.GetAll());
            test.AssertPublicMethod<Repository<Dog>, List<Dog>>(r => r.GetAll());
            test.Execute();
        }

        [TemplatedTestMethod("b. Repository<T>.GetAll() takes nothing and returns list of all entities"), TestCategory("Exercise 4C")]
        public void RepositoryGetAllReturnsListOfEntities()
        {
            Repository<Dog> repository = new Repository<Dog>();

            repository.Add(new Dog() { ID = 0 });
            repository.Add(new Dog() { ID = 1 });

            Assert.AreEqual(2, repository.GetAll().Count);
        }
        #endregion

        #region Exercise 4D
        [TestMethod("a. Repositoty<T>.Update(T entity) is a public method"), TestCategory("Exercise 4D")]
        public void RepositoryUpdateIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Repository<ICloneable>, ICloneable>((r, e) => r.Update(e));
            test.AssertPublicMethod<Repository<Dog>, Dog>((r, e) => r.Update(e));
            test.Execute();
        }

        [TemplatedTestMethod("b. Repository<T>.Update(T entity) updates an entity"), TestCategory("Exercise 4D")]
        public void RepositoryUpdateUpdatesAnEntity()
        {
            Dog dog = new Dog() 
            {
                Name = "Name"
            };
            Repository<Dog> repository = new Repository<Dog>();
            repository.Add(dog);

            dog.Name = "NewName";
            repository.Update(dog);

            Assert.AreEqual("NewName", repository.GetAll()[0].Name);
        }
        #endregion

        #region Exercise 4E
        [TestMethod("a. Repositoty<T>.Delete(T entity) is a public method"), TestCategory("Exercise 4E")]
        public void RepositoryDeleteIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Repository<ICloneable>, ICloneable>((r, e) => r.Update(e));
            test.AssertPublicMethod<Repository<Dog>, Dog>((r, e) => r.Update(e));
            test.Execute();
        }

        [TemplatedTestMethod("a. Repository<T>.Delete(TEntity) deletes an entity"), TestCategory("Exercise 4E")]
        public void RepositoryDeleteDeletesAnEntity()
        {
            Repository<Dog> repository = new Repository<Dog>();
            Dog dog = new Dog();

            repository.Add(dog);
            repository.Delete(dog);

            Assert.AreEqual(0, repository.GetAll().Count);
        }
        #endregion
    }
}
