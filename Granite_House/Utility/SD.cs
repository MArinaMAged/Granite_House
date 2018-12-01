using System.IO;

namespace Granite_House.Utility
{
    public class SD
    {
        public const string DefaultProductImage = "No-image-available.jpg";
        public static string ImageFolder = "images" + Path.DirectorySeparatorChar.ToString() + "ProductImage";

        public const string AdminEndUser = "Admin";
        public const string SuperAdminEndUser = "SuperAdmin";
    }
}
