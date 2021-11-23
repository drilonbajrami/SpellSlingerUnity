using System.Collections;
using System.Collections.Generic;
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
        // Temporary
        [SerializeField] private Image _animationImg;
        [SerializeField] private Sprite _animationSprite;

        [Space(10)]
        [Header("Tutorial step message")]
        [TextArea(1, 5)]
        [SerializeField] private string _message;

        // Start is called before the first frame update
        void Start()
        {
            backdrop.sprite = Tutorial.tutorialBackground;

            if(_animationSprite != null)
            { 
                _animationImg.sprite = _animationSprite;
                _animationImg.gameObject.SetActive(true); 
            }
            else _animationImg.gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            _textMessage.text = _message;
        }
    }
}
