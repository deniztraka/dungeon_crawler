using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Interfaces
{
    interface IDb
    {
        T GetByID<T>(string id);
    }
}