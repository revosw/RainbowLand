using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;


    private TypeWriterEffect typeWriterEffect;

    public bool IsOpen { get; private set; } //Only dialogueUI can set, but outsiders can GET.

    private void Start()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
        Destroy(dialogueObject);
    }

    //Steps through the list of dialogues using the type writer effect.
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach(string dialogue in dialogueObject.Dialogue)
        {
            yield return typeWriterEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Keyboard.current[Key.E].wasPressedThisFrame);            
        }
        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
