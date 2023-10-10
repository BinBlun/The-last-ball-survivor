using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAhead : MonoBehaviour
{
    public float speed;
    public Rigidbody bulletRb;
    private GameObject focalPoint;
    private float powerupStrength = 30.0f;
    private float zDestroy = 25.0f;
    private float xDestroy = 25.0f;
    public bool hasPowerupGun;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        hasPowerupGun = GameObject.Find("Player").GetComponent<PlayerController>().hasPowerupGun; 
    }
    // Update is called once per frame
    void Update()
    {
        bulletRb.AddForce(focalPoint.transform.forward * speed);

        if (transform.position.z < -zDestroy || transform.position.z > zDestroy)
        {
            Destroy(gameObject);
        }

        if (transform.position.x < -xDestroy || transform.position.x > xDestroy)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerupGun)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Destroy(gameObject);
            Debug.Log("Collided with: " + collision.gameObject.name + " " + gameObject.name);
        }
    }
}
