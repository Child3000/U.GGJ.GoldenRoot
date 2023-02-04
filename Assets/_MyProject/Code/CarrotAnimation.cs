using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarrotAnimation : MonoBehaviour
{
    [SerializeField] private Transform location;

    [SerializeField] private float jumpForce = 0.25f;
    [SerializeField] private float jumpDistance = 2f;



    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMoveY(jumpDistance, jumpForce))
            .Append(this.transform.DOShakeScale(0.5f))
            .AppendInterval(1.5f)
            .Append(this.transform.DORotate(new Vector3(0f, 900f, 0f), 2f, RotateMode.FastBeyond360))
            .Join(this.transform.DOScale(0.01f, 1.5f))
            .AppendCallback(DestroyMe);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
