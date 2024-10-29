using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{

    [SerializeField] float depth = 1;
    Player player;

    private void Awake(){
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float actualVelocity = player.velocity.x / depth;
        Vector3 pos = transform.position;

        pos.x -= actualVelocity * Time.fixedDeltaTime;

        if (pos.x <= -25){
            pos.x = 80;
        }

        transform.position = pos;
    }
}
