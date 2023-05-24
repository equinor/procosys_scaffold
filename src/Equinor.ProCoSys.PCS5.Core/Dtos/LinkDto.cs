using System;

namespace Equinor.ProCoSys.PCS5.Application.Dtos;

public class LinkDto
{
    public LinkDto(Guid guid, string title, string url, string rowVersion)
    {
        Guid = guid;
        Title = title;
        Url = url;
        RowVersion = rowVersion;
    }

    public Guid Guid { get; }
    public string Title { get; }
    public string Url { get; }
    public string RowVersion { get; }
}
