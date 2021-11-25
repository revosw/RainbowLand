using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorFilter : MonoBehaviour
{
    [SerializeField] ColorFilter.Filter activeFilter;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("Global Volume").GetComponent<ColorFilter>().SetFilter(activeFilter);
    }
}
