using MyUI;
using SaveLoadSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MenuStateMachineSystem
{
    public class AudioMenuState : MenuState
    {
        [Header("AUDIO MIXER")]
        [SerializeField] private AudioMixer audioMixer;
        [Space]
        [SerializeField] private CustomSlider MasterSlider;


        public override void Init()
        {
            base.Init();
            SaveLoad.LoadAudio(out int _master);

            OnMasterSliderChange(_master);
            MasterSlider.Slider.onValueChanged.RemoveAllListeners();
            MasterSlider.Slider.onValueChanged.AddListener(OnMasterSliderChange);
        }


        public void OnMasterSliderChange(float value)
        {
            MasterSlider.Slider.value = value;
            UpdateVolume(MasterSlider, SaveLoad.MASTER_VOLUME);
            SaveAudio();
        }


        private void SaveAudio()
        {
            SaveLoad.SaveAudio((int)MasterSlider.Slider.value);
        }

        private void UpdateVolume(CustomSlider customSlider, string playerPrefsKey)
        {
            float linearVolume = customSlider.Slider.value / 100f;
            float dB;

            if (linearVolume > 0.001f)
            {
                dB = 20f * Mathf.Log10(linearVolume);
            }
            else
            {
                dB = -80f;
            }
            customSlider.SliderValue.text = $"{customSlider.Slider.value}%";
            audioMixer.SetFloat(playerPrefsKey, dB);
        }

    }
}
