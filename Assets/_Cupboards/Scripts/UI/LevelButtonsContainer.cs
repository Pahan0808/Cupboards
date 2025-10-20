using System.Collections.Generic;
using System.IO;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Cupboards
{
    public partial class LevelButtonsContainer
    {
        private readonly List<Button> _buttons;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        [SerializeField] private LevelButton _levelButtonPrefab;

        #region Injection
        
        private GameManager _gameManager;

        [Inject]
        public void Construct(
            GameManager gameManager
        )
        {
            _gameManager = gameManager;
        }

        #endregion
    }

    public partial class LevelButtonsContainer : MonoBehaviour
    {
        private void Awake()
        {
            CreateButtons();
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }

        private static IEnumerable<string> GetAllFileNames()
        {
            var allTextAssets = Resources.LoadAll<TextAsset>("");
    
            var levelFiles = allTextAssets
                .Where(asset => asset.name.StartsWith("level_"))
                .Select(asset => asset.name);
    
            return levelFiles.ToList();
        }

        private void CreateButtons()
        {
            foreach (var fileName in GetAllFileNames())
            {
                var levelButton = Instantiate(_levelButtonPrefab, transform);
                levelButton.SetName(fileName);
                levelButton.Clicked.AsObservable().Subscribe(_ =>
                {
                    _gameManager.LoadLevel(fileName);
                }).AddTo(_compositeDisposable);
            }
        }
    }
}
