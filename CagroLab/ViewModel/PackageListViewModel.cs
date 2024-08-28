using CagroLab.Models;

namespace CagroLab.ViewModel
{
    public class PackageListViewModel
    {
        public int? Account_Id { get; set; }

        public List<Package>? Packages { get; set; }

        public CreatePackageDto NewPackage { get; set; }
    }

    public class CreatePackageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int? Account_Id { get; set; }
    }
}
