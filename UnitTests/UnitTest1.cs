using Microsoft.VisualStudio.TestTools.UnitTesting;
using sappy;
using System;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Spotify s = new Spotify("1", "2", "http://localhost/callback");
            Uri test = s.Authorise(Spotify.Scopes.app_remote_control | Spotify.Scopes.user_read_email);
            Assert.IsNotNull(s.state);
            Debug.WriteLine(s.state);
        }
    }
}
