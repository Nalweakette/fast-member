using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FastMember.Tests
{
    public static class AssertEx
    {
        #region "Static Methods"

        public static T Throws<T>(Action mustThrowAction)
            where T : Exception
        {
            try
            {
                mustThrowAction();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(T));
                return (T)e;
            }

            return null;
        }

        #endregion
    }
}