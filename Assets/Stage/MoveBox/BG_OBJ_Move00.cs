using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_OBJ_Move00 : MonoBehaviour
{
    public float speedX = 1;
    public float speedY = 0;
    public float speedZ = 0;
    public float second = 1;

    float time = 0f;

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        float s = Mathf.Sin(time * 3.14f / second);
        this.transform.Translate(speedX * s / 50, speedY * s / 50, speedZ * s /50);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
