using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Oshiran.UI
{
    public class ButtonOpeningPresenter : IAsyncStartable
    {
        readonly ButtonOpening buttonOpening;
        readonly SaveFile saveFile;

        [Inject]
        public ButtonOpeningPresenter(ButtonOpening buttonOpening, SaveFile saveFile)
        {
            this.buttonOpening = buttonOpening;
            this.saveFile = saveFile;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => saveFile.IsLoaded, cancellationToken: cancellation);

            buttonOpening.SetEnable(saveFile.Data.PlayOnce);
        }
    }
}