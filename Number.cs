using TMPro;
using UnityEngine;

public class Number : MonoBehaviour
{
    public TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1f);
    }

  
}
