using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cupboards
{
    public class ConfigParser
    {
        public GameConfig Parse(string configText)
        {
            var lines = configText.Split('\n')
                .Select(line => line.Trim())
                .Where(line => string.IsNullOrEmpty(line) == false)
                .ToArray();

            var config = new GameConfig
            {
                ChipCount = GetChipCount(lines),
                PositionCount = GetPositionCount(lines)
            };
            config.Positions = GetPositions(lines, config.PositionCount);

            var initialLayoutIndex = 2 + config.PositionCount;
            config.InitialLayout = GetInitialLayout(lines, initialLayoutIndex);
            config.TargetLayout = GetTargetLayout(lines, initialLayoutIndex + 1);

            var connectionIndex = initialLayoutIndex + 2;
            config.ConnectionCount = int.Parse(lines[connectionIndex]);
            config.Connections = GetConnections(lines, connectionIndex + 1, config.ConnectionCount);

            return config;
        }

        private static int GetChipCount(string[] lines)
        {
            return int.Parse(lines[0]);
        }

        private static List<ConnectionModel> GetConnections(string[] lines, int startIndex, int connectionCount)
        {
            var connections = new List<ConnectionModel>();
            for (var i = startIndex; i < startIndex + connectionCount; i++)
            {
                var connection = lines[i].Split(',').Select(x => int.Parse(x.Trim()) - 1).ToArray();
                connections.Add(new ConnectionModel(connection[0], connection[1]));
            }

            return connections;
        }

        private static List<int> GetInitialLayout(string[] lines, int layoutIndex)
        {
            return lines[layoutIndex].Split(',').Select(x => int.Parse(x.Trim()) - 1).ToList();
        }

        private static int GetPositionCount(string[] lines)
        {
            return int.Parse(lines[1]);
        }

        private static List<PositionModel> GetPositions(string[] lines, int positionsCount)
        {
            var positions = new List<PositionModel>();
            for (var i = 2; i < 2 + positionsCount; i++)
            {
                var values = lines[i].Split(',').Select(value => float.Parse(value.Trim())).ToArray();
                positions.Add(new PositionModel(i - 2, new Vector2(values[0], values[1])));
            }

            return positions;
        }

        private static List<int> GetTargetLayout(string[] lines, int layoutIndex)
        {
            return lines[layoutIndex].Split(',').Select(x => int.Parse(x.Trim()) - 1).ToList();
        }
    }
}
