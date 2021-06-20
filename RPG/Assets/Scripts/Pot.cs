using Assets.Scripts.Hit;
using System.Collections;
using UnityEngine;


/// <summary>
/// Classe repr�sentant un pot
/// </summary>
/// <inheritdoc/>
public class Pot : MonoBehaviour, IBrakeable
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        gameObject.SetActive(false);
    }

    public void Brake()
    {
        Smash();
    }
}
