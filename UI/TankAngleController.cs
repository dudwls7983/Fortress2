using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAngleController : MonoBehaviour
{

    GameObject tank;
    SpriteRenderer tankRenderer;
    bool isSetTank;
    int tankAngle;

    // Use this for initialization
    void Start()
    {
        isSetTank = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSetTank == false || tank == null)
            return;


        int dir = tankRenderer.flipX ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        transform.rotation = Quaternion.Euler(0, 0, tank.transform.rotation.eulerAngles.z + (tankAngle * dir));
    }

    public void SetMyTank(GameObject tank, int angle)
    {
        this.tank = tank;
        isSetTank = true;
        tankAngle = angle;

        tankRenderer = tank.GetComponent<SpriteRenderer>();
    }
}
