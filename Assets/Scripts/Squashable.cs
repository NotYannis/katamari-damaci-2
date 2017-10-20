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
        if(collision.gameObject.tag == "Player") {
            if (!_isSquashing) {
                GetComponent<Collider>().enabled = false;
                StartCoroutine(SquashEffect());
            }
        }
    }
    
    public IEnumerator SquashEffect() {
        AkSoundEngine.PostEvent("ecraser", gameObject);
        _isSquashing = true;
        float t = 0f;
        Vector3 scale = new Vector3();

        Vector3 baseScale = transform.parent.localScale;
        Vector3 endScale = new Vector3(1f, 0.05f, 1f);

        while (t < 1) {
            t += Time.deltaTime / _squashRate;
            scale = Vector3.Lerp(baseScale, endScale, t);
            transform.parent.localScale = scale;
            yield return null;
        }

        StartCoroutine(Reflate());
    }

    public IEnumerator Reflate() {
        yield return new WaitForSeconds(2f);
        AkSoundEngine.PostEvent("gonfler", gameObject);

        float t = 0f;
        Vector3 scale = new Vector3();

        Vector3 baseScale = transform.parent.localScale;
        Vector3 endScale = new Vector3(1f, 1f, 1f);

        while (t < 1) {
            t += Time.deltaTime / _squashRate;
            scale = Vector3.Lerp(baseScale, endScale, t);
            transform.parent.localScale = scale;
            yield return null;
        }

        GetComponent<Collider>().enabled = true;
        _isSquashing = false;
    }
}
