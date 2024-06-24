using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheckInfo : MonoBehaviour
{
    public bool grounded = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        // Tarkistetaan osuma tietyllä layerilla (esim. layer 3 tappaa pelaajan)
        if (col.gameObject.layer == 3)
        {
            KillPlayer();
        }
    }

    void CheckGroundedStatus()
    {
        // Aseta raycastin suunta ja pituus
        Vector2 raycastOrigin = transform.position;
        float raycastDistance = 0.1f;
        LayerMask groundLayer = LayerMask.GetMask("Ground"); // Varmista että 'Ground' layer on asetettu oikein

        // Suorita raycast alaspäin
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayer);

        // Jos osutaan maahan, asetetaan grounded trueksi, muuten falseksi
        if (hit.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance, Color.red); // Piirrä raycastin debug-näkyvyyden vuoksi
    }


    public void KillPlayer()
    {
        StartCoroutine(WaitUntilCamUnFreeze(GameObject.Find("PlayerCamera").GetComponent<FollowPlayer>(), 0.5f));
    }

    void Update()
    {
        // Tarkista onko pelaaja maassa
        CheckGroundedStatus();
    }

    IEnumerator WaitUntilCamUnFreeze(FollowPlayer fP, float secs)
    {
        float oldMaxSpeed = fP.maxSpeed;
        fP.maxSpeed = 500f;

        yield return new WaitForSecondsRealtime(secs);


        GameObject.Find("Player").GetComponent<PlayerMotor>().SummonPlayer(1);
        fP.maxSpeed = oldMaxSpeed;
    }
}
