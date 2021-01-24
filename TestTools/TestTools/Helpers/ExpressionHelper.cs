﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TestTools.Helpers
{
    public class ExpressionHelper
    {
        public static Expression<Func<T, T>> Identity<T>()
        {
            return value => value;
        }

        public static Expression<Action<T1>> Assignment<T1, TValue>(Expression<Func<T1, TValue>> leftSide, TValue rightSide)
        {
            throw new NotImplementedException();
        }

        public static Expression<Action<T1, T2>> Assignment<T1, T2, TValue>(Expression<Func<T1, TValue>> leftSide, Expression<Func<T2, TValue>> rightSide) {
            throw new NotImplementedException();
        }

        public static Expression<Action<T1, T2, T3>> Assignment<T1, T2, T3, TValue>(Expression<Func<T1, TValue>> leftSide, Expression<Func<T2, T3, TValue>> rightSide)
        {
            throw new NotImplementedException();
        }

        public static Expression<Func<T1, bool>> Equality<T1, TValue>(Expression<Func<T1, TValue>> leftSide, TValue rightSide)
        {
            throw new NotImplementedException();
        }

        public static Expression<Func<T1, bool>> And<T1>(Expression<Func<T1, bool>> leftSide, Expression<Func<T1, bool>> rightSide)
        {
            throw new NotImplementedException();
        }

        public static Expression<Func<T1, T2, bool>> And<T1, T2>(Expression<Func<T1, bool>> leftSide, Expression<Func<T2, bool>> rightSide)
        {
            throw new NotImplementedException();
        }

        public static Expression<Action<T, TDelegate>> Subscribe<T, TDelegate>(string eventName)
        {
            throw new NotImplementedException();
        }
    }
}
