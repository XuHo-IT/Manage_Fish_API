using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

public class CloudinaryRepository
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryRepository(IConfiguration configuration)
    {
        var cloudinaryAccount = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(cloudinaryAccount);
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream())
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}
