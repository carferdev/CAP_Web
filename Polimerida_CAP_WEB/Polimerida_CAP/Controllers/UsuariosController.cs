using Microsoft.AspNetCore.Mvc;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Polimerida_CAP.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _ctx;
        public UsuariosController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            var usuarios = _ctx.Usuario.Select(u => new UsuarioViewModel
            {
                Idusuario = u.Idusuario,
                Usuario = u.Usuario1 ?? "",
                Password = u.Password ?? "",
                RegStatus = u.RegStatus
            }).ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var usuario = new Usuario
            {
                Usuario1 = vm.Usuario,
                Password = Polimerida_CAP.Helpers.JwtHelper.EncryptPassword(vm.Password, "Polimerida_CAP"),
                RegStatus = vm.RegStatus
            };
            _ctx.Usuario.Add(usuario);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(uint id)
        {
            var usuario = _ctx.Usuario.FirstOrDefault(u => u.Idusuario == id);
            if (usuario == null) return NotFound();
            var vm = new UsuarioViewModel
            {
                Idusuario = usuario.Idusuario,
                Usuario = usuario.Usuario1 ?? "",
                Password = usuario.Password ?? "",
                RegStatus = usuario.RegStatus
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, UsuarioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var usuario = _ctx.Usuario.FirstOrDefault(u => u.Idusuario == id);
            if (usuario == null) return NotFound();
            usuario.Usuario1 = vm.Usuario;
            usuario.Password = Polimerida_CAP.Helpers.JwtHelper.EncryptPassword(vm.Password, "Polimerida_CAP");
            usuario.RegStatus = vm.RegStatus;
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(uint id)
        {
            var usuario = _ctx.Usuario.FirstOrDefault(u => u.Idusuario == id);
            if (usuario == null) return NotFound();
            var vm = new UsuarioViewModel
            {
                Idusuario = usuario.Idusuario,
                Usuario = usuario.Usuario1 ?? "",
                Password = usuario.Password ?? "",
                RegStatus = usuario.RegStatus
            };
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var usuario = _ctx.Usuario.FirstOrDefault(u => u.Idusuario == id);
            if (usuario == null) return NotFound();
            _ctx.Usuario.Remove(usuario);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
} 