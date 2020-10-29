using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public interface ITransition
    {
        Task Run(VisualElement element);
    }
}
