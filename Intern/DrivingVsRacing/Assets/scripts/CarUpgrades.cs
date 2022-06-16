using UnityEngine;
using UnityEngine.UI;

public class CarUpgrades : MonoBehaviour
{
    public int xPointer = -520; // được sử dụng cho giá trị x của các tấm bạt
    public vehicleList list;
    public GameObject chilldObject;
    [Header("exitPanelButton")]
    public GameObject exitPanelButton;

    [Header("enginePanel")]
    public GameObject engineUpgrade;
    public Text enginePrice;
    public bool engineFlag = false;

    [Header("spoilerPanel")]
    public GameObject spoilerUpgrade;
    public Text spoilerPrice;
    public bool spoilerFlag;
    
    [Header("nitrusPanel")]
    public GameObject nitrusUpgrade;
    public Text nitrusPrice;
    public bool nitrusFlag;

    [Header("colorPanel")]
    public GameObject colorUpgrade;
    public Text colorPrice;
    public bool colorFlag;

    [Header("wheelPanel")]
    public GameObject wheelUpgrade;
    public Text wheelPrice;
    public bool wheelFlag;

    [Header("handlingPanel")]
    public GameObject handlingUpgrade;
    public Text handlingPrice;
    public bool handlingFlag;

    public GameObject[] generatedArray;

    private void Start(){
        organiseUpgradesPanel();
    }
    private void disablePanel(){
        engineUpgrade.SetActive(false);
        spoilerUpgrade.SetActive(false);
        nitrusUpgrade.SetActive(false);
        colorUpgrade.SetActive(false);
        wheelUpgrade.SetActive(false);
        handlingUpgrade.SetActive(false);
    }

    private void organiseUpgradesPanel() {
        xPointer = -520;
        if (list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().engineUpgrade){
            engineUpgrade.SetActive(true);
            engineUpgrade.transform.localPosition = new Vector3(xPointer, 0);
            enginePrice.text = "";
            xPointer += 210;
        }        
        if (list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().handlingUpgrade){
            handlingUpgrade.SetActive(true);
            handlingPrice.text = "";
            handlingUpgrade.transform.localPosition = new Vector3(xPointer, 0);
            xPointer += 210;
        }        
        if (list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().nitrusUpgrade){
            nitrusUpgrade.SetActive(true);
            nitrusPrice.text = "";
            nitrusUpgrade.transform.localPosition = new Vector3(xPointer, 0);
            xPointer += 210;
        }      
        if (list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().wheelUpgrade){
            wheelUpgrade.SetActive(true);
            wheelPrice.text = "";
            wheelUpgrade.transform.localPosition = new Vector3(xPointer, 0);
            xPointer += 210;
        }       
        if (list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().spoilerUpgrade){
            spoilerUpgrade.SetActive(true);
            spoilerPrice.text = "";
            spoilerUpgrade.transform.localPosition = new Vector3(xPointer, 0);
            xPointer += 210;
        }
        if (list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().colorUpgrade){
            colorUpgrade.SetActive(true);
            colorPrice.text = "";
            colorUpgrade.transform.localPosition = new Vector3(xPointer, 0);
            xPointer += 210;
        }


    }

    public void backButton(){
        if (generatedArray.Length == 0) return ;
        for(int i = 0;i<generatedArray.Length;i++){
            Destroy(generatedArray[i].gameObject);
        }
        generatedArray = null;
        exitPanelButton.SetActive(true);
        organiseUpgradesPanel();
        engineFlag = false;
        spoilerFlag = false;
        nitrusFlag = false;
        handlingFlag = false;
        colorFlag = false;
        wheelFlag = false;
    }

    // các nút nâng cấp>
    public void engineUpgradeButton(){
        if (engineFlag) return;
        xPointer = -520;
        int[] length = list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().P_engineUpgrade;
        generatedArray = new GameObject[length.Length];
        for (int i = 0; i < length.Length ; i++) {
            generateArray("engineUpgrade", engineUpgrade, length, i, enginePrice,
                //list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_engineUpgrade
                PlayerPrefs.GetInt((list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName + "engineUpgrade").ToString())
                ) ;
        }

        engineFlag = true;
        disablePanel();
        exitPanelButton.SetActive(false);
    }
    
    public void spoilerUpgradeButton(){
        if (spoilerFlag) return;
        xPointer = -520;
        int[] length = list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().P_spoilerUpgrade;
        generatedArray = new GameObject[length.Length];
        for (int i = 0; i < length.Length ; i++) {
            generateArray("spoilerUpgrade", spoilerUpgrade, length,i, spoilerPrice,
                PlayerPrefs.GetInt((list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName + "spoilerUpgrade").ToString())
                );
        }

        spoilerFlag = true;
        disablePanel();
        exitPanelButton.SetActive(false);
    }
   
    public void nitrusUpgradeButton(){
        if (nitrusFlag) return;
        xPointer = -520;
        int[] length = list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().P_nitrusUpgrade;
        generatedArray = new GameObject[length.Length];
        for (int i = 0; i < length.Length ; i++) {
            generateArray("nitrusUpgrade", nitrusUpgrade, length,i, nitrusPrice,
                PlayerPrefs.GetInt((list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName + "nitrusUpgrade").ToString())
                );
        }

        nitrusFlag = true;
        disablePanel();
        exitPanelButton.SetActive(false);
    }
   
    public void colorUpgradeButton(){
        if (colorFlag) return;
        xPointer = -520;
        int[] length = list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().P_colorUpgrade;
        generatedArray = new GameObject[length.Length];
        for (int i = 0; i < length.Length ; i++) {
            generateArray("colorUpgrade", colorUpgrade, length,i, colorPrice,
                PlayerPrefs.GetInt((list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName + "colorUpgrade").ToString())
                );
        }

        colorFlag = true;
        disablePanel();
        exitPanelButton.SetActive(false);
    }   
  
    public void wheelUpgradeButton(){
        if (wheelFlag) return;
        xPointer = -520;
        int[] length = list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().P_wheelUpgrade;
        generatedArray = new GameObject[length.Length];
        for (int i = 0; i < length.Length ; i++) {
            generateArray("wheelUpgrade", wheelUpgrade, length,i, wheelPrice,
                PlayerPrefs.GetInt((list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName + "wheelUpgrade").ToString())
                );
        }

        wheelFlag = true;
        disablePanel();
        exitPanelButton.SetActive(false);
    }   
    public void handlingUpgradeButton(){
        if (handlingFlag) return;
        xPointer = -520;
        int[] length = list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().P_handlingUpgrade;
        generatedArray = new GameObject[length.Length];
        for (int i = 0; i < length.Length ; i++) {
            generateArray("handlingUpgrade", handlingUpgrade, length,i, handlingPrice,
                PlayerPrefs.GetInt((list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<controller>().carName + "handlingUpgrade").ToString())
                );
        }

        handlingFlag = true;
        disablePanel();
        exitPanelButton.SetActive(false);
    }
    
    private void buy(string testUpgradeString, int i, int k,int upgradeType){
        if(PlayerPrefs.GetInt("currency") >= i && upgradeType < k )
        {
            PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") - i);
            applyPurchase(testUpgradeString , k);
            refreshCurrentCanvas();
        }

        if (i == 0)
        {
            applyPurchase(testUpgradeString, k);
            refreshCurrentCanvas();
        }

    }
    // trình tạo ui
    private void generateArray (string testUpgradeString , GameObject obj ,int[] length,int i,Text priceTextBox,int upgradeType){

        if (upgradeType == 0) priceTextBox.text = "selected";
        if(upgradeType == i) priceTextBox.text = "selected";
        else priceTextBox.text =(length[i] != 0)? length[i].ToString() : "owned";

        GameObject childObject = Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
        childObject.transform.parent = chilldObject.transform;
        childObject.transform.localScale = new Vector3(1, 1, 1);
        childObject.transform.localPosition = new Vector3(xPointer, 0);
        childObject.GetComponent<Button>().onClick.AddListener(delegate { buy(testUpgradeString,length[i],i, upgradeType); }) ;
        childObject.SetActive(true);
        generatedArray[i] = childObject;
        xPointer += 210;
    }
    // chức năng làm mới canvas
    private void refreshCurrentCanvas(){
        if (engineFlag) {
            for (int i = 0; i < generatedArray.Length; i++){
                Destroy(generatedArray[i].gameObject);
            }
            generatedArray = null;
            engineFlag = false;
            engineUpgradeButton();
        }
        else if (spoilerFlag){
            for (int i = 0; i < generatedArray.Length; i++){
                Destroy(generatedArray[i].gameObject);
            }
            generatedArray = null;
            spoilerFlag = false;
            spoilerUpgradeButton();
        }   
        else if (nitrusFlag){
            for (int i = 0; i < generatedArray.Length; i++){
                Destroy(generatedArray[i].gameObject);
            }
            generatedArray = null;
            nitrusFlag = false;
            nitrusUpgradeButton();
        }       
        else if (colorFlag){
            for (int i = 0; i < generatedArray.Length; i++){
                Destroy(generatedArray[i].gameObject);
            }
            generatedArray = null;
            colorFlag = false;
            colorUpgradeButton();
        }        
        else if (wheelFlag){
            for (int i = 0; i < generatedArray.Length; i++){
                Destroy(generatedArray[i].gameObject);
            }
            generatedArray = null;
            wheelFlag = false;
            wheelUpgradeButton();
        }        
        else if (handlingFlag){
            for (int i = 0; i < generatedArray.Length; i++){
                Destroy(generatedArray[i].gameObject);
            }
            generatedArray = null;
            handlingFlag = false;
            handlingUpgradeButton();
        }
    
                
                
    }
    private void applyPurchase(string testUpgradeString ,int value){

        switch (testUpgradeString){
            case "engineUpgrade":
                list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_engineUpgrade = value;
                break;       
            case "spoilerUpgrade":
                list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_spoilerUpgrade = value;
                break;         
            case "nitrusUpgrade":
                list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_nitrusUpgrade = value;
                break;       
            case "wheelUpgrade":
                list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_wheelUpgrade = value;
                break;           
            case "colorUpgrade":
                list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_colorUpgrade = value;
                break;           
            case "handlingUpgrade":
                list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().I_handlingUpgrade = value;
                break;           
        }
        list.vehicles[PlayerPrefs.GetInt("pointer")].GetComponent<carModifier>().test();
        //print("k value > "+k + "price Value > " + i);

    }

}