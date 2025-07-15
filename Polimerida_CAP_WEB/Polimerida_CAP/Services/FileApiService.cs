using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Polimerida_CAP.Services;

public interface IFileApiService
{
    Task<string> UploadFileAsync(IFormFile file, string folder, string fileName);
}

public class FileApiService : IFileApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public FileApiService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["FileApiSettings:BaseUrl"] ?? throw new ArgumentNullException("FileApiSettings:BaseUrl", "Base URL for file API is required");
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder, string fileName)
    {
        using var formData = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();
        using var fileContent = new StreamContent(fileStream);
        formData.Add(fileContent, "file", fileName);
        formData.Add(new StringContent(folder), "folder");
        formData.Add(new StringContent(fileName), "fileName");
        var response = await _httpClient.PostAsync(_baseUrl, formData);
        response.EnsureSuccessStatusCode();
        var imageUrl = await response.Content.ReadAsStringAsync();
        return imageUrl.Trim('"');
    }
} 