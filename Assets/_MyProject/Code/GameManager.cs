using UnityEngine;
using DG.Tweening;
using TMPro;

namespace GoldenRoot
{
    using MenuScene;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform _CameraTransform;
        [SerializeField] private MenuSceneManager _MenuSceneManager;

        [Header("Player Character")]
        [SerializeField] private CharacterFactory _CharacterFactory ;
        [SerializeField] private Transform _P1ModelTrans;
        [SerializeField] private Transform _P2ModelTrans;

        [Header("UI")]
        [SerializeField] private GameObject _MainMenu;
        [SerializeField] private GameObject _GameOverMenu;
        [SerializeField] private TextMeshProUGUI _CenterText;

        [Header("Audio")]
        [SerializeField] private AudioSource _AudioSource;
        [SerializeField] private AudioClip _MenuClip;
        [SerializeField] private AudioClip _GameplayClip;
        [SerializeField] private AudioClip _ReadyGoLowPitch;
        [SerializeField] private AudioClip _ReadyGoHighPitch;

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

        private void Start()
        {
            this.PlayClip(this._MenuClip, true, 1.0f);
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     this._GameState = GameState.Paused;
            // }
        }

        private void PlayClip(AudioClip clip, bool loop, float volume)
        {
            this._AudioSource.Stop();
            this._AudioSource.clip = clip;
            this._AudioSource.loop = loop;
            this._AudioSource.volume = volume;
            this._AudioSource.Play();
        }

        public void PlayButtonPressed()
        {
            this._MainMenu.SetActive(false);
            GamePointManager.Singleton.ResetPoints();
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
                    this._AudioSource.Stop();

                    Sequence seq = DOTween.Sequence();
                    seq.AppendCallback(() =>
                        {
                            this._CenterText.text = "Ready";
                            this._AudioSource.PlayOneShot(this._ReadyGoLowPitch);
                        })
                        .AppendInterval(1.0f)
                        .AppendCallback(() =>
                        {
                            this._CenterText.text = "Set";
                            this._AudioSource.PlayOneShot(this._ReadyGoLowPitch);
                        })
                        .AppendInterval(1.0f)
                        .AppendCallback(() =>
                        {
                            this._CenterText.text = "GO!";
                            this._AudioSource.PlayOneShot(this._ReadyGoHighPitch);
                        })
                        .AppendInterval(1.0f)
                        .AppendCallback(() =>
                        {
                            this._CenterText.text = "";
                            this._GameState = GameState.Playing;
                            GameTimeManager.Singleton.SetCanvasActive(true);
                            GameTimeManager.Singleton.StartCountdown();
                            GameTimeManager.Singleton.OnTimerEnd += this.OnGameEnd;

                            this.PlayClip(this._GameplayClip, false, 0.7f);
                        }
                    );
                }
            );
        }

        public void ReturnToMainMenu()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(this._CameraTransform.DOJump(new Vector3(4f, 4f, -9f), 0.5f, 1, 3.0f))
                .Join(this._CameraTransform.DORotate(new Vector3(0.01f, 0f, 0f), 3.0f, RotateMode.Fast))
                .AppendCallback(() => this._MainMenu.SetActive(true));
        }

        public void GameOverToMainMenu()
        {
            this.ReturnToMainMenu();
            this.PlayClip(this._MenuClip, true, 1.0f);
        }

        private void MoveCameraToGameplay()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(this._CameraTransform.DOJump(new Vector3(4f, 9f, 4f), 0.5f, 1, 3.0f))
               .Join(this._CameraTransform.DORotate(new Vector3(90f, 0f, 0f), 3.0f, RotateMode.Fast));
        }

        private void OnGameEnd()
        {
            GameTimeManager.Singleton.SetCanvasActive(false);
            this._GameOverMenu.SetActive(true);
            this._GameState = GameState.Menu;
        }
    }

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
    }
}
