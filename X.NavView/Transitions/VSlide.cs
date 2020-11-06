using System.Threading.Tasks;
using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public class VSlide : Transition
    {
        private readonly Page page = Application.Current.MainPage;

        public VSlide() : base("_VSlide", VisualElement.TranslationYProperty)
        {
        }

        protected override double GetPropertyValue(double propertyValue) => propertyValue / page.Height;

        protected override Task RunAnimation(string animationName, string propertyName, double from, double to, uint duration, Easing easing)
        {
            var value = page.Height;

            return View.AnimatePercent(animationName, propertyName, from, to, value, duration, easing);
        }
    }
}