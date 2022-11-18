using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Donggle    lastDonggle;
    public GameObject dongglePrefab;
    public Transform  donggleGroup;
    public GameObject effectPrefab;
    public Transform  effectGroup;

    public int maxLevel;
    
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
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        
        GameObject instant = Instantiate(dongglePrefab, donggleGroup);
        Donggle instantDonggle = instant.GetComponent<Donggle>();
        instantDonggle.effect = instantEffect;
        
        return instantDonggle;
    }

    private void NextDonggle()
    {
        Donggle newDonggle = GetDonggle();
        lastDonggle         = newDonggle;
        lastDonggle.manager = this;
        lastDonggle.level   = Random.Range(0, maxLevel);
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
