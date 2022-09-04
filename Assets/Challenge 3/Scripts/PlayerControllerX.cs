using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float yBound = 11.5f;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip groundSound;
    public AudioClip explodeSound;
    public bool isLowEnough;
    public bool touchedGround;


    // Start is called before the first frame update
    void Start()
    {
        isLowEnough = true;
        touchedGround = false;
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && isLowEnough && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
        //if the ballon is around a specific area in the y region the player shouldnt apply further force
        if (transform.position.y < yBound)
        {
            isLowEnough = true;
        }
        else
        {
            isLowEnough = false;
        }
        //check if the ballon is on the ground and apply a bounce
        if(touchedGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * 150);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        //if player collides with the ground
        else if(other.gameObject.CompareTag("Ground"))
        {
            touchedGround = true;
            playerAudio.PlayOneShot(groundSound, 1.0f);
        }

    }
    private void OnCollisionExit(Collision other)
    {
        //check if the player is done colliding with the ground
        if(other.gameObject.CompareTag("Ground"))
        {
            touchedGround = false;
        }
    } 

}
