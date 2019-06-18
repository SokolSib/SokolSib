using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animals.DTM.Model;
using Microsoft.EntityFrameworkCore;

namespace Animals.DTM
{
    public class AnimalsRepozitory : DbContext, IAnimalsRepozitory
    {
        public AnimalsRepozitory(DbContextOptions<AnimalsRepozitory> options)
            : base(options)
        {
        }

        private DbSet<DAL.Animals> Animals { get; set; }

        private DbSet<DAL.AnimalTypeColors> AnimalTypeColors { get; set; }

        private DbSet<DAL.Locations> Locations { get; set; }

        private DbSet<DAL.Regions> Regions { get; set; }

        private DbSet<DAL.AnimalColors> AnimalColors { get; set; }

        private DbSet<DAL.AnimalTypes> AnimalTypes { get; set; }

        public IEnumerable<AnimalType> Get()
        {
            return GetFromDal().ToArray();
        }

        public async Task<AnimalType> Get(int id)
        {
            return await GetFromDal().SingleOrDefaultAsync(m => m.Id == id);
        }

        public IEnumerable<AnimalType> Find(string[] regions, string color, string type)
        {
            regions = regions.Select(x => x.ToLower()).ToArray();
            return GetFromDal().Where(m => (regions == null || regions.Length == 0 || regions.Contains(m.RegionName.ToLower())) &&
                                                 (string.IsNullOrEmpty(color) || m.ColorName.ToLower() == color.ToLower()) &&
                                                 (string.IsNullOrEmpty(type) || m.TypeName.ToLower() == type.ToLower())).ToArray();
        }

        public async Task Del(int id)
        {
            Animals.Remove(new DAL.Animals() { Id = id });
            await SaveChangesAsync();
        }

        public async Task<bool> Upsert(AnimalType animal, bool isUpdate = false)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    var existAnimal = Animals.SingleOrDefault(x => x.Id == animal.Id);
                    if (isUpdate && existAnimal is null)
                        throw new KeyNotFoundException();
                    else
                        existAnimal.Name = animal.Name;

                    var existRegion = Regions.SingleOrDefault(x => x.RegionName == animal.RegionName);
                    if (existRegion == null)
                    {
                        existRegion = new DAL.Regions()
                        {
                            RegionName = animal.RegionName
                        };
                        Regions.Add(existRegion);
                        await SaveChangesAsync();
                    }
                    var existLocation = Locations.SingleOrDefault(x => x.LocationName == animal.LocationName);
                    if (existLocation == null)
                    {
                        existLocation = new DAL.Locations()
                        {
                            LocationName = animal.LocationName,
                            RegionId = existRegion.Id
                        };
                        Locations.Add(existLocation);
                        await SaveChangesAsync();
                    }
                    var existColor = AnimalColors.SingleOrDefault(x => x.Color == animal.ColorName);
                    if (existColor == null)
                    {
                        existColor = new DAL.AnimalColors()
                        {
                            Color = animal.ColorName
                        };
                        AnimalColors.Add(existColor);
                        await SaveChangesAsync();
                    }
                    var existType = AnimalTypes.SingleOrDefault(x => x.TypeName == animal.TypeName);
                    if (existType == null)
                    {
                        existType = new DAL.AnimalTypes()
                        {
                            TypeName = animal.TypeName
                        };
                        AnimalTypes.Add(existType);
                        await SaveChangesAsync();
                    }
                    var existTypeColor = AnimalTypeColors.SingleOrDefault(x => x.TypeColorName == animal.TypeColorName);
                    if (existTypeColor == null)
                    {
                        existTypeColor = new DAL.AnimalTypeColors()
                        {
                            TypeColorName = animal.TypeColorName,
                            ColorId = existColor.Id,
                            TypeId = existType.Id
                        };
                        AnimalTypeColors.Add(existTypeColor);
                    }
                    else
                    {
                        existTypeColor.ColorId = existColor.Id;
                        existTypeColor.TypeId = existType.Id;
                    }
                    await SaveChangesAsync();
                    if (existAnimal == null)
                    {
                        existAnimal = new DAL.Animals()
                        {
                            LocationId = existLocation.Id,
                            TypeColorId = existTypeColor.Id,
                            Name = animal.Name
                        };
                        Animals.Add(existAnimal);
                    }
                    else Animals.Update(existAnimal);
                    await SaveChangesAsync();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool Exists(int id)
        {
            return Animals.Any(e => e.Id == id);
        }

        public EntityState State
        {
            get { return Entry(Animals).State; }
            set { Entry(Animals).State = value; }
        }

        private IQueryable<AnimalType> GetFromDal()
        {
            return from animal in Animals
                   join typeColor in AnimalTypeColors on animal.TypeColorId equals typeColor.Id
                   join color in AnimalColors on typeColor.ColorId equals color.Id
                   join type in AnimalTypes on typeColor.TypeId equals type.Id
                   join location in Locations on animal.LocationId equals location.Id
                   join region in Regions on location.RegionId equals region.Id
                   select new AnimalType(animal, typeColor, color, type, location, region);
        }
    }
}
