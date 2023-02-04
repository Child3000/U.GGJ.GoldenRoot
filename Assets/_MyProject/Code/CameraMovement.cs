using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    //[SerializeField] private Vector3 startPos = 
    public void MoveToGamePlay()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOJump(new Vector3(4f, 9f, 4f), 0.5f, 1, 3.0f))
           .Join(this.transform.DORotate(new Vector3(90f, 0f, 0f), 3.0f, RotateMode.Fast));
    }
}
