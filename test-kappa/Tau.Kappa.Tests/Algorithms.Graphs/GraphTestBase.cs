using NUnit.Framework;
using TauCode.Data.Graphs;

namespace Tau.Kappa.Tests.Algorithms.Graphs;

[TestFixture]
public abstract class GraphTestBase
{
    protected IGraph Graph { get; set; }

    [SetUp]
    public void SetUpBase()
    {
        this.Graph = new Graph();
    }

    [TearDown]
    public void TearDownBase()
    {
        this.Graph = null;
    }
}
