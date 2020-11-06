using System.Threading.Tasks;
using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public class HSlide : Transition
    {
        readonly Page page = Application.Current.MainPage;

        public HSlide() : base("_HSlide", VisualElement.TranslationXProperty)
        {

        }

        protected override double GetPropertyValue(double propertyValue) => propertyValue / page.Width;

        protected override Task RunAnimation(string animationName, string propertyName, double from, double to, uint duration, Easing easing)
        {
            var value = page.Width;

            return View.AnimatePercent(animationName, propertyName, from, to, value, duration, easing);
        }
    }

}
