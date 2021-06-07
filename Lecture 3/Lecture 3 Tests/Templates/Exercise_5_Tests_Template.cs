using Lecture_3_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_3_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_3_Tests
{
    [TemplatedTestClass]
    public class Exercise_5_Tests_Template
    {
        #region Exercise 5A
        [TestMethod("a. BankAccount.Balance is public read-only decimal property"), TestCategory("Exercise 5A")]
        public void BankAccountBalanceIsDecimalProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<BankAccount, decimal>(b => b.Balance);
            test.Execute();
        }

        [TestMethod("b. BankAccount.BorrowingRate is public decimal property"), TestCategory("Exercise 5A")]
        public void BankAccountBorrowingRateIsPublicDecimalProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<BankAccount, decimal>(b => b.BorrowingRate);
            test.Execute();
        }

        [TestMethod("c. BankAccount.SavingsRate is public decimal property"), TestCategory("Exercise 5A")]
        public void BankAccountSavingsRateIsPublicDecimalProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<BankAccount, decimal>(b => b.SavingsRate);
            test.Execute();
        }

        [TemplatedTestMethod("d. BankAccount.Balance is initalized as 0M"), TestCategory("Exercise 5A")]
        public void BalanceIsInitilizedAs0()
        {
            BankAccount account = new BankAccount();
            Assert.AreEqual(0M, account.Balance);
        }

        [TemplatedTestMethod("e. BankAccount.BorrowingRate is initalized as 0.06M"), TestCategory("Exercise 5A")]
        public void BorrowingRateIsInitilizedAs0Point6()
        {
            BankAccount account = new BankAccount();
            Assert.AreEqual(0.06M, account.BorrowingRate);
        }

        [TemplatedTestMethod("f. BankAccount.SavingsRate is initalized as 0.02M"), TestCategory("Exercise 5A")]
        public void SavingsRateIsInitilizedAs0Point2()
        {
            BankAccount account = new BankAccount();
            Assert.AreEqual(0.02M, account.SavingsRate);
        }

        [TemplatedTestMethod("g. BankAccount.BorrowingRate ignores assignment of 0.05M"), TestCategory("Exercise 5A")]
        public void BankAccountBorrowingRateIgnoresAssignmentOfFivePercent() 
        {
            BankAccount account = new BankAccount();
            
            account.BorrowingRate = 0.05M;

            Assert.AreEqual(0.06M, account.BorrowingRate);
        }

        [TemplatedTestMethod("h. BankAccount.SavingsRate ignores assignment of 0.03M"), TestCategory("Exercise 5A")]
        public void BankAccountSavingsRateIgnoresAssignmentOfThreePercent()
        {
            BankAccount account = new BankAccount();

            account.SavingsRate = 0.03M;

            Assert.AreEqual(0.02M, account.SavingsRate);
        }
        #endregion

        #region Exercise 5B
        [TemplatedTestMethod("a. BankAccount.Deposit(int amount) adds amount to Balance"), TestCategory("Exercise 5B")]
        public void BankAccountDepositAddsAmountToBalance()
        {
            BankAccount account = new BankAccount();

            account.Deposit(50M);

            Assert.AreEqual(50M, account.Balance);
        }

        [TemplatedTestMethod("b. BankAccount.Deposit(int amount) does not change balance on negative amount"), TestCategory("Exercise 5B")]
        public void BankAccountDepositDoesNotChangeBalanceOnNegativeAmount()
        {
            BankAccount account = new BankAccount();

            account.Deposit(-1M);

            Assert.AreEqual(0, account.Balance);
        }

        [TemplatedTestMethod("c. BankAccount.Withdraw(int amount) subtracts amount of Balance"), TestCategory("Exercise 5B")]
        public void BankAccountWithdrawSubtractsAmountOfBalance()
        {
            BankAccount account = new BankAccount();

            account.Withdraw(50M);

            Assert.AreEqual(-50M, account.Balance);
        }

        [TemplatedTestMethod("d. BankAccount.Withdraw(int amount) does not change Balance on negative amount"), TestCategory("Exercise 5B")]
        public void BankAccountWithdrawDoesNotChangeBalanceOnNegativeAmount()
        {
            BankAccount account = new BankAccount();

            account.Deposit(-1M);

            Assert.AreEqual(0, account.Balance);
        }
        #endregion

        #region Exercise 5C
        [TestMethod("a. BankAccount.AccrueOrChargeInterest() is a public method"), TestCategory("Exercise 5C")]
        public void BankAccountAccrueOrChargeInterestIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<BankAccount>(b => b.AccrueOrChargeInterest());
            test.Execute();
        }

        [TemplatedTestMethod("b. BankAccount.AccrueOrChargeInterest() charges BorrowingRate if Balance < 0"), TestCategory("Exercise 5C")]
        public void BankAccountAccrueOrChargeInterestChargesBorrowingRateIfBalanceIsNegative()
        {
            BankAccount account = new BankAccount() { BorrowingRate = 0.1M };
            account.Withdraw(50);

            account.AccrueOrChargeInterest();

            Assert.AreEqual(-45.0M, account.Balance);
        }

        [TemplatedTestMethod("c. BankAccount.AccrueOrChargeInterest() does not affect Balance if Balance = 0"), TestCategory("Exercise 5C")]
        public void BankAccountAccrueOrChargeInterestDoesNotAffectBalanceIfBalanceEqualsZero()
        {
            BankAccount account = new BankAccount();

            account.AccrueOrChargeInterest();

            Assert.AreEqual(0M, account.Balance);
        }

        [TemplatedTestMethod("d. BankAccount.AccrueOrChargeInterest() accrue SavingsRate if Balance > 0"), TestCategory("Exercise 5C")]
        public void BankAccountAccrueOrChargeInterestAccrueSavingsRateIfBalanceIsPositive()
        {
            BankAccount account = new BankAccount() { SavingsRate = 0.01M };
            account.Deposit(50);

            account.AccrueOrChargeInterest();

            Assert.AreEqual(50.5M, account.Balance);
        }
        #endregion
    }
}
