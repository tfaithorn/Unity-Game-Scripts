using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRepository //: DbRepository, IRepository<Ability>
{
    /*
    const string tableName = "ability";
    const string fields = "*";
    const string tableJoins = "";
    const string orderBy = "name ASC";

    public override string GetTableName()
    {
        return tableName;
    }

    public override string GetFields()
    {
        return fields;
    }

    public override string GetTableJoins()
    {
        return tableJoins;
    }

    public override string GetOrderBy()
    {
        return orderBy;
    }

    public List<Ability> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        var result = GetResult(criteria);
        var abilities = new List<Ability>();

        foreach (var row in result)
        {
            var ability = new Ability(
                (long)row["id"],
                (string)row["name"],
                (string)row["className"]
            );

            if (row["cooldown"] != null)
            {
                ability.cooldown = new Timer((float)row["cooldown"]);
            }

            if (row["icon"] != null)
            {
                ability.icon = (string)row["icon"];
            }

            if (row["duration"] != null)
            {
                ability.duration = new Timer((float)row["duration"]);
            }

            abilities.Add(ability);
        }

        return abilities;
    }
    */
}
