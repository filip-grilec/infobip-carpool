using System;

namespace Carpool.Time
{
    public class CustomCurrentTimeProvider : ITimeProvider
    {
        private readonly DateTime _currentTime;

        public CustomCurrentTimeProvider(DateTime currentTime)
        {
            _currentTime = new DateTime(currentTime.Ticks, DateTimeKind.Utc);
        }
        
        
        public DateTime NowUtc() => _currentTime;
    }
}