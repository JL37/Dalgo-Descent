public class GameStateManager
{
    //Making it as singleton cause why not
    private static GameStateManager m_singleInstance;

    public static GameStateManager Get_Instance
    {
        get
        {
            if(m_singleInstance == null)
                m_singleInstance = new GameStateManager();
            return m_singleInstance;
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