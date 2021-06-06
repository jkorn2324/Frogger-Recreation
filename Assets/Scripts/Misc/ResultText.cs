using Frogger.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Frogger.Misc
{

    /// <summary>
    /// The Text Result used by the EndGame Menu.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ResultText : MonoBehaviour
    {

        #region structs

        /// <summary>
        /// The Game result text data.
        /// </summary>
        [System.Serializable]
        public struct GameResultTextData
        {
            // The color of the text.
            [SerializeField]
            private Color color;

            // The display text.
            [SerializeField]
            private string displayText;

            // The game result.
            [SerializeField]
            private GameManager.GameResult gameResult;

            #region properties

            public Color Color
                => this.color;

            public string DisplayText
                => this.displayText;

            public GameManager.GameResult Result
                => this.gameResult;

            #endregion
        }

        #endregion

        #region fields

        // Gets the result of the game.
        private GameManager.GameResult? _gameResult;

        // Reference of the text.
        private Text _text;

        // The text data information serialized.
        [SerializeField]
        private List<GameResultTextData> textData;

        // The text data information.
        private Dictionary<GameManager.GameResult, GameResultTextData> _textData
            = new Dictionary<GameManager.GameResult, GameResultTextData>();

        #endregion

        #region methods

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            this._text = this.GetComponent<Text>();
            this._text.text = "";

            this._gameResult = GameManager.Result;

            // Adds the text data to the dictionary.
            foreach (GameResultTextData dat in textData)
            {
                if (!this._textData.ContainsKey(dat.Result))
                    this._textData.Add(dat.Result, dat);
            }

            if (this._gameResult.HasValue
                && this._textData.ContainsKey(this._gameResult.Value))
            {
                GameResultTextData data = this._textData[this._gameResult.Value];
                this._text.color = data.Color;
                this._text.text = data.DisplayText;
            }
        }

        #endregion
    }
}
