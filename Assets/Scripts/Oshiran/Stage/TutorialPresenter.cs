using VContainer;
using VContainer.Unity;

namespace Oshiran.Stage
{
    public class TutorialPresenter : IStartable
    {
        readonly TutorialSequencer tutorialSequencer;

        [Inject]
        public TutorialPresenter(GameEventRelay gameEventRelay, TutorialSequencer tutorialSequencer)
        {
            this.tutorialSequencer = tutorialSequencer;

            gameEventRelay.OnGameRestart += () =>
            {
                if (tutorialSequencer.enabled)
                {
                    tutorialSequencer.Cancel();
                    tutorialSequencer.Restart();
                }
            };

            gameEventRelay.OnGameClear += () =>
            {
                if (tutorialSequencer.enabled)
                {
                    tutorialSequencer.Cancel();
                }
            };
        }

        public void Start()
        {
            if (tutorialSequencer.enabled)
            {
                tutorialSequencer.Restart();
            }
        }
    }
}