using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    GameObject tentacle;
    Animator anim;
    bool captured;
    bool scared;
    bool isWalking;
    bool isRunning;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public GameObject alien;
    float distance; // Distance between human and alien
    private RaycastHit rcHit;

    // Start is called before the first frame update
    void Start()
    {
        captured = false;
        anim = GetComponent<Animator>();
        alien = GameObject.Find("TheThing");
        float randomTime = Random.Range(10, 21);
        InvokeRepeating("SetWalk", 0, randomTime);
    }

    // Update is called once per frame
    void Update()
    {
        isWalking = anim.GetBool("Walk");
        isRunning = anim.GetBool("Run");

        // If captured, human gets stuck in alien's tentacles
        if (captured)
        {
            transform.position = tentacle.transform.position;
        }

        // If the alien gets too close, human gets scared and run
        distance = Vector3.Distance(alien.transform.position, transform.position);
        if (distance < 10) scared = true;
        else if (distance > 30) scared = false;

        if (scared)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", true);
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
            AvoidCollisions();
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if (isWalking)
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            AvoidCollisions();
        }
    }

    // Check collision with alien's tentacles
    private void OnTriggerEnter(Collider collision)
    {
        bool alienIsAttacking = alien.GetComponent<PlayerController>().isAttacking;
        if (collision.gameObject.tag == "Tentacle" && alienIsAttacking)
        {
            captured = true;
            tentacle = collision.gameObject;
        }
    }

    // Destroy human when the alien absorbs them
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && captured)
        {
            GameController.humans--;
            Destroy(gameObject);
        }
    }

    // Each time this function is called
    // there's 50% posibilities for the human to walk
    // and 50% to stand still
    void SetWalk()
    {
        float rand = Random.Range(1, 11);

        if (rand > 5 && !isRunning && !isWalking)
        {
            anim.SetBool("Walk", true);
        }
        else if (rand < 5 && !isRunning && isWalking)
        {
            anim.SetBool("Walk", false);
        }
    }

    void AvoidCollisions()
    {
        // Check if they're going to hit something
        if (Physics.Raycast(transform.position, transform.forward, out rcHit, 10))
        {
            if (rcHit.collider.gameObject.tag == "Player" ||
                rcHit.collider.gameObject.tag == "Wall" ||
                rcHit.collider.gameObject.tag == "Car" ||
                rcHit.collider.gameObject.tag == "Human")
            {
                // Rotate in random direction
                transform.Rotate(new Vector3(0, Random.Range(-90, 91), 0));
            }
        }
    }
}
