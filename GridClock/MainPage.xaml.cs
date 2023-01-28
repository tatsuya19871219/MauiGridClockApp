using GridClock.ViewModel;
using System.Diagnostics;

namespace GridClock
{
    public partial class MainPage : ContentPage
    {
        IDispatcherTimer timer;

        static double updateSpan = 0.05;

        int? _clockHeight = null;

        public MainPage(GridClockViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            //header.Text = "Initializing...";

            //
            _ = Initialize(vm);

            //header.Text = "";

        }

        private async Task Initialize(GridClockViewModel vm)
        {

            while (_clockHeight is null)
            {
                await Task.Delay(100);
            }

            int height = (int)_clockHeight;

            vm.InitializeViewModel(HGrid, MGrid, SGrid, height);

            // Create timer
            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(updateSpan);
            timer.Tick += (s, e) => ClockUpdateCallback(s, e, vm);
            timer.Start();
        }

        private void ClockUpdateCallback(object sender, EventArgs eventArgs, GridClockViewModel vm)
        {
            //Debug.Print("Callback is called");

            vm.UpdateViewModel();

        }


        // for size allocation to each visual element
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //
            Debug.Print($"(w,h) = {width}, {height}");

            double occupacy = 0.9f;
            double hw_ratio = 3.0f;

            // change clock size here
            double heightDesire = occupacy * height;
            double widthDesire = heightDesire * hw_ratio;

            if (widthDesire > width)
            {
                widthDesire = occupacy * width;
                heightDesire = widthDesire / hw_ratio;
            }

            _clockHeight = (int)heightDesire;

            Debug.Print($"Clock height is updated => {_clockHeight}");

        }
    }
}