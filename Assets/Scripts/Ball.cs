
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class Ball : MonoBehaviour
{

    [SerializeField] private float scaleUp = 1.6f;
    public int id;

    private bool isSelected = false;

    public GameObject vfxDestroy;

    public void SetCollected()
    {
        if (isSelected) return;
        isSelected = true;
        transform.DOScale(scaleUp, 1f);
        GetComponent<CircleCollider2D>().enabled = false;
    }
    public void SetUnCollected()
    {
        isSelected = false;
        transform.DOScale(1, 1f);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void PlayVfx()
    {
        GameObject explosion = Instantiate(vfxDestroy, transform.position, transform.rotation);
        Destroy(explosion, .75f);
    }
}