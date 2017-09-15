using dojo_league.Models;
using System.Collections.Generic;
namespace dojo_league.Factory
{
    public interface IFactory<T> where T : BaseEntity
    {
    }
}