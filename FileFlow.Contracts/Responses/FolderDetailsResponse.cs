namespace FileFlow.Contracts.Responses;

public class FolderDetailsResponse
{
    public FileFolderResponse Folder { get; set; } = null!;
    public ICollection<FileFolderResponse> Children { get; set; } = new List<FileFolderResponse>();
}
