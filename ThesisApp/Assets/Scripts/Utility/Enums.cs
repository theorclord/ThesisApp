using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public enum PCGNodeType
    {
        A,
        B,
        C,
        D,
        S,
        E
    }
    public enum NodeType
    {
        START,
        GOAL,
        NORMAL,
        TRADING,
        DISTRESS,
        RANDOM
    }

    public enum EventSpec
    {
        GATHER,
        RESEARCH/*,
        DIPLOMACY*/
    }

    public enum EventDifficulty
    {
        BASIC,
        INTERMEDIATE,
        HARD
    }

    public enum EventOutcomeType
    {
        SUCCESS,
        NEUTRAL,
        FAILURE
    }

    public enum EventChance
    {
        CRITICAL,
        NORMAL
    }
    public enum BoardType
    {
        RESOURCE,
        ROOM
    }

    public enum Location
    {
        MINE,
        QUARRY,
        WRECKAGE,
        FACTORY,
        VILLAGE,
        FOREST,
        ROCKFORMATION,
        MAGICSITE,
        LAKE,
        RUINS,
        CATHEDRAL,
        SETTLEMENT
    }
    class Enums
    {
    }
}
