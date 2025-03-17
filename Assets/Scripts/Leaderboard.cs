using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<Text> names;

    [SerializeField]
    private List<Text> scores;

    private string publicLeaderboardKey;

    [SerializeField]
    private GameManager gm;

    public void Awake()
    {
        publicLeaderboardKey = gm.levelScriptable.keyRanking;
    }

    public void GetLeaderboard()
    {
        publicLeaderboardKey = gm.levelScriptable.keyRanking;
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>{
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        publicLeaderboardKey = gm.levelScriptable.keyRanking;
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }
}
