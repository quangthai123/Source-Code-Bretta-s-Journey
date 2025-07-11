using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextFx : MonoBehaviour
{
    [SerializeField] private float popUpTime = .3f;
    [SerializeField] private float lifeTime = .6f;
    private float popUpTimer;
    public float popUpDistance = 1f;
    private TextMeshProUGUI damageText;
    private TextMeshProUGUI damageTextBG;
    private void OnEnable()
    {
        damageText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        damageTextBG = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        StartCoroutine(PopUpFx());
    }
    public void SetDamage(float _damage)
    {
        string damage = Mathf.Round(_damage).ToString();
        damageText.text = damage;
        damageTextBG.text = damage;
    }
    private IEnumerator PopUpFx()
    {
        popUpTimer = popUpTime;
        while(popUpTimer > 0)
        {
            popUpTimer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x, transform.position.y + popUpDistance), popUpDistance/popUpTime*Time.deltaTime);
            yield return null;
        }
        Invoke("Despawn", lifeTime);
    }
    private void Despawn()
    {
        DamageTextSpawner.Instance.Despawn(transform);
    }
}
