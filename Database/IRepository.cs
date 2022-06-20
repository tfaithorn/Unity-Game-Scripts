using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepository<T>
{
    List<T> GetByCriteria(List<SqlClient.Expr> criteria = null);
}
