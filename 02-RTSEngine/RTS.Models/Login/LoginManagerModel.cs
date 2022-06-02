using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS.Configuration;

namespace RTS.Models
{
    [Serializable]
    public class LoginManagerModel
    {
        #region Constantes
        public const int NICKNAME_OK = 0;
        public const int NICKNAME_TOO_LONG = 1;
        public const int NICKNAME_TOO_SHORT = 2;
        public const int MIN_LENGTH_FOR_NICKNAME = 3;
        public const int MAX_LENGTH_FOR_NICKNAME = 20;

        #endregion
        public bool IsPseudoValid(string nickname)
        {
            if (nickname.Length < MIN_LENGTH_FOR_NICKNAME || nickname.Length > MAX_LENGTH_FOR_NICKNAME)
                return false;

            return true;
        }

        /// <summary>
        /// Check if the pseudo is valid and return the reason if it is not
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public int ReasonForInvalidPseudo(string nickname)
        {
            if (nickname.Length < MIN_LENGTH_FOR_NICKNAME)
                return NICKNAME_TOO_SHORT;
            else if (nickname.Length > MAX_LENGTH_FOR_NICKNAME)
                return NICKNAME_TOO_LONG;
            else
                return NICKNAME_OK;
        }
    }
}
