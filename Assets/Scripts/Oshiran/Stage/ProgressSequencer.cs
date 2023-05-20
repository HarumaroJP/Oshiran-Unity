using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Oshiran.Stage
{
    public class ProgressSequencer : IStartable
    {
        readonly SaveFile saveFile;
        readonly ProgressViewHandler progressViewHandler;
        readonly StageMover stageMover;
        readonly GameEventRelay gameEventRelay;

        [Inject]
        public ProgressSequencer(SaveFile saveFile, ProgressViewHandler progressViewHandler, StageMover stageMover, GameEventRelay gameEventRelay)
        {
            this.saveFile = saveFile;
            this.progressViewHandler = progressViewHandler;
            this.stageMover = stageMover;
            this.gameEventRelay = gameEventRelay;
        }

        public void Start()
        {
            stageMover.OnGoal += gameEventRelay.OnClear;
            stageMover.OnGoal += ShowResult;
            gameEventRelay.OnGameRestart += stageMover.OnRestart;
            stageMover.OnProgressUpdate += progressViewHandler.UpdateInGameProgress;

            gameEventRelay.OnGameRestart += OnGameRestart;
            gameEventRelay.OnGameClear += progressViewHandler.SwitchToResult;
            gameEventRelay.OnPlayerDeath += progressViewHandler.SwitchToResult;
            gameEventRelay.OnPlayerDeath += ShowResult;

            stageMover.OnStart();
        }

        CancellationTokenSource animateResultCanceller;

        void ShowResult()
        {
            int prevProgress = saveFile.GetProgress(gameEventRelay.gameMode);
            int progress = (int)stageMover.Progress;


            animateResultCanceller = new CancellationTokenSource();
            CancellationToken cToken = animateResultCanceller.Token;
            progressViewHandler.AnimateResult(progress, progress > prevProgress, cToken).Forget();

            saveFile.SetProgress(gameEventRelay.gameMode, progress);
        }

        void OnGameRestart()
        {
            animateResultCanceller?.Cancel();
            animateResultCanceller?.Dispose();

            progressViewHandler.SwitchToInGameView();
        }
    }
}