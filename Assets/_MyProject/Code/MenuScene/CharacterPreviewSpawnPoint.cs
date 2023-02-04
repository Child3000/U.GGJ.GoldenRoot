using UnityEngine;

namespace GoldenRoot.MenuScene
{
    public class CharacterPreviewSpawnPoint : MonoBehaviour
    {
        [SerializeField] private PlayerReference.PlayerID _PlayerType;

        public PlayerReference.PlayerID PlayerType => _PlayerType;
    }
}