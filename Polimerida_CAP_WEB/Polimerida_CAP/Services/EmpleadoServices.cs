using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using Polimerida_CAP.Helpers;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Polimerida_CAP.Services;

public class EmpleadoServices : IEmpleadoServices
{
    private readonly AppDbContext _ctx;
    private readonly IFileApiService _fileApiService;
    private readonly IDispositivoServices _dispositivoServices;
    private readonly string _imageBasePath;

    public EmpleadoServices(
        AppDbContext ctx, 
        IFileApiService fileApiService,
        IDispositivoServices dispositivoServices,
        IConfiguration configuration)
    {
        _ctx = ctx;
        _fileApiService = fileApiService;
        _dispositivoServices = dispositivoServices;
        _imageBasePath = configuration["FileConsultaSettings:BaseUrl"] ?? "C:\\image\\imagesapp";
    }

    public async Task<IEnumerable<EmpleadoViewModel>> GetAllAsync()
    {
        return await _ctx.Empleado
            .Select(e => new EmpleadoViewModel
            {
                IdEmpleado = (int)e.Idempleado,
                PrimerNombre = e.Primernombre,
                ApellidoPaterno = e.Apellidopaterno,
                ApellidoMaterno = e.Apellidomaterno,
                Email = e.Email,
                IdDepartamento = (int?)e.Iddepartamento,
                IdPuesto = (int?)e.Idpuesto,
                UrlFoto = e.Nombreimagen,
                RegStatus = e.RegStatus,
                Sexo = e.Sexo,
                FechaIngreso = e.Fechaingreso,
                IdDispositivo = (int?)e.Iddispositivo,
                IdSubgrupo = (int?)e.Idsubgrupo,
                Credencial = e.Credencial,
                FechaBaja = e.Fechabaja == DateTime.MinValue ? (DateTime?)null : e.Fechabaja,
                DepartamentoNombre = _ctx.Departamento.Where(d => d.Iddepartamento == e.Iddepartamento).Select(d => d.Descripcion).FirstOrDefault() ?? "",
                PuestoNombre = _ctx.Puesto.Where(p => p.Idpuesto == e.Idpuesto).Select(p => p.Descripcion).FirstOrDefault() ?? "",
                DispositivoNombre = _ctx.Dispositivo.Where(d => d.Iddispositivo == e.Iddispositivo).Select(d => d.Descripcion).FirstOrDefault() ?? "",
                TurnoNombre = (
                    from sg in _ctx.Subgrupo
                    join t in _ctx.Turno on sg.Idturno equals (int)t.Idturno
                    where sg.Idsubgrupo == e.Idsubgrupo
                    select t.Nombre
                ).FirstOrDefault() ?? ""
            })
            .ToListAsync();
    }

    public async Task<EmpleadoViewModel?> GetByIdAsync(int id)
    {
        var e = await _ctx.Empleado.FindAsync((uint)id);
        if (e == null) return null;

        return new EmpleadoViewModel
        {
            IdEmpleado = (int)e.Idempleado,
            PrimerNombre = e.Primernombre,
            ApellidoPaterno = e.Apellidopaterno,
            ApellidoMaterno = e.Apellidomaterno,
            Email = e.Email,
            IdDepartamento = (int?)e.Iddepartamento,
            IdPuesto = (int?)e.Idpuesto,
            UrlFoto = e.Nombreimagen,
            RegStatus = e.RegStatus,
            Sexo = e.Sexo,
            FechaIngreso = e.Fechaingreso,
            IdDispositivo = (int?)e.Iddispositivo,
            IdSubgrupo = (int?)e.Idsubgrupo,
            Credencial = e.Credencial
        };
    }

    public async Task<(EmpleadoViewModel, DeviceResponse?)> CreateAsync(EmpleadoViewModel vm)
    {
        string? imageUrl = null;
        DeviceResponse? deviceResponse = null;

        if (vm.Foto is { Length: > 0 })
        {
            string folder = "default";
            string fileName = vm.Credencial.ToString(); // sin extensión
            if (vm.IdDispositivo.HasValue)
            {
                var device = await _ctx.Dispositivo.FindAsync((uint)vm.IdDispositivo.Value);
                if (device != null)
                    folder = device.Descripcion;
            }
            imageUrl = await _fileApiService.UploadFileAsync(vm.Foto, folder, fileName);
            vm.UrlFoto = fileName + System.IO.Path.GetExtension(vm.Foto?.FileName ?? "");
        }

        var extension = System.IO.Path.GetExtension(vm.Foto?.FileName ?? "");
        var ent = new Empleado
        {
            Primernombre = vm.PrimerNombre,
            Apellidopaterno = vm.ApellidoPaterno,
            Apellidomaterno = vm.ApellidoMaterno ?? "",
            Email = vm.Email,
            Iddepartamento = (uint)(vm.IdDepartamento ?? 0),
            Idpuesto = (uint)(vm.IdPuesto ?? 0),
            RegStatus = vm.RegStatus,
            Segundonombre = "",
            Sexo = vm.Sexo,
            Fechaingreso = vm.FechaIngreso ?? DateTime.UtcNow,
            Credencial = vm.Credencial,
            Nombreimagen = vm.Credencial.ToString() + extension,
            Iddispositivo = vm.IdDispositivo ?? 0,
            Fechabaja = DateTime.MinValue,
            Idsubgrupo = vm.IdSubgrupo,
        };

        _ctx.Empleado .Add(ent);
        await _ctx.SaveChangesAsync();
        vm.IdEmpleado = (int)ent.Idempleado;

        // Register employee in the device if a device is selected
        if (vm.IdDispositivo.HasValue)
        {
            var device = await _ctx.Dispositivo .FindAsync((uint)vm.IdDispositivo.Value);
            if (device != null)
            {
                string folder = device.Descripcion;
                string deviceImageUrl = $"{_imageBasePath.TrimEnd('/')}/{folder}/{ent.Nombreimagen}";
                deviceResponse = await _dispositivoServices.RegisterEmployeeWithFaceAsync(device.Ip, ent, deviceImageUrl);
            }
        }

        return (vm, deviceResponse);
    }

    public async Task<(EmpleadoViewModel?, DeviceResponse?)> UpdateAsync(int id, EmpleadoViewModel vm)
    {
        var ent = await _ctx.Empleado.FindAsync((uint)id);
        if (ent == null) return (null, null);

        string? imageUrl = null;
        DeviceResponse? deviceResponse = null;

        if (vm.Foto is { Length: > 0 })
        {
            string folder = "default";
            string fileName = vm.Credencial.ToString(); // sin extensión
            if (vm.IdDispositivo.HasValue)
            {
                var device = await _ctx.Dispositivo.FindAsync((uint)vm.IdDispositivo.Value);
                if (device != null)
                    folder = device.Descripcion;
            }
            imageUrl = await _fileApiService.UploadFileAsync(vm.Foto, folder, fileName);
            vm.UrlFoto = fileName + System.IO.Path.GetExtension(vm.Foto?.FileName ?? "");
        }

        // Store the old device ID to check if it changed
        var oldDeviceId = ent.Iddispositivo;

        ent.Primernombre = vm.PrimerNombre;
        ent.Apellidopaterno = vm.ApellidoPaterno;
        ent.Apellidomaterno = vm.ApellidoMaterno ?? "";
        ent.Email = vm.Email;
        ent.Sexo = vm.Sexo;
        ent.Fechaingreso = vm.FechaIngreso ?? ent.Fechaingreso;
        ent.Iddepartamento = (uint)(vm.IdDepartamento ?? 0);
        ent.Idpuesto = (uint)(vm.IdPuesto ?? 0);
        ent.Iddispositivo = vm.IdDispositivo ?? 0;
        ent.Idsubgrupo = vm.IdSubgrupo;
        ent.Credencial = vm.Credencial;
        if (imageUrl != null)
        {
            ent.Nombreimagen = vm.Credencial.ToString() + System.IO.Path.GetExtension(vm.Foto?.FileName ?? "");
        }
        ent.RegStatus = vm.RegStatus;

        await _ctx.SaveChangesAsync();

        // If the device has changed or if we're adding a device for the first time
        bool imagenModificada = imageUrl != null;
        if (vm.IdDispositivo.HasValue)
        {
            var device = await _ctx.Dispositivo .FindAsync((uint)vm.IdDispositivo.Value);
            if (device != null)
            {
                string folder = device.Descripcion;
                string deviceImageUrl = $"{_imageBasePath.TrimEnd('/')}/{folder}/{ent.Nombreimagen}";
              
                deviceResponse = await _dispositivoServices.EditarUsuarioConFaceAsync(device.Ip, ent, deviceImageUrl, imagenModificada);
            }
        }

        return (vm, deviceResponse);
    }

    public async Task<bool> DeleteAsync(int id, DateTime? fechaBaja = null)
    {
        var ent = await _ctx.Empleado.FindAsync((uint)id);
        if (ent == null) return false;
        ent.RegStatus = "B";
        ent.Fechabaja = fechaBaja ?? DateTime.Now;
        await _ctx.SaveChangesAsync();
        // Si tiene dispositivo, eliminar también en el dispositivo
        if (ent.Iddispositivo != 0)
        {
            var device = await _ctx.Dispositivo.FindAsync((int)ent.Iddispositivo);
            if (device != null)
            {
                await _dispositivoServices.EliminarUsuarioAsync(device.Ip, ent);
            }
        }
        return true;
    }
} 