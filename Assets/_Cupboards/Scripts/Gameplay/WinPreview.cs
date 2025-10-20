using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VContainer;

namespace Cupboards
{
    public partial class WinPreview
    {
        private readonly List<GameObject> _gameObjects = new();
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _chipPrefab;
        [SerializeField] private LineRenderer _connectionPrefab;
        
        [Header("Settings")]
        [SerializeField] private float _scale = 0.5f;

        private GameConfig _config;

        #region Injection
        
        private ColorService _colorService;

        [Inject]
        public void Construct(ColorService colorService)
        {
            _colorService = colorService;
        }

        #endregion

        public void Create(GameConfig config)
        {
            _config = config;
            DestroyGameObjects();
            InstantiateConnections();
            InstantiateChips();
        }

        private Vector3 ConvertForPreview(Vector2 originalPosition)
        {
            var scaledPosition = originalPosition * _scale;
            return new Vector3(scaledPosition.x, scaledPosition.y, 0);
        }

        private void DestroyGameObjects()
        {
            foreach (var previewObject in _gameObjects.Where(go => go != null))
            {
                Destroy(previewObject);
            }

            _gameObjects.Clear();
        }

        private void InstantiateChips()
        {
            for (var i = 0; i < _config.ChipCount; i++)
            {
                var chip = Instantiate(_chipPrefab, transform);
                
                var position = _config.Positions[_config.TargetLayout[i]];
                var scaledLocalPosition = ConvertForPreview(position.Position);
                scaledLocalPosition.z = -5f;
                chip.transform.SetLocalPositionAndRotation(scaledLocalPosition, Quaternion.identity);
                
                var spriteRenderer = chip.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = _colorService[i];
                }

                _gameObjects.Add(chip);
            }
        }

        private void InstantiateConnections()
        {
            foreach (var connectionModel in _config.Connections)
            {
                var connection = Instantiate(_connectionPrefab, transform);
                connection.positionCount = 2;
                connection.SetPosition(0, ConvertForPreview(_config.Positions[connectionModel.Start].Position));
                connection.SetPosition(1, ConvertForPreview(_config.Positions[connectionModel.End].Position));
                _gameObjects.Add(connection.gameObject);
            }
        }
    }

    public partial class WinPreview : MonoBehaviour
    {
        private void OnDestroy()
        {
            DestroyGameObjects();
        }
    }
}
