using UnityEngine;
using System.Collections;

public class bullet_ : MonoBehaviour {
    
    // Use this for initialization
    void Awake() {
    }
	void Start ()
    {
        Destroy(gameObject,2);
	}
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(new Vector3(0,0,1));
	}
}
