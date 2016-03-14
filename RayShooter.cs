using UnityEngine;
using System.Collections;

public class RayShooter : MonoBehaviour {
	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void OnGUI() {
		int size = 12;
		float posX = (_camera.pixelWidth * 0.5f) - (size * 0.25f);
		float posY = (_camera.pixelHeight * 0.5f) - (size * 0.5f);
		GUI.Label (new Rect (posX, posY, size, size), "*");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 point = new Vector3(_camera.pixelWidth * 0.5f, _camera.pixelHeight * 0.5f, 0f);
			Ray ray = _camera.ScreenPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Debug.Log ("Hit " + hit.point);
				StartCoroutine(SphereIndicator(hit.point));
			}
		}
	}

	private IEnumerator SphereIndicator(Vector3 pos) {

		float radius = 0.2f;
		for (int i = 0; i < 10; i++) {
			drawRing (pos.x,pos.y,pos.z,radius);
			yield return new WaitForSeconds(0.2f);
			radius += 0.3f;
		}
	}

	private void drawRing(float A,float B,float z, float radius) {
		// (x-A)^2 + (y-B)^2 = radius^2
		// r^2 - (x-A)^2 = (y-B)^2
		// y = B +- sqrt(radius^2 - (x-A)^2)
		float radsquared = radius * radius;

		for (float x = (A-radius); x < (A+radius); x=x+(radius / 100)) {
			float xMinusASquared = ((x-A)*(x-A));
			if (radsquared > xMinusASquared) {
				GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
				float y = B + Mathf.Sqrt(radsquared - xMinusASquared);
				float y2 = B - Mathf.Sqrt(radsquared - xMinusASquared);
				sphere.transform.position = new Vector3(x,y,z);
				GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere2.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
				sphere2.transform.position = new Vector3(x,y2,z);
			}
		}
	}
}
