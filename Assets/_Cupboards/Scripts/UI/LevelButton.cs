using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cupboards
{
    public partial class LevelButton
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _textBox;
        
        public UnityEvent Clicked => _button.onClick;

        public void SetName(string buttonName)
        {
            _textBox.text = buttonName.Replace(".txt", "");
        }
    }

    public partial class LevelButton : MonoBehaviour
    {
    }
}
