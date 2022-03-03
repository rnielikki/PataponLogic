﻿namespace PataRoad.Core.Character
{
    public struct BossAttackMoveSegment
    {
        internal string Action { get; }
        internal float Distance { get; }
        /// <summary>
        /// Maximum distance from <see cref="Patapons.PataponsManager"/> position. Cannot exceed this distance. -1 means no maximum distance limit.
        /// </summary>
        internal float MaxDistance { get; }
        internal BossAttackMoveSegment(string action, float distacne, float maxDistance = 10)
        {
            Action = action;
            Distance = distacne;
            MaxDistance = maxDistance;
        }
    }
}
