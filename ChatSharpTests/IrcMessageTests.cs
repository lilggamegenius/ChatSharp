using System;
using ChatSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        [TestMethod]
        public void NewValidMessage_Tags()
        {
            IrcMessage fromMessage = new IrcMessage("@a=123;b=456;c=789 :user!~ident@host PRIVMSG target :Lorem ipsum dolor sit amet");
            KeyValuePair<string, string>[] compareTags = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("a", "123"),
                new KeyValuePair<string, string>("b", "456"),
                new KeyValuePair<string, string>("c", "789")
            };
            CollectionAssert.AreEqual(fromMessage.Tags, compareTags);
        }

        [TestMethod]
        public void NewValidMessage_Tags02()
        {
            IrcMessage fromMessage = new IrcMessage("@aaa=bbb;ccc;example.com/ddd=eee :nick!ident@host.com PRIVMSG me :Hello");
            KeyValuePair<string, string>[] compareTags = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("aaa", "bbb"),
                new KeyValuePair<string, string>("ccc", ""),
                new KeyValuePair<string, string>("example.com/ddd", "eee"),
            };
            CollectionAssert.AreEqual(fromMessage.Tags, compareTags);
        }

        [TestMethod]
        public void NewValidMessage_TagsWithSemicolon()
        {
            IrcMessage fromMessage = new IrcMessage(@"@a=123\:456;b=456\:789;c=789\:123 :user!~ident@host PRIVMSG target :Lorem ipsum dolor sit amet");
            KeyValuePair<string, string>[] compareTags = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("a", "123;456"),
                new KeyValuePair<string, string>("b", "456;789"),
                new KeyValuePair<string, string>("c", "789;123"),
            };
            CollectionAssert.AreEqual(fromMessage.Tags, compareTags);
        }
    }
}
