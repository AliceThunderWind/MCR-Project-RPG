using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private bool isAlive = false;
    public GameObject[] characters;
    private string selectedCharacterDataName = "SelectedCharacter";
    int selectedCharacter;
    public GameObject playerObject;


    // Start is called before the first frame update
    void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt(selectedCharacterDataName, 0);
        playerObject = Instantiate(characters[selectedCharacter],
            transform.GetChild(0).position, 
            characters[selectedCharacter].transform.rotation);
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive == false)
        {
            Start();
        }
    }
}
