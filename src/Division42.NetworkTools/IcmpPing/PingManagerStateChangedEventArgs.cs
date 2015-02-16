using System;

namespace Division42.NetworkTools.IcmpPing
{
    public class PingManagerStateChangedEventArgs : EventArgs
    {
        public PingManagerStateChangedEventArgs(PingManagerStates newState)
        {
            NewState = newState;
        }

        public PingManagerStates NewState { get; set; }
    }
}