using System.Threading;
using Cysharp.Threading.Tasks;
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

        public async UniTask AnimateResult(int progress, bool isNewRecord, CancellationToken cancellationToken)
        {
            progressResult.ResetDisplay();

            await progressResult.DOValue(progress, 0.5f).Play().ToUniTask(cancellationToken: cancellationToken);

            if (isNewRecord)
            {
                progressResult.NewRecord();
            }
            else
            {
                progressResult.NormalRecord();
            }
        }

        public void UpdateInGameProgress(int progress)
        {
            progressView.SetValue(progress);
        }
    }
}