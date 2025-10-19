using UnityEngine;

namespace Cupboards
{
    public class CameraSetup : MonoBehaviour
    {
        private const float Height = 300f;
        private void Start()
        {
            var mainCamera = Camera.main;
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = Height / 2f;
            mainCamera.transform.position = new Vector3(200f, 200f, -10f);
        }
    }
}
