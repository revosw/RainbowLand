using Player;
using UnityEngine;

namespace Pickups
{
    public class ShootingPickup : Pickup
    {
        public override void DoOnTrigger(Collider2D other)
        {
            other.gameObject.GetComponent<PlayerController>().canShoot = true;

            base.DoOnTrigger(other);
        }
    }
}