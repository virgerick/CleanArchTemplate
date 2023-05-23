﻿namespace CleanArchTemplate.Shared.Requests;
public abstract class PagedRequest
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string[] Orderby { get; set; } = Array.Empty<string>();
}
