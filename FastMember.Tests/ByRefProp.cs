using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FastMember.Tests
{
    public class ByRefProp
    {
        #region "Nested Types"

        public class Foo
        {
            #region "Members"

            private int m_val;

            #endregion

            #region "Properties"

            public ref int Ref => ref this.m_val;

            public ref readonly int RefReadOnly => ref this.m_val;

            public int Val
            {
                get => this.m_val;
                set => this.m_val = value;
            }

            #endregion
        }

        #endregion

        #region "Methods"

        [TestMethod]
        public void CanGetByRef()
        {
            var foo = new Foo { Val = 42 };

            var acc = ObjectAccessor.Create(foo);
            Assert.AreEqual(42, (int)acc["Val"]);
            Assert.AreEqual(42, (int)acc["Ref"]);
            Assert.AreEqual(42, (int)acc["RefReadOnly"]);
        }

        [TestMethod]
        public void CanSetByRef()
        {
            var foo = new Foo { Val = 42 };
            var acc = ObjectAccessor.Create(foo);
            acc["Val"] = 43;
            Assert.AreEqual(43, foo.Val);

            acc["Ref"] = 44;
            Assert.AreEqual(44, foo.Val);

            var ex = AssertEx.Throws<ArgumentOutOfRangeException>(() =>
                                                                  {
                                                                      acc["RefReadOnly"] = 45;
                                                                  });
            Assert.AreEqual("name", ex.ParamName);
            Assert.AreEqual(44, foo.Val);
        }

        #endregion
    }
}