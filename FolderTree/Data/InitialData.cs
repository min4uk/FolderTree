using FolderTree.Models;
using static NuGet.Packaging.PackagingConstants;

namespace FolderTree.Data
{
    public static class InitialData
    {
        public static List<Folder> InitialDataListGeneration()
        {
            List<Folder> initialFolders = new List<Folder>
            {
                new Folder { Name = "Creating Digital Images", ParrentId = null},
                new Folder { Name = "Resources", ParrentId = 1 },
                new Folder { Name = "Primary Sources", ParrentId = 2 },
                new Folder { Name = "Secondary Sources", ParrentId = 2 },
                new Folder { Name = "Evidence", ParrentId = 1 },
                new Folder { Name = "Graphic Products", ParrentId = 1 },
                new Folder { Name = "Process", ParrentId = 6 },
                new Folder { Name = "Final Product", ParrentId = 6 },
            };

            return initialFolders;
        }
    }
}
