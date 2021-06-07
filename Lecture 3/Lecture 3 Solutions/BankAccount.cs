using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure.Attributes;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_3_Solutions
{
    public class BankAccount
    {
        private decimal _balance;
        private decimal _borrowingRate;
        private decimal _savingsRate;

        public BankAccount()
        {
            BorrowingRate = 0.06M;
            SavingsRate = 0.02M;
        }

        [ReadonlyProperty]
        public decimal Balance {
            get { return _balance; }
            private set
            {
                if (-100000 <= value && value <= 250000)
                    _balance = value;
            }
        }

        public decimal BorrowingRate
        {
            get { return _borrowingRate; }
            set
            {
                if (value >= 0.06M)
                    _borrowingRate = value;
            }
        }

        public decimal SavingsRate
        {
            get { return _savingsRate; }
            set
            {
                if (value <= 0.02M)
                    _savingsRate = value;
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount >= 0)
                Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount >= 0)
                Balance -= amount;
        }

        public void AccrueOrChargeInterest()
        {
            if (Balance > 0)
                Balance += Balance * SavingsRate;
            if (Balance < 0)
                Balance -= Balance * BorrowingRate;
        }
    }
}
