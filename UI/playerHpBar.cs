using UnityEngine;
using System.Collections;

public class playerHpBar : MonoBehaviour
{
    public Vector3 offset;          // The offset at which the Health Bar follows the player.

    private Transform player;       // Reference to the player.


    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Set the position to the player's position with the offset.



        Vector3 lavapiont = new Vector3(0, Screen.height, 0);
        Vector3 screenV = Camera.main.WorldToScreenPoint(transform.position);
        lavapiont.z = screenV.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(lavapiont);

        transform.position = worldPos;
    }
}
