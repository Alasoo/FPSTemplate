using System.Collections.Generic;
using MyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuStateMachineSystem
{
    public class VisualMenuState : MenuState
    {
        [Header("BUTTON")]
        [SerializeField] private Button videoButton;  //CustomButton
        [Header("DROPDOWN")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;

        private Resolution[] resolutions;

        public override void Init()
        {
            base.Init();
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                if (options.Contains(option)) continue;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            SetQuality(0);
        }


        public void SetQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }
        public void SetResolution(int index)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public override void Enter()
        {
            base.Enter();
            //videoButton.Select(true);
        }
        public override void Exit()
        {
            base.Exit();
            //videoButton.Select(false);
        }
    }
}