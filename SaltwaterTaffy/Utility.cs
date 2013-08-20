using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaltwaterTaffy.Utility
{
    
    public static class StringExtensions
    {
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }
    }

    /// <summary>
    /// Interface definition of Either monad
    /// </summary>
    /// <typeparam name="Tl">type of the Left value</typeparam>
    /// <typeparam name="Tr">type of the Right value</typeparam>
    public interface IEither<out Tl, out Tr>
    {
        /// <summary>
        /// Check the type of the value held and invoke the matching handler function
        /// </summary>
        /// <typeparam name="U">the return type of the handler functions</typeparam>
        /// <param name="ofLeft">handler for the Left type</param>
        /// <param name="ofRight">handler for the Right type</param>
        /// <returns>the value returned by the invoked handler function</returns>
        U Case<U>(Func<Tl, U> ofLeft, Func<Tr, U> ofRight);

        /// <summary>
        /// Check the type of the value held and invoke the matching handler function
        /// </summary>
        /// <param name="ofLeft">handler for the Left type</param>
        /// <param name="ofRight">handler for the Right type</param>
        void Case(Action<Tl> ofLeft, Action<Tr> ofRight);
    }

    /// <summary>
    /// Static helper class for Either monad
    /// </summary>
    public static class Either
    {
        private sealed class LeftImpl<Tl, Tr> : IEither<Tl, Tr>
        {
            private readonly Tl value;

            public LeftImpl(Tl value)
            {
                this.value = value;
            }

            public U Case<U>(Func<Tl, U> ofLeft, Func<Tr, U> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException("ofLeft");

                return ofLeft(value);
            }

            public void Case(Action<Tl> ofLeft, Action<Tr> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException("ofLeft");

                ofLeft(value);
            }
        }

        private sealed class RightImpl<Tl, Tr> : IEither<Tl, Tr>
        {
            private readonly Tr value;

            public RightImpl(Tr value)
            {
                this.value = value;
            }

            public U Case<U>(Func<Tl, U> ofLeft, Func<Tr, U> ofRight)
            {
                if (ofRight == null)
                    throw new ArgumentNullException("ofRight");

                return ofRight(value);
            }

            public void Case(Action<Tl> ofLeft, Action<Tr> ofRight)
            {
                if (ofRight == null)
                    throw new ArgumentNullException("ofRight");

                ofRight(value);
            }
        }

        /// <summary>
        /// Create an Either with Left value
        /// </summary>
        /// <typeparam name="Tl">type of the Left value</typeparam>
        /// <typeparam name="Tr">type of the Right value</typeparam>
        /// <param name="value">the value to hold</param>
        /// <returns>an Either with the specified Left value</returns>
        public static IEither<Tl, Tr> Left<Tl, Tr>(Tl value)
        {
            return new LeftImpl<Tl, Tr>(value);
        }

        /// <summary>
        /// Create an Either with Right value
        /// </summary>
        /// <typeparam name="Tl">type of the Left value</typeparam>
        /// <typeparam name="Tr">type of the Right value</typeparam>
        /// <param name="value">the value to hold</param>
        /// <returns>an Either with the specified Right value</returns>
        public static IEither<Tl, Tr> Right<Tl, Tr>(Tr value)
        {
            return new RightImpl<Tl, Tr>(value);
        }

        /// <summary>
        /// Create an Either with the specified value
        /// </summary>
        /// <typeparam name="Tl">type of the Left value</typeparam>
        /// <typeparam name="Tr">type of the Right value</typeparam>
        /// <param name="value">the value to hold</param>
        /// <returns>an Either with the specified value</returns>
        public static IEither<Tl, Tr> Create<Tl, Tr>(Tl value)
        {
            return new LeftImpl<Tl, Tr>(value);
        }

        /// <summary>
        /// Create an Either with the specified value
        /// </summary>
        /// <typeparam name="Tl">type of the Left value</typeparam>
        /// <typeparam name="Tr">type of the Right value</typeparam>
        /// <param name="value">the value to hold</param>
        /// <returns>an Either with the specified value</returns>
        public static IEither<Tl, Tr> Create<Tl, Tr>(Tr value)
        {
            return new RightImpl<Tl, Tr>(value);
        }
    }
}
