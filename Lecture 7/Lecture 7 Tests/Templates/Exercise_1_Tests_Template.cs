using Lecture_7_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_7_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_7_Tests
{
    [TemplatedTestClass]
    public class Exercise_1_Tests_Template
    {
        #region Exercise 1A
        [TestMethod("a. Pair<T1, T2>.Fst is public read-only T1 property"), TestCategory("Exercise 1A")]
        public void FstIsPublicReadOnlyT1Property()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyField<Pair<string, int>, string>(p => p.Fst); 
            test.AssertPublicReadonlyField<Pair<double, int>, double>(p => p.Fst);
            test.Execute();
        }

        [TestMethod("b. Pair<T1, T2>.Snd is public read-only T2 property"), TestCategory("Exercise 1A")]
        public void SndIsPublicReadOnlyT2Property()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest(); 
            test.AssertPublicReadonlyField<Pair<string, int>, int>(p => p.Snd);
            test.AssertPublicReadonlyField<Pair<string, double>, double>(p => p.Snd);
            test.Execute();
        }
        #endregion

        #region Exercise 1B
        [TestMethod("a. Pair<T1, T2> has constructor that takes T1 argument and T2 argument"), TestCategory("Exercise 1B")]
        public void PairHasCorrectConstructor()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<string, int, Pair<string, int>>((v1, v2) => new Pair<string, int>(v1, v2));
            test.AssertPublicConstructor<string, double, Pair<string, double>>((v1, v2) => new Pair<string, double>(v1, v2));
            test.Execute();
        }

        [TemplatedTestMethod("b. Pair<T1, T2> constructor sets Fst"), TestCategory("Exercise 1B")]
        public void PairConstructorSetsFst()
        {
            Pair<string, int> pair = new Pair<string, int>("abc", 5);
            Assert.AreEqual("abc", pair.Fst);
        }

        [TemplatedTestMethod("c. Pair<T1, T2> constructor sets Snd"), TestCategory("Exercise 1B")]
        public void PairConstructorSetsSnd()
        {
            Pair<string, int> pair = new Pair<string, int>("abc", 5);
            Assert.AreEqual(5, pair.Snd);
        }
        #endregion

        #region Exercise 1C
        [TestMethod("a. Pair<T1, T2>.Swap() is public method"), TestCategory("Exercise 1C")]
        public void PairSwapIsPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Pair<string, int>, Pair<int, string>>(p => p.Swap());
            test.AssertPublicMethod<Pair<string, double>, Pair<double, string>>(p => p.Swap());
            test.Execute();
        }
        
        [TemplatedTestMethod("b. Pair<T1, T2>.Swap() switches Fst and Snd values around"), TestCategory("Exercise 1C")]
        public void PairSwapSwitchesFstAndSnd()
        {
            Pair<string, int> pair1 = new Pair<string, int>("abc", 5);

            Pair<int, string> pair2 = pair1.Swap();

            Assert.AreEqual(5, pair2.Fst);
            Assert.AreEqual("abc", pair2.Snd);
        }
        #endregion

        #region Exercise 1D
        [TestMethod("a. Pair<T1, T2>.SetFst<C>(C value) is public method"), TestCategory("Exercise 1D")]
        public void PairSetFstIsPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Pair<string, int>, double, Pair<double, int>>((p, d) => p.SetFst(d));
            test.AssertPublicMethod<Pair<string, int>, long, Pair<long, int>>((p, d) => p.SetFst(d));
            test.Execute();
        }

        [TestMethod("b. Pair<T1, T2>.SetSnd<C>(C value) is public method"), TestCategory("Exercise 1D")]
        public void PairSetSndIsPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Pair<string, int>, double, Pair<string, double>>((p, d) => p.SetSnd(d));
            test.AssertPublicMethod<Pair<string, int>, long, Pair<string, long>>((p, d) => p.SetSnd(d));
            test.Execute();
        }

        [TemplatedTestMethod("c. Pair<T1, T2>.SetFst<C>(C value) returns new Pair with Fst = value"), TestCategory("Exercise 1D")]
        public void PairSetFstReturnsNewPair()
        {
            Pair<string, int> pair1 = new Pair<string, int>("abc", 5);

            Pair<double, int> pair2 = pair1.SetFst(7.0);

            Assert.AreEqual(7.0, pair2.Fst);
            Assert.AreEqual(5, pair2.Snd);
        }

        [TemplatedTestMethod("c. Pair<T1, T2>.SetSnd<C>(C value) returns new Pair with Snd = value"), TestCategory("Exercise 1D")]
        public void PairSetSndReturnsNewPair()
        {
            Pair<string, int> pair1 = new Pair<string, int>("abc", 5);

            Pair<string, double> pair2 = pair1.SetSnd(7.0);

            Assert.AreEqual("abc", pair2.Fst);
            Assert.AreEqual(7.0, pair2.Snd);
        }
        #endregion
    }
}
