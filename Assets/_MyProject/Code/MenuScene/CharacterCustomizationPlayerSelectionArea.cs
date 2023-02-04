using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoldenRoot.MenuScene
{
    public class CharacterCustomizationPlayerSelectionArea : MonoBehaviour
    {
        [SerializeField] private CharacterFactory _CharacterFactory;

        [SerializeField] private TMP_Text _CharacterNameText;
        [SerializeField] private TMP_Text _OKText;
        [SerializeField] private Button _NextButton;
        [SerializeField] private Button _PrevButton;

        [SerializeField] private CharacterPreviewSpawnPoint SpawnPoint;
        [SerializeField] private CharacterCustomizationTimer _Timer;

        [SerializeField] private MenuSelectionResult _Result;
        
        private int navigationIndex = 0;
        public int NavigationIndex => navigationIndex;
        
        private GameObject characterInstance;
        private bool _IsReady;

        public bool IsReady
        {
            get => _IsReady;
            set
            {
                var oldValue = _IsReady;
                if (oldValue != value)
                {
                    _IsReady = value;
                    RefreshDisplay();
                }
            }
        }
        
        private void Awake()
        {
            _NextButton.onClick.AddListener(OnNextButtonHandler);
            _PrevButton.onClick.AddListener(OnPrevButtonHandler);

            _Timer.OnTimerEnd += OnTimerEndHandler;
            
            RefreshDisplay();
        }

        private void OnDestroy()
        {
            _Timer.OnTimerEnd -= OnTimerEndHandler;
        }

        private void OnTimerEndHandler()
        {
            // Do not read and respond to any input.
            enabled = false;
            
            // Force ready
            IsReady = true;
            
            // Save decision.
            if (SpawnPoint.PlayerType == PlayerReference.PlayerID.P1) _Result.P1SelectionIndex = navigationIndex;
            if (SpawnPoint.PlayerType == PlayerReference.PlayerID.P2) _Result.P2SelectionIndex = navigationIndex;
        }

        private void Update()
        {
            var playerType = SpawnPoint.PlayerType;
            if (!IsReady && GlobalInputMapping.IsDown(GlobalInputMapping.InputMap.MoveRight, playerType))
            {
                _NextButton.onClick.Invoke();
            }
            if (!IsReady && GlobalInputMapping.IsDown(GlobalInputMapping.InputMap.MoveLeft, playerType))
            {
                _PrevButton.onClick.Invoke();
            }
            if (GlobalInputMapping.IsDown(GlobalInputMapping.InputMap.MoveUp, playerType))
            {
                IsReady = true;
            }
            if (GlobalInputMapping.IsDown(GlobalInputMapping.InputMap.MoveDown, playerType))
            {
                IsReady = false;
            }
        }

        private void OnNextButtonHandler()
        {
            navigationIndex++;
            if (navigationIndex >= _CharacterFactory.TotalCount) navigationIndex = 0;
            if (navigationIndex < 0) navigationIndex = _CharacterFactory.LastIndex;
            
            RefreshDisplay();
        }

        private void OnPrevButtonHandler()
        {
            navigationIndex--;
            if (navigationIndex >= _CharacterFactory.TotalCount) navigationIndex = 0;
            if (navigationIndex < 0) navigationIndex = _CharacterFactory.LastIndex;
            
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            var characterData = _CharacterFactory.Get(navigationIndex);

            _CharacterNameText.text = characterData.CharacterName;

            _OKText.gameObject.SetActive(IsReady);
            
            HandleCharacterChange(characterData);
        }

        private void HandleCharacterChange(CharacterFactory.CharacterData characterData)
        {
            if (characterInstance != null)
            {
                Destroy(characterInstance);
            }

            Transform t = SpawnPoint.transform;
            characterInstance = Instantiate(characterData.CharacterPrefab, t.position, t.rotation);
            characterInstance.transform.localScale = t.localScale;
        }
    }
}