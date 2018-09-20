using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class PlatformType
    {
        public int Id { get; set; }

        public virtual Platform Type { get; set; }

        public virtual ICollection<GamePlatformType> GamePlatformtypes { get; set; }
    }

    public enum Platform
    {
        desktop,
        mobile,
        browser,
        console
    }
}
