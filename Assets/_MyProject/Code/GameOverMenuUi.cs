using System;
using TMPro;
using UnityEngine;

namespace GoldenRoot
{
    public class GameOverMenuUi : MonoBehaviour
    {
        public struct ScoreStats
        {
            public int ScoreP1;
            public int ScoreP2;
        }
        
        [SerializeField] private TMP_Text _P1ScoreText;
        [SerializeField] private TMP_Text _P2ScoreText;
        [SerializeField] private TMP_Text _P1ResultText;
        [SerializeField] private TMP_Text _P2ResultText;

        [Header("Selection Dynamic Resize")]

        [SerializeField] private float _TotalPanelWidth = 1920;

        [SerializeField] private RectTransform _P1Selection;

        [SerializeField] private RectTransform _P2Selection;
        
        private void OnEnable()
        {
            RefreshUi(GamePointManager.Singleton ? new ScoreStats()
            {
                ScoreP1 = Mathf.FloorToInt(GamePointManager.Singleton.GetPoint(PlayerReference.PlayerID.P1)),
                ScoreP2 = Mathf.FloorToInt(GamePointManager.Singleton.GetPoint(PlayerReference.PlayerID.P2))
            } : new ScoreStats() {ScoreP1 = 80, ScoreP2 = 0});
        }

        private void RefreshUi(ScoreStats score)
        {
            _P1ScoreText.text = score.ScoreP1.ToString();
            _P2ScoreText.text = score.ScoreP2.ToString();
            
            if (score.ScoreP1 == score.ScoreP2)
            {
                _P1ResultText.text = _P2ResultText.text = $"DRAW";
            }
            else if (score.ScoreP1 > score.ScoreP2)
            {
                _P1ResultText.text = $"WIN";
                _P2ResultText.text = $"LOSE";
            } else
            {
                _P1ResultText.text = $"LOSE";
                _P2ResultText.text = $"WIN";
            }

            var totalScore = score.ScoreP1 + score.ScoreP2;
            var defaultRatio = 0.5f;
            const float maxRatio = 0.8f;
            const float minRatio = 0.2f;
            var p1Ratio = totalScore != 0 ? Mathf.Clamp((score.ScoreP1 / (float)totalScore), minRatio, maxRatio) : defaultRatio;
            var p2Ratio = totalScore != 0 ? Mathf.Clamp((score.ScoreP2 / (float)totalScore), minRatio, maxRatio) : defaultRatio;
            _P1Selection.sizeDelta = new Vector2(p1Ratio * _TotalPanelWidth, _P1Selection.sizeDelta.y);
            _P2Selection.sizeDelta = new Vector2(p2Ratio * _TotalPanelWidth, _P2Selection.sizeDelta.y);
        }
    }
}