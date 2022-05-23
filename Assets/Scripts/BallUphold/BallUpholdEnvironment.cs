using CustomAgent;
using TMPro;
using UnityEngine;
using Utils;

namespace BallUphold
{
    public class BallUpholdEnvironment : LearningEnvironment
    {
        [SerializeField] private TextMeshProUGUI stepCountText;

        public override void Awake()
        {
            base.Awake();
            stepCountText.text = "0";
        }
        
        public override void ShowEpisodeResult(EpisodeEndResult result)
        {
            stepCountText.text = result.stepCount.ToString();
        }
    }
}
