using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Shared
{
    public class BlazorTimer : ComponentBase, IDisposable
    {
        private Timer? _timer;

        private DateTime startTime;

        private DateTime stopTime;

        private double elapsedtime;

        private bool running = false;

        [Parameter]
        public int Interval
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback Ticked
        {
            get;
            set;
        }

        public BlazorTimer()
        {
        }

        public void Dispose()
        {
            Timer timer = this._timer;
            if (timer != null)
            {
                timer.Dispose();
            }
            else
            {
            }
        }

        public double ElapsedTimeSecs()
        {
            TimeSpan timeSpan;
            timeSpan = (!this.running ? this.stopTime - this.startTime : DateTime.Now - this.startTime);
            return timeSpan.TotalSeconds;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.Start();
        }

        public void Start()
        {
            if (this._timer != null)
            {
                Timer timer = this._timer;
                if (timer != null)
                {
                    timer.Dispose();
                }
                else
                {
                }
                this._timer = null;
            }
            this._timer = new Timer(new TimerCallback(this.timerTicked), null, 0, this.Interval);
            this.startTime = DateTime.Now;
            this.running = true;
        }

        public void Stop()
        {
            this.stopTime = DateTime.Now;
            Timer timer = this._timer;
            if (timer != null)
            {
                timer.Dispose();
            }
            else
            {
            }
            this._timer = null;
        }

        private void Test()
        {
            this.Ticked.InvokeAsync();
        }

        private async void timerTicked(object state)
        {
            this.elapsedtime = this.ElapsedTimeSecs();
            await this.Ticked.InvokeAsync();
            await base.InvokeAsync(new Action(this.StateHasChanged));
        }
    }
}