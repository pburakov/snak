using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    Vector2 direction = Vector2.right;
    List<Transform> tail = new List<Transform>();
    bool ate = false;
    public GameObject tailPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Move", 0.15f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            direction = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            direction = -Vector2.up; // '-up' means 'down'
        else if (Input.GetKey(KeyCode.LeftArrow))
            direction = -Vector2.right; // '-right' means 'left'
        else if (Input.GetKey(KeyCode.UpArrow))
            direction = Vector2.up;
    }

    void Move()
    {
        Vector2 currentPos = transform.position;
        // Move head one pixel into the direction
        // (will create a gap between the head and the first element of the tail)
        transform.Translate(direction);

        if (ate)
        {
            GameObject tailPiece = (GameObject)Instantiate(tailPrefab, currentPos, Quaternion.identity);
            tail.Insert(0, tailPiece.transform);
            ate = false;
        }
        else
        if (tail.Count > 0) // is there a tail?
        {
            // Move last element to a gap between the head
            // and the first element of the tail
            tail.Last().position = currentPos;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("FoodPrefab"))
        {
            ate = true;
            Destroy(collision.gameObject);
        }
        else // collided with tail or border
        {
            direction = Vector2.zero;
        }
    }
}
