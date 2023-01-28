namespace GridClock.Model
{
    public class GridClockModel
    {

        // binding functions for day/night triggers
        Action _daytimeAction;
        Action _nighttimeAction;


        public bool IsJustOneSecondPassed { get; private set; }
        public bool IsMoreThanOneSecondPassed { get; private set; }


        public bool IsSecondUpdate { get; private set; }
        public bool IsMinuteUpdate { get; private set; }
        public bool IsHourUpdate { get; private set; }


        DateTime _datetime;

        DateTime _datetime_stamp;


        // Day and Night
        int _dayTimeFrom = 6;
        int _nightTimeFrom = 18;

        TimeSpan _timePassedFromStamp;

        public GridClockModel()
        {
            _datetime = DateTime.Now;

            Stamp();

            IsJustOneSecondPassed = false;
            IsMoreThanOneSecondPassed = false;

            Update();
        }


        public void SetDayTimeAction(Action action) => _daytimeAction += action;
        public void SetNightTimeAction(Action action) => _nighttimeAction += action;


        public void SetDayTime(int dayTimeFrom) => _dayTimeFrom = dayTimeFrom;
        public void SetNightTime(int nightTimeFrom) => _nightTimeFrom = nightTimeFrom;

        public void Update()
        {

            _datetime = DateTime.Now;

            _timePassedFromStamp = _datetime - _datetime_stamp;

            // check whether all digit is same
            IsHourUpdate = !(_datetime_stamp.Hour == _datetime.Hour);
            IsMinuteUpdate = !(_datetime_stamp.Minute == _datetime.Minute);
            IsSecondUpdate = !(_datetime_stamp.Second == _datetime.Second);


            if (_timePassedFromStamp.TotalSeconds < 1.0f)
            {
                // for adjustment
                if (_datetime_stamp.Second != _datetime.Second)
                {
                    IsJustOneSecondPassed = true;
                    IsMoreThanOneSecondPassed = false;
                }
                else
                {
                    IsJustOneSecondPassed = false;
                    IsMoreThanOneSecondPassed = false;
                }
            }
            else if (_timePassedFromStamp.TotalSeconds > 2.0f)
            {
                IsJustOneSecondPassed = false;
                IsMoreThanOneSecondPassed = true;
            }
            else
            {
                IsJustOneSecondPassed = true;
                IsMoreThanOneSecondPassed = false;
            }

           
            if (IsJustOneSecondPassed && IsJustSwitchToDayTime()) _daytimeAction();
            if (IsJustOneSecondPassed && IsJustSwitchToNightTime()) _nighttimeAction();

        }

        // timestamp managed by view model
        public void Stamp() => _datetime_stamp = _datetime;

        //public DateTime GetDateTime() => _datetime;

        public bool IsAM() => _datetime.Hour < 12;

        public bool IsDayTime() => (_dayTimeFrom <= _datetime.Hour) && (_datetime.Hour < _nightTimeFrom);

        public bool IsZeroOclockIn12() => (_datetime.Hour % 12 == 0) && (_datetime.Minute == 0) && (_datetime.Second == 0);

        private bool IsJustSwitchToDayTime() => (_datetime.Hour == _dayTimeFrom) && (_datetime.Minute == 0) && (_datetime.Second == 0);

        private bool IsJustSwitchToNightTime() => (_datetime.Hour == _nightTimeFrom) && (_datetime.Minute == 0) && (_datetime.Second == 0);


        public int GetHour12() => _datetime.Hour % 12;

        public int GetHour() => _datetime.Hour;
        public int GetMinute() => _datetime.Minute;
        public int GetSecond() => _datetime.Second;
        public int GetMillisecond() => _datetime.Millisecond;

    }
}