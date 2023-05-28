using WAFIA.Database.Types;
using WAFIA.Database.Connectors;
using Wafia.Database;

namespace Test {
    [TestClass]
    public class ACTest {
        [TestMethod]
        public async Task AddDeleteTest() {
            Database? testBase = Database.Create("../../../connection_data.txt");

            Assert.IsNotNull(testBase);

            Account testAcc = new(0, "test_acc_1_l", "test_acc_1_m", "test_acc_1_p", false);
            var addOkRes = await testBase.AC.Add(testAcc);
            Assert.IsTrue(addOkRes);

            var addAgainRes = await testBase.AC.Add(testAcc);
            Assert.IsFalse(addAgainRes);

            var deleteOkRes = await testBase.AC.Delete(testAcc.Login);
            Assert.IsTrue(deleteOkRes);

            var deleteAgainRes = await testBase.AC.Delete(testAcc.Login);
            Assert.IsFalse(deleteAgainRes);

            string longWord = "looooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong";

            testAcc = new(0, longWord, "test_acc_1_m", "test_acc_1_p", false);
            var addLongLoginRes = await testBase.AC.Add(testAcc);
            Assert.IsFalse(addLongLoginRes);

            testAcc = new(0, "test_acc_1_l", longWord, "test_acc_1_p", false);
            var addLongMailRes = await testBase.AC.Add(testAcc);
            Assert.IsFalse(addLongMailRes);

            testAcc = new(0, "test_acc_1_l", "test_acc_1_m", longWord, false);
            var addLongPasswordRes = await testBase.AC.Add(testAcc);
            Assert.IsFalse(addLongPasswordRes);
        }

        [TestMethod]
        public async Task GetsTest() {
            Database? testBase = Database.Create("../../../connection_data.txt");

            Assert.IsNotNull(testBase);

            Account testAcc1 = new(0, "test_acc_1_l", "test_acc_1_m", "test_acc_1_p", false);
            Account testAcc2 = new(0, "test_acc_2_l", "test_acc_2_m", "test_acc_2_p", false);
            await testBase.AC.Add(testAcc1);
            await testBase.AC.Add(testAcc2);

            var getIdMailOkRes = await testBase.AC.GetIdMail(testAcc1.Login);
            Assert.AreEqual(getIdMailOkRes.Value.Value, testAcc1.Mail);

            string notExistLogin = "IvanovIvanIvanich";
            var getIdMailNotExistRes = await testBase.AC.GetIdMail(notExistLogin);
            Assert.IsNull(getIdMailNotExistRes);

            var getIdRightsOkRes = await testBase.AC.GetIdRights(testAcc1.Login, testAcc1.Password);
            Assert.AreEqual(getIdRightsOkRes.Value.Value, testAcc1.IsAdmin);

            string notExistPassword = "PasswordPassword";
            var getIdMailWrongPasswordRes = await testBase.AC.GetIdRights(testAcc1.Login, notExistPassword);
            var getIdMailWrongLoginRes = await testBase.AC.GetIdRights(notExistLogin, testAcc1.Password);
            Assert.IsNull(getIdMailWrongPasswordRes);
            Assert.IsNull(getIdMailWrongLoginRes);

            var getIdMailDiffDataRes = await testBase.AC.GetIdRights(testAcc1.Login, testAcc2.Login);
            Assert.IsNull(getIdMailDiffDataRes);

            var getOkRes = await testBase.AC.Get(getIdRightsOkRes.Value.Key);
            Assert.IsNotNull(getOkRes);
            Assert.AreEqual(getOkRes.Id, getIdRightsOkRes.Value.Key);
            Assert.AreEqual(getOkRes.Login, testAcc1.Login);
            Assert.AreEqual(getOkRes.Password, testAcc1.Password);
            Assert.AreEqual(getOkRes.Mail, testAcc1.Mail);
            Assert.AreEqual(getOkRes.IsAdmin, testAcc1.IsAdmin);

            var getNotExistIdRes = await testBase.AC.Get(0);
            Assert.IsNull(getNotExistIdRes);

            await testBase.AC.Delete(testAcc1.Login);
            await testBase.AC.Delete(testAcc2.Login);
        }

        [TestMethod]
        public async Task EditsTest() {
            Database? testBase = Database.Create("../../../connection_data.txt");

            Assert.IsNotNull(testBase);

            Account testAcc = new(0, "test_acc_1_l", "test_acc_1_m", "test_acc_1_p", false);
            await testBase.AC.Add(testAcc);

            var changeRightsOkRes = await testBase.AC.ChangeRights(testAcc.Login, true);
            Assert.IsTrue(changeRightsOkRes);
            Assert.AreEqual((await testBase.AC.GetIdRights(testAcc.Login, testAcc.Password)).Value.Value, true);

            changeRightsOkRes = await testBase.AC.ChangeRights(testAcc.Login, false);
            Assert.IsTrue(changeRightsOkRes);
            Assert.AreEqual((await testBase.AC.GetIdRights(testAcc.Login, testAcc.Password)).Value.Value, false);

            string notExistLogin = "IvanovIvanIvanich";
            var changeRightsNotExistRes = await testBase.AC.ChangeRights(notExistLogin, true);
            Assert.IsFalse(changeRightsNotExistRes);

            var id = (await testBase.AC.GetIdRights(testAcc.Login, testAcc.Password)).Value.Key;

            Account editAcc = new(id, "test_acc_2_l", "test_acc_2_m", "test_acc_2_p", true);

            var editOkRes = await testBase.AC.Edit(editAcc);
            Assert.IsTrue(editOkRes);
            var acc = await testBase.AC.Get(id);
            Assert.AreEqual(acc.Login, editAcc.Login);
            Assert.AreEqual(acc.Password, editAcc.Password);
            Assert.AreEqual(acc.Mail, editAcc.Mail);
            Assert.AreNotEqual(acc.IsAdmin, editAcc.IsAdmin);

            editAcc.Id = 0;

            var editWrongIdRes = await testBase.AC.Edit(editAcc);
            Assert.IsFalse(editWrongIdRes);

            await testBase.AC.Delete(editAcc.Login);
        }
    }
}