using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Image))]
    public class BoosterView : MonoBehaviour
    {
        [SerializeField] 
        private Image _booster;
        
        public void SetBoosterViewFill(float percent)
        {
            _booster.fillAmount = percent;
        }
    }
}