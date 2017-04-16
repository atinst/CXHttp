
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CXHttpNS;

namespace CXHttpUnitTests
{
    [TestClass]
    public class CXHttpUnitTest
    {
        [TestMethod]
        public void TestGet()
        {
            var getTask = CXHttp.Connect("http://www.baidu.com").Get();
            getTask.RunSynchronously();

            var contentTask = getTask.Result.Content();
            contentTask.RunSynchronously();

            Assert.IsFalse(contentTask.Result == "");
        }
    }
}
