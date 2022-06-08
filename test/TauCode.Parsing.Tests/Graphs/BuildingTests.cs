using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TauCode.Extensions;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Building.Impl;
using TauCode.Parsing.Graphs.Reading.Impl;

namespace TauCode.Parsing.Tests.Graphs;

[TestFixture]
public class BuildingTests
{
    [Test]
    public void Build_ValidInput_BuildsGraph()
    {
        // Arrange
        var script = this.GetType().Assembly.GetResourceText("cli-grammar-native.lisp", true);
        var reader = new GraphScriptReader();
        var groupMold = reader.ReadScript(script.AsMemory());
        var builder = new GraphBuilder
        {
            CustomVertexBuilders = new List<IVertexBuilder>()
            {
                new CliVertexBuilder(),
            },
        };

        // Act
        var graph = builder.Build(groupMold);
        var rep = graph.PrintGraph();

        // Assert
        var expectedRep = this.GetType().Assembly.GetResourceText(".expected-sd-graph.txt", true);
        Assert.That(rep, Is.EqualTo(expectedRep));
    }
}
