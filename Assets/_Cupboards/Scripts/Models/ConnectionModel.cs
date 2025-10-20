using System;

namespace Cupboards
{
    [Serializable]
    public class ConnectionModel
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public ConnectionModel(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
