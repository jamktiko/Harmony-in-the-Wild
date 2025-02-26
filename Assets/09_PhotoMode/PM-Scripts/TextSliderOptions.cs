using UnityEngine;

namespace PhotoMode
{

    [CreateAssetMenu(fileName = "TextSliderOption", menuName = "ScriptableObjects/Text Slider Options", order = 1)]
    public class TextSliderOptions : ScriptableObject
    {
        public string optionName;
        public string optionEvent;
    }
}