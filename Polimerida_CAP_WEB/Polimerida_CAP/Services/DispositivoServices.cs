using Microsoft.EntityFrameworkCore;
using Polimerida_CAP.Models;
using Polimerida_CAP.Services.Data;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Polimerida_CAP.Services;

public class DispositivoServices : IDispositivoServices
{
    private readonly AppDbContext _ctx;
    private readonly DeviceSettings _deviceSettings;
    private const string FACE_API_ENDPOINT = "/ISAPI/Intelligent/FDLib/FaceDataRecord?format=json";
    
    public DispositivoServices(AppDbContext ctx, IOptions<DeviceSettings> deviceSettings)
    {
        _ctx = ctx;
        _deviceSettings = deviceSettings.Value;
    }

    public async Task<DeviceResponse> RegisterEmployeeWithFaceAsync(string ipAddress, Empleado employee, string? faceUrl = null)
    {
        var response = new DeviceResponse();
        var baseUrl = $"http://{ipAddress}";
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(_deviceSettings.Username, _deviceSettings.Password)
        };

        try
        {
            using (var client = new HttpClient(handler))
            {
                // Register user info
                var userRequest = new UserInfoRequest
                {
                    UserInfo = new UserInfo
                    {
                        employeeNo = employee.Credencial.ToString(),
                        name = $"{employee.Primernombre} {employee.Segundonombre} {employee.Apellidopaterno} {employee.Apellidomaterno}".Trim(),
                        userType = "normal",
                        doorRight = "1",
                        RightPlan = new[]
                        {
                            new RightPlan { doorNo = 1, planTemplateNo = "1" }
                        },
                        gender = employee.Sexo,
                        localUIRight = false,
                        maxOpenDoorTime = 0,
                        userVerifyMode = "",
                        Valid = new Valid
                        {
                            enable = true,
                            beginTime = employee.Fechaingreso.ToString("yyyy-MM-ddTHH:mm:ss"),
                            endTime = "2032-12-12T23:59:59",
                            timeType = "local"
                        }
                    }
                };

                var userJson = JsonSerializer.Serialize(userRequest);
                var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");
                var userResponse = await client.PostAsync(baseUrl + _deviceSettings.ApiEndpoint, userContent);
                
                response.UserResponse = await userResponse.Content.ReadAsStringAsync();
                response.UserSuccess = userResponse.IsSuccessStatusCode;

                // If face URL is provided, register face data
                if (!string.IsNullOrEmpty(faceUrl))
                {
                    var faceData = new FaceData
                    {
                        faceLibType = "blackFD",
                        FDID = "1",
                        FPID = employee.Credencial.ToString(),
                        featurePointType = "face",
                        faceURL = faceUrl
                    };

                    var faceJson = JsonSerializer.Serialize(faceData);
                    var faceContent = new StringContent(faceJson, Encoding.UTF8, "application/json");
                    var faceResponse = await client.PostAsync(baseUrl + FACE_API_ENDPOINT, faceContent);
                    
                    response.FaceResponse = await faceResponse.Content.ReadAsStringAsync();
                    response.FaceSuccess = faceResponse.IsSuccessStatusCode;
                }
            }
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.UserSuccess = false;
            response.FaceSuccess = false;
        }

        return response;
    }

    public async Task<IEnumerable<DispositivoViewModel>> GetAllAsync()
    {
        return await _ctx.Dispositivo   .Where(x=>x.RegStatus  =="A")
            .Select(x=> new DispositivoViewModel
            {
                IdDispositivo = (int)x.Iddispositivo  ,
                Descripcion = x.Descripcion  ,
                Clase = x.Clase  ,
                Division = x.Division  ,
                Tipo = x.Tipo  ,
                Ip = x.Ip  ,
                RegStatus = x.RegStatus  
            }).ToListAsync();
    }

    public async Task<DispositivoViewModel?> GetByIdAsync(int id)
    {
        var d = await _ctx.Dispositivo .FindAsync((uint)id);
        if(d==null) return null;
        return new DispositivoViewModel
        {
            IdDispositivo = (int)d.Iddispositivo  ,
            Descripcion = d.Descripcion  ,
            Clase = d.Clase  ,
            Division = d.Division  ,
            Tipo = d.Tipo  ,
            Ip = d.Ip ,
            RegStatus = d.RegStatus   
        };
    }

    public async Task<DispositivoViewModel> CreateAsync(DispositivoViewModel vm)
    {
        var ent = new Services.Data.Dispositivo
        {
            Descripcion = vm.Descripcion,
            Clase = vm.Clase,
            Division = vm.Division,
            Tipo = vm.Tipo,
            Ip = vm.Ip,
            RegStatus = vm.RegStatus
        };
        _ctx.   Dispositivo .Add(ent);
        await _ctx.SaveChangesAsync();
        vm.IdDispositivo = (int)ent.Iddispositivo;
        return vm;
    }

    public async Task<DispositivoViewModel?> UpdateAsync(int id, DispositivoViewModel vm)
    {
        var ent = await _ctx.Dispositivo  .FindAsync((uint)id);
        if(ent==null) return null;
        ent.Division   = vm.Descripcion;
        ent.Clase   = vm.Clase;
        ent.Division   = vm.Division;
        ent.Tipo   = vm.Tipo;
        ent.Ip   = vm.Ip;
        ent.RegStatus   = vm.RegStatus;
        await _ctx.SaveChangesAsync();
        return vm;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ent = await _ctx.Dispositivo  .FindAsync((int)id);
        if(ent==null) return false;
        ent.RegStatus   = "B";
        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<DeviceResponse> EditarUsuarioConFaceAsync(string ipAddress, Empleado employee, string? faceUrl, bool imagenModificada)
    {
        var response = new DeviceResponse();
        var baseUrl = $"http://{ipAddress}";
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(_deviceSettings.Username, _deviceSettings.Password)
        };
        try
        {
            using (var client = new HttpClient(handler))
            {
                // Modificar datos del usuario
                var userRequest = new UserInfoRequest
                {
                    UserInfo = new UserInfo
                    {
                        employeeNo = employee.Credencial.ToString(),
                        name = $"{employee.Primernombre} {employee.Segundonombre} {employee.Apellidopaterno} {employee.Apellidomaterno}".Trim(),
                        userType = "normal",
                        doorRight = "1",
                        RightPlan = new[]
                        {
                            new RightPlan { doorNo = 1, planTemplateNo = "1" }
                        },
                        gender = employee.Sexo,
                        localUIRight = false,
                        maxOpenDoorTime = 0,
                        userVerifyMode = "",
                        Valid = new Valid
                        {
                            enable = true,
                            beginTime = employee.Fechaingreso.ToString("yyyy-MM-ddTHH:mm:ss"),
                            endTime = "2032-12-12T23:59:59",
                            timeType = "local"
                        }
                    }
                };
                var userJson = System.Text.Json.JsonSerializer.Serialize(userRequest);
                var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");
                var userResponse = await client.PutAsync(baseUrl + _deviceSettings.ApiEndpoint.Replace("Record", "Modify"), userContent);
                response.UserResponse = await userResponse.Content.ReadAsStringAsync();
                response.UserSuccess = userResponse.IsSuccessStatusCode;
                // Si la imagen fue modificada, elimina la cara anterior y registra la nueva
                if (imagenModificada && !string.IsNullOrEmpty(faceUrl))
                {
                    // Eliminar cara anterior
                    string endpointDelete = "/ISAPI/Intelligent/FDLib/FDSearch/Delete?format=json&FDID=1&faceLibType=blackFD";
                    var deleteRequest = new FpidRequest
                    {
                        FPID = new List<Fpid>
                        {
                            new Fpid { value = employee.Credencial.ToString() }
                        }
                    };
                    string jsonDelete = System.Text.Json.JsonSerializer.Serialize(deleteRequest);
                    var contentDelete = new StringContent(jsonDelete, Encoding.UTF8, "application/json");
                    await client.PutAsync(baseUrl + endpointDelete, contentDelete);
                    // Registrar nueva cara
                    var faceData = new FaceData
                    {
                        faceLibType = "blackFD",
                        FDID = "1",
                        FPID = employee.Credencial.ToString(),
                        featurePointType = "face",
                        faceURL = faceUrl
                    };
                    var faceJson = System.Text.Json.JsonSerializer.Serialize(faceData);
                    var faceContent = new StringContent(faceJson, Encoding.UTF8, "application/json");
                    var faceResponse = await client.PostAsync(baseUrl + FACE_API_ENDPOINT, faceContent);
                    response.FaceResponse = await faceResponse.Content.ReadAsStringAsync();
                    response.FaceSuccess = faceResponse.IsSuccessStatusCode;
                }
            }
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.UserSuccess = false;
            response.FaceSuccess = false;
        }
        return response;
    }

    public async Task<DeviceResponse> EliminarFotoEmpleadoAsync(string ipAddress, Empleado employee)
    {
        var response = new DeviceResponse();
        var baseUrl = $"http://{ipAddress}";
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(_deviceSettings.Username, _deviceSettings.Password)
        };
        try
        {
            using (var client = new HttpClient(handler))
            {
                // Eliminar la cara del empleado
                string endpointDelete = "/ISAPI/Intelligent/FDLib/FDSearch/Delete?format=json&FDID=1&faceLibType=blackFD";
                var deleteRequest = new FpidRequest
                {
                    FPID = new List<Fpid>
                    {
                        new Fpid { value = employee.Credencial.ToString() }
                    }
                };
                string jsonDelete = System.Text.Json.JsonSerializer.Serialize(deleteRequest);
                var contentDelete = new StringContent(jsonDelete, Encoding.UTF8, "application/json");
                var deleteResponse = await client.PutAsync(baseUrl + endpointDelete, contentDelete);
                response.FaceResponse = await deleteResponse.Content.ReadAsStringAsync();
                response.FaceSuccess = deleteResponse.IsSuccessStatusCode;
            }
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.FaceSuccess = false;
        }
        return response;
    }

    public async Task<DeviceResponse> EliminarUsuarioAsync(string ipAddress, Empleado employee)
    {
        var response = new DeviceResponse();
        var baseUrl = $"http://{ipAddress}";
        var endpoint = "/ISAPI/AccessControl/UserInfo/Delete?format=json";
        var handler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(_deviceSettings.Username, _deviceSettings.Password)
        };
        try
        {
            using (var client = new HttpClient(handler))
            {
                var employeeObj = new EmployeeDelete
                {
                    EmployeeNo = employee.Credencial.ToString()
                };
                var userInfoDelCond = new UserInfoDelCond
                {
                    EmployeeNoList = new List<EmployeeDelete> { employeeObj }
                };
                var requestBody = new RequestBodyDelete
                {
                    UserInfoDelCond = userInfoDelCond
                };
                string json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var deleteResponse = await client.PutAsync(baseUrl + endpoint, content);
                response.UserResponse = await deleteResponse.Content.ReadAsStringAsync();
                response.UserSuccess = deleteResponse.IsSuccessStatusCode;
            }
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.UserSuccess = false;
        }
        return response;
    }
} 