using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI del dialogo")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Mensaje del Npc")]
    [TextArea]
    public string[] message;

    private int index = 0;

    public bool IsDialogueActive => dialoguePanel.activeSelf;

    public void StartDialogue()
    {
        if (message.Length == 0) return;

        index = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = message[index];
    }

    public void ContinueDialogue()
    {
        if (!IsDialogueActive) return;

        index++;

        if (index < message.Length)
        {
            dialogueText.text = message[index];
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }


}
