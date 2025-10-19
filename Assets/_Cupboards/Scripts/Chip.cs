using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using R3;
using UnityEngine.EventSystems;

namespace Cupboards
{
    public partial class Chip
    {
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.yellow;
        [SerializeField] private SpriteRenderer _spriteRenderer;
    
        public readonly Subject<Chip> Clicked = new();
        
        public int CurrentPositionId { get; private set; }
        

        public void Deselect()
        {
            _spriteRenderer.color = _defaultColor;
        }

        public void Setup(int startPositionId)
        {
            CurrentPositionId = startPositionId;
        }

        public async UniTask MoveAsync(Vector2 targetPosition, int targetPositionId, float duration = 0.5f)
        {
            DOTween.Kill(this);
            await transform.DOMove(targetPosition, duration)
                .SetEase(Ease.OutCubic)
                .SetId(this)
                .OnComplete(() => CurrentPositionId = targetPositionId)
                .ToUniTask();
        }

        public void Select()
        {
            _spriteRenderer.color = _selectedColor;
        }
    }

    public partial class Chip : MonoBehaviour
    {
        private void Awake()
        {
            _spriteRenderer.color = _defaultColor;

        }
        
        private void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }

    public partial class Chip : IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked.OnNext(this);
        }
    }
}
