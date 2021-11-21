using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerShooter : MonoBehaviour
{
    public float range;
    private float distToPlayer;
    private float shootCD = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (distToPlayer <= range) //check if player is in range to be shot at.
        {
            if (Time.time > shootCD)
            {
                shootCD = Time.time + 5f;
                Shoot();
                Shoot();
                Shoot();
                Shoot();
                Shoot();
            }
        }
    }

    public GameObject bullet; //bullet prefab.
    public Transform shootFrom; //shoot from this point.
    void Shoot()
    {
        Vector2 shootFromVector = new Vector2(shootFrom.position.x, shootFrom.position.y);
        GameObject newBullet = Instantiate(bullet, shootFromVector, Quaternion.identity);

        Vector3 targetDir = Quaternion.AngleAxis(Random.Range(-45.0f, 45.0f), Vector3.forward) * transform.up;
        newBullet.GetComponent<Rigidbody2D>().velocity = targetDir * 15;

    }
}
