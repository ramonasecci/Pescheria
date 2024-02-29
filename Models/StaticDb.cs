namespace Pescheria.Models
{
    public static class StaticDb
    {
        private static int _maxId = 3;
        private static List<Fish> _fishes = [
            new Fish(){FishId = 1, Name = "Trota", IsSeaFish = false, Price = 1500},
            new Fish(){FishId = 2, Name = "Salmone", IsSeaFish = false, Price = 3000},
            new Fish(){FishId = 3, Name = "Sgombro", IsSeaFish = true, Price = 800},
        ];

        public static List<Fish> GetAll()
        {
            return _fishes;
        }

        public static List<Fish> GetAllDeleted()
        {
            List<Fish> deletedFishes = [];
            foreach (var fish in _fishes)
            {
                if (fish.DeletedAt is not null)
                {
                    deletedFishes.Add(fish);
                }
            }
            return deletedFishes;

            //return _fishes;
        }

        public static Fish? Recover(int idToRecover)
        {
            int? index = findFishIndex(idToRecover);

            if (index is not null)
            {
                var fishRecovered = _fishes[(int)index];
                fishRecovered.DeletedAt = null;
                return fishRecovered;
            }

            return null;
        }

        public static Fish? GetById(int? id)
        {
            if (id is null) return null;

            for(int i = 0; i < _fishes.Count; i++)
            {
                Fish fish = _fishes[i];
                if (fish.FishId == id)
                {
                    return fish;
                }
            }

            return null;
        }

        public static Fish Add(string name, bool isSeaFish, int price)
        {
            _maxId++;
            var fish = new Fish() { FishId = _maxId, Name = name, IsSeaFish = isSeaFish, Price = price };
            _fishes.Add(fish);
            return fish;
        }

        public static Fish? Modify(Fish fish)
        {
            foreach(var fishInList in _fishes)
            {
                if(fishInList.FishId== fish.FishId)
                {
                    fishInList.Name = fish.Name;
                    fishInList.Price = fish.Price;
                    fishInList.IsSeaFish = fish.IsSeaFish;
                    return fishInList;
                }
               
               

            }
            return null;
        }


        public static Fish? FishSoftDelete(Fish fish)
        {
            //ha bisogno di un ulteriore campo nella tabella deletedAt(o null o data di eliminazione)
            int? deletedIndex = findFishIndex(fish.FishId);
            if (deletedIndex != null)
            {
                var fishDelited = _fishes[(int)deletedIndex];
                fishDelited.DeletedAt = DateTime.Now;
            }
                return fish;
        }




        public static Fish? HardDelete(int idToDelete)
        {


            //cancella definitivamente l'informazione 
            int? deletedIndex = findFishIndex(idToDelete);
            if (deletedIndex != null)
            { 
                var fishDelited = _fishes[(int)deletedIndex];
                _fishes.RemoveAt((int)deletedIndex);
                return fishDelited;
            }
               
                
            

            return null;

        }

        private static int? findFishIndex(int idToDelete)
        {
            int i;
            bool fishFound = false;
            for (i = 0; i < _fishes.Count; i++)
            {
                if (_fishes[i].FishId == idToDelete)
                {
                    fishFound = true;
                    break;
                }
            }
            if (fishFound) return i;
            return null;
        }

    }
}
