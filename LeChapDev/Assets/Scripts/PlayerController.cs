using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public float jumpForce;
    public GameObject hatObject;

    //[HideInInspector]
    public Rigidbody rig;
    public Player photonPlayer;

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
    }

    private void Move()
    {
        //standard controls
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        //establishes a velocity based on the player inputs
        rig.velocity = new Vector3(x, rig.velocity.y, z);
    }

    private void TryJump()
    {
        //raycasts from the player's position downward 0.7 units. if the raycast hits, the player jumps
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, 0.7f))
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        Debug.Log(player.ActorNumber);

        GameManager.instance.players[id - 1] = this; //sets the game manager to an index in relation to its Photon id

        //photonView is part of the MonoBehaviorPun extension so i changed that
        if (photonView.IsMine)
        {
            rig.isKinematic = true; // game physics should only affect the clients individually
        }
    }
}
