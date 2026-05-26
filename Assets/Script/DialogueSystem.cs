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

   


    public void Start()
    {
        
    }

    public void StartDialogue()
    {
        npcName.text = name;
        if (message.Length == 0) return;

        index = 0;
        dialoguePanel.SetActive(true);
        dialogueName.SetActive(true);
        dialogueText.text = message[index];
        SoundController.Instance.PlayRandomMQ();
    }

    public void ContinueDialogue()
    {
        if (!IsNpcNameActive) return;
        if (!IsDialogueActive) return;
        
        index++;

        if (index < message.Length)
        {
            dialogueText.text = message[index];
            SoundController.Instance.PlayRandomMQ();
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

}
