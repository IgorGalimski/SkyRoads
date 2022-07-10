using UnityEngine;
using System.Collections;
using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;

namespace SpaceShooter
{
	public class CameraShake : MonoBehaviour
	{
		[SerializeField] private float _shakeDuration = 1f;

		[SerializeField] private float _shakeAmount = 0.7f;

		[SerializeField] private float _decreaseFactor = 1.0f;

		private Vector3 _originalPosition;

		private float _currentShakeDuration;

		private void Awake()
		{
			MessageSystemManager.AddListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
		}

		private void OnDestroy()
		{
			MessageSystemManager.RemoveListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
		}

		private void OnGameFail(LevelFailData levelFailData)
		{
			MessageSystemManager.RemoveListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);

			StartCoroutine(ShakeCamera());
		}

		private IEnumerator ShakeCamera()
		{
			_originalPosition = transform.position;

			_currentShakeDuration = _shakeDuration;

			while (_currentShakeDuration > float.Epsilon)
			{
				transform.position = _originalPosition + Random.insideUnitSphere * _shakeAmount;

				_currentShakeDuration -= Time.deltaTime * _decreaseFactor;

				yield return null;
			}
		}
	}
}