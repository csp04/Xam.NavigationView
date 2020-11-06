using System.Threading.Tasks;

namespace X.NavView.Transitions
{
    public interface ITransition
    {
        Task Run();
        void Cancel();
    }
}
