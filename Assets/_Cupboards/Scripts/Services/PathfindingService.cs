using System.Collections.Generic;

namespace Cupboards
{
    public class PathfindingService
    {
        public List<int> FindReachablePositions(int startPosition, List<int> occupiedPositions, List<ConnectionModel> connections)
        {
            var queue = new Queue<int>();
            queue.Enqueue(startPosition);
            var reachable = new List<int>();
            var visited = new HashSet<int> { startPosition };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var connection in connections)
                {
                    var neighbor = -1;
                    if (connection.Start == current) neighbor = connection.End;
                    if (connection.End == current) neighbor = connection.Start;

                    if (neighbor == -1 || visited.Contains(neighbor) || occupiedPositions.Contains(neighbor))
                    {
                        continue;
                    }
                    
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    reachable.Add(neighbor);
                }
            }

            return reachable;
        }
    }
}
