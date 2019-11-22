using UnityEngine;

public enum TankType
{
    Cannon = 0,
    Missile,
    Catapult,
}

public class Tank
{
    public int          m_iRemainEnergy;
    public int          m_iMaxEnergy;

    public int          m_iRemainMove;
    public int          m_iMaxMove;

    public bool         m_isShooting;
    public int          m_iCurrentPower;
    public int          m_iLastPower;
    public int          m_iShootingValue;

    public int          m_iMinAngle;
    public int          m_iMaxAngle;
    public int          m_iCurrentAngle;

    public int          m_iBombType; // 0 = normal, 1 = special
    public int[]        m_iBombRadius;
    public int[]        m_iBombCount;

    public int          m_iDelay; // 낮을수록 턴을 빨리 갖는다.

    public bool         m_isDeadTank; // 죽은 탱크

    public TankType     m_iTankType; // 0 = cannon, 1 = missile, 2 = catapult
    public AudioClip[]  m_audioClips;



    // TODO data from type
    public Tank(TankType type)
    {
        m_iBombRadius = new int[2];
        m_iBombCount = new int[2];
        switch (type)
        {
            case TankType.Cannon:
                m_iMaxEnergy = m_iRemainEnergy = 920;

                m_iMaxMove = 150;


                m_iMinAngle = 25;
                m_iMaxAngle = 45;
                m_iCurrentAngle = m_iMinAngle;

                m_iBombRadius[0] = 60;
                m_iBombRadius[1] = 8;

                m_iBombCount[0] = 1;
                m_iBombCount[1] = 1;

                m_iBombType = 0;

                m_iDelay = 580;

                m_isDeadTank = false;
                break;
            case TankType.Missile:
                m_iMaxEnergy = m_iRemainEnergy = 1100;

                m_iMaxMove = 200;


                m_iMinAngle = 20;
                m_iMaxAngle = 45;
                m_iCurrentAngle = m_iMinAngle;

                m_iBombRadius[0] = 20;
                m_iBombRadius[1] = 15;

                m_iBombCount[0] = 4;
                m_iBombCount[1] = 1;

                m_iBombType = 0;

                m_iDelay = 560;

                m_isDeadTank = false;
                break;
            case TankType.Catapult:
                m_iMaxEnergy = m_iRemainEnergy = 950;

                m_iMaxMove = 180;


                m_iMinAngle = 0;
                m_iMaxAngle = 90;
                m_iCurrentAngle = m_iMinAngle;

                m_iBombRadius[0] = 20;
                m_iBombRadius[1] = 15;

                m_iBombCount[0] = 1;
                m_iBombCount[1] = 1;

                m_iBombType = 0;

                m_iDelay = 560;

                m_isDeadTank = false;
                break;
        }
        m_iRemainMove = 0;

        m_isShooting = false;
        m_iCurrentPower = 0;
        m_iLastPower = 0;
        m_iShootingValue = 0;

        m_iTankType = type;

        m_audioClips = new AudioClip[6];
        m_audioClips[0] = Tool.CreateAudioClip(Resources.Load("Sounds/Weapons/" + GetTankPath() + "/move"));
        m_audioClips[1] = Tool.CreateAudioClip(Resources.Load("Sounds/Weapons/" + GetTankPath() + "/unmove"));
        m_audioClips[2] = Tool.CreateAudioClip(Resources.Load("Sounds/Weapons/" + GetTankPath() + "/fire_0"));
        m_audioClips[3] = Tool.CreateAudioClip(Resources.Load("Sounds/Weapons/" + GetTankPath() + "/fire_1"));
        m_audioClips[4] = Tool.CreateAudioClip(Resources.Load("Sounds/Weapons/" + GetTankPath() + "/bomb_0"));
        m_audioClips[5] = Tool.CreateAudioClip(Resources.Load("Sounds/Weapons/" + GetTankPath() + "/bomb_1"));
    }

    public string GetTankPath()
    {
        string path = "";
        switch(m_iTankType)
        {
            case TankType.Cannon:
                path = "Cannon";
                break;
            case TankType.Missile:
                path = "Missile";
                break;
            case TankType.Catapult:
                path = "Catapult";
                break;
        }
        return path;
    }
}