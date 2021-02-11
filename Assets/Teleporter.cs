﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter destination;
    public Vector3 exitPosition;
    public bool reverseControls;
    public bool reverseVelocity;
    public static float playerCooldown = 0f;

    public GameObject teleporterSparksPrefab;

    public Vector3 center;

    private float baseGlowValue = 0f;
    private MaterialPropertyBlock mpb;
    public Color emissionColor;
    private bool isFlashing = false;
    public GameObject teleporterFlashPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mpb = new MaterialPropertyBlock();
        StartCoroutine(Glow());
    }

    // Update is called once per frame
    void Update()
    {
        playerCooldown = Mathf.Clamp(playerCooldown - Time.deltaTime, 0f, 1f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Player" && playerCooldown <= 0) {
            Instantiate(teleporterFlashPrefab, GameObject.FindGameObjectWithTag("GameUI").transform);
            float playerYRelativeToTeleporter = other.transform.position.y - center.y;
            other.transform.position = destination.exitPosition + new Vector3(0f, playerYRelativeToTeleporter, 0f);
            playerCooldown = 0.5f;
            if (reverseControls) {
                CameraFollow cameraScript = Camera.main.GetComponent<CameraFollow>();
                CameraFollow.CameraDistance *= -1f;
                cameraScript.isReversed = !cameraScript.isReversed;
                cameraScript.SnapToNewPosition();
                other.GetComponent<PlayerBehaviour>().isReversed = !other.GetComponent<PlayerBehaviour>().isReversed;
            }
            if (reverseVelocity) {
                Vector3 velocity = other.GetComponent<Rigidbody>().velocity;
                Vector3 angularVelocity = other.GetComponent<Rigidbody>().angularVelocity;
                other.GetComponent<Rigidbody>().velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
                other.GetComponent<Rigidbody>().angularVelocity = new Vector3(angularVelocity.x, angularVelocity.y, -angularVelocity.z);
                other.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
            }
            Instantiate(teleporterSparksPrefab, destination.exitPosition, Quaternion.identity);
            StartFlash();
            destination.StartFlash();
        }
        
    }

    private IEnumerator Glow() {
        while (true) {
            if (!isFlashing) {
                mpb.SetColor("_EmissionColor", emissionColor * (2f + (Mathf.Sin(Time.time) * 0.5f)));
                GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Flash() {
        isFlashing = true;
        float duration = 1f;
        while (duration > 0f) {
            mpb.SetColor("_EmissionColor", emissionColor * (2f + (Mathf.Sin(Time.time) * 0.5f) + duration * 4f));
            GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isFlashing = false;
    }

    public void StartFlash() {
        StartCoroutine(Flash());
    }
}
