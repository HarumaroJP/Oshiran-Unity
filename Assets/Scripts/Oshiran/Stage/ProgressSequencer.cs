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

            gameEventRelay.OnGameRestart += progressViewHandler.SwitchToInGameView;
            gameEventRelay.OnGameClear += progressViewHandler.SwitchToResult;
            gameEventRelay.OnPlayerDeath += progressViewHandler.SwitchToResult;
            gameEventRelay.OnPlayerDeath += ShowResult;

            stageMover.OnStart();
        }

        void ShowResult()
        {
            int prevProgress = saveFile.GetProgress(gameEventRelay.gameMode);
            int progress = (int)stageMover.Progress;

            progressViewHandler.AnimateResult(progress, progress > prevProgress);

            saveFile.SetProgress(gameEventRelay.gameMode, progress);
        }
    }
}