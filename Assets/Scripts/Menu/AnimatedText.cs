using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu {
    /// <summary>
    /// Component used to animate a text field using a list of strings
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class AnimatedText : UIBehaviour {
        #region Public Variables
        [SerializeField] private float timePerFrame;
        [SerializeField] private List<string> animationTexts;
        #endregion

        #region Private Variables
        private TMP_Text _text;
        private int _index;
        #endregion

        #region Behaviour Callbacks
        protected override void Awake() {
            base.Awake();
            _text = GetComponent<TMP_Text>();
        }

        protected override void OnEnable() {
            base.OnEnable();
            InvokeRepeating(nameof(NextFrame), 0f, timePerFrame);
        }

        protected override void OnDisable() {
            base.OnDisable();
            CancelInvoke(nameof(NextFrame));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the next frame of the animation
        /// </summary>
        private void NextFrame() {
            _text.SetText(animationTexts[_index]);
            _index = _index + 1 == animationTexts.Count ? 0 : _index + 1;
        }
        #endregion
    }
}