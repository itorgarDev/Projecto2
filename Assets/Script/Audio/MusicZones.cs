using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZones : MonoBehaviour
{
    public int zoneToMusic;

    public void OnTriggerEnter()
    {
        switch(zoneToMusic)
        {
            case 0: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 1: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 2: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 3: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 4: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 5: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 6: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            case 7: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
            default: return;
        }
    }
}
