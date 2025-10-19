using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cupboards
{
    [Serializable]
    public class GameConfig
    {
        public int ChipCount;
        public int PositionCount;
        public List<PositionModel> Positions;
        public List<int> InitialLayout;
        public List<int> TargetLayout;
        public int ConnectionCount;
        public List<ConnectionModel> Connections;

        public void InitializeWithDefault()
        {
            ChipCount = 1;
            PositionCount = 2;
            Positions = new List<PositionModel>
            {
                new(0, new Vector2(100, 100)),
                new(2, new Vector2(200, 100))
            };
            InitialLayout = new List<int> { 0 };
            TargetLayout = new List<int> { 1 };
            ConnectionCount = 1;
            Connections = new List<ConnectionModel>
            {
                new(0, 1)
            };
        }
    }
}
