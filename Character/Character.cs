using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;


public class Character : MonoBehaviour
{
    public long id;
    public new long name;

    public Guid guid;
    public string displayName;
    public Transform characterModelTransform;
    public Animator animator;
    public StatsController statsController;

    private void Awake()
    {
        statsController = GetComponent<StatsController>();
    }
}
