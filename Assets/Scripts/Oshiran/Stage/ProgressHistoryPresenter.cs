using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Oshiran.Stage
{
    public class ProgressHistoryPresenter : IAsyncStartable
    {
        readonly SaveFile saveFile;
        readonly ProgressHistoryView historyView;

        [Inject]
        public ProgressHistoryPresenter(SaveFile saveFile, ProgressHistoryView historyView)
        {
            this.saveFile = saveFile;
            this.historyView = historyView;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => saveFile.IsLoaded, cancellationToken: cancellation);

            (int easy, int normal, int hard) progress = saveFile.GetProgress();
            historyView.SetProgress(progress.easy, progress.normal, progress.hard);
        }
    }
}