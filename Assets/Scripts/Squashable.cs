using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squashable : MonoBehaviour {
    private bool _isSquashing = false;
    private float _squashRate = 0f;

    void Start() {
        _squashRate = GameManager.squashRate;
        EventManager.StartListening("squashValueChanged", OnValueChange);
    }

    void Destroy() {
        EventManager.StopListening("squashValueChanged", OnValueChange);
    }

    void OnValueChange() {
        Debug.Log("Value changed");
        _squashRate = GameManager.squashRate;
    }

    public void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision !");
        if(collision.gameObject.tag == "Player") {
            if (!_isSquashing) {
                Destroy(GetComponent<Collider>());
                if(transform.parent != null) {
                    StartCoroutine(transform.parent.GetComponent<Squashable>().SquashEffect());
                } else {
                    StartCoroutine(SquashEffect());
                }
            }
        }
    }
    
    public IEnumerator SquashEffect() {
        _isSquashing = true;
        float t = 0f;
        Vector3 scale = new Vector3();

        Vector3 baseScale = transform.localScale;
        Vector3 endScale = new Vector3(1f, 0.05f, 1f);

        while (t < 1) {
            t += Time.deltaTime / _squashRate;
            scale = Vector3.Lerp(baseScale, endScale, t);
            transform.localScale = scale;
            yield return null;
        }

        if (transform.parent != null) {
            transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            StartCoroutine(transform.parent.GetComponent<Squashable>().Reflate());
        }
        else {
            gameObject.AddComponent<MeshCollider>();
            gameObject.AddComponent<MeshCollider>().convex = true;
            StartCoroutine(Reflate());
        }
    }

    public IEnumerator Reflate() {
        yield return new WaitForSeconds(2f);

        float t = 0f;
        Vector3 scale = new Vector3();

        Vector3 baseScale = transform.localScale;
        Vector3 endScale = new Vector3(1f, 1f, 1f);

        while (t < 1) {
            t += Time.deltaTime / _squashRate;
            scale = Vector3.Lerp(baseScale, endScale, t);
            transform.localScale = scale;
            yield return null;
        }
    }
}
