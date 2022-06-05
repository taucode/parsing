using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TauCode.Extensions;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;

namespace TauCode.Parsing.Tests.Graphs
{
    [TestFixture]
    public class ReadingTests
    {
        [Test]
        public void Read_ValidInput_ReadsMold()
        {
            // Arrange
            var script = this.GetType().Assembly.GetResourceText("cli-grammar-native.lisp", true);
            var reader = new GraphScriptReader();

            // Act
            var groupMold = reader.ReadScript(script.AsMemory());

            // Assert
            Assert.Pass(); // todo: check mold?
        }
    }
}
