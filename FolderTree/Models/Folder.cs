namespace FolderTree.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Folder ParrentFolder { get; set; }
        public int ParrentId { get; set; }
        public List<Folder> SubFolders { get; set; } = new();

    }
}
