using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Level level;

    public float speed = 5f;

    public float gravity = 10f;

    public Direction lastDir = Direction.DOWN;

    private void Update()
    {
        float v = Input.GetAxis("Vertical");

        if (Input.GetAxis("Horizontal") < -0.01)
        {
            Move(Vector2.left);
            lastDir = Direction.LEFT;
        }

        if (Input.GetAxis("Horizontal") > 0.01)
        {
            Move(Vector2.right);
            lastDir = Direction.RIGHT;
        }

        if (v < -0.01) lastDir = Direction.DOWN;
        if (v > 0.01) lastDir = Direction.UP;

        if (Input.GetButtonDown("Fire1"))
        {
            Dig();
        }

        // Gravity
        Gravity();

        Replace();
    }

    public void Gravity()
    {
        Vector2 downCheck = (Vector2)transform.position + Vector2.down / 2f;
        if (!level.IsBlocking(downCheck))
        {
            level.transform.position += (Vector3)(Vector2.up * gravity * Time.deltaTime);
        }
        if (level.IsBlocking(downCheck))
        {
            level.transform.position = new Vector3(Mathf.FloorToInt(level.transform.position.x), Mathf.FloorToInt(level.transform.position.y));
        }
    }

    public void Move(Vector2 dir)
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void Replace()
    {
        Vector2 left = (Vector2)transform.position + Vector2.left / 2f;
        if (level.IsBlocking(left))
        {
            transform.position = new Vector3(Mathf.FloorToInt(transform.position.x) + 0.5f, Mathf.FloorToInt(transform.position.y) + 0.5f);
        }
        Vector2 right = (Vector2)transform.position + Vector2.right / 2f;
        if (level.IsBlocking(right))
        {
            transform.position = new Vector3(Mathf.FloorToInt(transform.position.x) + 0.5f, Mathf.FloorToInt(transform.position.y) + 0.5f);
        }
    }

    public void Dig()
    {
        Vector2 offset = Vector2.down;
        switch (lastDir)
        {
            case Direction.LEFT: offset = Vector2.left; break;
            case Direction.UP: offset = Vector2.up; break;
            case Direction.RIGHT: offset = Vector2.right; break;
            case Direction.DOWN: offset = Vector2.down; break;
        }

        Vector2 digPosition = (Vector2)transform.position + offset;
        level.Dig(digPosition);
    }
}
