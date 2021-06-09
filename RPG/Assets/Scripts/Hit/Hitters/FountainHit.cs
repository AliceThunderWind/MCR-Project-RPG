using Assets.Scripts.Hit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainHit : MonoBehaviour
{
    private Mediator mediator = Mediator.Instance;
    private List<MutableTuple<Collider2D, int>> playersInRange = new List<MutableTuple<Collider2D, int>>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playersInRange.Count == 0) return;

        foreach (MutableTuple<Collider2D, int> character in playersInRange)
        {
            if(Time.time >= character.Item2) {

                character.Item2 = Mathf.FloorToInt(Time.time) + 1;
                HpIncreaseCommand cmd = new HpIncreaseCommand();
                cmd.Hp = 10f;
                cmd.What = character.Item1;
                mediator.Publish(cmd);
                    
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ICharacter fighter = other.GetComponent<ICharacter>();
        if(fighter != null)
        {
            playersInRange.Add(new MutableTuple<Collider2D, int>(other, 1));
        }
        

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        playersInRange.Remove(playersInRange.Find((MutableTuple<Collider2D, int> what) => {
            return what.Item1 == other;
        }));
    }
}
