using NUnit.Framework;

namespace AlgorithmsAndOtherImplementationsTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void KaratsubaMultiplicationTest1()
        {
            Assert.AreEqual("8539734222673567065463550869546574495034888535765114961879601127067743044893204848617875072216249073013374895871952806582723184",
                new AlgorithmsAndOtherImplementations.KaratsubaMultiplication()
                .Run("3141592653589793238462643383279502884197169399375105820974944592",
                "2718281828459045235360287471352662497757247093699959574966967627"));
        }
    }
}