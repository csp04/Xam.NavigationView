using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public class Transitions : List<ITransition>, ITransition
    {
        public bool Sequential { get; set; } = false;

        public void Cancel() => this.ForEach(t => t.Cancel());
        public async Task Run()
        {
            if(Sequential)
            {
                foreach(var t in this)
                {
                    await t.Run();
                }
            }
            else
            {
                var tasks = new List<Task>();

                foreach(var t in this)
                {
                    tasks.Add(t.Run());
                }

                await Task.WhenAll(tasks);
            }
        }

        internal void SetView(VisualElement element)
        {
            foreach(var item in this)
            {
                if(item is Transition t)
                {
                    t.View = element;
                }
                else if(item is Transitions ts)
                {
                    ts.SetView(element);
                }
            }
        }

        internal void UnsetView()
        {
            foreach (var item in this)
            {
                if (item is Transition t)
                {
                    t.View = null;
                }
                else if (item is Transitions ts)
                {
                    ts.UnsetView();
                }
            }
        }
    }
}
