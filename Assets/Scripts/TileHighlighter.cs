using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    List<GameObject> collidedTiles;

    // Use this for initialization
    void Start()
    {
        collidedTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Enter something " + other.gameObject);
        if(other.gameObject.layer == 8)
        {
            //collidedTiles.Add(other.gameObject);
            //collidedTiles.Sort((l, r) =>
            //    ((l.transform.position - transform.position).sqrMagnitude - (l.transform.position - transform.position).sqrMagnitude).Sgn());
            //collidedTiles[0].gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(1, 0, 1);
            other.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(0, 0, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print("Exit something " + other.gameObject);
        if (other.gameObject.layer == 8)
        {
            //collidedTiles.Remove(other.gameObject);
            //collidedTiles.Sort((l, r) =>
            //    ((l.transform.position - transform.position).sqrMagnitude - (l.transform.position - transform.position).sqrMagnitude).Sgn());
            //collidedTiles[0].gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(1, 0, 1);
            //other.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(1, 1, 1);
            other.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(1, 1, 1);
        }
    }
}
