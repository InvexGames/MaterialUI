using UnityEngine;

public class Test : MonoBehaviour{
	public RectTransform target;
	
	void Start () {
		Vector3 leftMosPos = target.localPosition;
		leftMosPos.x = target.rect.x;
		
		GetComponent<RectTransform>().localPosition = leftMosPos;
	}
}