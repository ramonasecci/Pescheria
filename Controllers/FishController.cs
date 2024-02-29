using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Pescheria.Models;


namespace Pescheria.Controllers
{
    public class FishController : Controller
    {
        // lista dei pesci
        public IActionResult Index()
        {
            return View(StaticDb.GetAll());
        }

        // pagina di dettaglio di un singolo pesce
        public IActionResult Details([FromRoute] int? id)
        {
            if (id.HasValue)  // (id is null)
            {
                return View(StaticDb.GetById(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
                //retrun Redirect("https://localhost:7029");
            }
        }

        // pagina con un form per l'aggiunta di un nuovo pesce
        public IActionResult Add()
        {
            return View();
        }

        // indirizzo per gestire il submit del form della pagina Add
        [HttpPost]
        public IActionResult Add(Fish fish, IFormFile image)
        {
            //salviamo i file che ci è stato inviato, devono finire nella wwwroot
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            //prendiamo il path del file input del view
            string fileName = Path.GetFileName(image.FileName);
            //crea il file
            FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            image.CopyTo(stream);

            var newFish = StaticDb.Add(fish.Name, fish.IsSeaFish, fish.Price *100);
            //return Redirect("https://localhost:7029/fish/details/" + fish.FishId);
            return RedirectToAction("Details", new { id = newFish.FishId });
        }

        // pagina con un form per la modififica di un pesce
        public IActionResult Edit([FromRoute] int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Index", "Fish");
            }

            var fish = StaticDb.GetById(id);
            if (fish is null) return View("Error");

            // gestire il caso di id non passato
            return View(fish);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {

            var fish = StaticDb.GetById(id);

            return View(fish);

        }

        [HttpPost]
        public IActionResult Delete(Fish fish)
        {

            var fishDeleted = StaticDb.HardDelete(fish.FishId);
            if(fishDeleted is not null)
            {

            TempData["MessageSuccess"] = $"Risorsa {fishDeleted.Name} eliminata";
            //mandare alert di avviso
            return RedirectToAction("Index");
            }

            TempData["MessageError"] = "C'è stato un problema durante l'eliminazione";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSoft(Fish fish)
        {
            var fishDeleted = StaticDb.FishSoftDelete(fish);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Fish fish)
        {

            var updatedFish = StaticDb.Modify(fish);
            if (updatedFish is null) return View("Error");
            return RedirectToAction("Details", new { id = updatedFish.FishId });

        }

        // rotta (indirizzo) per poter elimanare un pesce

        [HttpGet]
        public IActionResult Cestino()
        {
            var fishesDeleted = StaticDb.GetAllDeleted();
            return View(fishesDeleted);
        }

        [HttpPost]
        public IActionResult Recover(Fish fish)
        {
            var recoveredFish = StaticDb.Recover(fish.FishId);
            if (recoveredFish is null)
            {
                return RedirectToAction("Cestino");
            }
            return RedirectToAction("Index");
        }


        /*********************/

        public IActionResult GetFile()
        {
            return PhysicalFile(Directory.GetCurrentDirectory() + "\\Contents\\spedire.txt", "text/plain");
        }

        public IActionResult GetSlug()
        {
            return Json("questo-slug-buono");
        }
    
}
}



