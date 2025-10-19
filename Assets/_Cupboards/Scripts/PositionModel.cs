using System;
using UnityEngine;

namespace Cupboards
{
    [Serializable]
    public class PositionModel
    {
        public int ID { get; private set; }
        public Vector2 Position { get; private set; }

        public PositionModel(int id, Vector2 position)
        {
            ID = id;
            Position = position;
        }
    }
}
