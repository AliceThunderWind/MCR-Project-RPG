using Assets.Scripts.Mediator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe représentant une porte (de fin de niveau)
/// </summary>
public class Gate : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameMediator mediator;
    [SerializeField] private GameObject exit;

    public bool isOpen { get; set; } = false;
    private Animator animator;


    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool("open", isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("open", isOpen);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen && other.CompareTag("attackable"))
        {
            mediator.PlayerChangeLevel(other, exit);
        }
    }

   
}
