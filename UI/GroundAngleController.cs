using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAngleController : MonoBehaviour {

    GameObject tank;
    SpriteRenderer tankRenderer;
    bool isSetTank;

	// Use this for initialization
	void Start () {
        isSetTank = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (isSetTank == false || tank == null)
            return;


        transform.rotation = Quaternion.Euler(0, 0, tank.transform.rotation.eulerAngles.z);
        int dir = tankRenderer.flipX ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
	}

    public void SetMyTank(GameObject tank)
    {
        this.tank = tank;
        isSetTank = true;

        tankRenderer = tank.GetComponent<SpriteRenderer>();
    }
}
