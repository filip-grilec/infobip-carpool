using System;

namespace Carpool.Time
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime NowUtc() => DateTime.UtcNow;
    }
}