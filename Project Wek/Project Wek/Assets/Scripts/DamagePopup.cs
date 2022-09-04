using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float delay = 0f;

    void Start()
    {
        Destroy(gameObject.transform.parent.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        //Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }

    public void SetText(int dmg)
    {
        gameObject.GetComponent<TextMeshPro>().text = ""+dmg;
    }
}
