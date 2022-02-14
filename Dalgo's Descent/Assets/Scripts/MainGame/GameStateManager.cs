public class GameStateManager
{
    //Making it as singleton cause why not
    private static GameStateManager single_instance;

    public static GameStateManager Get_Instance
    {
        get
        {
            if(single_instance == null)
                single_instance = new GameStateManager();
            return single_instance;
        }
    }

    public GameState CurrentGameState
    {
        get;private set;
    }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged; //call this to change the state


    private GameStateManager()
    {
        //do nothing for now
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState) //if the state is equal to the current, return the func
            return;

        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }

}