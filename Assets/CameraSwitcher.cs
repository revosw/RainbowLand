using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera camToActivate;

    private void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.SetActive(false);
        camToActivate.gameObject.SetActive(true);
    }
}
