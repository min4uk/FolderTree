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
                new Folder { Id = 1, Name = "Creating Digital Images", ParrentId = null},
                new Folder { Id = 2, Name = "Resources", ParrentId = 1 },
                new Folder { Id = 3, Name = "Primary Sources", ParrentId = 2 },
                new Folder { Id = 4, Name = "Secondary Sources", ParrentId = 2 },
                new Folder { Id = 5, Name = "Evidence", ParrentId = 1 },
                new Folder { Id = 6, Name = "Graphic Products", ParrentId = 1 },
                new Folder { Id = 7, Name = "Process", ParrentId = 6 },
                new Folder { Id = 8, Name = "Final Product", ParrentId = 6 },
            };

            return initialFolders;
        }
    }
}
