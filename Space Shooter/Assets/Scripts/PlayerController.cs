using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    public SimpleTouchPad touchPad;
    public SimpleTouchAreaButton areaButton;

    private float nextFire;

    private Rigidbody rb;
    private AudioSource audioSource;
    private Quaternion calibrationQuaternion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        CalibrateAccelerometer();
    }

    private void Update()
    {
        if (areaButton.CanFire() && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        //Vector3 acceleration = FixAcceleration(Input.acceleration);
        //rb.velocity = new Vector3(acceleration.x, 0.0f, acceleration.y) * speed;

        Vector2 direction = touchPad.GetDirection();
        rb.velocity = new Vector3(direction.x, 0.0f, direction.y) * speed;

        rb.position = new Vector3
            (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapShot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapShot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    Vector3 FixAcceleration(Vector3 acceleration)
    {
        return calibrationQuaternion * acceleration;
    }
}