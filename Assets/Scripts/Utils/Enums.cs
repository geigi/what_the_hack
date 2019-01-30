using System;

namespace Utils
{
    public static class Enums
    {
        public enum TileState
        {
            FREE,
            OCCUPIED,
            BLOCKED,
            DISABLED
        }

        public enum EmployeeState
        {
            PAUSED,
            IDLE,
            WALKING,
            WORKING
        }
    }
}