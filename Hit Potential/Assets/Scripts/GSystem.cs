using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSystem : MonoBehaviour
{
    [SerializeField] GameObject SoundIndicator;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Create sound indicator objective
    public void CreateSoundIndicator(GameObject parent, int size, bool fromPlayer)
    {
        //Instantiate object and set if it was made by player or not
        var sound = Instantiate(SoundIndicator, parent.transform);
        sound.GetComponent<SoundIndicator>().fromPlayer = fromPlayer;
        Destroy(sound, size);
    }
}
