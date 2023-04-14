using DG.Tweening;
using Oshiran.Stage;
using Oshiran.UI;
using VContainer;
using VContainer.Unity;

namespace Oshiran.Module
{
    public class PlayerSetup : IStartable
    {
        readonly PlayerController playerController;
        readonly AudioClipPlayer audioClipPlayer;
        readonly OnaraMetorView onaraMetorView;
        readonly StageMover stageMover;

        [Inject]
        public PlayerSetup(PlayerController playerController, AudioClipPlayer audioClipPlayer, OnaraMetorView onaraMetorView, StageMover stageMover)
        {
            this.playerController = playerController;
            this.audioClipPlayer = audioClipPlayer;
            this.onaraMetorView = onaraMetorView;
            this.stageMover = stageMover;
        }

        public void Start()
        {
            PlayerStatus playerStatus = playerController.GetPlayerStatus();
            playerStatus.OnSetOnara += onaraMetorView.SetOnara;

            audioClipPlayer.PlayBGM();

            stageMover.SetTweenSpeed(playerController.GetPlayerSpec().runSpeed);

            playerController.Setup();
        }
    }
}