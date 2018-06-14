using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util.Language;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Chinese Text
    /// </summary>
    public struct ChineseText
    {
        #region fields

        /// <summary>
        /// full text
        /// </summary>
        string _text;

        /// <summary>
        /// chinese spelling
        /// </summary>
        string _spelling;

        /// <summary>
        /// spelling short form
        /// </summary>
        string _spellingShort;

        /// <summary>
        /// spelling is inited
        /// </summary>
        bool _spellingInit;

        #endregion

        #region constructor

        /// <summary>
        /// instance a ChineseText object
        /// </summary>
        /// <param name="text">full chinese text</param>
        public ChineseText(string text)
        {
            _text = text.Trim();
            _spelling = "";
            _spellingShort = "";
            _spellingInit = false;
        }

        #endregion

        #region Propertys

        /// <summary>
        /// spelling
        /// </summary>
        public string Spelling
        {
            get
            {
                return GetSpelling();
            }
        }

        /// <summary>
        /// spelling short
        /// </summary>
        public string SpellingShort
        {
            get
            {
                return GetSpellingShort();
            }
        }

        /// <summary>
        /// chinese text
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// get spelling
        /// </summary>
        /// <returns></returns>
        string GetSpelling()
        {
            if (_spellingInit)
            {
                InitSpelling();
            }
            return _spelling;
        }

        /// <summary>
        /// get spelling short
        /// </summary>
        /// <returns></returns>
        string GetSpellingShort()
        {
            if (_spellingInit)
            {
                InitSpelling();
            }
            return _spellingShort;
        }

        /// <summary>
        /// init spelling
        /// </summary>
        void InitSpelling()
        {
            if (!_text.IsNullOrEmpty())
            {
                var chineseUtil = this.Instance<IChineseLanguage>();
                if (chineseUtil == null)
                {
                    return;
                }
                _spelling = chineseUtil.GetSpellingBySimpleChinese(_text);
                _spellingShort = chineseUtil.GetSpellingShortSimpleChinese(_text);
            }
            _spellingInit = true;
        }

        /// <summary>
        /// override ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _text;
        }

        /// <summary>
        /// implicit convert to string
        /// </summary>
        /// <param name="text"></param>
        public static implicit operator string(ChineseText text)
        {
            return text.Text;
        }

        #endregion
    }
}
