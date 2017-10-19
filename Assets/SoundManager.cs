
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    static public SoundManager instance;

	void Start () {
        AkSoundEngine.PostEvent("music_switch", gameObject);
        AkSoundEngine.SetSwitch("music", "jour", gameObject);

        EventManager.StartListening("OnPlayerEnterWater", OnWater);
        EventManager.StartListening("OnPlayerEnterGrass", OnGrass);
        EventManager.StartListening("OnPlayerEnterLeaf", OnLeaf);
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void OnWater() {
        AkSoundEngine.PostEvent("roule_eau", gameObject);
    }

    void OnGrass() {
        AkSoundEngine.PostEvent("roule_herbe", gameObject);
    }

    void OnLeaf() {
        AkSoundEngine.PostEvent("roule_feuille", gameObject);
    }

    void OnDestroy() {
        EventManager.StopListening("OnPlayerEnterWater", OnWater);
        EventManager.StopListening("OnPlayerEnterGrass", OnGrass);
        EventManager.StopListening("OnPlayerEnterLeaf", OnLeaf);
    }

    void SetVolume(int volume) {
        AkSoundEngine.SetRTPCValue("volume_control_music", volume);
    }

    void SetSFXVolume(int volume) {
        AkSoundEngine.SetRTPCValue("Volume_control", volume);
    }

    public void SetCycle(string state) {
        switch(state) {
            case "day":
                AkSoundEngine.SetSwitch("music", "jour", gameObject);
                break;

            case "night":
                AkSoundEngine.SetSwitch("music", "nuit", gameObject);
                break;

            default:
                AkSoundEngine.SetSwitch("music", "jour", gameObject);
                break;
        }
    }
	
    void Update () {
		
	}
}
