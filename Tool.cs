using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //public static Vector3 PositionToPixelCoord(Vector3 position)
    //{

    //}

    public static void EmitWeaponSound(string sound_path, bool loop = false)
    {
        AudioSource audio = GameObject.Find("BombSound").GetComponent<AudioSource>();
        audio.loop = loop;
        audio.clip = (AudioClip)Instantiate(Resources.Load(sound_path));
        audio.Play(0);
    }

    public static void EmitWeaponSound(AudioClip audioclip, bool loop = false)
    {

        AudioSource audio = GameObject.Find("BombSound").GetComponent<AudioSource>();
        audio.loop = loop;
        audio.clip = audioclip;
        audio.Play(0);
    }

    public static AudioClip CreateAudioClip(Object obj)
    {
        return (AudioClip)Instantiate(obj);
    }

    public static GameObject CreateExplosionParticle(Vector3 position, int particleCount)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/explosion"), position, new Quaternion());
        if(go != null)
        {
            CreateAndDestroy cad = go.GetComponent<CreateAndDestroy>();
            cad.Destroy(4.5f);

            ParticleSystem ps = go.GetComponent<ParticleSystem>();
            var burst = ps.emission.GetBurst(0);
            burst.count = particleCount;
        }
        return go;
    }

    public static int ToPixel(float x)
    {
        return (int)(x * 80);
    }
}
