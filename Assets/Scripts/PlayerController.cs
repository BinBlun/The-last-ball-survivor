using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public GameObject powerupGunIndicator;
    public bool hasPowerup = false;
    public bool hasPowerupGun = false;
    public GameObject bulletPrefab;

    Vector3 direction;
    Quaternion rotGoal;

    float rotationY = 90.0f; // Rotate 90 degrees around the Y-axis
    float rotationZ = 90.0f; // Rotate 90 degrees around the Z-axis

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * speed * verticalInput) ;

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupGunIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (hasPowerupGun == true)
        {
            CreateBullet();
        }      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("PowerupGun"))
        {
            hasPowerupGun = true;
            powerupGunIndicator.gameObject.SetActive(true) ;
            Destroy(other.gameObject);
            StartCoroutine(PowerupGunCountDownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.gameObject.SetActive(false);
        hasPowerup = false;
    }

    IEnumerator PowerupGunCountDownRoutine()
    {
        yield return new WaitForSeconds(10);
        powerupGunIndicator.gameObject.SetActive(false);
        hasPowerupGun = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);

            Debug.Log("Collided with: " + collision.gameObject.name + " with power up set to " + hasPowerup);
        } 
    }

    private void CreateBullet()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction = (transform.position - focalPoint.transform.forward * -1);

            // Create Quaternions for the rotations
            Quaternion rotationYQuaternion = Quaternion.Euler(0, rotationY, 0); // Y-axis rotation
            Quaternion rotationZQuaternion = Quaternion.Euler(0, 0, rotationZ); // Z-axis rotation

            Quaternion finalRotation = rotationYQuaternion * rotationZQuaternion;

            //Launch a project titlte form player
            Instantiate(bulletPrefab, direction, finalRotation);
        }
    }
}
