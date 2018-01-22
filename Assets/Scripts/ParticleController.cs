using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    // Use this for initialization
    private IEnumerator Start()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    }
