using UnityEngine;
using DG.Tweening;

namespace GoldenRoot
{
    using MenuScene;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MenuSceneManager _MenuSceneManager;
        [SerializeField] private CharacterFactory _CharacterFactory ;

        [SerializeField] private Transform _P1ModelTrans;
        [SerializeField] private Transform _P2ModelTrans;

        [SerializeField] private Transform _CameraTransform;

        private GameState _GameState;

        public static GameManager Singleton;

        public GameState GameState => _GameState;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                this._GameState = GameState.Menu;
            } else
            {
                Destroy(this.gameObject);
            }
        }

        public void PlayButtonPressed()
        {
            this._MenuSceneManager.StartMenuSceneInAdditiveMode(
                (s) =>
                {
                    CharacterFactory.CharacterData p1Data = this._CharacterFactory.Get(s.P1SelectionIndex);
                    CharacterFactory.CharacterData p2Data = this._CharacterFactory.Get(s.P2SelectionIndex);

                    Destroy(this._P1ModelTrans.GetChild(0).gameObject);
                    Destroy(this._P2ModelTrans.GetChild(0).gameObject);

                    Instantiate(p1Data.CharacterPrefab, this._P1ModelTrans);
                    Instantiate(p2Data.CharacterPrefab, this._P2ModelTrans);

                    this.MoveCameraToGameplay();
                    this._GameState = GameState.Playing;
                }
            );
        }

        private void MoveCameraToGameplay()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(this._CameraTransform.DOJump(new Vector3(4f, 9f, 4f), 0.5f, 1, 3.0f))
               .Join(this._CameraTransform.DORotate(new Vector3(90f, 0f, 0f), 3.0f, RotateMode.Fast));
        }
    }

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
    }
}
