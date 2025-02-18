using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSkeletonAnimationController : EnemyAnimationController
{
    private Transform head;
    protected override void Start()
    {
        base.Start();
        head = transform.parent.Find("HeadOfSkeleton");
    }
    private void EnableHead() => head.gameObject.SetActive(true);
}
