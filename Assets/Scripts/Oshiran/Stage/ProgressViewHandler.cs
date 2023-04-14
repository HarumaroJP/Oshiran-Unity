using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oshiran.Stage
{
    public class ProgressViewHandler : MonoBehaviour
    {
        [SerializeField] ProgressView progressView;
        [SerializeField] ProgressView progressResult;

        public void SwitchToInGameView()
        {
            progressView.gameObject.SetActive(true);
            progressResult.ResetDisplay();
            progressResult.gameObject.SetActive(false);
        }

        public void SwitchToResult()
        {
            progressView.gameObject.SetActive(false);
            progressResult.gameObject.SetActive(true);
        }

        public void AnimateResult(int progress, bool isNewRecord)
        {
            progressResult.DOValue(progress, 0.5f)
                .OnComplete(() =>
                {
                    if (isNewRecord)
                    {
                        progressResult.NewRecord();
                    }
                    else
                    {
                        progressResult.NormalRecord();
                    }
                })
                .Play();
        }

        public void UpdateInGameProgress(int progress)
        {
            progressView.SetValue(progress);
        }
    }
}