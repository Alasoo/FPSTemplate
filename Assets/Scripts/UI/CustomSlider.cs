using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyUI
{
    public class CustomSlider : MonoBehaviour
    {
        [Header("SLIDER")]
        [SerializeField] private Slider slider;
        [Header("TEXT")]
        [SerializeField] private TMP_Text sliderValue;

        public Slider Slider => slider;
        public TMP_Text SliderValue => sliderValue;




        public void OnValueChange(float value)
        {
            sliderValue.text = value + "%";
        }


    }
}
