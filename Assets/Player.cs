using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    int _rayCount = 1;

    Vector2 _velocity = Vector2.zero;

    void Start()
    {
        // Keyboard.current.IsPressed()
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }
}
