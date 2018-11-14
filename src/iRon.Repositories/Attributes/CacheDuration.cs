using iRon.Repositories.Enums;

namespace iRon.Repositories.Attributes
{
    using System;

    public class CacheDuration: Attribute
    {
        private readonly Duration duration;
        public CacheDuration(Duration duration)
        {
            this.duration = duration;
        }

        public Duration Duration => this.duration;
    }
}
