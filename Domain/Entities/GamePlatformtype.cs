using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class GamePlatformType
    {
        public int PlatformTypeId { get; set; }
        public virtual PlatformType PlatformType { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
