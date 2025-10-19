using UnityEngine;

namespace Cupboards
{
    public class CameraSetup : MonoBehaviour
    {
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        
        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
            {
                Setup();
            }
        }

        private void Setup()
        {
            var orthographicHeight = Camera.main.orthographicSize;
            var orthographicWidth = orthographicHeight * Camera.main.aspect;
            Camera.main.transform.position = new Vector3(orthographicWidth, orthographicHeight, -10f);
            
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }
    }
}