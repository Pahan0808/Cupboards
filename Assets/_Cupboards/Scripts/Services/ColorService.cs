using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Cupboards
{
    public class ColorService
    {
        private readonly Color[] _baseColors = 
        {
            new Color32(8, 37, 204, 255),
            new Color32(65, 105, 204, 255),
            new Color32(120, 81, 169, 255),
            new Color32(202, 44, 146, 255),
            new Color32(202, 1, 71, 255),
            new Color32(155, 45, 48, 255),
            new Color32(243, 71, 35, 255),
            new Color32(230, 214, 144, 255),
            new Color32(62, 180, 137, 255),
            new Color32(72, 6, 7, 255),
        };
        
        private Dictionary<int, Color> _colorDictionary = new();
        
        public Color this[int index] => _colorDictionary.ContainsKey(index) ? _colorDictionary[index] : Color.white;

        public void GenerateRandomColors(int count)
        {
            _colorDictionary = new Dictionary<int, Color>();
            var colors = _baseColors.ToList();
        
            for (var i = colors.Count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (colors[i], colors[randomIndex]) = (colors[randomIndex], colors[i]);
            }
        
            for (var i = 0; i < count && i < colors.Count; i++)
            {
                _colorDictionary[i] = colors[i];
            }
        
            for (var i = colors.Count; i < count; i++)
            {
                _colorDictionary[i] = new Color(Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
            }
        }
    }
}
