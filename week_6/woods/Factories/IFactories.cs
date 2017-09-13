using woods.Models;
using System.Collections.Generic;
namespace woods.Factory
{
    public interface IFactory<T> where T : BaseEntity
    {
    }
}