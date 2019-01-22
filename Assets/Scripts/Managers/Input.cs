using System;
using DefaultNamespace.MessageSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class Input : MonoBehaviour
    {
        [SerializeField] private float _tolerance;
    
        void Update()
        {
            float vecticalAxis = UnityEngine.Input.GetAxis("Vertical");
            float horizontalAxis = UnityEngine.Input.GetAxis("Horizontal");

            if (Math.Abs(vecticalAxis) > _tolerance || Math.Abs(horizontalAxis) > _tolerance)
            {
                //Debug.Log(vecticalAxis + " " + horizontalAxis);
                
                MessageSystemManager.Invoke(MessageType.OnAxisInput, new AxisData(horizontalAxis, vecticalAxis));
            }
            
            

            // Make it move 10 meters per second instead of 10 meters per frame...
            //translation *= Time.deltaTime;
            //rotation *= Time.deltaTime;

            // Move translation along the object's z-axis
            //transform.Translate(0, 0, translation);
        }
    }
}