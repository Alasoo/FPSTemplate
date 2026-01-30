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



        public static void SaveAudio(int _master)
        {
            PlayerPrefs.SetInt(MASTER_VOLUME, _master);
        }

        public static void LoadAudio(out int _master)
        {
            _master = PlayerPrefs.GetInt(MASTER_VOLUME, 50);
        }
        #endregion


    }
}
