using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView photonView;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public TextMeshProUGUI PlayerNameText;

    public bool isGrounded = false;
    public float moveSpeed;
    public float jumpForce;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            PlayerCamera.SetActive(false);
            PlayerNameText.text = photonView.Owner.NickName;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }
        else
        {
            SmoothMove();
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
            anim.SetFloat("H", x);
            anim.SetFloat("V", y);
        }

        rb.velocity = new Vector2(x, y) * moveSpeed;

        if (x > 0.1)
        {
            photonView.RPC("FlipFalse", RpcTarget.AllBuffered);
        }
        else if (x < -0.1)
        {
            photonView.RPC("FlipFalse", RpcTarget.AllBuffered);
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }
}
