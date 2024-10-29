using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float rightOfGround;
    
    public float rightOfScreen;
    
    bool groundGenerated = false;

    public Obstacle obstacle;

    BoxCollider2D collider;

    private void Awake(){

        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (collider.size.y / 2);
        rightOfScreen = Camera.main.transform.position.x * 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){

        Vector3 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        rightOfGround = transform.position.x + (collider.size.x / 2);

        if (rightOfGround < 0){
            Destroy(gameObject);
            return;
        }

            if (!groundGenerated){
                if(rightOfGround < rightOfScreen){
                    groundGenerated = true;
                    GenerateGround();
             }
        }

        transform.position = pos;
    }

    void GenerateGround(){
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector3 pos;

        float height1 = player.jumpVelocity * player.maxJumpTime;
        float timeDif = player.jumpVelocity / -player.gravity;
        float height2 = player.jumpVelocity * timeDif + (.5f * (player.gravity * (timeDif * timeDif)));
        float maxJumpHeight = height1 + height2;
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualY = Random.Range(minY,maxY);

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.7f){
            pos.y = Random.Range(1,2.7f);
        }

        float timeDif2 = timeDif + player.maxJumpTime;
        float timeDif3 = Mathf.Sqrt((2.0f * (maxY - actualY)) /  -player.gravity);
        float totalTime = timeDif2 + timeDif3;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += rightOfGround;
        float minX = rightOfScreen + 5;
        float actualX = Random.Range(minX,maxX);

        pos.x = actualX + goCollider.size.x / 2;
        pos.z = 0;

        go.transform.position = pos;
        
        Ground goGround = go.GetComponent<Ground>();

        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);


        int obstacleNum = Random.Range(0,4);
        for (int i=0;i<obstacleNum;i++){
            float y = Random.Range(7,28);
            if (y >= goGround.groundHeight)
            {
                GameObject newObstacle = Instantiate(obstacle.gameObject);
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left,right);
                Vector3 obstaclePos = new Vector3(x,y,0);
                newObstacle.transform.position = obstaclePos;
            }
        }
    }
}
