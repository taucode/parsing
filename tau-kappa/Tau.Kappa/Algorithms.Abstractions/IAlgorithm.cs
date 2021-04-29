using System.Threading;
using System.Threading.Tasks;

namespace Tau.Kappa.Algorithms.Abstractions
{
    public interface IAlgorithm
    {
        void Run();
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
