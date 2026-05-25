using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    [Header("AudioSources")]

    private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Sfx Barun")]

    [SerializeField] private AudioClip walkSfx;
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip dashSfx;
    [SerializeField] private AudioClip damageSfx;
    [SerializeField] private AudioClip bridge;
    [SerializeField] private AudioClip deathSfx;

    [Header("Sfx Other")]

    [SerializeField] private AudioClip checkpoint;
    [SerializeField] private AudioClip wood;
    [SerializeField] private AudioClip woodDestroyed;
    [SerializeField] private AudioClip stone;
    [SerializeField] private AudioClip stoneDestroyed;
    [SerializeField] private AudioClip health;

    [Header("Sfx Maoqius")]

    [SerializeField] private AudioClip sfxMQ1;
    [SerializeField] private AudioClip sfxMQ2;
    [SerializeField] private AudioClip sfxMQ3;
    [SerializeField] private AudioClip sfxMQ4;
    [SerializeField] private AudioClip sfxMQ5;
    AudioClip[] systemAudios;

    [Header("Sfx Enemy")]

    [SerializeField] private AudioClip cAttack;
    [SerializeField] private AudioClip cDeath;
    [SerializeField] private AudioClip cSteps;
    [SerializeField] private AudioClip cDamage;
    [SerializeField] private AudioClip cExclamation;
    [SerializeField] private AudioClip cLiberation;

    [Header("Sfx Ambience")]

    [SerializeField] private AudioClip ambVillages;
    [SerializeField] private AudioClip ambGrass;
    [SerializeField] private AudioClip ambRiver;
    [SerializeField] private AudioClip ambMountains;

    [Header("Sfx Zhuque")]

    [SerializeField] private AudioClip zBark;
    [SerializeField] private AudioClip zDamage;
    [SerializeField] private AudioClip zFire;
    [SerializeField] private AudioClip zFlying;
    [SerializeField] private AudioClip zRespawn;
    [SerializeField] private AudioClip zExplosion;
    [SerializeField] private AudioClip zEgg;

    [Header("Sfx Xuanwu")]

    [SerializeField] private AudioClip xMelee;
    [SerializeField] private AudioClip xShield;
    [SerializeField] private AudioClip xDialogue;
    [SerializeField] private AudioClip xSnake;
    [SerializeField] private AudioClip xDamage;

    [Header("Sfx Xuanwu")]

    [SerializeField] private AudioClip xMelee;
    [SerializeField] private AudioClip xShield;
    [SerializeField] private AudioClip xDialogue;
    [SerializeField] private AudioClip xSnake;
    [SerializeField] private AudioClip xDamage;

    [Header("BSO")]

    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip tutorial;
    [SerializeField] private AudioClip mainVillage;
    [SerializeField] private AudioClip river;
    [SerializeField] private AudioClip village1;
    [SerializeField] private AudioClip mountain;
    [SerializeField] private AudioClip village2;
    [SerializeField] private AudioClip boss1;
    [SerializeField] private AudioClip boss2;

    public float globalCooldown = 0.05f;

    private Dictionary<AudioClip, float> lastPlayTime = new Dictionary<AudioClip, float>();

    void Awake()
    {
        Instance = this;
    }


}
