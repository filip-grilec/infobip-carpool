using System;

namespace Carpool.Time
{
    public interface ITimeProvider
    {
        DateTime NowUtc();
    }
}