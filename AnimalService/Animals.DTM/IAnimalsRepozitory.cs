using Animals.DTM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Animals.DTM
{
    public interface IAnimalsRepozitory : IDisposable
    {
        IEnumerable<AnimalType> Get();

        Task<AnimalType> Get(int id);

        Task Del(int id);

        Task<bool> Upsert(AnimalType animal, bool isUpdate = false);
        
        bool Exists(int id);

        EntityState State { get; set; }

        IEnumerable<AnimalType> Find(string[] regions, string color, string type);
    }
}
