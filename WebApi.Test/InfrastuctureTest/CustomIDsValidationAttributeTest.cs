using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApi.Infrastucture;

namespace WebApi.Test.InfrastuctureTest
{
    [TestFixture]
    class CustomIDsValidationAttributeTest
    {

        [Test]
        public void IsValid_GiveProperData_ReturnsTrue()
        {
            // arrange
            List<int> ids = new List<int>() { 1, 2, 3 };
            var context = new ValidationContext(ids);
            var attrib = new CustomIDsValidationAttribute();

            // act
            var result = attrib.IsValid(ids);

            // assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void IsValid_GiveSameIds_ReturnsFalse()
        {
            // arrange
            List<int> ids = new List<int>() { 1, 2, 2 };
            var context = new ValidationContext(ids);
            var attrib = new CustomIDsValidationAttribute();

            // act
            var result = attrib.IsValid(ids);

            // assert
            Assert.That(result, Is.False);
            Assert.That(attrib.ErrorMessage == " ID values should be distinct.");
            
        }

        [Test]
        public void IsValid_GiveIdsWhichAreLessThanZero_ReturnsFalse()
        {
            // arrange
            List<int> ids = new List<int>() { 1, -1, -4 };
            var context = new ValidationContext(ids);
            var attrib = new CustomIDsValidationAttribute();

            // act
            var result = attrib.IsValid(ids);

            // assert
            Assert.That(result, Is.False);
            Assert.That(attrib.ErrorMessage == "ID should not be zero or less.");

        }
    }
}
