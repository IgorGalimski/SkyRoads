using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BackgroundView : MonoBehaviour
    {
        [SerializeField] 
        private Image _background;

        public float Alpha
        {
            get => _background.color.a;

            set
            {
                var backgroundColor = _background.color;
                backgroundColor.a = value;
                _background.color = backgroundColor;
            }
        }

        public bool Blocker
        {
            get => _background.raycastTarget;
            set => _background.raycastTarget = value;
        }
    }
}