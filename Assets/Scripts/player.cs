using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class player : NetworkBehaviour
{
    private bool onFloor = true;
    private float jump_force = 7F;
    private Rigidbody body;
    public TextMesh playerNameText;
    public GameObject floatingInfo;

    private Material playerMaterialClone;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    void OnColorChanged(Color _Old, Color _New)
    {
        playerNameText.color = _New;
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        body = GetComponent<Rigidbody>();
        floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string _name, Color _col)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name;
        playerColor = _col;
    }
    void Update()
    {
        if (!isLocalPlayer){
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }
    
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);

        if (Input.GetKeyDown(KeyCode.Space) && onFloor){
            body.AddForce(new Vector3(0, jump_force, 0), ForceMode.Impulse);
            onFloor = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Floor")
            onFloor = true;
    }
}
