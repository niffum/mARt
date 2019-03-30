/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBarManager : MonoBehaviour {

	[SerializeField]
	private GameObject start;

	[SerializeField]
	private GameObject end;

	[SerializeField]
	private GameObject bar;

	private int maxDepth;

	private int currentDepth;

	private float startYPosBar;

	private float distStartEnd;

	[SerializeField]
	private float slideSpeed = 1.5f;
	
	private Vector3 lastPosition;

    private double newY;

	void Start()
	{
		CalculateSteps();
	}

	void Update()
	{
		UpdateBarPosition();
	}
	
	private void CalculateSteps()
	{
		Vector3 startCenter = start.transform.localPosition;
		Vector3 endCenter = end.transform.localPosition;

		Vector3 startExtents = start.GetComponent<MeshRenderer>().bounds.extents;
		Vector3 endExtents = end.GetComponent<MeshRenderer>().bounds.extents;
		Vector3 barExtens = bar.GetComponent<MeshRenderer>().bounds.extents;

		//distStartEnd = Vector3.Distance(startCenter, endCenter) - (startExtents.y + endExtents.y);

		distStartEnd = Vector3.Distance(startCenter, endCenter);

		//startYPosBar = startCenter.y - startExtents.y - barExtens.y;
		startYPosBar = startCenter.y;
	}

	public double CalculateCurrentBarYPosition(int depth)
	{
		//float newYPos = Mathf.Lerp(startYPosBar, endYPosBar, stepSize / depth);
		if(depth > 0 )
		{
            newY = startYPosBar - depth / (maxDepth - 1) * distStartEnd;
            return newY;
		}
		else
		{
			return  startYPosBar;
		}
		
	}

	private void UpdateBarPosition()
	{
		double newYPosition = CalculateCurrentBarYPosition(currentDepth);
		//float translateBy = Mathf.Lerp(lastPosition.y, newYPosition, slideSpeed);

		var tmpPos = bar.transform.localPosition;
		bar.transform.localPosition = new Vector3(tmpPos.x, (float)newYPosition, tmpPos.z);//.Translate(0f, translateBy, 0f);
	}

	public void SetMaxDepth(int newMaxDepth)
	{
		maxDepth = newMaxDepth;
	}

	public void SetCurrentDepth(int newCurrentDepth)
	{
		currentDepth = newCurrentDepth;
	}
}
