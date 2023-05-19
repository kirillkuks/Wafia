using Wafia.Database;

namespace Test {

    [TestClass]
    public class GCTest {
        [TestMethod]
        public async Task CountryTest() {
            Database? testBase = Database.Create("../../../connection_data.txt");

            Assert.IsNotNull(testBase);

            var getEmptyRes = await testBase.GC.GetCountries();
            Assert.AreEqual(getEmptyRes.Count, 0);

            string notExistName1 = "Country1";
            var addOkRes = await testBase.GC.AddCountry(notExistName1);
            Assert.IsTrue(addOkRes);

            var addAgainRes = await testBase.GC.AddCountry(notExistName1);
            Assert.IsFalse(addAgainRes);

            var getOneRes = await testBase.GC.GetCountries();
            Assert.AreEqual(getOneRes.Count, 1);
            Assert.AreEqual(getOneRes[0].Name, notExistName1);
            Assert.IsTrue(getOneRes[0].Id > 0);

            string longName = "looooooooooooooooooooooooooooooooooooooooooooooooo" +
                              "oooooooooooooooooooooooooooooooooooooooooooooooooog"; 
            var addLongNameRes = await testBase.GC.AddCountry(longName);
            Assert.IsFalse(addLongNameRes);

            string notExistName2 = "Country2";
            addOkRes = await testBase.GC.AddCountry(notExistName2);
            Assert.IsTrue(addOkRes);

            var getTwoRes = await testBase.GC.GetCountries();
            Assert.AreEqual(getTwoRes.Count, 2);
            Assert.AreEqual(getTwoRes[0].Name, notExistName1);
            Assert.AreEqual(getTwoRes[1].Name, notExistName2);

            var dellOkRes = await testBase.GC.DeleteCountry(notExistName1);
            Assert.IsTrue(dellOkRes);

            var dellAgainRes = await testBase.GC.DeleteCountry(notExistName1);
            Assert.IsFalse(dellAgainRes);

            getOneRes = await testBase.GC.GetCountries();
            Assert.AreEqual(getOneRes.Count, 1);
            Assert.AreEqual(getOneRes[0].Name, notExistName2);

            dellOkRes = await testBase.GC.DeleteCountry(notExistName2);
            Assert.IsTrue(dellOkRes);

            getEmptyRes = await testBase.GC.GetCountries();
            Assert.AreEqual(getEmptyRes.Count, 0);
        }

        [TestMethod]
        public async Task CityTest() {
            Database? testBase = Database.Create("../../../connection_data.txt");

            Assert.IsNotNull(testBase);

            string country1 = "Country1";
            string country2 = "Country2";
            await testBase.GC.AddCountry(country1);
            await testBase.GC.AddCountry(country2);

            var countries = await testBase.GC.GetCountries();

            var getEmptyRes = await testBase.GC.GetCities(countries[0]);
            Assert.AreEqual(getEmptyRes.Count, 0);

            string city1 = "city1";
            string city2 = "city2";
            var addOkRes = await testBase.GC.AddCity(countries[0], city1);
            Assert.IsTrue(addOkRes);

            addOkRes = await testBase.GC.AddCity(countries[0], city2);
            Assert.IsTrue(addOkRes);

            addOkRes = await testBase.GC.AddCity(countries[1], city1);
            Assert.IsTrue(addOkRes);

            var addAgainRes = await testBase.GC.AddCity(countries[0], city1);
            Assert.IsFalse(addAgainRes);

            string longName = "looooooooooooooooooooooooooooooooooooooooooooooooo" +
                              "oooooooooooooooooooooooooooooooooooooooooooooooooog";
            var addLongNameRes = await testBase.GC.AddCity(countries[0], longName);
            Assert.IsFalse(addLongNameRes);

            var getTwoRes = await testBase.GC.GetCities(countries[0]);
            Assert.AreEqual(getTwoRes.Count, 2);
            Assert.AreEqual(getTwoRes[0].Name, city1);
            Assert.AreEqual(getTwoRes[0].Country, countries[0].Id);
            Assert.AreEqual(getTwoRes[1].Name, city2);
            Assert.AreEqual(getTwoRes[1].Country, countries[0].Id);

            var getOneRes = await testBase.GC.GetCities(countries[1]);
            Assert.AreEqual(getOneRes.Count, 1);
            Assert.AreEqual(getOneRes[0].Name, city1);
            Assert.AreEqual(getOneRes[0].Country, countries[1].Id);

            var deleteOkRes = await testBase.GC.DeleteCity(getTwoRes[0].Id);
            Assert.IsTrue(deleteOkRes);

            getOneRes = await testBase.GC.GetCities(countries[0]);
            Assert.AreEqual(getOneRes.Count, 1);
            Assert.AreEqual(getOneRes[0].Name, city2);
            Assert.AreEqual(getOneRes[0].Country, countries[0].Id);

            var deleteAgainRes = await testBase.GC.DeleteCity(getTwoRes[0].Id);
            Assert.IsFalse(deleteAgainRes);

            getOneRes = await testBase.GC.GetCities(countries[0]);
            Assert.AreEqual(getOneRes.Count, 1);

            deleteOkRes = await testBase.GC.DeleteCity(city2, countries[0]);
            Assert.IsTrue(deleteOkRes);

            getEmptyRes = await testBase.GC.GetCities(countries[0]);
            Assert.AreEqual(getEmptyRes.Count, 0);

            deleteOkRes = await testBase.GC.DeleteCity(city1, countries[1]);
            Assert.IsTrue(deleteOkRes);

            await testBase.GC.DeleteCountry(country1);
            await testBase.GC.DeleteCountry(country2);
        }
    }
}
