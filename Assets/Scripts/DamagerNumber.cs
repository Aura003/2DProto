using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class DamagerNumber : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    [Header("Animations")]
    public float lifeTime;
    public float moveDistance;

    private float timer;
    private Vector3 startPos;
    private Color baseColor;
    private GameManager manager;
    public void Initialize(GameManager gm)
    {
        manager = gm;
    }
    public void DisplayDamage(int damageAmount, Vector3 position, bool isCritical, Color c)
    {
        transform.position = position;
        startPos = position;
        timer = 0f;

        baseColor = c;
        baseColor.a = 1f;

        damageText.text = damageAmount.ToString();

        damageText.color = isCritical ? Color.red : baseColor;
        damageText.fontSize = isCritical ? 320 : 250;
        damageText.transform.localScale = isCritical ? Vector3.one * 1.35f : Vector3.one;

        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float normalizedTime = timer / lifeTime;
        this.transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * moveDistance, normalizedTime);

        Color c = baseColor;
        c.a = 1 - normalizedTime;
        damageText.color = c;

        if(timer>lifeTime)
            this.gameObject.SetActive(false);
    }
}
