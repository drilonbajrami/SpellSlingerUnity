using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SpellSlinger
{
    public class TutorialStep : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image backdrop;
        [SerializeField] private TMP_Text _textMessage;

        [Space(10)]
        [Header("Tutorial step message")]
        [TextArea(1, 5)]
        [SerializeField] private string _message;

        // Start is called before the first frame update
        void Start()
        {
            backdrop.sprite = transform.parent.GetComponent<Tutorial>().tutorialBackground;
        }

        private void OnValidate()
        {
            _textMessage.text = _message;
        }
    }
}
