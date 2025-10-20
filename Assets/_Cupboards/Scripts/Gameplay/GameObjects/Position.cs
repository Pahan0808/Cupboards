using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cupboards
{
    public partial class Position
    {
        [Header("Colors")]
        [SerializeField] private Color _defaultColor = Color.gray;
        [SerializeField] private Color _validColor = Color.green;
        [Space]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public readonly Subject<Position> Clicked = new();
        public int PositionId { get; private set; }

        public void Highlight()
        {
            _spriteRenderer.color = _validColor;
        }

        public void Setup(int positionId, Vector2 position)
        {
            PositionId = positionId;
            transform.localPosition = position;
        }

        public void Unhighlight()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }

    public partial class Position : MonoBehaviour
    {
        private void Awake()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }

    public partial class Position : IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked.OnNext(this);
        }
    }
}
