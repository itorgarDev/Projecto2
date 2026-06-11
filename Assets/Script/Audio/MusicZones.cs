using UnityEngine;

public class MusicZones : MonoBehaviour
{
    public int zoneToMusic;
    public int activeZoneCount = 0; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        activeZoneCount++;

        
        if (activeZoneCount >= 1)
        {
            switch (zoneToMusic)
            {
                case 0: MusicController.Instance.PlayMusic(MusicController.Instance.tutoZone); break;
                case 1: MusicController.Instance.PlayMusic(MusicController.Instance.tutoVillage); break;
                case 2: MusicController.Instance.PlayMusic(MusicController.Instance.zone1); break;
                case 3: MusicController.Instance.PlayMusic(MusicController.Instance.village1); break;
                case 4: MusicController.Instance.PlayMusic(MusicController.Instance.village2); break;
                case 5: MusicController.Instance.PlayMusic(MusicController.Instance.boss1); break;
                case 6: MusicController.Instance.PlayMusic(MusicController.Instance.boss2); break;
                default: return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        activeZoneCount = Mathf.Max(0, activeZoneCount - 1);

        // Solo parar si el jugador ha salido de todas las zonas
        if (activeZoneCount == 0)
        {
            MusicController.Instance.StopMusicSmooth();
        }
    }
}
