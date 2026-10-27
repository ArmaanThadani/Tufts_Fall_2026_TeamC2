using UnityEngine;

public class Camera3DLERP : MonoBehaviour {
    private Transform target;     // drag intended Player object into Inspector slot
    public float smoother = 5f;
    public Vector3 offset = new Vector3(4, 2, 0);     // adjust the offset in the editor

    void Start(){
       target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate () {
       Vector3 newPos = target.position + offset;
       Vector3 smPos = Vector3.Lerp (transform.position, newPos, smoother * Time.deltaTime);
       transform.position = smPos;

       transform.LookAt (target);
    }
} 