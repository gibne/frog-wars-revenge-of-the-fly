using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] public float gravity = -400;

    [SerializeField] float acceleration = 10;

    [SerializeField] float maxAcceleration = 10;

    public float distance = 0;

    public Vector3 velocity;

    [SerializeField] float xMaxVelocity = 100;

    [SerializeField] public float jumpVelocity = 50;

    [SerializeField] float groundHeight = 10;

    [SerializeField] public float maxJumpTime = .4f;

    [SerializeField] float maxMaxJumpTime = .4f;

    [SerializeField] float jumpTimer = 0.0f;

    [SerializeField] float groundThreshold =3;

    [SerializeField] bool onGround = false;

    [SerializeField] bool jumpHold = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if(onGround || groundDistance <= groundThreshold){
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.pitch = Random.Range(0.9f,1.1f);
                audioSource.Play();
                onGround = false;
                velocity.y = jumpVelocity;
                jumpHold = true;
                jumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)){
            jumpHold = false;
        }
    }

    private void FixedUpdate(){
        Vector2 pos = transform.position;

        if(!onGround){

            if (jumpHold){
                jumpTimer += Time.fixedDeltaTime;
                if (jumpTimer >= maxJumpTime){
                    jumpHold = false;
                }
            }

            pos.y += velocity.y * Time.fixedDeltaTime;
            if(!jumpHold){
            velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector3 raycastOrigin = new Vector3(pos.x + .7f, pos.y);
            Vector3 raycastDirection = Vector3.up;
            float raycastDistance = velocity.y * Time.fixedDeltaTime;

            RaycastHit2D hit2D = Physics2D.Raycast(raycastOrigin,raycastDirection,raycastDistance);
            if (hit2D.collider != null){
                Ground ground =  hit2D.collider.GetComponent<Ground>();

                if (ground != null){
                    groundHeight = ground.groundHeight;
                    pos.y = groundHeight;
                    velocity.y = 0;
                    onGround = true;
                }
            }
            Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red);
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if(onGround){

            float velocityRatio = velocity.x / xMaxVelocity;
            acceleration = maxAcceleration * (1 -velocityRatio);
            maxJumpTime = maxMaxJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;

            if (velocity.x >= xMaxVelocity){
                velocity.x = xMaxVelocity;
            }

            Vector3 raycastOrigin = new Vector3(pos.x - .7f, pos.y);
            Vector3 raycastDirection = Vector3.up;
            float raycastDistance = velocity.y * Time.fixedDeltaTime;

            RaycastHit2D hit2D = Physics2D.Raycast(raycastOrigin,raycastDirection,raycastDistance);
            if (hit2D.collider == null){
                onGround = false;
            }
            Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.yellow);
        }

        transform.position = pos;
    }
}
