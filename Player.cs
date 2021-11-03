using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour // Main script
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform GroundCheck;
    bool isGrounded;
    Animator anim;
    int curHp;
    int maxHp = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    bool CanTP = true;
    public bool inWater = false;
    bool isClimbing = false;
    int coins = 0;
    bool canHit = true;
    public GameObject blueGem, greenGem;
    int gemCount = 0;
    float hitTimer = 0f;
    public Image PlayerCountDown;
    float insideTimer = -1f;
    public float insideTimerUp = 30;
    public Image insideCountDown;
    public Inventory inventory;
    public SoundEffector soundEffector;
    public Joystick joystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    void Update() //Animation, Jump
    {
        if (inWater && !isClimbing) //Animation of player in water
        {
            anim.SetInteger("State", 4);
            isGrounded = false;
            if (joystick.Horizontal >= 0.3f || joystick.Horizontal >= -0.3f)
            {
                Flip();
            }
            CheckGround();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Jump
                rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }
        else // Animation of player idle and walk
        {
            CheckGround();
            if (joystick.Horizontal < 0.3f && joystick.Horizontal > -0.3f && (isGrounded) && !isClimbing) // Animation of player idle
            {
                anim.SetInteger("State", 1);
            }
            else // Animation of player walk
            {
                Flip();
                if (isGrounded && !isClimbing)
                {
                    anim.SetInteger("State", 2);
                }
            }
        }

        if (insideTimer >= 0f) // Timer
        {
            insideTimer += Time.deltaTime;
            if (insideTimer >= insideTimerUp)
            {
                insideTimer = 0;
                RecountHp(-1);
            }
            else
                insideCountDown.fillAmount = 1 - (insideTimer / insideTimerUp);
        }
    }

    public void Jump() // Player jump
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
    }

    void FixedUpdate() // Player movement
    {
        if (joystick.Horizontal >= 0.1f)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        else if (joystick.Horizontal <= -0.1f)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    void Flip() // Player rotation
    {
        if (joystick.Horizontal >= 0.3f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (joystick.Horizontal <= -0.3f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void CheckGround() // Check: Does the player touch the ground
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing) // Animation of player jump
            anim.SetInteger("State", 3);
    }

    public void RecountHp(int deltaHp) // Recount of Player`s lives
    {
        if (deltaHp < 0 && canHit)
            {
            curHp = curHp + deltaHp;
            soundEffector.PlayHitSound();
            StopCoroutine(OnHit());
            isHit = true;
            StartCoroutine(OnHit());
            } 
        else if (deltaHp >= 1 && curHp < 3)
            {
            curHp = curHp + deltaHp;
            }
        else if (deltaHp >= 1 && curHp >= 3)
            {
            inventory.Add_heart();
            }
        if(curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit() // Contact with the enemy. Change sprite color
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.01f, GetComponent<SpriteRenderer>().color.b + 0.01f);

        if (GetComponent<SpriteRenderer>().color.g == 1f) { 
            StopCoroutine(OnHit());
            canHit = true;

        }
        if (GetComponent<SpriteRenderer>().color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(OnHit());
    }

    void Lose() // Calling a method of another class
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision) // Action of key, door, coin, heart, mushroom, blue gem, green gem, TimerButtonStart and TimerButtonStop
    {
        if (collision.gameObject.tag == "Key") {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();
            soundEffector.PlayKeySound();
        }

        if (collision.gameObject.tag == "Door") {
            if (collision.gameObject.GetComponent<Door>().isOpen && CanTP) {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                CanTP = false;
                StartCoroutine(TPwait());
            }
            else if (key)
            {
                collision.gameObject.GetComponent<Door>().Unlock();
                soundEffector.PlayDoorSound();
            }
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins++;
            soundEffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "Heart" && curHp < 3)
        {
            Destroy(collision.gameObject);
            RecountHp(1);
            soundEffector.PlayHeartSound();
        }
        else if (collision.gameObject.tag == "Heart" && curHp >= 3) {
            Destroy(collision.gameObject);
            inventory.Add_heart();
            soundEffector.PlayHeartSound();
        }

        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
            soundEffector.PlayHitSound();
        }

        if (collision.gameObject.tag == "Blue_Gem")
        {
            Destroy(collision.gameObject); 
            inventory.Add_blue_gem();
            soundEffector.PlayCoinSound();
        }


        if (collision.gameObject.tag == "Green_Gem")
        {
            Destroy(collision.gameObject);
            inventory.Add_green_gem();
            soundEffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "TimerButtonStart")
        {
            insideTimer = 0f;
            soundEffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "TimerButtonStop")
        {
            insideTimer = -1f;
            insideCountDown.fillAmount = 0f;
            soundEffector.PlayCoinSound();
        }
    }

    IEnumerator TPwait() // Coroutine for wait for certain actions
    {
        yield return new WaitForSeconds(1f);
        CanTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision) // Action (Stay) for tags: "ladder" and animation for Ladder, "icy", "lava (dangerous area)" 
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
            {
                anim.SetInteger("State", 6);
            }
            else
            {
                anim.SetInteger("State", 5);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f; 
            }
        }

        if (collision.gameObject.tag == "Lava")
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0f;
                PlayerCountDown.fillAmount = 1f;
                RecountHp(-1);
            }
            else
                PlayerCountDown.fillAmount = 1 - (hitTimer / 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Action (Exit) for tags "ladder", "icy" and "lava (dangerous area)"
    {
        if (collision.gameObject.tag == "Ladder") {
            isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic; 
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 7f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }

        if (collision.gameObject.tag == "Lava")
        {
            hitTimer = 0f;
            PlayerCountDown.fillAmount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Action (Enter) for tags "Spring" and "slowing down"
    {
        if (collision.gameObject.tag == "Spring")
        {
            StartCoroutine(SpringAnim(collision.gameObject.GetComponentInParent<Animator>())); 
        }

        if(collision.gameObject.tag == "QuickSand")
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) //Action (Exit) for tags "slowing down"
    {
        if (collision.gameObject.tag == "QuickSand")
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    IEnumerator SpringAnim(Animator an) // Animation of spring
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);
    }

    IEnumerator NoHit() // Animation of blue gem (invincibility)
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);

        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(9f);
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;

        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
    }

    IEnumerator SpeedBonus() // Animation of green gem (super speed)
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);

        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;

        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);
    }

    void CheckGems(GameObject obj) // location of HUD gems
    {
        if (gemCount == 1)
        {
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        }
        else if(gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.6f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.6f, greenGem.transform.localPosition.z);
        }
    }

    IEnumerator Invis (SpriteRenderer spr, float time) // Smooth disappearance of HUD gems after the end of their duration
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }

    public int GetCoins() // Return coins for the store
    {
        return coins;
    }

    public int GetHp() // Return current Player`s lives
    {
        return curHp;
    }

    public void BlueGem()
    {
        StartCoroutine(NoHit());
    }

    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    }
}
