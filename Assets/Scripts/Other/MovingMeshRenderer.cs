using System;

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MovingMeshRenderer : MonoBehaviour 
{
    private enum MovingDirection
    {
        moveX = 0, 
        moveY = 1, 
        moveXY = 2
    };
    
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    [SerializeField]
    private MovingDirection _movingDirection = MovingDirection.moveXY;

    [SerializeField] 
    private bool _randomDirection;

    [SerializeField] [Range(0f, 1f)]
    private float _xSpeed = 0.001f;
    
    [SerializeField] [Range(0f, 1f)]
    private float _ySpeed = 0.001f;

    private Vector2 _savedOffset;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        
        _savedOffset = _meshRenderer.material.GetTextureOffset(MainTex);

        if (_randomDirection)
        {
            Array movingDirections = Enum.GetValues(typeof(MovingDirection));

            //_movingDirection = (MovingDirection)(movingDirections.GetValue(Random.Range(0, movingDirections.Length)));
            _movingDirection = movingDirections.GetRandomElement<MovingDirection>();
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _meshRenderer.material.SetTextureOffset("_MainTex", GetOffset());
    }

    private Vector2 GetOffset()
    {
        Vector2 offset = Vector2.zero;
        
        float tmpX = Mathf.Repeat(Time.time * _xSpeed, 1);
        float tmpY = Mathf.Repeat(Time.time * _ySpeed, 1);

        switch (_movingDirection)
        {
            case MovingDirection.moveX:
            {
                offset = new Vector2(tmpX, _savedOffset.y);
                
                break;
            }

            case MovingDirection.moveY:
            {
                offset = new Vector2(_savedOffset.x, tmpY);
                
                break;
            }

            case MovingDirection.moveXY:
            {
                offset = new Vector2(tmpX, tmpY);
                
                break;
            }
        }
        
        return offset;
    }
}
