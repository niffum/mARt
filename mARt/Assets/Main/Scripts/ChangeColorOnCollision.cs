using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnCollision : MonoBehaviour {

    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private Color collisionColor = Color.green;

    private Material _material;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }
        if (renderer != null)
        {
            _material = renderer.material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER SANDMAN");
        _material.color = Color.Lerp(_material.color, collisionColor, 30F * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        _material.color = Color.Lerp(_material.color, defaultColor, 30F * Time.deltaTime);
    }

}
