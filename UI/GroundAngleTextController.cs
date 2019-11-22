using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundAngleTextController : MonoBehaviour {

    GameObject tank;
    SpriteRenderer tankRenderer;
    Text text;
    bool isSetTank;

	// Use this for initialization
	void Start () {
        isSetTank = false;
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isSetTank == false || tank == null)
            return;

        int dir = tankRenderer.flipX ? 1 : -1;
        int groundAngle = (int)(tank.transform.rotation.eulerAngles.z * dir);
        groundAngle = groundAngle > 180 ? (groundAngle - 360) : groundAngle;
        groundAngle = groundAngle < -180 ? (groundAngle + 360) : groundAngle;
        text.text = groundAngle.ToString();
	}

    public void SetMyTank(GameObject tank)
    {
        this.tank = tank;
        isSetTank = true;

        tankRenderer = tank.GetComponent<SpriteRenderer>();
    }
}
