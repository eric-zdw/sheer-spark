using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionColor : MonoBehaviour
{
    ParticleSystemRenderer psr;
    MaterialPropertyBlock matBlock;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        psr = GetComponent<ParticleSystemRenderer>();
        matBlock = new MaterialPropertyBlock();
        matBlock.SetColor("_Color", color);
        matBlock.SetColor("_EmissionColor", color);
        psr.SetPropertyBlock(matBlock);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
