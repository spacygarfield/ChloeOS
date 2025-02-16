using ChloeOS.Core.Models.FS;
using Directory = ChloeOS.Core.Models.FS.Directory;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.Client.Models;

public class BrowsableContent {

    public required Directory[] Folders { get; init; } = [];
    public required File[] Files { get; init; } = [];

    public static explicit operator BrowsableContent(Directory directory)
        => new () {
            Files = directory.SubFiles.ToArray(),
            Folders = directory.SubDirectories.ToArray()
        };

}