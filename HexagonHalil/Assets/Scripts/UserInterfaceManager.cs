using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UserInterfaceManager : GridClass {

	public Text score;
	public Text widthValueText;
	public Text heightValueText;
	public Text colorCountText;
	public Slider widthSlider;
	public Slider heightSlider;
	public Slider colorCountSlider;
	public GameObject preparationScreen;
	public GameObject colorSelectionParent;
	public GameObject gameOverScreen;
	public List<Color> availableColors;
	public bool tick;

	private GridManager GridManagerObject;
	private int colorCount;
	private int blownHexagons;
	private int bombCount;

	public static UserInterfaceManager instance;



	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	void Start () {
		bombCount = 0;
		GridManagerObject = GridManager.instance;
		blownHexagons = 0;
		colorCount = 7;
		InitializeUI();


	}
	
	void Update () {
		if (tick) {
			StartGameButton();  
			tick = false;
		}
	}


    // Calculatibg score with score const.
	public void Score(int x) {
		blownHexagons += x;
		score.text = (SCORE_CONSTANT * blownHexagons).ToString();
		if (Int32.Parse(score.text) > BOMB_SCORE_THRESHOLD*bombCount + BOMB_SCORE_THRESHOLD) {
			++bombCount;
			GridManagerObject.SetBombProduction();
		}
	}

    // Gameover pops up when the game end. 
	public void GameEnd() {
		gameOverScreen.SetActive(true);
	}

	public void BackButton (string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

    // Setting width of the grid.
	public void WidthSliderChange() {
		widthValueText.text = ((widthSlider.value-MINIMUM_GRID_WIDTH)*2 + MINIMUM_GRID_WIDTH).ToString(); 
	}

    // Seting height of the grid.
	public void HeightSliderChange() {
		heightValueText.text = heightSlider.value.ToString();
	}

    // Choosing colors with color slider.
	public void ColorCountSliderChange() {
		int childCount = colorSelectionParent.transform.childCount;
		int newCount = (int)colorCountSlider.value;
		colorCountText.text = newCount.ToString();
		

		if (newCount > colorCount) {
			for (int i=0; i<childCount; ++i) {
				if (!colorSelectionParent.transform.GetChild(i).gameObject.activeSelf) {
					colorSelectionParent.transform.GetChild(i).gameObject.SetActive(true);
					break;
				}
			}
		}

		else if (newCount < colorCount) {
			for (int i = 0; i<childCount; ++i) {
				if (i+1>=childCount) {
					colorSelectionParent.transform.GetChild(i).gameObject.SetActive(false);
					break;
				}

				else if (!colorSelectionParent.transform.GetChild(i+1).gameObject.activeSelf) {
					colorSelectionParent.transform.GetChild(i).gameObject.SetActive(false);
					break;
				}
			}
		}

		colorCount = newCount;
	}


    // Starting game and setting grids with values that comes from sliders.
    public void StartGameButton() {
		preparationScreen.SetActive(false);
		GridManagerObject.SetGridHeight((int)heightSlider.value);
		GridManagerObject.SetGridWidth((int)(widthSlider.value-MINIMUM_GRID_WIDTH)*2 + MINIMUM_GRID_WIDTH);

		List<Color> colors = new List<Color>();
		List<Color> chosenColors = new List<Color>();


        colors.Add(Color.white);
        colors.Add(Color.red);
        colors.Add(Color.blue);
		colors.Add(Color.yellow);
        colors.Add(Color.cyan);
        colors.Add(Color.green);
        colors.Add(Color.black);

        //Adding chosen colors from list that includes all colors.
        chosenColors = colors.GetRange(0, colorCount);

		GridManagerObject.SetColorList(chosenColors);
		GridManagerObject.InitializeGrid();
	}



	private void InitializeUI() {
		Default();

		for (int i=0; i<colorSelectionParent.transform.childCount-colorCount; ++i) {
			colorSelectionParent.transform.GetChild(colorSelectionParent.transform.childCount -i -1).gameObject.SetActive(false);
		}
	}



	public void Default() {
		heightSlider.value = DEFAULT_GRID_HEIGHT;
		widthSlider.value = DEFAULT_GRID_WIDTH;
		colorCountSlider.value = DEFAULT_COLOR_COUNT;
		colorCount = DEFAULT_COLOR_COUNT;
		widthValueText.text = ((widthSlider.value-MINIMUM_GRID_WIDTH)*2 + MINIMUM_GRID_WIDTH).ToString();
		heightValueText.text = heightSlider.value.ToString();
		score.text = blownHexagons.ToString();




	}
}
