using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private float size;
    
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(size, 0.25f);
        DOVirtual.DelayedCall(0.8f, () => transform.DOScale(0.5f, 0.25f).onComplete += () => Destroy(gameObject));
    }

    public void InitializeText(float damage, Color color, float size)
    {
        textMeshPro.text = damage != Mathf.Floor(damage) ? damage.ToString("F1") : damage.ToString("F0");
        textMeshPro.color = color;
        this.size = size;
    }
}
