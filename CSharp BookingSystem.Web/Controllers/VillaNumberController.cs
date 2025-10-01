public IActionResult Edit(int villaNumberId)
{
    var villaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId);
    if (villaNumber == null)
    {
        return RedirectToAction("Error", "Home");
    }

    VillaNumberVM villaNumberVM = new()
    {
        VillaNumber = villaNumber,
        VillaList = _context.Villas.Select(v => new SelectListItem
        {
            Text = v.Name,
            Value = v.Id.ToString()
        }).ToList()
    };

    return View(villaNumberVM);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(VillaNumberVM vm)
{
    if (ModelState.IsValid)
    {
        _context.VillaNumbers.Update(vm.VillaNumber);
        _context.SaveChanges();
        TempData["success"] = "The villa Number has been updated successfully";
        return RedirectToAction("Index");
    }

    vm.VillaList = _context.Villas.Select(x => new SelectListItem
    {
        Text = x.Name,
        Value = x.Id.ToString()
    }).ToList();
    return View(vm);
}