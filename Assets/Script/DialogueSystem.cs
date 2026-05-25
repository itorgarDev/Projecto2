using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI del dialogo")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject dialogueName;
    public TMP_Text npcName;

    [Header("Nombre del Npc")]
    [TextArea]
    public string name;

    [Header("Mensaje del Npc")]
    [TextArea]
    public string[] message;

    private int index = 0;

    public bool IsDialogueActive => dialoguePanel.activeSelf;
    public bool IsNpcNameActive => dialogueName.activeSelf;

    [Header("Sonidos Maoqius")]
    private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioClip sonido1;
    [SerializeField] private AudioClip sonido2;
    [SerializeField] private AudioClip sonido3;
    [SerializeField] private AudioClip sonido4;
    [SerializeField] private AudioClip sonido5;
    AudioClip[] systemAudios; 


    public void Start()
    {
          audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        if (sfxGroup != null)
            audioSource.outputAudioMixerGroup = sfxGroup;

        systemAudios = new AudioClip[] { sonido1, sonido2, sonido3, sonido4, sonido5 };

    }

    public void StartDialogue()
    {
        npcName.text = name;
        if (message.Length == 0) return;

        index = 0;
        dialoguePanel.SetActive(true);
        dialogueName.SetActive(true);
        dialogueText.text = message[index];
        SoundSystem();
    }

    public void ContinueDialogue()
    {
        if (!IsNpcNameActive) return;
        if (!IsDialogueActive) return;
        
        index++;

        if (index < message.Length)
        {
            dialogueText.text = message[index];
            SoundSystem();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueName.SetActive(false);
    }


    public void SoundSystem()
    {
        if (systemAudios == null || systemAudios.Length == 0)
            return;

        int index = Random.Range(0, systemAudios.Length);
        audioSource.PlayOneShot(systemAudios[index],0.25f);

    }

}
