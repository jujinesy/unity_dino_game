using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_System : MonoBehaviour {

	public int Current_State;

	public Terrain_Movement Terrain_Script;
	public GameObject Dino;
	public GameObject Landscape;
	public GameObject Dino_Image;
	public GameObject Main_Menu;
	public GameObject SHOP;
	public Button btnUp;
	public Button btnDown;
	public GameObject TapToStartRestart;
	public GameObject Blink_Text_Overlap;
	public GameObject Score_Overlap;
	public GameObject Pause_Overlap;
	public Dino_Animation Dino_Anim;
	public bool Bow_Down = false;
	public GameObject GameOver_Overlap;
	public bool Is_Game_Over = false;
	public Text HighScore_Text;
	public AudioSource Background_Audio;
	public Text Score_Text;
	public Text My_Score;
	public float Added_Speed = 0;
	public Button btnContinue;
	public Button btnMain;
	public bool bContine=false;


	public int Coin = 0;

	void Awake() {
		DontDestroyOnLoad (this.gameObject);	
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
	}

	// Use this for initialization
	void Start () {
		Current_State = 0;
		Perform_Current_State();
		if (PlayerPrefs.HasKey("Highscore") == true)
		{
			int H = PlayerPrefs.GetInt("Highscore");
			HighScore_Text.text = "HI " + H.ToString("D6");
		}
		else
		{
			HighScore_Text.text = "HI 000000";
		}
		Coin = PlayerPrefs.GetInt("Coin", 0);
	}

	public void SaveCoin(){
		PlayerPrefs.SetInt("Coin", Coin);
	}

	void Perform_Current_State()
	{
		if (Current_State == 0)
		{
			Landscape.SetActive(true);
			btnUp.gameObject.SetActive (false);
			btnDown.gameObject.SetActive (false);
			btnContinue.gameObject.SetActive (false);
			btnMain.gameObject.SetActive (false);
			TapToStartRestart.SetActive (false);
			Terrain_Script.enabled = false;
			Dino.SetActive(false);
		}
		else if (Current_State == 1)
		{
			btnUp.gameObject.SetActive (false);
			btnDown.gameObject.SetActive (false);
			btnContinue.gameObject.SetActive (false);
			btnMain.gameObject.SetActive (false);
			Dino.SetActive(true);
			TapToStartRestart.SetActive (true);
			Dino_Image.GetComponent<Dino_Animation>().Starting = true;
			Dino_Image.GetComponent<Dino_Animation>().Dino_Animation_Type = 0;
			Dino_Image.GetComponent<Dino_Animation>().Jumped = false;
			Dino_Image.GetComponent<Dino_Animation>().Jumping = false;
			Dino_Image.GetComponent<Dino_Animation>().Y_Acceleration = 0;
			if (!bContine) {	
				Dino_Image.GetComponent<Dino_Animation> ().Score_Value = 0;
				Dino_Image.GetComponent<Dino_Animation> ().Current_Score.text = "000000";
			}
			bContine = false;

			Terrain_Script.Visible_All();

			Vector3 p_pos = Dino.transform.localPosition;
			p_pos.y = -30;
			Dino.transform.localPosition = p_pos;

			Terrain_Script.Move_Terrain = false;
			Terrain_Script.Create_Objects = false;
			Terrain_Script.enabled = true;
			Blink_Text_Overlap.SetActive(true);
			Main_Menu.SetActive(false);
		}
		else if (Current_State == 2)
		{
			TapToStartRestart.SetActive (false);
			btnUp.gameObject.SetActive (true);
			btnDown.gameObject.SetActive (true);
			btnContinue.gameObject.SetActive (false);
			btnMain.gameObject.SetActive (false);
			Pause_Overlap.SetActive(false);
			Terrain_Script.Move_Terrain = true;
			Terrain_Script.Create_Objects = true;
			Dino_Image.GetComponent<Dino_Animation>().enabled = true;
		}
		else if (Current_State == 3)
		{
			TapToStartRestart.SetActive (true);
			btnUp.gameObject.SetActive (false);
			btnDown.gameObject.SetActive (false);
			btnContinue.gameObject.SetActive (false);
			btnMain.gameObject.SetActive (false);
			Pause_Overlap.SetActive(true);
			Terrain_Script.Move_Terrain = false;
			Terrain_Script.Create_Objects = false;
			Dino_Image.GetComponent<Dino_Animation>().enabled = false;
		}
		else if (Current_State == 4)
		{
			TapToStartRestart.SetActive (false);
			btnUp.gameObject.SetActive (false);
			btnDown.gameObject.SetActive (false);
			btnContinue.gameObject.SetActive (true);
			btnMain.gameObject.SetActive (true);
			GameOver_Overlap.SetActive(true);
			Terrain_Script.Move_Terrain = false;
			Terrain_Script.Create_Objects = false;

			Score_Overlap.SetActive (false);


			if (PlayerPrefs.HasKey("Highscore") == false)
			{
				PlayerPrefs.SetInt("Highscore", Dino_Image.GetComponent<Dino_Animation>().Score_Value);
			}
			else
			{
				int S_Val = PlayerPrefs.GetInt("Highscore");
				int Cur_Val = Dino_Image.GetComponent<Dino_Animation>().Score_Value;
				if (Cur_Val > S_Val)
				{
					PlayerPrefs.SetInt("Highscore", Cur_Val);
				}
			}

			int New_H = PlayerPrefs.GetInt("Highscore");
			HighScore_Text.text = "HI " + New_H.ToString("D6");

			My_Score.text = "Score "+Dino_Image.GetComponent<Dino_Animation> ().Score_Value.ToString("D6");
			Score_Text.text=HighScore_Text.text;

			Dino_Image.GetComponent<Dino_Animation>().Do_Die();

			//Save highscore

			//Update highscore
		}
	}

	public void Screen_Starting()
	{
		Blink_Text_Overlap.SetActive(false);
		Score_Overlap.SetActive(true);
	}

	public void New_Game_Click()
	{
		if (Current_State == 0)
		{
			Current_State = 1;
			Perform_Current_State();
		}
	}

	public void SHOP_Click()
	{
		Main_Menu.SetActive (false);
		SHOP.SetActive (true);
	}

	public void SHOP_BACK()
	{
		Main_Menu.SetActive (true);
		SHOP.SetActive (false);
	}

	public void EXIT_Click()
	{
		if (Current_State == 0)
		{
			Application.Quit();
		}
	}

	public void Start_Game()
	{
		if (Current_State == 1)
		{
			Current_State = 2;
			Perform_Current_State();
		}
	}

	public void Touch_Down_Up()
	{
		if (Current_State == 2)
		{
			Dino_Anim.Bow_Pressed = false;
			Bow_Down = false;
		}
	}

	public void Game_Over()
	{
		if (Current_State == 2)
		{
			Background_Audio.Stop();
			Bow_Down = false;
			Dino_Anim.Bow_Pressed = false;
			Is_Game_Over = true;
			Current_State = 4;
			Perform_Current_State();
		}
	}

	public void Replay_Game()
	{
		if (Coin > 0) {
			Coin--;
			SaveCoin ();
			if (Current_State == 4) {
				Added_Speed = 0;
				Background_Audio.loop = true;
				Background_Audio.Play ();
				Is_Game_Over = false;
				Current_State = 1;
				Terrain_Script.Clear_All ();
				GameOver_Overlap.SetActive (false);
				bContine = true;

				Perform_Current_State ();
			}
		}
	}

	public void Ready_Main()
	{
		if (Current_State == 4) {
			Added_Speed = 0;
			Background_Audio.loop = true;
			Background_Audio.Play ();
			Is_Game_Over = false;
			Current_State = 0;
			Terrain_Script.Clear_All ();
			GameOver_Overlap.SetActive (false);
			Main_Menu.SetActive (true);

			btnUp.gameObject.SetActive (false);
			btnDown.gameObject.SetActive (false);

			TapToStartRestart.SetActive (false);
			Terrain_Script.enabled = false;
			Dino.SetActive (false);

			Terrain_Script.InVisible_All ();

			Blink_Text_Overlap.SetActive (false);



			Perform_Current_State ();
		}
	}

	public void Touch_Down_Pressed()
	{
		if (Current_State == 4) 
		{
			//Replay_Game ();
		}
		else if (Current_State == 3)
		{
			Current_State = 2;
			Perform_Current_State();
		}
		else if (Current_State == 2)
		{
			Bow_Down = true;
			Dino_Anim.Bow_Pressed = true;

			Dino_Anim.Do_Bow();
		}
		else if (Current_State == 1)
		{
			Debug.Log ("1\n");
			Bow_Down = false;
			Dino_Anim.Do_Jump();
		}
	}

	public void Touch_Up()
	{
		if (Current_State == 4) 
		{
			//Replay_Game ();
		}
		else if (Current_State == 3)
		{
			Current_State = 2;
			Perform_Current_State();
		}
		else if (Current_State == 2)
		{
			Dino_Anim.Bow_Pressed = false;
			Bow_Down = false;
			Dino_Anim.Do_Jump();
		}
		else if (Current_State == 1)
		{
			Debug.Log ("2\n");
			Bow_Down = false;
			Dino_Anim.Do_Jump();
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (Current_State == 2)
		{
			Current_State = 3;
			Perform_Current_State();
		}
	}

	public void Pause_Game()
	{
		if (Current_State == 2)
		{
			Current_State = 3;
			Perform_Current_State();
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	// Use this for initialization
	void OnGUI(){
		AutoResize (800, 480);
		//
		//		GUI.skin = newSkin;
		GUI.skin.button.fontSize = 22;
		GUI.skin.box.fontSize = 22;
		GUI.skin.box.alignment = TextAnchor.MiddleCenter;

		if (GUI.Button (new Rect (50, 5, 100, 30), "COIN")) {
		}
		GUI.Box (new Rect (150, 5, 100, 30), "x" + Coin);
	}

	static void AutoResize(int screenWidth, int screenHeight)
	{
		Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
	}

	public void Pay100(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 1;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071697", "100", "100");
	}

	public void Pay500(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 5;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071698", "500", "500");
	}

	public void Pay1000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 10;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071699", "1000", "1000");
	}

	public void Pay5000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 50;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071700", "5000", "5000");
	}

	public void Pay10000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 100;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071701", "10000", "10000");
	}

	public void Pay50000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 500;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071702", "50000", "50000");
	}

	public void Pay100000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 1000;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071703", "100000", "100000");
	}

	public void Pay300000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 3000;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071704", "300000", "300000");
	}

	public void Pay3000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 30;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071705", "3000", "3000");
	}

	public void Pay8000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 80;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071706", "8000", "8000");
	}

	public void Pay110000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 1100;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071707", "110000", "110000");
	}

	public void Pay220000(){
		GameObject.Find("Game_System").GetComponent<IapSample>().Pay = 2200;
		GameObject.Find("Game_System").GetComponent<IapSample>().RequestPaymenet("OA00712253", "0910071708", "220000", "220000");
	}
}