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

    public void CreateSoundIndicator(GameObject parent, int size, bool fromPlayer)
    {
        var sound = Instantiate(SoundIndicator, parent.transform);
        sound.GetComponent<SoundIndicator>().fromPlayer = fromPlayer;
        Destroy(sound, size);
    }
}
