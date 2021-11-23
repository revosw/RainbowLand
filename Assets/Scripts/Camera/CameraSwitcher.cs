using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera camToActivate;

    private void OnTriggerEnter2D(Collider2D other) {
        CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        camToActivate.gameObject.SetActive(true);
    }
}
