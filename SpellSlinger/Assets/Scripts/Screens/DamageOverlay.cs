using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SpellSlinger
{
    public class DamageOverlay : MonoBehaviour
    {
        private Image _overlay;

        [Range(0.1f, 1.0f)][SerializeField] private float pulseRate = 0.5f;

        private float minOpacity;
        private float maxOpacity;
        private bool isFading = false;

        [SerializeField] [Range(0.0f, 1.0f)] public float minAlpha;
        [SerializeField] [Range(0.0f, 1.0f)] public float maxAlpha;

        private void OnDeath(object sender, EventArgs e) => gameObject.SetActive(false);

        private void Awake() => _overlay = GetComponent<Image>();

        private void OnEnable()
        {
            Health.Damage += OnDamage;
            Health.Death += OnDeath;
        }

        private void OnDisable()
        {
            Health.Damage -= OnDamage;
            Health.Death -= OnDeath;
            ResetOverlay();
        }

        private void Update()
        {
            if (isFading) OverlayPulse();
        }

        private void OverlayPulse() {
            if (_overlay.color.a <= minOpacity) FadeIn();
            else if (_overlay.color.a >= maxOpacity) FadeOut();
        }

        private void FadeOut() => _overlay.DOFade(minOpacity, pulseRate);
        private void FadeIn() => _overlay.DOFade(maxOpacity, pulseRate);

        public void OnDamage(object source, int lives)
        {
            if (lives == 3) ResetOverlay();
            else {
                isFading = true;
                if (lives == 2) SetMinMaxOpacity(minAlpha, (minAlpha + maxAlpha) / 2);
                else SetMinMaxOpacity((minAlpha + maxAlpha) / 2, maxAlpha);
            } 
        }

        private void SetMinMaxOpacity(float min, float max) {
            minOpacity = min;
            maxOpacity = max;
        }

        private void ResetOverlay() {
            isFading = false;
            SetMinMaxOpacity(0f,0f);
            FadeOut();
        }
    }
}