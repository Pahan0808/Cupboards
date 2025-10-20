using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;

namespace Cupboards
{
    public partial class GameManager
    {
        private readonly List<Chip> _chips = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly List<Connection> _connections = new();
        private readonly List<Position> _positions = new();
        
        [Header("Config")]
        [SerializeField] private string _defaultFileName = "level1.txt";
        
        [Header("Containers")]
        [SerializeField] private Transform _chipContainer;
        [SerializeField] private Transform _connectionContainer;
        [SerializeField] private Transform _positionContainer;
        
        [Header("Prefabs")]
        [SerializeField] private Chip _chipPrefab;
        [SerializeField] private Connection _connectionPrefab;
        [SerializeField] private Position _positionPrefab;
        
        private GameConfig _config;
        private List<int> _currentLayout;
        private bool _isMoving;
        private Chip _selectedChip;
        private List<int> _validMovePositions = new();

        #region Injection
        
        private ColorService _colorService;
        private ConfigParser _configParser;
        private GameOverWindow _gameOverWindow;
        private FileService _fileService;
        private PathfindingService _pathfindingService;
        private WinPreview _winPreview;

        [Inject]
        public void Construct(
            ColorService colorService,
            ConfigParser configParser,
            GameOverWindow gameOverWindow,
            FileService fileService,
            PathfindingService pathfindingService,
            WinPreview winPreview
        )
        {
            _colorService = colorService;
            _configParser = configParser;
            _gameOverWindow = gameOverWindow;
            _fileService = fileService;
            _pathfindingService = pathfindingService;
            _winPreview = winPreview;
        }

        #endregion

        public void LoadLevel(string levelName)
        {
            ClearAll();
            LoadGameConfig(levelName);
            InitializeGame();
        }

        private static void DestroyObjects(IEnumerable<MonoBehaviour> monoBehaviourCollection)
        {
            foreach (var monoBehaviour in monoBehaviourCollection)
            {
                if (monoBehaviour != null && monoBehaviour.gameObject != null)
                {
                    Destroy(monoBehaviour.gameObject);
                }
            }
        }

        private void CheckGameOver()
        {
            var isGameOver = true;
            for (var i = 0; i < _config.ChipCount; i++)
            {
                if (_currentLayout[i] == _config.TargetLayout[i]) continue;
                isGameOver = false;
                break;
            }

            if (isGameOver)
            {
                _gameOverWindow.gameObject.SetActive(true);
            }
        }

        private void ClearAll()
        {
            DestroyObjects(_chips);
            _chips.Clear();
            DestroyObjects(_connections);
            _connections.Clear();
            DestroyObjects(_positions);
            _positions.Clear();
        }

        private void ClearValidPositions()
        {
            foreach (var positionId in _validMovePositions)
            {
                _positions[positionId].Unhighlight();
            }

            _validMovePositions.Clear();
        }

        private void CreateChips()
        {
            for (var i = 0; i < _config.ChipCount; i++)
            {
                var positionId = _config.InitialLayout[i];
                var position = _config.Positions[positionId];

                var chip = Instantiate(_chipPrefab, _chipContainer);
                chip.transform.SetLocalPositionAndRotation(position.Position, Quaternion.identity);
                chip.Setup(positionId, _colorService[i]);
                chip.Clicked.Subscribe(OnChipClicked).AddTo(_compositeDisposable);
                _chips.Add(chip);
            }
        }

        private void CreateConnections()
        {
            foreach (var connectionModel in _config.Connections)
            {
                var connection = Instantiate(_connectionPrefab, _connectionContainer);
                connection.Setup(_config.Positions[connectionModel.Start].Position, _config.Positions[connectionModel.End].Position);
                _connections.Add(connection);
            }
        }

        private void CreatePositions()
        {
            foreach (var positionModel in _config.Positions)
            {
                var position = Instantiate(_positionPrefab, _positionContainer);
                position.Setup(positionModel.ID, positionModel.Position);
                position.Clicked.Subscribe(OnPositionClicked).AddTo(_compositeDisposable);
                _positions.Add(position);
            }
        }

        private void InitializeGame()
        {
            _colorService.GenerateRandomColors(_config.ChipCount);
            _currentLayout = new List<int>(_config.InitialLayout);
            _winPreview.Create(_config);
            CreateConnections();
            CreateChips();
            CreatePositions();
        }

        private void LoadGameConfig(string fileName)
        {
            var configText = _fileService.LoadFile(fileName);

            if (string.IsNullOrEmpty(configText))
            {
                Debug.LogError($"Failed to load config file: {fileName}");
                _config = new GameConfig();
                _config.InitializeWithDefault();
                return;
            }

            _config = _configParser.Parse(configText);
        }

        private async UniTask MoveChipAsync(Chip chip, int targetPositionId)
        {
            _isMoving = true;
            ClearValidPositions();
            _currentLayout[_chips.IndexOf(chip)] = targetPositionId;
            await chip.MoveAsync(_config.Positions[targetPositionId].Position, targetPositionId, 0.5f);

            _isMoving = false;
            _selectedChip.Deselect();
            _selectedChip = null;
            CheckGameOver();
        }

        private void OnChipClicked(Chip chip)
        {
            if (_isMoving) return;

            if (_selectedChip != null)
            {
                _selectedChip.Deselect();
                ClearValidPositions();
            }

            _selectedChip = chip;
            _selectedChip.Select();
            _validMovePositions = _pathfindingService.FindReachablePositions(_selectedChip.CurrentPositionId, _currentLayout, _config.Connections);
            ShowValidPositions();
        }

        private void OnPositionClicked(Position position)
        {
            if (_selectedChip == null || _isMoving) return;

            if (_validMovePositions.Contains(position.PositionId))
            {
                MoveChipAsync(_selectedChip, position.PositionId).Forget();
            }
        }

        private void ShowValidPositions()
        {
            foreach (var positionId in _validMovePositions)
            {
                _positions[positionId].Highlight();
            }
        }
    }

    public partial class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            LoadGameConfig(_defaultFileName);
            InitializeGame();
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
