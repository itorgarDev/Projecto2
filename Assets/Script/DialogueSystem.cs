using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI del diálogo")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject dialogueName;
    public TMP_Text npcName;

    [Header("Tipo de personaje")]
    public int characterType = 0;
    public bool isImportant;

    [Header("Nombre del NPC")]
    [TextArea]
    public string name;

    [Header("Mensajes del NPC")]
    [TextArea]
    public string[] message;

    

    private int index = 0;

    public bool IsDialogueActive => dialoguePanel.activeSelf;
    public bool IsNpcNameActive => dialogueName.activeSelf;

    public void StartDialogue()
    {
        npcName.text = name;
        if (message.Length == 0) return;

        index = 0;
        dialoguePanel.SetActive(true);
        dialogueName.SetActive(true);

        // Asignamos el color del texto segun el tipo de MaoQiu
        Color textColor= Color.white;
        switch (characterType)
        {
            case 4:
                textColor = Color.yellow;
                break;
            case 1:
                textColor = Color.green;
                break;
            case 2:
                textColor = Color.cyan;
                break;
            case 3:
                textColor = Color.magenta; 
                break;
            case 0:
                textColor = Color.white;
                break;
        }

        npcName.color = textColor;
        dialogueText.color = textColor;

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
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&isImportant)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.sfxNoti);
        }


    }
}

// Enum fuera de la clase principal
