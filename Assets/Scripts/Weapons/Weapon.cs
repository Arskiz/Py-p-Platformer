using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Weapon : MonoBehaviour
{
    // List of all the weapons available. All weapons set accordingly in the inspector.
    PlayerMotor playerMotor;
    Inventory inv;
    public List<WeaponInstance> weapons = new List<WeaponInstance>();
    public float playerVelocity = 0f;
    [Header("Weapon Variables:")]
    int shotsUntilReloadOnEmpty = 5;
    int currentShotTilReload = 0;
    [Header("Debug:")]
    public bool debugMode;
    [Header("Important Weapon Stuff:")]
    public int selectedWeapon = 0;
    public bool aiming;
    public bool shooting;
    public bool isShootingEmpty;
    public Sprite noGunSprite;
    [Header("Weapon movement and position:")]
    [SerializeField] float smoothVelocityX;
    [SerializeField] float smoothVelocityY;
    [SerializeField] public float weaponPositionSmoothness;
    float playerCalculatedPosition;
    float weaponCalculatedPosition;
    public GameObject playerObject;
    GameObject weaponObject;

    [Header("WeaponCrosshairs:")]
    [SerializeField] Texture2D crosshairTexture;

    void Start()
    {
        playerMotor = FindAnyObjectByType<PlayerMotor>();
        playerObject = GameObject.Find("Player").transform.GetChild(0).gameObject;
        if (weapons.Count > 0)
        {
            weaponObject = weapons[selectedWeapon].gameObject;
        }
        inv = FindAnyObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weapons != null && weapons.Count > 0)
        {
            SetWeaponPos();
            SetWeaponUIStats();
            HandleAiming();
            HandleShooting(weapons[selectedWeapon]);
        }
    }

    void HandleCrosshair()
    {
        switch (aiming)
        {
            case true:
                //Cursor.visible = false;
                Cursor.SetCursor(crosshairTexture, new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2), CursorMode.Auto);

                break;

            case false:
                //Cursor.visible = true;
                Cursor.SetCursor(null, new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2), CursorMode.Auto);
                break;
        }
    }

    // Handles the weapon position.
    // Weapon is adjusted by the player,
    // and thus gets the approppriate
    // X and Y coordinates with Offsets.
    void SetWeaponPos()
    {
        weaponObject = weapons[selectedWeapon].gameObject;
        float X = 0;
        float Y = 0;

        switch (aiming)
        {
            case false:
                switch (weapons[selectedWeapon].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX)
                {
                    case true:
                        X = -weapons[selectedWeapon].offsetX;
                        Y = weapons[selectedWeapon].offsetY;
                        break;

                    case false:
                        X = weapons[selectedWeapon].offsetX;
                        Y = weapons[selectedWeapon].offsetY;
                        break;
                }
                break;

            case true:
                switch (weapons[selectedWeapon].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX)
                {
                    case false:
                        X = weapons[selectedWeapon].offsetAimingX;
                        Y = weapons[selectedWeapon].offsetAimingY;
                        break;

                    case true:
                        X = -weapons[selectedWeapon].offsetAimingX;
                        Y = weapons[selectedWeapon].offsetAimingY;
                        break;
                }
                break;

        }

        if (debugMode)
        {
            int randomWeapon = Random.Range(0, weapons.Count - 1);
            selectedWeapon = randomWeapon;
        }

        if (!aiming)
        {
            float smoothDampedPosX = Mathf.SmoothDamp(weapons[selectedWeapon].transform.position.x, playerObject.transform.position.x + X, ref smoothVelocityX, weaponPositionSmoothness);
            float smoothDampedPosY = Mathf.SmoothDamp(weapons[selectedWeapon].transform.position.y, playerObject.transform.position.y - Y, ref smoothVelocityY, weaponPositionSmoothness);
            SetDampedPos(selectedWeapon, smoothDampedPosX, smoothDampedPosY);
        }
        else
        {
            float smoothDampedPosX = Mathf.SmoothDamp(weapons[selectedWeapon].transform.position.x, playerObject.transform.position.x + X, ref smoothVelocityX, weaponPositionSmoothness);
            float smoothDampedPosY = Mathf.SmoothDamp(weapons[selectedWeapon].transform.position.y, playerObject.transform.position.y - Y, ref smoothVelocityY, weaponPositionSmoothness);
            SetDampedPos(selectedWeapon, smoothDampedPosX, smoothDampedPosY);
        }

    }

    void HandleAiming()
    {
        if (weapons.Count > 0)
        {
            switch (debugMode)
            {
                case true:
                    if (VarHelper.CheckKeybindPressed(playerMotor._aimingKeybind))
                    {
                        aiming = !aiming;
                    }
                    break;

                case false:
                    if (VarHelper.CheckKeybindHold(playerMotor._aimingKeybind))
                    {
                        aiming = true;
                    }
                    else
                    {
                        aiming = false;
                    }
                    break;
            }
        }
        HandleCrosshair();
    }

    void HandleShooting(WeaponInstance wpInstance)
    {
        switch (wpInstance.WeaponType)
        {
            case WeaponTypes.Types.AR:
                if (VarHelper.CheckKeybindPressed(KeyCode.Mouse0))
                {
                    HandleShootForAll(0, wpInstance); // Shoot once
                }
                else if (VarHelper.CheckKeybindHold(KeyCode.Mouse0))
                {
                    HandleShootForAll(1, wpInstance); // Shoot constantly
                }
                break;

            case WeaponTypes.Types.Pistol:
                if (VarHelper.CheckKeybindPressed(KeyCode.Mouse0))
                {
                    HandleShootForAll(0, wpInstance); // Shoot once
                }
                break;
        }
    }

    void HandleShootForAll(int shootType, WeaponInstance wpInstance)
    {
        if (!shooting && !isShootingEmpty)
        {
            if (wpInstance.currentAmmo > 0)
            {
                StartCoroutine(Fire(shootType, wpInstance));
            }
            else if (wpInstance.currentAmmo < 1 && wpInstance.currentReserve > 0)
            {
                StartCoroutine(EmptyClip(0, wpInstance, wpInstance.emptyClipTime));
            }
            else if (wpInstance.currentAmmo < 1 && wpInstance.currentReserve < 1)
            {
                StartCoroutine(EmptyClip(1, wpInstance, wpInstance.emptyClipTime));
            }
        }
    }
    IEnumerator Fire(int typeOf, WeaponInstance wpInstance)
    {
        shooting = true;
        switch (typeOf)
        {
            case 0:
                playerMotor._playerObject.GetComponent<AudioSource>().PlayOneShot(weapons[selectedWeapon].shootOneSound);
                break;

            case 1:
                //playerMotor._playerObject.GetComponent<AudioSource>().PlayOneShot(weapons[selectedWeapon].shootConstantlySound);
                playerMotor._playerObject.GetComponent<AudioSource>().PlayOneShot(weapons[selectedWeapon].shootConstantlySound);
                break;
        }

        if (selectedWeapon >= 0 && weapons.Count > 0)
        {
            wpInstance.currentAmmo -= 1;
            HandleRecoil(wpInstance);
        }

        yield return new WaitForSecondsRealtime(wpInstance.fireRate / 100);

        shooting = false;
    }
    void HandleRecoil(WeaponInstance wpInstance)
    {
        bool flipped = playerObject.transform.GetComponent<SpriteRenderer>().flipX;
        float recoilForce = wpInstance.recoil;
        float recoilNoise = Random.Range(recoilForce + 0.1f, recoilForce + 0.8f);
        switch (flipped)
        {
            case true:
                playerCalculatedPosition = recoilForce - recoilNoise;
                weaponCalculatedPosition = recoilForce * 2 - recoilNoise;
                break;

            case false:
                playerCalculatedPosition = recoilForce + recoilNoise;
                weaponCalculatedPosition = recoilForce * 2 + recoilNoise;
                break;
        }

        Debug.Log($"Calculated: {weaponCalculatedPosition}, now: {weaponObject.transform.position.x}");
        weaponObject.transform.position = new Vector3(Mathf.SmoothDamp(weaponObject.transform.position.x, playerObject.transform.position.x + weaponCalculatedPosition, ref wpInstance.recoilVelocity, 0.10f), weaponObject.transform.position.y, weaponObject.transform.position.z);
    }

    // UNUSED (will be in upcoming audiomanager script at some point!)
    IEnumerator FadeTrack(AudioClip targetClip)
    {
        AudioSource audioSource = playerMotor._playerObject.GetComponent<AudioSource>();
        AudioClip clip = audioSource.clip;
        float timeToFade = 0.05f;
        float timeElapsed = 0;

        if (audioSource.isPlaying)
        {
            audioSource.clip = targetClip;
            audioSource.Play();

            while (timeElapsed < timeToFade)
            {

            }
        }
        return null;
    }

    /// <summary>
    /// Plays empty clip sound when the weapon is fired when the currentAmmo is 0
    /// Usage: 0; For when you have ammo in your reserve
    /// Usage: 1; For when you don't have ammo in your reserve
    /// </summary>
    /// <param name="typeOf"></param>
    /// <param name="wpInstance"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator EmptyClip(int typeOf, WeaponInstance wpInstance, float waitTime)
    {
        isShootingEmpty = true;
        switch (typeOf)
        {
            case 0:
                if (currentShotTilReload == shotsUntilReloadOnEmpty)
                {
                    currentShotTilReload = 0;
                    ReloadWeapon(wpInstance);
                }
                else if (currentShotTilReload < shotsUntilReloadOnEmpty)
                {
                    playerMotor._playerObject.GetComponent<AudioSource>().PlayOneShot(wpInstance.emptyClipSound);
                    yield return new WaitForSecondsRealtime(waitTime / 100);
                    currentShotTilReload += 1;
                }

                break;

            case 1:
                playerMotor._playerObject.GetComponent<AudioSource>().PlayOneShot(wpInstance.emptyClipSound);
                yield return new WaitForSecondsRealtime(waitTime / 100);
                break;
        }

        isShootingEmpty = false;

    }

    void ReloadWeapon(WeaponInstance wpInstance)
    {
        if (wpInstance.currentReserve > wpInstance.maxAmmo)
        {
            wpInstance.currentAmmo = wpInstance.maxAmmo;
            wpInstance.currentReserve -= wpInstance.maxAmmo;
        }
        else if (wpInstance.currentReserve > 0 && wpInstance.currentReserve < wpInstance.maxReserve)
        {
            wpInstance.currentAmmo = wpInstance.currentReserve;
            wpInstance.currentReserve = 0;
        }
    }

    // Update ammo count to the UI Itself
    void SetWeaponUIStats()
    {
        if (selectedWeapon >= 0 && weapons.Count > 0)
        {
            GameObject.Find("Primary Ammo").GetComponent<TextMeshProUGUI>().text = weapons[selectedWeapon].currentAmmo.ToString();
            GameObject.Find("Reserve Ammo").GetComponent<TextMeshProUGUI>().text = weapons[selectedWeapon].currentReserve.ToString();
            GameObject.Find("GunImage").GetComponent<Image>().sprite = weapons[selectedWeapon].weaponSprite;
            GameObject.Find("GunName").GetComponent<TextMeshProUGUI>().text = weapons[selectedWeapon].weaponName;
        }
        else
        {
            GameObject.Find("Primary Ammo").GetComponent<TextMeshProUGUI>().text = "???";
            GameObject.Find("Reserve Ammo").GetComponent<TextMeshProUGUI>().text = "???";
            GameObject.Find("GunImage").GetComponent<Image>().sprite = noGunSprite;
            GameObject.Find("GunName").GetComponent<TextMeshProUGUI>().text = "??????";
        }
    }

    void SetDampedPos(int weaponID, float targetPosX, float targetPosY)
    {
        Vector3 newVec = new Vector3(targetPosX, targetPosY, weapons[weaponID].transform.position.z);
        weapons[weaponID].transform.position = newVec;
    }
}
