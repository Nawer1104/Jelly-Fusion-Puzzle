using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Level : MonoBehaviour
{
    public List<GameObject> gameObjects;

    List<Ball> listAllBall = new List<Ball>();
    List<Ball> listCollected = new List<Ball>();
    private bool canDrag = true;
    private bool isChoseStart = false;

    private Ball fistCollected;

    [SerializeField] private LineRenderer line;


    private void Awake()
    {
        listAllBall.Clear();
        listAllBall = FindObjectsOfType<Ball>(false).ToList();
        canDrag = true;
    }

    private void Update()
    {
        if (!canDrag) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            listCollected.Clear();
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                var tp = targetObject.GetComponent<Ball>();
                if (tp != null)
                {
                    if (fistCollected == null)
                    {
                        fistCollected = tp;
                        tp.SetCollected();
                        listCollected.Add(tp);
                        isChoseStart = true;
                        line.positionCount = 0;
                        line.positionCount++;
                        line.SetPosition(line.positionCount - 1, tp.transform.position);
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (isChoseStart)
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject)
                {
                    if (isChoseStart)
                    {
                        var tp = targetObject.GetComponent<Ball>();
                        if (tp != null)
                        {
                            if (fistCollected != null)
                            {
                                if (tp.id == fistCollected.id)
                                {
                                    tp.SetCollected();
                                    listCollected.Add(tp);
                                    line.positionCount++;
                                    line.SetPosition(line.positionCount - 1, tp.transform.position);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isChoseStart)
            {
                if (fistCollected != null)
                {

                    fistCollected = null;
                    StartCoroutine(Kill());
                }
            }

            isChoseStart = false;
        }
    }

    int CheckCount()
    {
        var t = fistCollected.id;
        return listAllBall.FindAll(l => l.id == t).Count;
    }

    IEnumerator Kill()
    {
        line.positionCount = 0;
        canDrag = false;
        int i = 0;
        while (i < listCollected.Count)
        {
            var tr = listCollected[i];
            tr.transform.DOScale(Vector3.one * 0.55f, 0.15f).OnComplete(() =>
            {
                tr.PlayVfx();
                tr.gameObject.SetActive(false);
                listAllBall.Remove(tr);
                i++;
            });
            yield return new WaitForSeconds(0.16f);
        }

        canDrag = true;

        if (listAllBall.Count == 0)
        {
            GameManager.Instance.CheckLevelUp();
        }
    }
}
