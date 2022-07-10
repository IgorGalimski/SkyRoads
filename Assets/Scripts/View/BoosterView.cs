using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.View
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