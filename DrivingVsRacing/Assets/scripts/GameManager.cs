using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    //public vehicle Vehicle;
    public vehicleList list;
    public controller RR;
    public GameObject neeedle;
    public GameObject startPosition;
    public Text kph;
    public Text currentPosition;
    public Text gearNum;
    public Slider nitrusSlider;
    private float startPosiziton = 32f, endPosition = -211f;
    private float desiredPosition;
    private GameObject[] presentGameObjectVehicles, fullArray;

    [Header("countdown Timer")]
    public float timeLeft = 4;
    public Text timeLeftText;

    [Header ("racers list")]
    public GameObject uiList;
    public GameObject uiListFolder;
    public GameObject backImage;
    private List<vehicle> presentVehicles;
    private List<GameObject> temporaryList;
    private GameObject[] temporaryArray;

    private int startPositionXvalue = -50-62;
    private bool arrarDisplayed = false,countdownFlag = false;
    private void Awake () {

        Instantiate (list.vehicles[PlayerPrefs.GetInt ("pointer")], startPosition.transform.position, startPosition.transform.rotation);
        RR = GameObject.FindGameObjectWithTag ("Player").GetComponent<controller> ();

        presentGameObjectVehicles = GameObject.FindGameObjectsWithTag ("AI");

        presentVehicles = new List<vehicle> ();
        foreach (GameObject R in presentGameObjectVehicles)
            presentVehicles.Add (new vehicle (R.GetComponent<inputManager> ().currentNode, R.GetComponent<controller> ().carName,R.GetComponent<controller> ().hasFinished));

        presentVehicles.Add (new vehicle (RR.gameObject.GetComponent<inputManager> ().currentNode, RR.carName , RR.hasFinished));

        temporaryArray = new GameObject[presentVehicles.Count];

        temporaryList = new List<GameObject> ();
        foreach (GameObject R in presentGameObjectVehicles)
            temporaryList.Add (R);
        temporaryList.Add (RR.gameObject);

        fullArray = temporaryList.ToArray ();
        //displayArray ();
        StartCoroutine (timedLoop ());
    }

    private void FixedUpdate () {
        if(RR.hasFinished)displayArray();
        kph.text = RR.KPH.ToString ("0");
        updateNeedle ();
        nitrusUI ();
        coundDownTimer();
    }

    public void updateNeedle () {
        desiredPosition = startPosiziton - endPosition;
        float temp = RR.engineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3 (0, 0, (startPosiziton - temp * desiredPosition));

    }

    public void changeGear () {
        gearNum.text = (!RR.reverse) ? (RR.gearNum + 1).ToString () : "R";

    }

    public void nitrusUI () {
        nitrusSlider.value = RR.nitrusValue / 45;
    }

    private void sortArray () {

        for (int i = 0; i < fullArray.Length; i++) {
            presentVehicles[i].hasFinished = fullArray[i].GetComponent<controller>().hasFinished;
            presentVehicles[i].name = fullArray[i].GetComponent<controller> ().carName;
            presentVehicles[i].node = fullArray[i].GetComponent<inputManager> ().currentNode;
        }

        if(!RR.hasFinished)
        for (int i = 0; i < presentVehicles.Count; i++) {
            for (int j = i + 1; j < presentVehicles.Count; j++) {
                if (presentVehicles[j].node < presentVehicles[i].node) {
                    vehicle QQ = presentVehicles[i];
                    presentVehicles[i] = presentVehicles[j];
                    presentVehicles[j] = QQ;
                }
            }                
        }

        
        if(arrarDisplayed)
        for (int i = 0; i < temporaryArray.Length; i++) {
            temporaryArray[i].transform.Find ("vehicle node").gameObject.GetComponent<Text> ().text = (presentVehicles[i].hasFinished)?"finished":"";
            temporaryArray[i].transform.Find ("vehicle name").gameObject.GetComponent<Text> ().text = presentVehicles[i].name.ToString ();
        }
        presentVehicles.Reverse();
        for (int i = 0; i < temporaryArray.Length; i++) {
            if(RR.carName == presentVehicles[i].name)
                currentPosition.text = ((i+1) + "/" + presentVehicles.Count).ToString();
        }



    }

    private void displayArray () {
        if(arrarDisplayed)return;
        uiList.SetActive(true);
        for (int i = 0; i < presentGameObjectVehicles.Length + 1; i++) {
            generateList (i, presentVehicles[i].hasFinished, presentVehicles[i].name);
        }

        startPositionXvalue = -50;
        arrarDisplayed = true;
        backImage.SetActive(true);
    }

    private void generateList (int index, bool num, string nameValue) {

        temporaryArray[index] = Instantiate (uiList);
        temporaryArray[index].transform.parent = uiListFolder.transform;
        //temporaryArray[index].gameObject.GetComponent<RectTransform>().localScale = new Vector3(2,2,2);
        temporaryArray[index].gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, startPositionXvalue);
        //temporaryArray[index].transform.position = new Vector3(0,startPositionXvalue,0);
        temporaryArray[index].transform.Find ("vehicle name").gameObject.GetComponent<Text> ().text = nameValue.ToString ();
        temporaryArray[index].transform.Find ("vehicle node").gameObject.GetComponent<Text> ().text =(num)?"finished":"";
        startPositionXvalue += 50;

    }

    private IEnumerator timedLoop () {
        while (true) {
            yield return new WaitForSeconds (.7f);
            sortArray ();
        }
    }

    private void coundDownTimer(){
        if(timeLeft <= -5) return;
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)unfreezePlayers();
        else freezePlayers();

        if(timeLeft > 1 )timeLeftText.text = timeLeft.ToString("0");
        else if(timeLeft >= -1 && timeLeft <= 1 )timeLeftText.text = "GO!";
        else timeLeftText.text ="";

    }
    
    private void freezePlayers(){
        if(countdownFlag) return;
            foreach(GameObject D in fullArray){
                D.GetComponent<Rigidbody>().isKinematic = true;
            }
            countdownFlag = true;
        
    }

    private void unfreezePlayers(){
        if(!countdownFlag) return;
            foreach(GameObject D in fullArray){
                D.GetComponent<Rigidbody>().isKinematic = false;
            }
            countdownFlag = false;
        
    }

    public void loadAwakeScene(){
        SceneManager.LoadScene("awakeScene");
    }
}