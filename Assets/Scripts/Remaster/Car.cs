using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    int [,] map;
    public GameObject level;
    public GameObject car;
    public GameObject[] arrows; //0 --> derecha, 1 --> abajo, 2 --> izquierda, 3 --> arriba.
    public int width = 0, high = 0;
    public int dir = 0; //0 --> derecha, 1 --> abajo, 2 --> izquierda, 3 --> arriba.
    public int posX=0, posY= 0;
    // Use this for initialization
    void Start () {
        map = new int[high, width];
        int it = 0;
        for (int i = 0; i < high; i++)
            for (int j = 0; j < width; j++)
            {
                if (level.transform.GetChild(it).gameObject.layer == 8) map[i, j] = 20;
                else if (level.transform.GetChild(it).gameObject.layer == 10) map[i, j] = 2;
                else map[i, j] = 1;
                it++;
            }
        string s = "";
        for (int i = 0; i < high; i++)
        {
            for (int j = 0; j < width; j++)
            {
                s += map[i, j].ToString() + " ";
            }
            //Debug.Log(s);
            s += "\n";
        }
        Debug.Log(s);
        //MoveToRight();
    }

    public bool MoveToRight()
    {
        
        if (map[posY, posX + 1] == 1) posX++;
        else return false;
        dir = 0;
        while (map[posY, posX] != 2)
        {
            posX++;
        }
        
        int ind = posY * (width - 1) + posY + (posX);
        float x  = level.transform.GetChild(ind).gameObject.transform.position.x;
        
        //transform.position = new Vector3(x, transform.position.y, transform.position.z);
        StartCoroutine(move(new Vector3(x, transform.position.y, transform.position.z), 20.0f));
        return true;

    }

   
    public bool MoveToLeft()
    {
        if (map[posY, posX - 1] == 1) posX--;
        else return false;
        dir = 2;
       
        while (map[posY, posX] != 2)
        {
            posX--;
        }
        int ind = posY * (width - 1) + posY + (posX);
        float x = level.transform.GetChild(ind).gameObject.transform.position.x;

        StartCoroutine(move(new Vector3(x, transform.position.y, transform.position.z), 20.0f));

        return true;
    }
    public bool MoveUp()
    {
        if (map[posY-1, posX] == 1) posY--;
        else return false;
        dir = 3;
        while (map[posY, posX] != 2)
        {
            posY--;
        }
        int ind = posY * (width - 1) + posY + posX;
        float y = level.transform.GetChild(ind).gameObject.transform.position.y;
        StartCoroutine(move(new Vector3(transform.position.x, y, transform.position.z), 20.0f));

        return true;
    }
    public bool MoveDown()
    {
        if (map[posY + 1, posX] == 1) posY++; 
        else return false;
        dir = 1;

       
        while (map[posY, posX] != 2)
        {
            posY++;
        }
        Debug.Log(posY + ", " + posX);
        int ind = posY * (width - 1) + posY + posX;
        float y = level.transform.GetChild(ind).gameObject.transform.position.y;
        StartCoroutine(move(new Vector3(transform.position.x, y, transform.position.z), 20.0f));

        return true;
    }

    void showArrows()
    {
        if (map[posY, posX + 1] != 20) arrows[0].gameObject.SetActive(true);
        if (map[posY + 1, posX] != 20) arrows[1].gameObject.SetActive(true);
        if (map[posY, posX-1] != 20) arrows[2].gameObject.SetActive(true);
        if (map[posY -1, posX] != 20) arrows[3].gameObject.SetActive(true);

        switch (dir)
        {
            case 0: arrows[2].gameObject.SetActive(false); break;
            case 1: arrows[3].gameObject.SetActive(false); break;
            case 2: arrows[0].gameObject.SetActive(false); break;
            case 3: arrows[1].gameObject.SetActive(false); break;
        }

    }

    IEnumerator move(Vector3 v, float t)
    {
        foreach (GameObject go in arrows) go.SetActive(false);
        car.transform.LookAt(v, car.transform.up);
        while (transform.position != v)
        {
           
          
            transform.position = Vector3.MoveTowards(transform.position, v, 1.0f / t);
            yield return null;
        }
        showArrows();

    }
    // Update is called once per frame
    void Update () {
		if (Input.GetKeyUp(KeyCode.D))
        {
            MoveToRight();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            MoveToLeft();
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            MoveUp();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            MoveDown();
        }
	}
}
