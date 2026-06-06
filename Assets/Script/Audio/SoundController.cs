using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Audio;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance;



    [Header("AudioSources")]

    private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Space(10)]
    [Header("Sfx Barun")]

    public AudioClip walkSfx;
    public AudioClip attackSfx;
    public AudioClip dashSfx;
    public AudioClip damageSfx;
    public AudioClip bridge;
    public AudioClip item;
    public AudioClip deathSfx;

    [Space(10)]
    [Header("Sfx Other")]

    public AudioClip checkpoint;
    public AudioClip wood;
    public AudioClip woodDestroyed;
    public AudioClip stone;
    public AudioClip stoneDestroyed;
    public AudioClip health;

    [Space(10)]
    [Header("Sfx Maoqius")]

    public AudioClip sfxMQ1;
    public AudioClip sfxMQ2;
    public AudioClip sfxMQ3;
    public AudioClip sfxMQ4;
    public AudioClip sfxMQ5;
    AudioClip[] systemAudios;

    [Space(10)]
    [Header("Sfx Enemy")]

    public AudioClip cAttack;
    public AudioClip cDeath;
    public AudioClip cSteps;
    public AudioClip cDamage;
    public AudioClip cExclamation;
    public AudioClip cLiberation;

    [Space(10)]
    [Header("Sfx Ambience")]

    public AudioClip ambVillages;
    public AudioClip ambGrass;
    public AudioClip ambRiver;
    public AudioClip ambMountains;

    [Space(10)]
    [Header("Sfx Zhuque")]

    public AudioClip zBark;
    public AudioClip zDamage;
    public AudioClip zFire;
    public AudioClip zFlying;
    public AudioClip zRespawn;
    public AudioClip zExplosion;
    public AudioClip zEgg;

    [Space(10)]
    [Header("Sfx Xuanwu")]

    public AudioClip xMelee;
    public AudioClip xShield;
    public AudioClip xDialogue;
    public AudioClip xSnake;
    public AudioClip xDamage;

    [Space(10)]
    [Header("Sfx Ambience")]

    public AudioClip zone1;
    public AudioClip zone2;
    public AudioClip zone3;

    public float globalCooldown = 1f;

    private Dictionary<AudioClip, float> lastPlayTime = new Dictionary<AudioClip, float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (sfxGroup != null)
            audioSource.outputAudioMixerGroup = sfxGroup;


        List<AudioClip> valid = new List<AudioClip>();
        if (sfxMQ1) valid.Add(sfxMQ1);
        if (sfxMQ2) valid.Add(sfxMQ2);
        if (sfxMQ3) valid.Add(sfxMQ3);
        if (sfxMQ4) valid.Add(sfxMQ4);
        if (sfxMQ5) valid.Add(sfxMQ5);
        systemAudios = valid.ToArray();

        AudioListener.pause = false;
        AudioListener.volume = 1f;

    }

    public void Start()
    {
        int scenetoSfx = SceneManager.GetActiveScene().buildIndex;
        switch (scenetoSfx)
        {
            case 5: PlayAmbience(zone1); break;
            case 6: PlayAmbience(zone2); break;
            case 7: PlayAmbience(zone3); break;
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioListener.pause = false;
        AudioListener.volume = 1f;

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (sfxGroup != null)
            audioSource.outputAudioMixerGroup = sfxGroup;

        Debug.Log($"[SoundController] Audio reactivado en escena: {scene.name}");
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        float now = Time.time;

        if (!lastPlayTime.ContainsKey(clip))
            lastPlayTime[clip] = -999f;

        if (now - lastPlayTime[clip] < globalCooldown)
            return;

        audioSource.PlayOneShot(clip,0.35f);
        lastPlayTime[clip] = now;
    }

    public void PlayRandomMQ()
    {
        if (systemAudios.Length == 0) return;

        int index = Random.Range(0, systemAudios.Length);
        audioSource.PlayOneShot(systemAudios[index], 0.15f);
    }

    public void PlayAmbience(AudioClip clip)
    {
        if (clip == null) return;

        // Configurar el AudioSource para ambiente
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

}
