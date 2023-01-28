using CommunityToolkit.Mvvm.ComponentModel;
using GridClock.Model;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace GridClock.ViewModel
{
    public partial class GridClockViewModel : ObservableObject
    {

        // Clock Model
        GridClockModel _clock;

        // Grids
        ClockGrid _hourGrid, _minuteGrid, _secondGrid;

        // Color setting
        static Color _dayBaseColor = Colors.DarkCyan;
        static Color _nightBaseColor = Colors.Magenta;


        [ObservableProperty]
        string headerText;

        [ObservableProperty]
        string footerText;

        [ObservableProperty]
        Color backgroundColor;

        public GridClockViewModel()
        {
            // Initialize clock model
            _clock = new GridClockModel();

            // set day/night time 
            //_clock.SetDayTime(8);
            //_clock.SetNightTime(18);

            BackgroundColor = Colors.Black;

        }

        public void InitializeViewModel(Grid hGrid, Grid mGrid, Grid sGrid, int clockHeight)
        {


            //
            _hourGrid = new ClockGrid(hGrid, clockHeight);
            _minuteGrid = new ClockGrid(mGrid, clockHeight);
            _secondGrid = new ClockGrid(sGrid, clockHeight);

            _hourGrid.SetGridSize(3, 4);
            _minuteGrid.SetGridSize(3, 2);
            _secondGrid.SetGridSize(3, 2);

            _hourGrid.Initialize();
            _minuteGrid.Initialize();
            _secondGrid.Initialize();

            //
            if (_clock.IsDayTime()) SetDayColor();
            else SetNightColor();

            // bind functions for day/night triggers
            _clock.SetDayTimeAction(ShowGridClockWithRefresh);
            _clock.SetNightTimeAction(ShowGridClockWithRefresh);

            // 
            VisualizeCurrentClock(true);

        }


        private void SetDayColor()
        {
            _hourGrid.SetBaseColor(_dayBaseColor);
            _minuteGrid.SetBaseColor(_dayBaseColor);
            _secondGrid.SetBaseColor(_dayBaseColor);
        }

        private void SetNightColor()
        {
            _hourGrid.SetBaseColor(_nightBaseColor);
            _minuteGrid.SetBaseColor(_nightBaseColor);
            _secondGrid.SetBaseColor(_nightBaseColor);
        }

        private void VisualizeCurrentClock(bool withRefresh = false)
        {
            // Background
            //if (_clock.IsDayTime()) BackgroundColor = Colors.White; 
            //else BackgroundColor = Colors.Black;

            // Header
            //ShowHeader();

            // Body
            ShowGridClock(withRefresh);

            // Footer
            //ShowFooter();

        }

        private void ShowHeader()
        {
            bool isAM = _clock.IsAM();

            if (isAM) HeaderText = "AM";
            else HeaderText = "PM";

            //DateTime dateTime = _clock.GetDateTime();
            //HeaderText = $"Current time is {dateTime}";
            //HeaderText = $"Current time is {DateTime.Now}";

            if (_clock.IsDayTime()) HeaderText += " (Daytime)";
            else HeaderText += " (Nighttime)";
        }

        private void ShowFooter()
        {
            int ms = _clock.GetMillisecond();
            FooterText = $"Milliseconds: {ms / 100} (x100)";
        }

        private void ShowGridClock(bool withRefresh = false)
        {

            if (_clock.IsMoreThanOneSecondPassed)
            {
                ShowGridClockWithRefresh();
                return;
            }

            if (!withRefresh)
            {
                // Less than one second passed from the last stamp
                if (!_clock.IsJustOneSecondPassed)
                {
                    //Debug.Print($"ms: {_clock.GetMillisecond()/100}");
                    return;
                }

                // update stamp
                _clock.Stamp();

                // second
                UpdateSecondGrids();

                // minute
                if (_clock.IsMinuteUpdate) UpdateMinuteGrids();

                // hour
                if (_clock.IsHourUpdate) UpdateHourGrids();

            }
            else ShowGridClockWithRefresh();
        }

        private void UpdateSecondGrids()
        {
            int s = _clock.GetSecond();
            int s10 = s / 10;

            if (s % 10 != 0)
            {
                int ss = (s % 10) - 1;
                _secondGrid.FillAt(s10, ss);
            }
            else
            {
                if (s10 != 0)
                {
                    int k = s10 - 1;
                    _secondGrid.FillAll(k);
                }
                else
                {
                    _secondGrid.Refresh();
                }
            }
        }

        private void UpdateMinuteGrids()
        {
            int m = _clock.GetMinute();
            int m10 = m / 10;

            if (m % 10 != 0)
            {
                int mm = (m % 10) - 1;
                _minuteGrid.FillAt(m10, mm);
            }
            else
            {

                if (m10 != 0)
                {
                    int k = m10 - 1;
                    _minuteGrid.FillAll(k);
                }
                else
                {
                    _minuteGrid.Refresh();
                }
            }
        }

        private void UpdateHourGrids()
        {

            int h = _clock.GetHour12();

            if (h != 0)
            {
                int hh = h - 1;
                _hourGrid.FillAll(hh);
            }
            else
            {
                if (_clock.IsZeroOclockIn12()) ShowGridClockWithRefresh();
            }
        }

        private void ShowGridClockWithRefresh()
        {
            if (_clock.IsDayTime()) SetDayColor();
            else SetNightColor();

            //RefreshClockGrid();
            _hourGrid.Refresh();
            _minuteGrid.Refresh();
            _secondGrid.Refresh();

            // second
            int s = _clock.GetSecond();
            int s10 = s / 10;

            for (int k = 0; k < s10; k++)
                _secondGrid.FillAll(k);

            int ss = s % 10;

            for (int k = 0; k < ss; k++)
                _secondGrid.FillAt(s10, k);

            // minute
            int m = _clock.GetMinute();
            int m10 = m / 10;

            for (int k = 0; k < m10; k++)
                _minuteGrid.FillAll(k);

            int mm = m % 10;

            for (int k = 0; k < mm; k++)
                _minuteGrid.FillAt(m10, k);

            // hour
            int h = _clock.GetHour12();

            for (int k = 0; k < h; k++)
                _hourGrid.FillAll(k);
        }


        public void UpdateViewModel()
        {
            _clock.Update();

            //
            VisualizeCurrentClock();

        }

    }
}