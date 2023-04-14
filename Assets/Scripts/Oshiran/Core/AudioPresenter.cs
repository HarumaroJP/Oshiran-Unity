using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Oshiran.Core
{
    public class AudioPresenter : IInitializable
    {
        readonly SaveFile saveFile;

        [Inject]
        public AudioPresenter(SaveFile saveFile, SettingView settingView)
        {
            this.saveFile = saveFile;

            settingView.GetVolume += saveFile.GetVolume;
            settingView.OnSettingViewClosed += SaveVolume;

            settingView.bgmSlider.onValueChanged.AddListener(v => AudioSystem.Instance.ChangeVolume(v, AudioSystem.Instance.SeVolume));
            settingView.seSlider.onValueChanged.AddListener(v => AudioSystem.Instance.ChangeVolume(AudioSystem.Instance.BgmVolume, v));
        }

        void SaveVolume()
        {
            saveFile.SaveVolume(AudioSystem.Instance.BgmVolume, AudioSystem.Instance.SeVolume);
        }

        public void Initialize() { }
    }
}