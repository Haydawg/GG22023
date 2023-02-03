using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{

    enum MovementMode
    {
        mouse,
        keyboard
    };

    [SerializeField] MovementMode movementType;
    [SerializeField] float speed;
    [SerializeField] Vector2 move;
    Camera cam;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
        
        switch (movementType)
        {
            case MovementMode.keyboard:
                move = Vector2.zero;
                move += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                controller.Move(move * Time.deltaTime);
                break;
            case MovementMode.mouse:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    move = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                if (move == new Vector2(transform.position.x, transform.position.y))
                {
                    move = Vector2.zero;
                }
                controller.Move(move * Time.deltaTime);


                break;
        }

    }
}
