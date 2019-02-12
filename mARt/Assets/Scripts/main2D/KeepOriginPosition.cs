using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOriginPosition : MonoBehaviour {

    private Vector3 originPosition;
	
	void Start () {
        originPosition = transform.position;
	}
	
	public void ReturnToOriginPosition()
    {
        transform.position = originPosition;
    }
}
