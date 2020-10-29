using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public sealed class TransitionCollection : Collection<ITransition>, ITransition
    {
        public bool IsSequential { get; set; } = false;

        public async Task Run(VisualElement element)
        {
            if (IsSequential)
            {
                foreach (var t in this)
                {
                    await t.Run(element);
                }
            }
            else
            {
                await Task.WhenAll(this.Select(t => t.Run(element)));
            }
        }
    }
}
