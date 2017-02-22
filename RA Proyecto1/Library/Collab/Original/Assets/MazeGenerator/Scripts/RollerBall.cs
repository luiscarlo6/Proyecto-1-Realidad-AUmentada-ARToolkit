using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//<summary>
//Ball movement controlls and simple third-person-style camera
//</summary>
public class RollerBall : MonoBehaviour {
    public Text timerText;
    public Text monedaText;
    public Text ganasteText;
    public Button tryButton;
    public Image imagenGanasteIzq;
    public Image imagenGanasteDer;

    //public float tiempo = 0.0f;
    private float startTime;
    public GameObject ViewCamera = null;
	public AudioClip JumpSound = null;
	public AudioClip HitSound = null;
	public AudioClip CoinSound = null;
    private GUIStyle style = new GUIStyle();
    private Rigidbody mRigidBody = null;
	private AudioSource mAudioSource = null;
	private bool mFloorTouched = false;

	public int numMonedas = 0;

	void Start () {
        startTime = Time.time;
        mRigidBody = GetComponent<Rigidbody> ();
		mAudioSource = GetComponent<AudioSource> ();
        timerText.text = PlayerPrefs.GetString("MejorScore");
        ganasteText.enabled = true;
        tryButton.gameObject.GetComponent<Renderer>().enabled = true;
        imagenGanasteDer.gameObject.SetActive(true);
		imagenGanasteIzq.gameObject.GetComponent<Renderer>().enabled = true;
	}
    private void Update()
    {
    	monedaText.text = PlayerPrefs.GetString("MejorScore");
        //monedaText.text = "Monedas: " + numMonedas;
        if (numMonedas == 1) {

        	ganasteText.enabled      = true;
        	tryButton.enabled 		 = true;
            imagenGanasteDer.gameObject.SetActive(true);

            imagenGanasteIzq.gameObject.SetActive(true);
        	return;
        }
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
        PlayerPrefs.SetString("MejorScore", timerText.text);

    }
    
    void FixedUpdate () {
		if (mRigidBody != null) {
			if (Input.GetButton ("Horizontal")) {
				mRigidBody.AddTorque(Vector3.back * Input.GetAxis("Horizontal")*10);
			}
			if (Input.GetButton ("Vertical")) {
				mRigidBody.AddTorque(Vector3.right * Input.GetAxis("Vertical")*10);
			}
			if (Input.GetButtonDown("Jump")) {
				if(mAudioSource != null && JumpSound != null){
					mAudioSource.PlayOneShot(JumpSound);
				}
				mRigidBody.AddForce(Vector3.up*200);
			}
		}
		if (ViewCamera != null) {
			Vector3 direction = (Vector3.up*2+Vector3.back)*2;
			RaycastHit hit;
			Debug.DrawLine(transform.position,transform.position+direction,Color.red);
			if(Physics.Linecast(transform.position,transform.position+direction,out hit)){
				ViewCamera.transform.position = hit.point;
			}else{
				ViewCamera.transform.position = transform.position+direction;
			}
			ViewCamera.transform.LookAt(transform.position);
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = true;
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		} else {
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		}
	}

	void OnCollisionExit(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Coin")) {
			if(mAudioSource != null && CoinSound != null){
				mAudioSource.PlayOneShot(CoinSound);
			}
			Destroy(other.gameObject);
			numMonedas += 1;
		}
	}
/*
	public void OnGUI()
	{
        Debug.Log(numMonedas);
        
        if (numMonedas == 8) {
            
            style.fontSize = 150;
            GUI.Label(new Rect(300, 150, 300, 90), "Ganaste!", style);
        }
        style.fontSize = 30;
        GUI.Label(new Rect(700, 10, 300, 90), "Monedas: " + numMonedas,style);
	}*/

}
