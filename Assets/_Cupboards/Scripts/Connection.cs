using UnityEngine;

namespace Cupboards
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public void Setup(Vector2 start, Vector2 end)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, new Vector3(start.x, start.y, transform.position.z));
            _lineRenderer.SetPosition(1, new Vector3(end.x, end.y, transform.position.z));
        }
    }
}
