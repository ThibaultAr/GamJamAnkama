using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Debug")]
    public bool debugMode = false;
    public int playerDebugNumber = 2;
    private int characterInitialized = 0;

    public GameObject characterPrefabs;
    public Dictionary<int, PlayerScriptableObject> playerList = new Dictionary<int, PlayerScriptableObject>();
    
    public Dictionary<int, bool> playerIsReleaseStick = new Dictionary<int, bool>();
    public Dictionary<int, bool> playerIsReady = new Dictionary<int, bool>();
    public Dictionary<int, bool> playerIsSet = new Dictionary<int, bool>();
    public List<string> selectedCharacters = new List<string>();
    GamePadState state;
    GamePadState prevState;
    float deadZone = 0.8f;

    public int nbOfPlayers;

    private bool _initialized = false;
    

    public List<CharacterSelection> charactersList;

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            _initialized = false;
            for (int i = 0; i < 4; ++i)
            {
                playerIsSet.Add(i, false);
                playerIsReleaseStick.Add(i, true);
                playerIsReady.Add(i, false);
            }
            SceneManager.LoadScene(1);
            return;
        }

        if(scene.buildIndex == 1)
        {
            _initialized = false;
            Init();
        }
            

        if(scene.buildIndex == 2)
        {
            for(int i = 0; i < 4 - playerList.Count; i++)
            {
                GameObject.Find("P" + (4-i).ToString()).gameObject.SetActive(false);
            }

            foreach(KeyValuePair<int, PlayerScriptableObject> kvp in playerList)
            {
                PlayerScriptableObject playerObject = kvp.Value;
                GameObject character = Instantiate(characterPrefabs, GameObject.Find("Anchor_P"+ playerObject.index).transform.position, Quaternion.identity);
                GameObject characterSprite = Instantiate(playerObject.sprite, Vector3.zero, Quaternion.identity);
                character.GetComponent<PlayerController>().spriteGO = characterSprite;
                character.GetComponent<PlayerController>().hitSprite = playerObject.hitSprite;
                GameObject.Find("P" + playerObject.index).GetComponent<CharacterUI>().avatar.sprite = playerObject.avatarSprite;
                character.GetComponent<KeyboardInputMgr>().playerIndex = (PlayerIndex)playerObject.index-1;
                characterSprite.GetComponent<CharacterSprite>().target = character.transform;
            }
        }
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerManger");
        SOUND.StartMusic();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!_initialized || SceneManager.GetActiveScene().buildIndex != 1)
            return;

        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected && !playerIsSet[i])
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIsSet[i] = true;
                nbOfPlayers++;
            }

            prevState = state;
            state = GamePad.GetState((PlayerIndex)i);
            // Detect if a button was pressed this frame
            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && !playerIsReady[i])
            {
                SelectCharacter(i, true);
            }

            if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && playerIsReady[i])
            {
                SelectCharacter(i, false);
            }

            if (state.ThumbSticks.Left.X > deadZone && playerIsReleaseStick[i] && !playerIsReady[i])
            {
                playerIsReleaseStick[i] = false;
                SwitchCharacter(i, true);
            }
            else if (state.ThumbSticks.Left.X < -deadZone && playerIsReleaseStick[i] && !playerIsReady[i])
            {
                playerIsReleaseStick[i] = false;
                SwitchCharacter(i, false);
            }else if(state.ThumbSticks.Left.X >= -deadZone && state.ThumbSticks.Left.X <= deadZone)
            {
                playerIsReleaseStick[i] = true;
            }
        }

        
    }

    private void SwitchCharacter(int playerIndex, bool next)
    {
        if (next)
            charactersList[playerIndex].NextCharacter();
        else
            charactersList[playerIndex].PreviousCharacter();
    }

    private void SelectCharacter(int playerIndex, bool select)
    {
        
        if(charactersList[playerIndex].Validate(select))
        {
            playerIsReady[playerIndex] = select;
            LaunchGame();
        }
            
    }

    private void LaunchGame()
    {
        int nbOfPlayerReady = 0;
        foreach (KeyValuePair<int, bool> kvp in playerIsReady)
        {
            if (kvp.Value)
                nbOfPlayerReady++;
        }

        if(debugMode && playerDebugNumber == nbOfPlayerReady)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (nbOfPlayerReady == nbOfPlayers && nbOfPlayers >= 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Init()
    {
        charactersList = new List<CharacterSelection>(){null, null, null, null};
        characterInitialized = 0;
        playerList.Clear();
        playerIsReleaseStick.Clear();
        playerIsReady.Clear();
        selectedCharacters.Clear();

        for (int i = 0; i < 4; ++i)
        {
            playerIsReleaseStick.Add(i, true);
            playerIsReady.Add(i, false);
        }

        _initialized = true;
    }

    public void initCharactersList(CharacterSelection character, int index)
    {
        charactersList[index] = character;
        characterInitialized++;

        if(characterInitialized == 4)
            DebugAutoSelect();
    }

    public void DebugAutoSelect()
    {
        if(debugMode && SceneManager.GetActiveScene().buildIndex == 1)
        {
            for(int i = 0; i < playerDebugNumber; i++)
            {
                if(!playerIsReady[i])
                    SelectCharacter(i, true);
            }
        }
    }
}
