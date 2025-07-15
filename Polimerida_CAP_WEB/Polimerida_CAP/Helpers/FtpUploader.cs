using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Polimerida_CAP.Helpers;

public class FtpUploader
{
    private readonly IConfiguration _cfg;
    public FtpUploader(IConfiguration cfg) => _cfg = cfg;

    public async Task<string> UploadAsync(IFormFile file, string carpeta = "")
    {
        var set = _cfg.GetSection("FtpSettings");
        var host = set["Host"];
        var user = set["User"];
        var pass = set["Password"];
        var remotePath = set["RemotePath"] ?? "/";
        var baseUrl = set["BaseUrl"] ?? "";
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var ftpPath = $"ftp://{host}{remotePath}{(string.IsNullOrWhiteSpace(carpeta) ? "" : carpeta + "/")}{fileName}";

        var req = (FtpWebRequest)WebRequest.Create(ftpPath);
        req.Method = WebRequestMethods.Ftp.UploadFile;
        req.Credentials = new NetworkCredential(user, pass);
        req.EnableSsl = false;

        using var stream = req.GetRequestStream();
        await file.CopyToAsync(stream);
        using var resp = (FtpWebResponse)await req.GetResponseAsync();
        if (resp.StatusCode is not (FtpStatusCode.ClosingData or FtpStatusCode.CommandOK))
            throw new InvalidOperationException($"Error FTP: {resp.StatusDescription}");

        // Solo guarda el nombre en la BD, pero retorna la URL pública para mostrar
        return fileName;
    }

    public string GetPublicUrl(string fileName)
    {
        var set = _cfg.GetSection("FtpSettings");
        var baseUrl = set["BaseUrl"] ?? "";
        return baseUrl + fileName;
    }
} 