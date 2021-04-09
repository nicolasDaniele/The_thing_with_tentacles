using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float backSpeed = 5f;
    public float angularVel = 60f;
    public float dieAnimationInvoke = 1.7f;
    float horiz; 
    public float vert;
    public bool isAttacking;
    private Animator anim;
    public GameController game;
    public Camera mainCam;
    public Camera endCam;
    public Light dirLight;
    public ParticleSystem particles;

    private void Awake()
    {
        endCam.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        // Forward and back movement 
        if (vert > 0.01)
        {
            transform.Translate(Vector3.forward * vert * forwardSpeed * Time.deltaTime);
        }
        else if (vert < 0.01)
        {
            transform.Translate(Vector3.forward * vert * backSpeed * Time.deltaTime);
        }

        // Rotation
        transform.Rotate(0, horiz * angularVel * Time.deltaTime, 0);

        // Animations
        anim.SetFloat("Speed", vert); // Walk

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack"); // Atacar
            isAttacking = true;
            Invoke("SetAttackToFalse", 1.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Morir, morir, morir x_x
        if (collision.gameObject.tag == "Car")
        {
            collision.gameObject.transform.position += collision.gameObject.transform.forward * 20;
            dirLight.transform.eulerAngles += new Vector3(0, 115, 0);
            mainCam.enabled = false;
            endCam.enabled = true;
            anim.SetTrigger("Die");
            anim.SetBool("Dead", true);
            Invoke("DisableAnimator", dieAnimationInvoke);
            game.state = GameController.GameStates.END;
        }

        // Particle system plays when a human is absorbed 
        if (collision.gameObject.tag == "Human" && isAttacking)
        {
            particles.Play();
        }
    }

    private void DisableAnimator()
    {
        anim.enabled = false;
    }

    private void SetAttackToFalse()
    {
        isAttacking = false;
    }
}
