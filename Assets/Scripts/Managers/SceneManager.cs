using System.Collections;
using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;
using SpaceShooter.View;
using UnityEngine;

namespace SpaceShooter.Managers
{
    [RequireComponent(typeof(BackgroundView))]
    public class SceneManager : BaseManager
    {
        [SerializeField] 
        private BackgroundView _view;

        protected override void Init()
        {
            MessageSystemManager.AddListener(MessageType.OnGameStart, OnGameStart);
            MessageSystemManager.AddListener(MessageType.OnGameReplay, OnGameReplay);

            StartSingleCoroutine(FadeOut());
        }

        private void OnDestroy()
        {
            MessageSystemManager.RemoveListener(MessageType.OnGameStart, OnGameStart);
            MessageSystemManager.RemoveListener(MessageType.OnGameReplay, OnGameReplay);
        }

        private void OnGameStart()
        {
            StartSingleCoroutine(LoadScene(1));
        }

        private void OnGameReplay()
        {
            StartSingleCoroutine(LoadScene(1));
        }

        private void StartSingleCoroutine(IEnumerator enumerator)
        {
            StopAllCoroutines();

            StartCoroutine(enumerator);
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            yield return StartCoroutine(FadeIn());

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

            yield return StartCoroutine(FadeOut());

            MessageSystemManager.Invoke(MessageType.OnGameLoad);
        }

        private IEnumerator FadeIn()
        {
            _view.Alpha = 0f;
            _view.Blocker = true;

            while (_view.Alpha < 1f)
            {
                _view.Alpha += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator FadeOut()
        {
            _view.Alpha = 1f;

            while (_view.Alpha > 0f)
            {
                _view.Alpha -= Time.deltaTime;

                yield return null;
            }

            _view.Blocker = false;
        }
    }
}
