using UnityEngine;
using System.Collections;

public class BugController : MonoBehaviour
{
    //objects/classes
    private GameObject explosion;
    private Transform self;
    private Transform target;
    private PlayerController playerController;

    //members/variables.
    private float _speed;
    private const float _speedMulti = 1.2f;
    private const float _maxSpeed = 9.0f;
    private const float _minSpeed = 4.0f;

    //called before start: get references
    private void Awake ()
    {
        explosion = (GameObject)Resources.Load("Explosion");
        self = GetComponent<Transform>();
        target = GameObject.Find("Application").GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //initalise members
    private void Start ()
    {
        //speed scales with bugs; clamp speed.
        _speed = Mathf.Clamp(playerController.Bugs * _speedMulti, _minSpeed, _maxSpeed);
        //Debug.Log("Speed = " + speed.ToString());
    }

    private void Update ()
    {
        float dist = _speed * Time.deltaTime;
        self.position = Vector3.MoveTowards(self.position, target.position, dist);
	}

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject == GameObject.Find("Application"))
        {
            //Debug.Log("collided with the application; destroying");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        playerController.Bugs--;
        Instantiate(explosion, self.transform.position, self.transform.rotation);
    }
}
