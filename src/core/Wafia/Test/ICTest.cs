﻿using WAFIA.Database;

namespace Test {

    [TestClass]
    public class ICTest {
        [TestMethod]
        public async Task ObjectTest() {
            InfrastructureConnector? ic = IC();

            Assert.IsNotNull(ic);

            ic.
        }
    }
}
