using Player;
using UnityEngine;

namespace Pickups
{
    public class WallJumpPickup : Pickup
    {
        public override void DoOnTrigger(Collider2D other)
        {
            if (other.CompareTag("player"))
            {
                // Debug.Log();
                other.gameObject.GetComponentInParent<PlayerController>().canWallJump = true;
                other.gameObject.GetComponentInParent<PlayerController>().canWallGrab = true;
            }
            
            base.DoOnTrigger(other);
        }

        public void SetActiveState(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}