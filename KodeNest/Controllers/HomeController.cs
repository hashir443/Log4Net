using System.Threading.Tasks;
using KodeNest.Entity;
using KodeNest.Service.Interface;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IHomeService _service;

    public HomeController(IHomeService service)
    {
        _service = service;
    }

    public async Task<ActionResult> Index()
    {
        var homes = await _service.GetAllAsync();
        return View(homes);
    }

    public async Task<ActionResult> Details(int id)
    {
        var home = await _service.GetByIdAsync(id);
        return View(home);
    }

    public ActionResult Create() => View();

    [HttpPost]
    public async Task<ActionResult> Create(HomeRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        await _service.CreateAsync(request);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Edit(int id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) return HttpNotFound();

        return View(new HomeRequest
        {
            Title = existing.Title,
            Address = existing.Address
        });
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, HomeRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        await _service.UpdateAsync(id, request);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int id)
    {
        var home = await _service.GetByIdAsync(id);
        return View(home);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
