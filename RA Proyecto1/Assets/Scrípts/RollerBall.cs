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
    public Text mejorTiempoText;
    public Text derText1;
    public Text derText2;
    public Text playerTiempoText;
    public Button tryButton;
    public Image imagenGanasteIzq;
    public Image imagenGanasteDer;
    public Image imagenGanasteAba;
    public AudioClip MazeSound = null;
	private AudioSource gameAudioSource = null;
	private bool play = false;
  

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
		gameAudioSource = GetComponent<AudioSource> ();
     
	}
    private void Update()
    {
    	monedaText.text = "Monedas: " + numMonedas;
        playerTiempoText.text = timerText.text;
        float t = Time.time - startTime;
        if (!play){
        	gameAudioSource.PlayOneShot(MazeSound);
        	play = true;
        }
        int minutes = ((int)t / 60);
        float seconds = (t % 60);
        if (numMonedas == 8) {

        	ganasteText.gameObject.SetActive(true);
        	tryButton.gameObject.SetActive(true);
            imagenGanasteDer.gameObject.SetActive(true);
            imagenGanasteIzq.gameObject.SetActive(true);
            imagenGanasteAba.gameObject.SetActive(true);
            mejorTiempoText.gameObject.SetActive(true);
            derText1.gameObject.SetActive(true);
            derText2.gameObject.SetActive(true);
            playerTiempoText.gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("Min"+this.tag) * 60 + PlayerPrefs.GetFloat("Seg"+this.tag) == 0 || PlayerPrefs.GetInt("Min" + this.tag) * 60 + PlayerPrefs.GetFloat("Seg" + this.tag) > minutes * 60 + seconds)
            {
                timerText.text = minutes.ToString() + ":" + seconds.ToString("f2");
                
                PlayerPrefs.SetInt("Min" + this.tag, minutes);
                PlayerPrefs.SetFloat("Seg" + this.tag, seconds);
            }
            mejorTiempoText.text = PlayerPrefs.GetInt("Min" + this.tag).ToString() + ":" + PlayerPrefs.GetFloat("Seg" + this.tag).ToString("f2");
            return;
        }
        timerText.text = minutes.ToString() + ":" + seconds.ToString("f2");
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
}
