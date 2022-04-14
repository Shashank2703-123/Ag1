/* Arnav Update : 09/03/2022 - Shashank pushed code : 07/03/2022
This file is a companion to THIRD EDIT of latest HalfScreenMovement.cs script
All additions / changes here correspond to THIRD EDIT of HalfScreenMovement.cs script
Need to do - speed up / down controls update - DEALT WITH IN THIS FILE
and also DEALT WITH to some extent - in THIRD EDIT of HalfScreenMovement.cs file also
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameConsts;

public class MovementController : MonoBehaviour
{

    Rigidbody rb;
    public float MoveSpeed = 5f;
    public float Velocity = 2000f;

    private float Slope = 0f;
    Vector3 Movement;
    public float HorMovement;


    [SerializeField] private float MaxSpeed;
    [SerializeField] private float MinSpeed; // Arnav added

    public bool stopBreaking;
    public bool stopBoosting;

    private float CarRot = 0;
    [SerializeField] GameObject car;
    public bool ismotion = false;
    public bool isgrounded = false;
    private bool isJumping = false;

    [SerializeField] public GameConsts.ControlType Controller;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        MaxSpeed = 4000f;
        MinSpeed = 0f;
        stopBoosting = true;
        stopBreaking = true;
        // Arnav set default values
        // Both can be varied as per testing and what is found to be best
    }

    public void InitBreak()
    {
        StartCoroutine(BreakCar());
    }
    public void Initboost()
    {
        StartCoroutine(BoostCar());
    }
    // Arnav added break and boost loop calls
    public void InitBreakLoop()
    {
        StartCoroutine(BreakCarLoop());
    }
    public void InitboostLoop()
    {
        StartCoroutine(BoostCarLoop());
    }

    public IEnumerator BreakCar()
    {
        float CurrentSpeed = Velocity;

        if (Velocity > MinSpeed)
        {
            Velocity -= 500f; // Arnav changed
        }
        else
        {
            Velocity = MinSpeed;
        }
        yield return new WaitForSeconds(0.1f); // Arnav modified

        //  Velocity = CurrentSpeed;
    }
    public IEnumerator BoostCar()
    {
        float CurrentSpeed = Velocity;

        if (Velocity < MaxSpeed)
        {
            Velocity += 500f; // Arnav changed
        }
        else
        {
            Velocity = MaxSpeed;
        }
        yield return new WaitForSeconds(0.1f); // Arnav modified

        //  Velocity = CurrentSpeed;
    }
    // Arnav added coroutines for break and boost loops
    public IEnumerator BreakCarLoop()
    {
        yield return new WaitForSeconds(0.5f);

        while (Velocity > MinSpeed)
        {
            // check if boolean to stop breaking is true
            if (!stopBreaking)
            {
                Velocity -= 500f;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }
    }
    public IEnumerator BoostCarLoop()
    {
        yield return new WaitForSeconds(0.5f);

        while (Velocity < MaxSpeed)
        {
            // check if boolean to stop boosting is true
            if (!stopBoosting)
            {
                Velocity += 500f;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }
    }

    public IEnumerator SpeedBoost(float speed)
    {
        float OriginalSpeed = MoveSpeed;
        MoveSpeed += speed;
        yield return new WaitForSeconds(2.6f);

        for (float i = MoveSpeed; i >= OriginalSpeed; i -= 0.5f)
        {
            MoveSpeed = i;
            yield return new WaitForSeconds(0.02f);
        }

        //  MoveSpeed -= 20f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boost")
        {
            StartCoroutine(SpeedBoost(20));
        }

        if (other.tag == "Turn")
        {
            CarRot = other.gameObject.GetComponent<TurnScript>().ObjRot;

            if (other.gameObject.GetComponent<TurnScript>().isSlope)
            {
                //  transform.rotation = Quaternion.Euler(-30, 0, 0);
            }
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Jump()
    {
        if (isgrounded)
        {

            //   rb.velocity = new Vector3(rb.velocity.x, 12, rb.velocity.z);
            rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
            isJumping = true;
            // isgrounded = false;
            Debug.Log("Jump");
        }

    }


    //public IEnumerator FloatReduce()
    //{
    //    for(float i = 0f; i <= 20f; i-= 5f)
    //    {
    //        car.transform.rotation = Quaternion.Euler(0, i, 0);
    //    }

    //    yield return new WaitForSeconds(
    //}

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // rb.AddForce(Vector3.up * 100);

            rb.velocity = new Vector3(rb.velocity.x, 7, rb.velocity.z);
            Debug.Log("Jump");
        }
        //   Debug.Log(Input.acceleration.x);
    }
    private void OnCollisionStay(Collision collision)
    {
        isgrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isgrounded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        isgrounded = true;

        if (isJumping)
        {
            isJumping = false;
        }
        Debug.Log(collision.gameObject.name);

    }
    void FixedUpdate()
    {
        if (Controller == ControlType.Touch)
        {
            if (isgrounded)
            {
                if (!isJumping)
                {
                    Movement = new Vector3(HorMovement, 0, 1);
                    rb.velocity = car.transform.forward * Time.deltaTime * Velocity;
                }
                else return;

            }
            else
            {
                Movement = new Vector3(0, 0, 1);
            }

            if (HorMovement == 1)
            {


                if (car.transform.rotation.y < 35)
                {
                    Quaternion targetrot = Quaternion.Euler(0, 35, 0);
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);
                }
                else
                    return;

                // LeanTween.rotateLocal(gameObject, new Vector3(0, 20, 0), 0.6f);

            }
            else if (HorMovement == 0)
            {
                Quaternion targetrotnul = Quaternion.Euler(0, CarRot, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrotnul, Time.deltaTime * 5f); ;

            }

            else if (HorMovement == -1)
            {
                Quaternion targetrot = Quaternion.Euler(0, -35, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);

            }
            //rb.MovePosition(transform.position + Movement * Time.deltaTime * MoveSpeed);

        }

        else if (Controller == ControlType.Motion)
        {
            if (isgrounded)
            {
                if (!isJumping)
                {
                    Movement = new Vector3(HorMovement, 0, 1);
                    rb.velocity = car.transform.forward * Time.deltaTime * Velocity;
                }
                else return;

            }
            else
            {
                Movement = new Vector3(0, 0, 1);
            }

            if (HorMovement > 0.25f)
            {


                if (car.transform.rotation.y < 35)
                {
                    Quaternion targetrot = Quaternion.Euler(0, 35, 0);
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);
                }
                else
                    return;

                // LeanTween.rotateLocal(gameObject, new Vector3(0, 20, 0), 0.6f);

            }
            else if (HorMovement < 0.25f && HorMovement > -0.25f)
            {
                Quaternion targetrotnul = Quaternion.Euler(0, 0, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrotnul, Time.deltaTime * 5f); ;

            }

            else if (HorMovement < -0.25f)
            {
                Quaternion targetrot = Quaternion.Euler(0, -35, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);

            }
            //  rb.MovePosition(transform.position + Movement * Time.deltaTime * MoveSpeed);
        }

        else if (Controller == ControlType.Swipe)
        {
            if (isgrounded)
            {
                if (!isJumping)
                {
                    Movement = new Vector3(HorMovement, 0, 1);
                    rb.velocity = car.transform.forward * Time.deltaTime * Velocity;
                }
                else return;

            }
            else
            {
                Movement = new Vector3(0, 0, 1);
            }

            if (HorMovement > 0.25f)
            {


                if (car.transform.rotation.y < 35)
                {
                    Quaternion targetrot = Quaternion.Euler(0, 35, 0);
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);
                }
                else
                    return;

                // LeanTween.rotateLocal(gameObject, new Vector3(0, 20, 0), 0.6f);

            }
            else if (HorMovement < 0.25f && HorMovement > -0.25f)
            {
                Quaternion targetrotnul = Quaternion.Euler(0, CarRot, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrotnul, Time.deltaTime * 5f); ;

            }

            else if (HorMovement < -0.25f)
            {
                Quaternion targetrot = Quaternion.Euler(0, -35, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);

            }
            //   rb.MovePosition(transform.position + Movement * Time.deltaTime * MoveSpeed);
        }

        else if (Controller == ControlType.Hybrid)
        {
            if (isgrounded)
            {
                if (!isJumping)
                {
                    Movement = new Vector3(HorMovement, 0, 1);
                    rb.velocity = car.transform.forward * Time.deltaTime * Velocity;
                }
                else return;

            }
            else
            {
                Movement = new Vector3(0, 0, 1);
            }

            // Arnav comment on car sprite rotation - can consider doing this only on curved road ?
            // Or not at all ?

            if (HorMovement == 1)
            {


                if (car.transform.rotation.y < 35)
                {
                    Quaternion targetrot = Quaternion.Euler(Slope, 35, 0);
                    car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);
                }
                else
                    return;

                // LeanTween.rotateLocal(gameObject, new Vector3(0, 20, 0), 0.6f);

            }
            else if (HorMovement == 0)
            {
                Quaternion targetrotnul = Quaternion.Euler(Slope, CarRot, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrotnul, Time.deltaTime * 5f); ;

            }

            else if (HorMovement == -1)
            {
                Quaternion targetrot = Quaternion.Euler(Slope, -35, 0);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetrot, Time.deltaTime * 5f);

            }
            //  rb.MovePosition(transform.position + Movement * Time.deltaTime * MoveSpeed);
        }








    }
}