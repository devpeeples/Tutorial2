using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public float jump;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    
    public GameObject WinTextObject;
    public GameObject LoseTextObject;
    
    private int scoreValue;
    private int livesValue;

    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    public Animator animator;
    private bool facingRight = true;
    
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        scoreValue = 0;

        rd2d = GetComponent<Rigidbody2D>();
        livesValue = 3;

        SetCountText();
        WinTextObject.SetActive(false);

        SetCountText();
        LoseTextObject.SetActive(false);

        musicSource.clip = musicClipOne;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    void SetCountText()
    {
        scoreText.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 4)   
        {
            livesValue = 3;
            transform.position = new Vector2(55.0f, 0.2f);
        }
        
        if (scoreValue == 8)
        {
            WinTextObject.SetActive(true);
            speed = 0;
            jump = 0;

            musicSource.clip = musicClipOne;
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
        }

        livesText.text = "Lives: " + livesValue.ToString();
        if (livesValue == 0)
        {
            LoseTextObject.SetActive(true);
            speed = 0;
            jump = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (hozMovement == 0) 
        {
            anim.SetBool("IsRunning", false);
        }

        else
        {
            anim.SetBool("IsRunning", true);
        }

        if (isOnGround == true)
        {
            anim.SetBool("IsJumping", false);
        }

        else
        {
            anim.SetBool("IsJumping", true);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            Destroy(collision.collider.gameObject);

            SetCountText();
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue = livesValue - 1;
            Destroy(collision.collider.gameObject);

            SetCountText();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
            }
        }
    }
    void Flip()
        {
            facingRight = !facingRight;
            Vector2 Scaler = transform.localScale;
            Scaler.x = Scaler.x * -1;
            transform.localScale = Scaler;
        }

}
