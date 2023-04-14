using System;
using DG.Tweening;
using UnityEngine;

namespace Oshiran.Stage
{
    public class StageMover : MonoBehaviour
    {
        [SerializeField] Transform goal;
        [SerializeField] Transform start;

        public float Progress => Mathf.InverseLerp(startPos, goalPos, currentPos) * 100;
        public float Length => Vector2.Distance(goal.position, start.position);

        float startPos, goalPos, currentPos;
        float tweenTime;

        Transform mainCamera;
        Tween cameraTween;

        public event Action<int> OnProgressUpdate;
        public event Action OnGoal;

        public void OnStart()
        {
            startPos = start.position.x;
            goalPos = goal.position.x;

            mainCamera = Camera.main.transform;
            mainCamera.position = start.position;

            cameraTween = mainCamera.DOMoveX(goalPos, tweenTime)
                .SetEase(Ease.Linear)
                .OnUpdate(UpdateCurrentPosition)
                .OnComplete(() => OnGoal?.Invoke())
                .Play();
        }

        public void OnRestart()
        {
            cameraTween.Rewind();
            cameraTween.Play();
        }

        public void Pause()
        {
            cameraTween.Pause();
        }

        public void SetTweenSpeed(float speed)
        {
            tweenTime = Vector2.Distance(start.position, goal.position) / speed;
        }

        void UpdateCurrentPosition()
        {
            currentPos = mainCamera.position.x;
            OnProgressUpdate?.Invoke((int)Progress);
        }
    }
}