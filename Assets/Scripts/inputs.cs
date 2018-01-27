using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputs : MonoBehaviour {

    public KeyCode[] input = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
    public string skin = "cat";
    private Rigidbody2D rb;
    private Collision2D collision;
    private HashSet<string> objectsTouching = new HashSet<string>();

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        GetComponent<Animator>().Play(skin);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (objectsTouching.Count > 0)
        {
            Motion();
        }
        Kick();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        objectsTouching.Add(collision.collider.name);
        if (Input.GetKey(input[0]) || Input.GetKey(input[1]))
        {
            GetComponent<Animator>().Play(skin + "_run");
        }
        else
        {
            GetComponent<Animator>().Play(skin);
        }
        if (collision.collider.GetComponent<Rigidbody2D>())
        {
            this.collision = collision;
        }
    }

    private void Kick ()
    {
        if (Input.GetKeyDown(input[3]) && collision != null)
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector3 position = collision.collider.transform.position;
            Vector2 force = new Vector2((position.x - contact.point.x) * 30000f, (position.y - contact.point.y) * 20000f);
            collision.collider.GetComponent<Rigidbody2D>()
                .AddForce(force);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        objectsTouching.Remove(collision.collider.name);
        if (this.collision != null && collision.collider.name == this.collision.collider.name)
        {
            this.collision = null;
        }
    }

    private void Motion ()
    {
        Anim();

        Vector2 direction = new Vector2(0, 0);
        if (Input.GetKey(input[0]))
        {
            direction.x = -0.3f;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(input[1]))
        {
            direction.x = 0.3f;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x + direction.x, position.y + direction.y, position.z);
        Jump(direction);

    }

    private void Anim ()
    {
        if (Input.GetKeyDown(input[0]) || Input.GetKeyDown(input[1]))
        {
            GetComponent<Animator>().Play(skin + "_run");
        }
        else if (Input.GetKeyUp(input[0]) || Input.GetKeyUp(input[1]))
        {
            GetComponent<Animator>().Play(skin);
        }
        else if (Input.GetKeyDown(input[3]))
        {
            GetComponent<Animator>().Play(skin + "_slide");
        }
    } 

    private void Jump (Vector2 direction)
    {
        if (Input.GetKeyDown(input[2]))
        {
            rb.AddForce(new Vector2(direction.x * 40000f, 20000f));
            GetComponent<Animator>().Play(skin + "_jump");
        }
    }
}
