using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;

    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float maxGoalSpeed = 4f;

    private bool isDragging;
    private bool inHole;

    private void Update() {
        PlayerInput();
    }

    private bool IsReady() {
        return rb.velocity.magnitude <= 0.2f;
    }

    private void PlayerInput() {
        if (!IsReady()) return;

        Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPosition);

        if (Input.GetMouseButtonDown(0) && distance <= 0.5f) DragStart();
        if (Input.GetMouseButton(0) && isDragging) DragChange(inputPosition);
        if (Input.GetMouseButtonUp(0) && isDragging) DragEnd(inputPosition);
    }

    private void  DragStart() {
        isDragging = true;
        lr.positionCount = 2;
    }   

    private void DragChange(Vector2 pos) {
        Vector2 direction = (Vector2)transform.position - pos;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude((direction * power) / 2, maxPower / 2));
    }

    private void DragEnd(Vector2 pos) {
        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isDragging = false;

        lr.positionCount = 0;   

        if (distance < 1f) {
            return;
        }

        Vector2 direction = (Vector2)transform.position - pos;

        rb.velocity = Vector2.ClampMagnitude(direction * power, maxPower);
    }

    private void CheckWinState() {
        if (inHole) return;

        if (rb.velocity.magnitude <= maxGoalSpeed) {
            inHole = true;

            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);

            //Level Complete
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Hole") CheckWinState();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Hole") CheckWinState();
    }

}
