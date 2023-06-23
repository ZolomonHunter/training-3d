using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTap : MonoBehaviour
{
    private Renderer coinRenderer;
    private Color[] colors = { Color.black, Color.blue, Color.green, Color.cyan, Color.magenta };
    private int currentColor = 0;

    void Start()
    {
        coinRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hitted");
                if (hit.transform.name == "Coin")
                {
                    currentColor = (currentColor == colors.Length - 1) ? 0 : currentColor + 1;
                    coinRenderer.material.color = colors[currentColor];
                }
            }
        }
        
    }
}
