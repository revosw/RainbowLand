using Player;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player") && collision.TryGetComponent(out PlayerController player)) {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player") && collision.TryGetComponent(out PlayerController player)) {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null; 
            }
        }
    }
    
        public void Interact(PlayerController player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
