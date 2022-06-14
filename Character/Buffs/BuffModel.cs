using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public static class BuffModel
{
    private static string dbName = "URI=file:database.db";

    public static Buff GetBuff(int id)
    {
        Buff buff = new Buff();


        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM buffs WHERE id = " + id + ";";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buff.id = reader.GetInt32(reader.GetOrdinal("id"));
                        buff.name = reader.GetString(reader.GetOrdinal("name"));
                        buff.isPermanent = reader.GetBoolean(reader.GetOrdinal("isPermanant"));
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }

        return buff;
    }
}
