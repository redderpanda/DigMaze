using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 1.0f;
    public float digRate = 0;
    public LayerMask ToDig;

    public Sprite exitSprite;
    public Sprite downDigSprite;
    public Sprite leftRightDigSprite;
    public ParticleSystem dirtParticles;
    public Canvas myCanvas;
    public Canvas myCanvas2;
    public Text diamondsCollectedText;
    public Text outcomeText;
    public GameObject gameController;

    public float timeToDig = 0;
    public int diamondsCollected = 0;
    public int totalDiamonds;


    Transform firePoint;

    //figure out what player is facing and return it
    private Vector2 getDirection()
    {
        Vector2 direction = new Vector2(0, 0);
        if (Input.GetAxis("Horizontal") < 0)
        {
            direction += Vector2.left;
            return direction;
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            direction += Vector2.right;
            return direction;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            direction += Vector2.down;
            return direction;
        }
        if(Input.GetAxis("Vertical") > 0)
        {
            direction += Vector2.up;
            return direction;
        }
        return direction;

    }

    //function that allows the player to dig.  Checks whether or not it is near an object that can be dug with RayCast2D.
    //Uses the direction to apply the right sprite.
    private void Dig()
    {
        Vector2 direction = getDirection();
        
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, direction, 1f, ToDig);
        Debug.DrawRay(firePointPosition, direction, Color.red);
        if (hit.collider != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            Instantiate(dirtParticles, this.transform);
            audio.Play();
            //Debug.DrawLine(firePointPosition, hit.point, Color.cyan);
            //Debug.Log("We Hit " + hit.collider.name);
            if (hit.collider.tag == "Bomb")
            {
                StartCoroutine(BombExplosion(hit));
                
            }

            if (direction == Vector2.left || direction == Vector2.right)
            {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = leftRightDigSprite;
            }
            if (direction == Vector2.down || direction == Vector2.up)
            {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = downDigSprite;
            }
        
            hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //function that is responsible for the effects for when the player digs up a bomb
    private IEnumerator BombExplosion(RaycastHit2D bomb)
    {
        gameController.GetComponent<AudioSource>().mute = true;
        AudioSource audio = bomb.collider.gameObject.GetComponent<AudioSource>();
        audio.Play();
        yield return new WaitForSeconds(2f);
        Instantiate(bomb.collider.gameObject.GetComponent<BombController>().explosionParticles, bomb.collider.gameObject.transform);
        Instantiate(bomb.collider.gameObject.GetComponent<BombController>().smokeParticles, this.transform);
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(3f);
        myCanvas.enabled = true;
        myCanvas2.enabled = false;
        myCanvas.transform.GetChild(1).gameObject.SetActive(true);
        outcomeText.text = "You Lose";

        

    }

    private void Awake()
    {
        firePoint = this.transform;
        diamondsCollectedText.text = "x " + diamondsCollected.ToString() + "/" + totalDiamonds.ToString();


    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exit trigger
        if(collision.gameObject.layer == 10)
        {
            if (totalDiamonds == diamondsCollected)
            {
                gameController.GetComponent<AudioSource>().mute = true;
                Debug.Log("you win");
                myCanvas.enabled = true;
                myCanvas2.enabled = false;
                myCanvas.transform.GetChild(0).gameObject.SetActive(true);
                outcomeText.text = "You Win";
                AudioSource audio = collision.gameObject.GetComponent<AudioSource>();
                audio.Play();
            }
        }

        //oneAway trigger
        if (collision.gameObject.layer == 13)
        {
            Debug.Log("bomb REALLY near");
            AudioSource audio = collision.gameObject.GetComponent<AudioSource>();
            audio.Play();

        }
        //twoAway trigger
        if (collision.gameObject.layer == 12)
        {
            Debug.Log("bomb near");
            AudioSource audio = collision.gameObject.GetComponent<AudioSource>();
            audio.Play();
        }
        //diamond trigger
        if(collision.gameObject.layer == 14)
        {
            Debug.Log("found gem");
            diamondsCollected++;
            diamondsCollectedText.text = "x " + diamondsCollected.ToString() + "/" + totalDiamonds.ToString();
            Destroy(collision.gameObject.transform.GetChild(0).gameObject);
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            AudioSource audio = collision.gameObject.GetComponent<AudioSource>();
            audio.Play();

        }


    }



    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetAxis("Vertical") > 0)
        {
            input += Vector3.up;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            input += Vector3.down;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            input += Vector3.right;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            input += Vector3.left;
        }

        input.Normalize();
        transform.position += input * speed * Time.deltaTime;
        if (digRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
                Dig();
            else
            {
                if(Input.GetButton("Fire1") && Time.time > timeToDig)
                {
                    timeToDig = Time.time + 1 / digRate;
                    Dig();
                }
            }
        }

    }

}
