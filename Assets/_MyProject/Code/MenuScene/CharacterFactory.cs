using UnityEngine;

namespace GoldenRoot.MenuScene
{
    [CreateAssetMenu(fileName = "CharacterFactory", menuName = "GoldenRoot/CharacterFactory", order = 0)]
    public class CharacterFactory : ScriptableObject
    {
        [System.Serializable]
        public class CharacterData
        {
            public string CharacterName;
            public GameObject CharacterPrefab;
            public Sprite CharacterIcon;
        }

        [SerializeField] private CharacterData[] _CharacterDatas;

        public int TotalCount => _CharacterDatas.Length;

        public int LastIndex => _CharacterDatas.Length - 1;

        public CharacterData Get(int index)
        {
            return _CharacterDatas[index];
        }
    }
}