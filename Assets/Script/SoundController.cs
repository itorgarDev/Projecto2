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

    [SerializeField] private AudioClip checkpoint;
    [SerializeField] private AudioClip wood;
    [SerializeField] private AudioClip woodDestroyed;
    [SerializeField] private AudioClip stone;
    [SerializeField] private AudioClip stoneDestroyed;
    [SerializeField] private AudioClip health;

    [Space(10)]
    [Header("Sfx Maoqius")]

    [SerializeField] private AudioClip sfxMQ1;
    [SerializeField] private AudioClip sfxMQ2;
    [SerializeField] private AudioClip sfxMQ3;
    [SerializeField] private AudioClip sfxMQ4;
    [SerializeField] private AudioClip sfxMQ5;
    AudioClip[] systemAudios;

    [Space(10)]
    [Header("Sfx Enemy")]

    [SerializeField] private AudioClip cAttack;
    [SerializeField] private AudioClip cDeath;
    [SerializeField] private AudioClip cSteps;
    [SerializeField] private AudioClip cDamage;
    [SerializeField] private AudioClip cExclamation;
    [SerializeField] private AudioClip cLiberation;

    [Space(10)]
    [Header("Sfx Ambience")]

    [SerializeField] private AudioClip ambVillages;
    [SerializeField] private AudioClip ambGrass;
    [SerializeField] private AudioClip ambRiver;
    [SerializeField] private AudioClip ambMountains;

    [Space(10)]
    [Header("Sfx Zhuque")]

    [SerializeField] private AudioClip zBark;
    [SerializeField] private AudioClip zDamage;
    [SerializeField] private AudioClip zFire;
    [SerializeField] private AudioClip zFlying;
    [SerializeField] private AudioClip zRespawn;
    [SerializeField] private AudioClip zExplosion;
    [SerializeField] private AudioClip zEgg;

    [Space(10)]
    [Header("Sfx Xuanwu")]

    [SerializeField] private AudioClip xMelee;
    [SerializeField] private AudioClip xShield;
    [SerializeField] private AudioClip xDialogue;
    [SerializeField] private AudioClip xSnake;
    [SerializeField] private AudioClip xDamage;

    public float globalCooldown = 0.05f;

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
        audioSource.PlayOneShot(systemAudios[index], 0.25f);
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
