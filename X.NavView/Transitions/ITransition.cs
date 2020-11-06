using System.Threading.Tasks;
using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public interface ITransition
    {
        Task Run();
        void Cancel();
    }
}
