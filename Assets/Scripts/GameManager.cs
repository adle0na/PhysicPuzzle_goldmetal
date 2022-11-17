using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Donggle lastDonggle;
    public GameObject dongglePrefab;
    public Transform donggleGroup;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        NextDonggle();
    }

    private Donggle GetDonggle()
    {
        GameObject instant = Instantiate(dongglePrefab, donggleGroup);
        Donggle instantDonggle = instant.GetComponent<Donggle>();
        return instantDonggle;
    }

    private void NextDonggle()
    {
        Donggle newDonggle = GetDonggle();
        lastDonggle = newDonggle;
        lastDonggle.level = Random.Range(0, 2);
        lastDonggle.gameObject.SetActive(true);
        
        StartCoroutine("WaitNext");
    }

    private IEnumerator WaitNext()
    {
        while (lastDonggle != null)
            yield return null;

        yield return new WaitForSeconds(2.5f);
        
        NextDonggle();
    }
    
    public void TouchDown()
    {
        if (lastDonggle == null)
            return;
        
        lastDonggle.Drag();
    }

    public void TouchUp()
    {
        if (lastDonggle == null)
            return;
        
        lastDonggle.Drop();
        lastDonggle = null;
    }
}
