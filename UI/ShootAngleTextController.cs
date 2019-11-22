using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootAngleTextController : MonoBehaviour
{

    GameObject tank;
    Text text;
    TankController tankController;
    bool isSetTank;

    // Use this for initialization
    void Start()
    {
        isSetTank = false;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSetTank == false || tank == null)
            return;

        int shootAngle = tankController.tankData.m_iCurrentAngle;
        text.text = shootAngle > 180 ? (shootAngle - 360).ToString() : shootAngle.ToString();
    }

    public void SetMyTank(GameObject tank)
    {
        this.tank = tank;
        isSetTank = true;

        tankController = tank.GetComponent<TankController>();
    }
}
