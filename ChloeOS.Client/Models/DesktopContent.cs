using ChloeOS.Core.Models.FS;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.Client.Models;

public class DesktopContent {

    public Folder[] Folders { get; init; }
    public File[] Files { get; init; }

}