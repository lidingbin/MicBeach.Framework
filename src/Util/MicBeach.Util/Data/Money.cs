using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Money
    /// </summary>
    public struct Money
    {
        #region constructor

        /// <summary>
        /// instance a money object
        /// </summary>
        /// <param name="amount">amount</param>
        public Money(decimal amount)
        {
            _amount = amount;
            _currency = _defaultCurrency;
        }

        /// <summary>
        /// instance a money object
        /// </summary>
        /// <param name="amount">amount</param>
        /// <param name="currency">currency</param>
        public Money(decimal amount, Currency currency)
        {
            _amount = amount;
            _currency = currency;
        }

        #endregion

        #region fields

        /// <summary>
        /// amount
        /// </summary>
        private decimal _amount;

        /// <summary>
        /// currency
        /// </summary>
        private Currency _currency;

        /// <summary>
        /// default currency
        /// </summary>
        private static Currency _defaultCurrency = Currency.CNY;

        #endregion

        #region Propertys

        /// <summary>
        /// get or set amount
        /// </summary>
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }

        /// <summary>
        /// get or set currency
        /// </summary>
        public Currency Currency
        {
            get
            {
                return _currency;
            }
        }

        /// <summary>
        /// get or set currencysign
        /// </summary>
        public string CurrencySign
        {
            get
            {
                return GetCurrencySign();
            }
        }

        #endregion

        #region static methods

        /// <summary>
        /// compare two MOney objects whether equal
        /// </summary>
        /// <param name="moneyOne">first money</param>
        /// <param name="moneyTwo">second money</param>
        /// <returns>whether equal</returns>
        public static bool Equals(Money moneyOne, Money moneyTwo)
        {
            return moneyOne._currency == moneyTwo._currency && moneyOne._amount == moneyTwo._amount;
        }

        /// <summary>
        /// verify whether can do calculate between two Money objects
        /// </summary>
        /// <param name="moneyOne">first money</param>
        /// <param name="moneyTwo">second money</param>
        private static void CalculateVerify(Money moneyOne, Money moneyTwo)
        {
            if (moneyOne._currency != moneyTwo._currency)
            {
                throw new Exception("both Money data don't hava the same Currency");
            }
        }

        /// <summary>
        /// set default currency
        /// </summary>
        /// <param name="defaultCurrency">default currency</param>
        public static void SetDefaultCurrency(Currency defaultCurrency)
        {
            _defaultCurrency = defaultCurrency;
        }

        #endregion

        #region methods

        /// <summary>
        /// get currency sign
        /// </summary>
        /// <returns></returns>
        string GetCurrencySign()
        {
            return string.Empty;
        }

        /// <summary>
        /// compare two Money objects whether equal
        /// </summary>
        /// <param name="obj">other Money object</param>
        /// <returns>whether equal</returns>
        public override bool Equals(object obj)
        {
            return Equals(this, (Money)obj);
        }
        public override int GetHashCode()
        {
            return string.Format("{0}_{1}", _currency, _amount).GetHashCode();
        }

        /// <summary>
        /// do add operation between two Money objects,must be the same currency can do this operation
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>calculate result</returns>
        public static Money operator +(Money one, Money two)
        {
            CalculateVerify(one, two);
            decimal newAmount = one.Amount + two.Amount;
            return new Money(newAmount, one._currency);
        }

        /// <summary>
        /// do subtraction operation between two Money objects,must be the same currency can do this operation
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>calculate result</returns>
        public static Money operator -(Money one, Money two)
        {
            CalculateVerify(one, two);
            decimal newAmount = one.Amount - two.Amount;
            return new Money(newAmount, one._currency);
        }

        /// <summary>
        /// do multiplication operation between two Money objects,must be the same currency can do this operation
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>calculate result</returns>
        public static Money operator *(Money one, Money two)
        {
            CalculateVerify(one, two);
            decimal newAmount = one.Amount * two.Amount;
            return new Money(newAmount, one._currency);
        }

        /// <summary>
        /// do division operation between two Money objects,must be the same currency can do this operation
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>calculate result</returns>
        public static Money operator /(Money one, Money two)
        {
            CalculateVerify(one, two);
            decimal newAmount = one.Amount / two.Amount;
            return new Money(newAmount, one._currency);
        }

        /// <summary>
        /// compare two Money objects whether equal
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>whether equal</returns>
        public static bool operator ==(Money one, Money two)
        {
            return Equals(one, two);
        }

        /// <summary>
        /// compare two Money objects whether not equal
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>whether not equal</returns>
        public static bool operator !=(Money one, Money two)
        {
            return !Equals(one, two);
        }

        /// <summary>
        /// determine whether the first money object less than second
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>determined result</returns>
        public static bool operator <(Money one, Money two)
        {
            CalculateVerify(one, two);
            return one.Amount < two.Amount;
        }

        /// <summary>
        /// determine whether the first money object greater than second
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>determined result</returns>
        public static bool operator >(Money one, Money two)
        {
            CalculateVerify(one, two);
            return one.Amount > two.Amount;
        }

        /// <summary>
        /// determine whether the first money object less than or equal to second
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>determined result</returns>
        public static bool operator <=(Money one, Money two)
        {
            CalculateVerify(one, two);
            return one.Amount <= two.Amount;
        }

        /// <summary>
        /// determine whether the first money object greater than or equal to second
        /// </summary>
        /// <param name="one">first money</param>
        /// <param name="two">second money</param>
        /// <returns>determined result</returns>
        public static bool operator >=(Money one, Money two)
        {
            CalculateVerify(one, two);
            return one.Amount >= two.Amount;
        }

        /// <summary>
        /// add amount,minus amount if amount value is a negative number
        /// </summary>
        /// <param name="amount">amount value</param>
        /// <returns>calculated money</returns>
        public Money AddAmount(decimal amount)
        {
            _amount += amount;
            return this;
        }

        /// <summary>
        /// minus amount，add amount if amount value is a negative number
        /// </summary>
        /// <param name="amount">amount</param>
        /// <returns>calculated money</returns>
        public Money SubtractAmount(decimal amount)
        {
            _amount -= amount;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// Currency
    /// </summary>
    public enum Currency
    {
        CNY = 110,
        USD = 120,
    }
}
