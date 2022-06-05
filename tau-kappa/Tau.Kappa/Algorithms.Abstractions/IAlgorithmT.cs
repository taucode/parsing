namespace Tau.Kappa.Algorithms.Abstractions
{
    public interface IAlgorithm<TInput, out TOutput> : IAlgorithm
    {
        TInput Input { get; set; }
        TOutput Output { get; }
    }
}