using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlay : MonoBehaviour
{
    [SerializeField] AudioSource a2;

    void LaserSound2()
    {
        a2.Play();
    }

}
