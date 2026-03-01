using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Score : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text scoreText;

    private void Update()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score :" + BalloonMovement.score + "/" + BalloonMovement.scoreLimit;
        }
    }
}
