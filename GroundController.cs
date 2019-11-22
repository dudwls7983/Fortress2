using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProtoTurtle.BitmapDrawing;

public class GroundController : MonoBehaviour {
    
    
    Texture2D original_texture;
    Texture2D texture;

    // Use this for initialization
    void Start ()
    {
        DrawOriginalMap();

        
    }

    //private void OnGUI()
    //{
    //    GUI.DrawTexture(new Rect(50, 50, 1536, 949), texture);
    //}

    // Update is called once per frame
    void Update ()
    {
        //if(Input.GetMouseButton(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    Plane xy = new Plane(Vector3.forward, 5);
        //    float distance;
        //    xy.Raycast(ray, out distance);
        //    Vector3 hit = ray.GetPoint(distance);
            
        //    int x = (int)(hit.x * 80);
        //    int y = (int)(hit.y * 80);
        //    ExplodeGround(x, y, 10);
        //}
        //BitmapDrawingExtensions.DrawRectangle(texture, new Rect(50, 50, 1536, 949), new Color(1, 1, 1, 1));
        //if (i >= 1535)
        //    return;
        //i++;
        //var colors = new Color[949];

        //for (var i = 0; i < 949; i++)
        //    colors[i].a = 0;
        
        //texture.SetPixels(i, 0, 1, 949, colors);

        //texture.Apply();
    }

    // original texture를 복사해서 texture를 만든다. texture를 이용해서 새로운 Sprite를 생성한다.
    void DrawOriginalMap()
    {
        original_texture = GetComponent<SpriteRenderer>().sprite.texture;
        texture = Instantiate(original_texture);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, 1536, 949), new Vector2(0.0f, 0.0f), 80);
    }
    
    public int GetGroundHeight(Vector3 vector)
    {
        return GetGroundHeight((int)(vector.x * 80), (int)(vector.y * 80));
    }
    public int GetGroundHeight(int x, int y)
    {
        // 이 값들은 잘못된 값들이다. 땅속으로 계속 내려간다.
        if (x <= 0 || y <= 0 || x > texture.width)
            return 1;

        Color color = texture.GetPixel(x, y);
        if(color.a == 0)
        {
            for (int i = y; i >= 0; i--)
            {
                color = texture.GetPixel(x, i);
                if (color.a > 0)
                {
                    return y - i;
                }
            }
        }
        else
        {
            if (texture.GetPixel(x, y + 1).a == 0)
                return 0;

            for (int i = y; i < 949; i++)
            {
                color = texture.GetPixel(x, i);
                if (color.a == 0)
                {
                    return y - i;
                }
            }
        }
        
        return y;
    }

    public bool CheckCollisionBetween(float x1, float y1, float x2, float y2, ref int resultx, ref int resulty)
    {
        float diff_x = x2 - x1;
        float diff_y = y2 - y1;
        int dir_x = Mathf.RoundToInt(Mathf.Abs(diff_x) / diff_x);
        int dir_y = Mathf.RoundToInt(Mathf.Abs(diff_y) / diff_y);
        if(Mathf.Abs(diff_x) > Mathf.Abs(diff_y))
        {
            for (float x = x1, y = y1; dir_x == 1 ? x < x2 : x > x2; x += 0.5f * dir_x)
            {
                resultx = (int)x;
                resulty = (int)y;

                if (texture.GetPixel(resultx, resulty).a != 0)
                    return true;

                y += (diff_y / diff_x) / 2 * dir_y;
            }
        }
        else
        {
            for (float x = x1, y = y1; dir_y == 1 ? y < y2 : y > y2; y += 0.5f * dir_y)
            {
                resultx = (int)x;
                resulty = (int)y;

                if (texture.GetPixel(resultx, resulty).a != 0)
                    return true;

                x += (diff_x / diff_y) / 2 * dir_x;
            }
        }

        return false;
    }

    public void ExplodeGround(int x, int y, int radius)
    {
        var color = new Color();
        color.a = 0;
        texture.DrawFilledCircle(x, texture.height-y, radius, color);
        texture.Apply();
    }
}
