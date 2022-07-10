﻿using UnityEditor;
using UnityEngine;

namespace SpaceShooter.Editor
{
    [CustomEditor(typeof(ObjectPool.ObjectPool))]
    public class ObjectPoolEditor : UnityEditor.Editor
    {
        private const int HANDLE_SIZE = 3;
        private const float Y_LENGHT = 200f;
        private const float OFFSET_COUNT = 100f;

        private SerializedProperty _startPositionProperty;
        private SerializedProperty _xValuesProperty;
        private SerializedProperty _offsetProperty;

        private ObjectPool.ObjectPool _objectPool;

        private void OnEnable()
        {
            _startPositionProperty = serializedObject.FindProperty("_startPosition");
            _xValuesProperty = serializedObject.FindProperty("_xValues");
            _offsetProperty = serializedObject.FindProperty("_offset");

            _objectPool = target as ObjectPool.ObjectPool;
        }

        private void OnSceneGUI()
        {
            Handles.SphereHandleCap(1, _startPositionProperty.vector3Value, Quaternion.identity, HANDLE_SIZE,
                EventType.Repaint);

            if (_objectPool.positionType == ObjectPool.ObjectPool.PositionType.RandomDistance)
            {
                DrawXValues();
            }
            else
            {
                DrawOffset();
            }
        }

        private void DrawXValues()
        {
            for (int i = 0; i < _xValuesProperty.arraySize; i++)
            {
                SerializedProperty xValue = _xValuesProperty.GetArrayElementAtIndex(i);

                Vector3 startPoint = new Vector3(xValue.floatValue, -Y_LENGHT, 0f);
                Vector3 endPoint = new Vector3(xValue.floatValue, Y_LENGHT, 0f);

                Handles.DrawLine(startPoint, endPoint);
            }
        }

        private void DrawOffset()
        {
            Vector3 previousPosition = _startPositionProperty.vector3Value;
            for (int i = 0; i < OFFSET_COUNT; i++)
            {
                Vector3 newPosition = previousPosition + _offsetProperty.vector3Value;

                Handles.CubeHandleCap(2, newPosition, Quaternion.identity, HANDLE_SIZE, EventType.Repaint);

                previousPosition = newPosition;
            }
        }
    }
}
