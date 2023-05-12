using Npgsql;
using System.Security.Principal;
using WAFIA.Database.Types;

namespace WAFIA.Database.Connectors
{
    public class AccountConnector
    {
        public AccountConnector(NpgsqlConnector connector)
        {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;

        public async Task<bool> Add(Account account)
        {
            cmd.CommandText = $"INSERT INTO account (login, mail, password, is_admin) VALUES (@Login, @Mail, @Password, @IsAdmin)";
            cmd.Parameters.AddWithValue("Login", account.Login);
            cmd.Parameters.AddWithValue("Mail", account.Mail);
            cmd.Parameters.AddWithValue("Password", account.Password);
            cmd.Parameters.AddWithValue("IsAdmin", account.IsAdmin);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                cmd.Parameters.Clear();
                return false;
            }
            cmd.Parameters.Clear();
            return true;
        }

        public async Task<KeyValuePair<long, bool>?> GetIdRights(string login, string password)
        {

            cmd.CommandText = "SELECT id, is_admin FROM account WHERE " +
                $"login = '{login}' AND " +
                $"password = '{password}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try
            {
                if (await reader.ReadAsync())
                {
                    var res = new KeyValuePair<long, bool>((long)reader["id"], (bool)reader["is_admin"]);
                    reader.Close();
                    return res;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch
            {
                reader.Close();
                return null;
            }
        }

        public async Task<KeyValuePair<long, string>?> GetIdMail(string login)
        {

            cmd.CommandText = $"SELECT id, mail FROM account WHERE login = '{login}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            try
            {
                if (await reader.ReadAsync())
                {
                    var res = new KeyValuePair<long, string>((long)reader["id"], (string)reader["mail"]);
                    reader.Close();
                    return res;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch
            {
                reader.Close();
                return null;
            }
        }

        public async Task<bool> ChangePassword(long id, string password)
        {

            cmd.CommandText = $"UPDATE account SET password='{password}' WHERE id = '{id}'";

            try
            {
                if (await cmd.ExecuteNonQueryAsync() == 0)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Edit(Account account)
        {
            if (account.Id == 0)
            {
                return false;
            }

            cmd.CommandText = $"UPDATE account" +
                $" SET login='{account.Login}'," +
                $"password='{account.Password}', " +
                $"mail='{account.Mail}'" +
                $"WHERE id = '{account.Id}'";

            try
            {
                if (await cmd.ExecuteNonQueryAsync() != 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeRights(string login, bool isAdmin)
        {

            cmd.CommandText = $"UPDATE account SET is_admin='{isAdmin}' WHERE login = '{login}'";

            try
            {
                if (await cmd.ExecuteNonQueryAsync() != 0)
                {
                    return true;
                };
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(string login)
        {
            cmd.CommandText = $"DELETE  FROM account WHERE login='{login}'";
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<Account?> Get(long id)
        {
            cmd.CommandText = "SELECT login, mail, password, is_admin FROM account WHERE " +
                $"id = '{id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try
            {
                if (await reader.ReadAsync())
                {
                    var account = new Account(
                        id,
                        (string)reader["login"],
                        (string)reader["mail"],
                        (string)reader["password"],
                        (bool)reader["is_admin"]);
                    reader.Close();
                    return account;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch
            {
                reader.Close();
                return null;
            }
        }
    }
}
