using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Oshiran.UI
{
    public class OnaraMetorView : MonoBehaviour
    {
        [SerializeField] Image onaraImage;

        public void SetOnara(float amount, float capacity)
        {
            DOTween.To(() => onaraImage.fillAmount,
                    x => onaraImage.fillAmount = x,
                    Mathf.InverseLerp(0f, capacity, amount),
                    0.4f)
                .Play();
        }
    }
}