using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Cupboards
{
    public partial class GameOverWindow
    {
        private readonly CompositeDisposable _compositeDisposable = new();
        
        [SerializeField] private Button _exitButton;
    }

    public partial class GameOverWindow : MonoBehaviour
    {
        private void Awake()
        {
            _exitButton.onClick.AsObservable().Subscribe(_ =>
            {
                gameObject.SetActive(false);
            }).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
