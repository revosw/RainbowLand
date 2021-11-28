using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorFilter : MonoBehaviour
{
    [SerializeField] ColorFilter.Filter activeFilter;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        GameObject.Find("Global Volume").GetComponent<ColorFilter>().SetFilter(activeFilter);
    }
}
