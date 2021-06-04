using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    private Animator anim;
    private Mediator mediator = Mediator.Instance;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mediator.Subscribe<PlayerHitCommand>(OnPlayerHit);
    }

    private void OnPlayerHit(PlayerHitCommand c)
    {
        if(c.What.GetComponent<Pot>() == this)
        {
            Smash();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Smash()
    {
        anim.SetBool("smashed", true);
        StartCoroutine(brakeCo());
    }

    IEnumerator brakeCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}
