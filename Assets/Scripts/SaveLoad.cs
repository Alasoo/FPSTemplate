using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaveLoadSystem
{
    public static class SaveLoad
    {
        #region AUDIO
        //VAMOS A GUARDAR LOS VALORES DEL AUDIO EN VALORES DEL SLIDER, HAY QUE HACER LA CONVERSION A DB
        public const string MASTER_VOLUME = "MasterVolume";
        public const string INTERFACE_VOLUME = "InterfaceVolume";
        public const string MUSIC_VOLUME = "MusicVolume";
        public const string EFFECTS_VOLUME = "EffectsVolume";


        public static void SaveAudio(int _master, int _interface, int _music, int _effects)
        {
            PlayerPrefs.SetInt(MASTER_VOLUME, _master);
            PlayerPrefs.SetInt(INTERFACE_VOLUME, _interface);
            PlayerPrefs.SetInt(MUSIC_VOLUME, _music);
            PlayerPrefs.SetInt(EFFECTS_VOLUME, _effects);
        }

        public static void LoadAudio(out int _master, out int _interface, out int _music, out int _effects)
        {
            _master = PlayerPrefs.GetInt(MASTER_VOLUME, 50);
            _interface = PlayerPrefs.GetInt(INTERFACE_VOLUME, 50);
            _music = PlayerPrefs.GetInt(MUSIC_VOLUME, 50);
            _effects = PlayerPrefs.GetInt(EFFECTS_VOLUME, 50);
        }
        #endregion


    }
}
