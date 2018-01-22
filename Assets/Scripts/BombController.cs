using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public BoxCollider2D twoSpacesAway;
    public BoxCollider2D oneSpaceAway;
    public ParticleSystem explosionParticles;
    public ParticleSystem smokeParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("bomb near");
        }
        if(collision == oneSpaceAway)
        {
            Debug.Log("bomb super near");
        }
    }

}
