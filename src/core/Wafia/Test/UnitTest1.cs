using WAFIA.Database;
using WAFIA.Database.Types;
using WAFIA.Database.Connectors;

namespace Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task TestA()
        {
            NpgsqlConnector nc = new("localhost", "postgres", "password", "WAFIA");
            AccountConnector ac = new(nc);

            Account acc1 = new(0, "acc1_l", "acc1_m", "acc1_p", false);
            var res1 = await ac.Add(acc1);
            Assert.AreEqual(res1, true);
            var res_e1 = await ac.Add(acc1);
            Assert.AreEqual(res_e1, false);

            var res2 = await ac.GetIdMail(acc1.Login);
            var res3 = await ac.GetIdRights(acc1.Login, acc1.Password);

            Assert.AreEqual(res2.Value.Value, acc1.Mail);
            Assert.AreEqual(res3.Value.Value, acc1.IsAdmin);
            Assert.AreEqual(res2.Value.Key, res3.Value.Key);

            bool newRights = true;
            await ac.ChangeRights(acc1.Login, newRights);
            var res4 = await ac.GetIdRights(acc1.Login, acc1.Password);
            Assert.AreEqual(res4.Value.Value, newRights);

            Account acc2 = new(0, "acc2_l", "acc2_m", "acc2_p", false);
            acc2.Id = res4.Value.Key;
            var res5 = await ac.Edit(acc2);
            Assert.AreEqual(res5, true);
            var res6 = await ac.Get(acc2.Id);
            Assert.AreEqual(acc2.Login, res6.Login);
            Assert.AreEqual(acc2.Mail, res6.Mail);
            Assert.AreEqual(acc2.Password, res6.Password);


            string editPassword = "acc2_p_edit";
            var res7 = await ac.ChangePassword(acc2.Id, editPassword);
            Assert.AreEqual(res7, true);
            var acc3 = await ac.Get(acc2.Id);
            Assert.AreEqual(editPassword, acc3.Password);

            await ac.Delete(acc1.Login);
        }
    }
}