using NUnit.Framework;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Building.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;

namespace TauCode.Parsing.Tests.Graphs;

[TestFixture]
public class GraphBuilderTests
{
    [Test]
    [TestCase(".graph-001.v1.lisp")]
    [TestCase(".graph-001.v2.lisp")]
    [TestCase(".graph-001.v3.lisp")]
    [TestCase(".graph-001.v4.lisp")]
    [TestCase(".graph-001.v5.lisp")]
    [TestCase(".graph-001.v6.lisp")]
    [TestCase(".graph-001.v7.lisp")]
    [TestCase(".graph-001.v8.lisp")]
    [TestCase(".graph-001.v9.lisp")]
    public void GraphBuilder_001_ProducesExpectedResult(string resourceName)
    {
        // Arrange
        var script = this.GetType().Assembly.GetResourceText(resourceName, true);

        IGraphScriptReader reader = new GraphScriptReader();
        var group = reader.ReadScript(script.AsMemory());

        IGraphBuilder graphBuilder = new GraphBuilder();

        // Act
        var graph = graphBuilder.Build(group);

        // Assert
        var text = graph.PrintGraph();
        var expectedGraph = this.GetType().Assembly.GetResourceText(".graph-001.txt", true);

        Assert.That(text, Is.EqualTo(expectedGraph));
    }

    [Test]
    public void GraphBuilder_002_ThrowsException()
    {
        // Arrange
        var script = this.GetType().Assembly.GetResourceText("graph-002.lisp", true);

        IGraphScriptReader reader = new GraphScriptReader();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => reader.ReadScript(script.AsMemory()));

        // Assert
        Assert.That(ex, Has.Message.StartWith("'element' is expected to be of type 'TauCode.Parsing.TinyLisp.Data.PseudoList'."));
        Assert.That(ex.ParamName, Is.EqualTo("element"));
    }
}