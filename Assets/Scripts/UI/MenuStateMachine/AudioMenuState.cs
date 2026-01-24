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
        [SerializeField] private CustomSlider MusicSlider;
        [SerializeField] private CustomSlider EffectsSlider;
        [SerializeField] private CustomSlider InterfaceSlider;

        [Header("BUTTON")]
        [SerializeField] private Button audioButton;    //customButton

        public override void Init()
        {
            base.Init();
            SaveLoad.LoadAudio(out int _master, out int _interface, out int _music, out int _effects);


            OnMasterSliderChange(_master);
            OnInterfaceSliderChange(_interface);
            OnEffectsSliderChange(_effects);
            OnMusicSliderChange(_music);

            MasterSlider.Slider.onValueChanged.AddListener(OnMasterSliderChange);
            InterfaceSlider.Slider.onValueChanged.AddListener(OnInterfaceSliderChange);
            MusicSlider.Slider.onValueChanged.AddListener(OnMusicSliderChange);
            EffectsSlider.Slider.onValueChanged.AddListener(OnEffectsSliderChange);
        }


        public void OnMasterSliderChange(float value)
        {
            MasterSlider.Slider.value = value;
            UpdateVolume(MasterSlider, SaveLoad.MASTER_VOLUME);
            SaveAudio();
        }
        public void OnInterfaceSliderChange(float value)
        {
            InterfaceSlider.Slider.value = value;
            UpdateVolume(InterfaceSlider, SaveLoad.INTERFACE_VOLUME);
            SaveAudio();
        }
        public void OnMusicSliderChange(float value)
        {
            MusicSlider.Slider.value = value;
            UpdateVolume(MusicSlider, SaveLoad.MUSIC_VOLUME);
            SaveAudio();
        }
        public void OnEffectsSliderChange(float value)
        {
            EffectsSlider.Slider.value = value;
            UpdateVolume(EffectsSlider, SaveLoad.EFFECTS_VOLUME);
            SaveAudio();
        }

        private void SaveAudio()
        {
            SaveLoad.SaveAudio((int)MasterSlider.Slider.value, (int)InterfaceSlider.Slider.value, (int)MusicSlider.Slider.value, (int)EffectsSlider.Slider.value);
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



        public override void Enter()
        {
            base.Enter();
            //audioButton.Select(true);
        }
        public override void Exit()
        {
            base.Exit();
            //audioButton.Select(false);
        }

    }
}
