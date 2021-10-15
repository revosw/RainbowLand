using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera playerFollowCamera;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        playerFollowCamera.gameObject.SetActive(false);
        virtualCamera.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        playerFollowCamera.gameObject.SetActive(true);
        virtualCamera.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
