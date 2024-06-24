using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Player Object")]
    public GameObject deathParticleSystemPrefab;
    public List<AudioClip> deathSounds = new List<AudioClip>();
    public bool useDebugging;
    public Vector3 startPos = new Vector3(-10.47f, 6.02f, 0f);
    public GameObject _playerObject;
    [Header("Player Stats-Related Variables:")]
    public int deaths;
    public int score;
    public float _maxCatSatisfyAmount = 10f;
    [Header("Player Movement-Related Variables:")]
    public float _playerMaxVelocity = 5f;
    public float _playerSpeed = 1f;
    public float _jumpForce = 5f;

    [Header("Keybinds:")]
    public KeyCode _movementRightKeybind;
    public KeyCode _movementLeftKeybind;
    public KeyCode _jumpKeybind;
    public KeyCode _suicideKeybind;
    public KeyCode _sprintKeybind;
    public KeyCode _aimingKeybind;

    [Header("Default Keybinds:")]
    public KeyCode _defaultMovementRightKeybind = KeyCode.D;
    public KeyCode _defaultMovementLeftKeybind = KeyCode.A;
    public KeyCode _defaultJumpKeybind = KeyCode.Space;
    public KeyCode _defaultSuicideKeybind = KeyCode.Z;
    public KeyCode _defaultSprintKeybind = KeyCode.LeftShift;
    public KeyCode _defaultAimingKeybind = KeyCode.Mouse1;

    // Start is called before the first frame update
    void Start()
    {
        _playerObject = this.gameObject;
        SummonPlayer();
        EmptyKeybindHandler();
    }

    public void SummonPlayer(int died=0){
        switch(died){
            case 1:
            GameObject deathParticles = Instantiate(deathParticleSystemPrefab.gameObject, _playerObject.transform.position, Quaternion.identity);
            deathParticles.GetComponent<ParticleSystem>().Play();
            Destroy(deathParticles, 1f);
            _playerObject.GetComponent<AudioSource>().PlayOneShot(deathSounds[Random.Range(0, deathSounds.Count - 1)]);
            _playerObject.transform.position = startPos;
            deaths += 1;
            break;
        }
        _playerObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        if(!useDebugging)
            _playerObject.transform.position = startPos;
    }

    void EmptyKeybindHandler(){
        if(_movementLeftKeybind == KeyCode.None)
            _movementLeftKeybind = _defaultMovementLeftKeybind;
        if(_movementRightKeybind == KeyCode.None)
            _movementRightKeybind = _defaultMovementRightKeybind;
        if(_jumpKeybind == KeyCode.None)
            _jumpKeybind = _defaultJumpKeybind;
        if(_suicideKeybind == KeyCode.None)
            _suicideKeybind = _defaultSuicideKeybind;
        if(_sprintKeybind == KeyCode.None)
            _sprintKeybind = _defaultSprintKeybind;
        if(_aimingKeybind == KeyCode.None)
            _aimingKeybind = _defaultAimingKeybind;
    }

    void GainScore(int amount){
        score += amount;
    }

    void Update(){
        GameObject.Find("Deaths").GetComponent<TextMeshProUGUI>().text = "Kuolemat: " + deaths.ToString();
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Pisteet: " + score.ToString();

        if(Input.GetKeyDown(_suicideKeybind)){
            GameObject.Find("FloorCheck").GetComponent<FloorCheckInfo>().KillPlayer();
        }
    }
}
