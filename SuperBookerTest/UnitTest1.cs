using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SuperBookerTest
{
    [TestClass]
    public class UnitTest1
    {
        private string _email;

        /// <summary>
        /// Tester med rigtige værdier om en studerende må booke inden for given tidsramme. Baseret på den tid de har valgt.
        /// </summary>
        /// <param name="tid">typen string. I programmet typen TimeSpan</param>
        [TestMethod]
        [DataRow("08:00")]
        [DataRow("08:01")]
        [DataRow("12:00")]
        [DataRow("16:29")]
        [DataRow("16:30")]
        public void StuderendeResevertionsTid_Test(string tid)
        {
            // Arrange
            TimeSpan testValue = TimeSpan.Parse($"{tid}");

            // Act
            bool actualValue = true;

            if (testValue <= TimeSpan.Parse("08:00") && testValue >= TimeSpan.Parse("16:30"))
            {
                actualValue = false;
            }

            // Assert
            Assert.IsTrue(actualValue);
        }

        /// <summary>
        /// Tester med forkerte værdier om en studerende må booke inden for given tidsramme. Baseret på den tid de har valgt.
        /// </summary>
        /// <param name="tid">typen string. I programmet typen TimeSpan</param>
        [TestMethod]
        [DataRow("00:00")]
        [DataRow("07:59")]
        [DataRow("16:31")]
        public void StuderendeResevertionsTidFejl_Test(string tid)
        {
            TimeSpan testValue = TimeSpan.Parse($"{tid}");

            bool exceptionThrown = false;

            try
            {
                if (testValue < TimeSpan.Parse("08:00") || testValue > TimeSpan.Parse("16:30"))
                {
                    throw new ArgumentOutOfRangeException("");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                throw new AssertFailedException(String.Format("An exception of type {0} was expected, but not thrown", typeof(ArgumentOutOfRangeException)));
            }

        }


        /// <summary>
        /// Tester med rigtige værdier om en underviser har lov til at slette baseret på hvor mange dage der er gået.
        /// </summary>
        /// <param name="dag">typen double. i programmet typen DateTime.</param>
        [TestMethod]
        [DataRow(3)]
        [DataRow(4)]
        public void UnderviserCanDelete_Test(double dag)
        {
            DateTime dato = DateTime.Now.AddDays(dag);
            DateTime dt = DateTime.Now.Date;


            bool excpectedValue = true;
            
            if (dato.Subtract(dt).Days < 3)
            {
                excpectedValue = false;
            }

            Assert.IsTrue(excpectedValue);
            
        }

        /// <summary>
        /// Tester med fejl værdier om en underviser har lov til at slette baseret på hvor mange dage der er gået.
        /// </summary>
        /// <param name="dag">typen double. i programmet typen DateTime.</param>
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void UnderviserCanDeleteFejl_Test(double dag)
        {
            DateTime dato = DateTime.Now.AddDays(dag);
            DateTime dt = DateTime.Now.Date;

            bool exceptionThrown = false;
            try
            {
                if (dato.Subtract(dt).Days < 3)
                {
                    throw new ArgumentOutOfRangeException("welp");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                throw new AssertFailedException(String.Format("An exception of type {0} was expected, but not thrown", typeof(ArgumentOutOfRangeException)));
            }
        }

        /// <summary>
        /// Propertien der skal validere om det er en gyldig email.
        /// </summary>
        public string EmailLogInd
        {
            get => _email;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(value);
                }

                if (!Regex.IsMatch(value, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                {
                    throw new ArgumentException(value);
                }

                if (value.Length >= 12)
                {
                    _email = value;
                }
                else
                {
                    throw new ArgumentException("");
                }
            }
        }

        /// <summary>
        /// Tester rigtige værdier på propertien EmailLogInd
        /// </summary>
        /// <param name="email">typen string. rigtig email værdi</param>
        [TestMethod]
        [DataRow("123qwe@zealand.dk")]
        public void EmailLogIndValidation_Test(string email) 
        {
            string expected = email;

            EmailLogInd = email;
            string actual = EmailLogInd;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tester fejl værdier på propertien EmailLogInd
        /// </summary>
        /// <param name="email">typen string. forkert email værdi</param>
        [TestMethod]
        [DataRow("")]
        [DataRow("123qwe")]
        [DataRow("123@s.dd")]
        [DataRow("123@sajdiasjdasio")]
        public void EmailLogIndValidationFejl_Test(string email)
        {

            Assert.ThrowsException<ArgumentException>(() => EmailLogInd = email);
        }
    }
}
