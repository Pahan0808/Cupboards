using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Cupboards
{
    public partial class ExitButton
    {
        private readonly CompositeDisposable _compositeDisposable = new();
        
        [SerializeField] private Button _exitButton;
    }

    public partial class ExitButton : MonoBehaviour
    {
        private void Awake()
        {
            _exitButton.onClick.AsObservable().Subscribe(_ => Application.Quit()).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
