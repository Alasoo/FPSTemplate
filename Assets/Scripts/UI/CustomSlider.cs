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


#if UNITY_EDITOR
        void OnValidate()
        {
            if (slider == null)
                slider = GetComponentInChildren<Slider>();

            if (sliderValue == null)
            {
                foreach (Transform child in transform)
                {
                    if (sliderValue == null && child.name == "SliderValue")
                        sliderValue = child.GetComponent<TMP_Text>();
                }
            }

            sliderValue.text = slider.value + "%";
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnValueChange);
        }
#endif
    }
}
