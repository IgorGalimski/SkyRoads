using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PoolableObject : MonoBehaviour
{
    [SerializeField]
    private float _boundsMutliplier = 1f;
    
    private Renderer _renderer;

    public Vector2 GetTopBorder()
    {
        return transform.position + _renderer.bounds.size*_boundsMutliplier;
    }
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
}