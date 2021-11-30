using Player;
using UnityEngine;

namespace Pickups
{
    public class ShootingPickup : Pickup
    {

        public GameObject canvas;
        public override void DoOnTrigger(Collider2D other)
        {
            if (other.CompareTag("player"))
            {
                other.gameObject.GetComponent<PlayerController>().canShoot = true;
                canvas.transform.position = transform.position;
                canvas.SetActive(true);                
            }


            base.DoOnTrigger(other);
        }
        
        public void SetActiveState(bool state)
        {
            var _player = GameObject.FindWithTag("player").transform;
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y+10, 1 );
            gameObject.SetActive(state);
        }
    }
}