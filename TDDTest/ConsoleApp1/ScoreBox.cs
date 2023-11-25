namespace ConsoleApp1;

public class ScoreBox
{
    private const int DefaultGoalScore = 1;
    public string PlayerName1 { get; set; }
    public string PlayerName2 { get; set; }
    private int PlayerScore1 { get; set; }
    private int PlayerScore2 { get; set; }

    private Dictionary<int, string> ScoreDescription = new Dictionary<int, string>()
    {
        { 1, "Fifteen" },
        { 2, "Thirty" },
    }; 
    

    public void Player1Goal()
    {
        PlayerScore1 += DefaultGoalScore;
    }

    public void Player2Goal()
    {
        PlayerScore2 += DefaultGoalScore;
    }

    public string GetDescription()
    {
        if (!IsDeuce())
        {
            if (IsDraw())
            {
                return String.Empty;
            }
            else
            {
                
            }
        }
        else
        {
            
        }

        return "love all";
    }

    private bool IsDraw()
    {
        return PlayerScore1 == PlayerScore2;
    }

    private bool IsDeuce()
    {
        return PlayerScore1 == PlayerScore2 && PlayerScore1 >= 3;
    }

    private bool IsPlayer1Bigger()
    {
        return PlayerScore1 > PlayerScore2;
    }
}