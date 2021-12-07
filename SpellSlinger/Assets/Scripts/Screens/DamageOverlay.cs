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

        private void Awake() => _overlay = GetComponent<Image>();

        private void OnEnable()
        {
            Health.Damage += OnDamage;
            Health.Death += OnDeath;
            ResetOverlay();
        }

        private void OnDisable()
        {
            Health.Damage -= OnDamage;
            Health.Death -= OnDeath;
            ResetOverlay();
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

        public void OnDamage(object source, int lives)
        {
            _alpha = maxAlphaValue / lives;
            _alpha = Mathf.Clamp(_alpha, 0f, maxAlphaValue);
            min = _alpha - differenceMinMax;
            max = _alpha + differenceMinMax;
            min = Mathf.Clamp01(min);
            max = Mathf.Clamp01(max);

            if (_alpha >= maxAlphaValue)
                dynamicChangeRate = changeRate - 0.0045f;    
        }

        private void OnDeath(object sender, EventArgs e) => gameObject.SetActive(false);

        private void ResetOverlay()
        {
            _alpha = 0.0f;
            _overlay.color = new Color(1, 1, 1, _alpha);
            min = Mathf.Clamp01(_alpha - differenceMinMax);
            max = Mathf.Clamp01(_alpha + differenceMinMax);
            dynamicChangeRate = changeRate;
        }
    }
}