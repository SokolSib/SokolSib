namespace Animals.DTM.Model
{
    public class AnimalType
    {
        public AnimalType(DAL.Animals animal, 
            DAL.AnimalTypeColors typeColor,
            DAL.AnimalColors color,
            DAL.AnimalTypes type,
            DAL.Locations location,
            DAL.Regions region)
        {
            Id = animal.Id;
            Name = animal.Name;
            TypeColorName = typeColor.TypeColorName;
            LocationName = location.LocationName;
            RegionName = region.RegionName;
            TypeName = type.TypeName;
            ColorName = color.Color;
            Url = type.Url;
        }

        public AnimalType(int id,
            string name,
            string typeColorName,
            string locationName,
            string regionName,
            string typeName,
            string colorName,
            string url)
        {
            Id = id;
            Name = name;
            TypeColorName = typeColorName;
            LocationName = locationName;
            RegionName = regionName;
            TypeName = typeName;
            ColorName = colorName;
            Url = url;
        }

        public string Name { get; }

        public string TypeName { get; }

        public string ColorName { get; }

        public string TypeColorName { get; }

        public string LocationName { get; }

        public string RegionName { get; }

        public string Url { get; }

        public int Id { get; }
    }
}
