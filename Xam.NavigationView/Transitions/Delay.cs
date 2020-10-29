using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public sealed class Delay : ITransition
    {
        public int Duration { get; set; } = 0;

        public async Task Run(VisualElement element)
        {
            if (Duration > 0)
            {
                await Task.Delay(Duration);
            }
        }
    }
}
