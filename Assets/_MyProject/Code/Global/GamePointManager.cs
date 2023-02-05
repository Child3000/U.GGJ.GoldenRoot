using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GoldenRoot
{
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class GamePointManager : MonoBehaviour
    {
        /************************************************************************************************************************/
        
        private const int DefaultExecutionOrder = -5000;

        /************************************************************************************************************************/
        // UI
        /************************************************************************************************************************/
        
        [System.Serializable]
        public class PlayerPointData
        {
            public PlayerReference.PlayerID PlayerType;
            [SerializeField] float _CurrentPoint;
            public TMP_Text PointText;

            public float CurrentPoint
            {
                get => _CurrentPoint;
                set
                {
                    _CurrentPoint = value;
                    UpdateUI();
                }
            }
            
            public void UpdateUI()
            {
                if (PointText)
                {
                    PointText.text = $"{PlayerType}'s Point: {CurrentPoint}";
                }
            }
        }

        [SerializeField] private PlayerPointData[] _PointsDatas;

        public static GamePointManager Singleton;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Singleton = this;

            foreach (var pointData in _PointsDatas)
            {
                pointData.CurrentPoint = pointData.CurrentPoint;
            }
        }

        public void AddPoints(PlayerReference.PlayerID type, int value)
        {
            var pointData = _PointsDatas.First(x => x.PlayerType == type);
            pointData.CurrentPoint += value;
        }

        public float GetPoint(PlayerReference.PlayerID playerType)
        {
            var pointData = _PointsDatas.First(x => x.PlayerType == playerType);
            return pointData.CurrentPoint;
        }
    }
}