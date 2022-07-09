using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AsteroidMaterials", menuName = "ScriptableObjects/AsteroidMaterialScriptableObject", order = 1)]
    public class AsteroidMaterialsScriptableObject : ScriptableObject
    {
        [SerializeField] 
        private Material _standardMaterial;

        [SerializeField] 
        private Material _boosterEnabledMaterial;

        public Material StandardMaterial => _standardMaterial;

        public Material BoosterEnabledMaterial => _boosterEnabledMaterial;
    }
}