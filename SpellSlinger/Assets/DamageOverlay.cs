using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SpellSlinger
{
    public class DamageOverlay : MonoBehaviour
    {
        private Image _overlay;

        private float _alpha = 0f;

        [Range(0.1f, 0.3f)][SerializeField] private float differenceMinMax = 0.15f;
        [Range(0.001f, 0.0075f)][SerializeField] private float changeRate = 0.0075f;
        [Range(0.7f, 0.9f)][SerializeField] private float maxAlphaValue = 0.85f;

        private float dynamicChangeRate;
        private float min;
        private float max;

        private void Start()
        {
            _overlay = GetComponent<Image>();
            _overlay.color = new Color(1,1,1,0);
            Health.Damage += OnDamage;
            min = _alpha - differenceMinMax;
            max = _alpha + differenceMinMax;
            dynamicChangeRate = changeRate;
        }

        private void Update()
        {
            if(_alpha > 0f)
            {
                if (_alpha <= min) dynamicChangeRate = changeRate;
                else if (_alpha >= max) dynamicChangeRate = -changeRate;
                _alpha += dynamicChangeRate;
                _overlay.color = new Color(1, 1, 1, _alpha);
            }  
        }

        public void OnDamage(object source, EventArgs e)
        {
            _alpha += maxAlphaValue * 0.8f;
            _alpha = Mathf.Clamp(_alpha, 0f, maxAlphaValue);
            min = _alpha - differenceMinMax;
            max = _alpha + differenceMinMax;

            if (_alpha >= maxAlphaValue)
                dynamicChangeRate = changeRate - 0.004f;

            min = Mathf.Clamp(min, 0f, 0.85f);
            max = Mathf.Clamp(max, 0f, 0.85f);
        }

        
    }
}
