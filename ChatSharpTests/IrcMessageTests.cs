using System;
using ChatSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatSharp.Tests
{
    [TestClass]
    public class IrcMessageTests
    {
        [TestMethod]
        public void NewValidMessage()
        {
            try
            {
                IrcMessage fromMessage = new IrcMessage(@":user!~ident@host PRIVMSG target :Lorem ipsum dolor sit amet");
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, got: {0}", e.Message);
            }
        }

        [TestMethod]
        public void NewValidMessage_Command()
        {
            IrcMessage fromMessage = new IrcMessage(@":user!~ident@host PRIVMSG target :Lorem ipsum dolor sit amet");
            Assert.AreEqual(fromMessage.Command, "PRIVMSG");
        }

        [TestMethod]
        public void NewValidMessage_Prefix()
        {
            IrcMessage fromMessage = new IrcMessage(@":user!~ident@host PRIVMSG target :Lorem ipsum dolor sit amet");
            Assert.AreEqual(fromMessage.Prefix, "user!~ident@host");
        }

        [TestMethod]
        public void NewValidMessage_Params()
        {
            IrcMessage fromMessage = new IrcMessage(@":user!~ident@host PRIVMSG target :Lorem ipsum dolor sit amet");
            string[] compareParams = new string[] { "target", "Lorem ipsum dolor sit amet" };
            CollectionAssert.AreEqual(fromMessage.Parameters, compareParams);
        }
    }
}
