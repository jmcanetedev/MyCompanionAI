﻿namespace MyCompanionAI.Core.DTOs;

public class _BaseDto
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime DeletedOn { get; set; }
    public string DeletedBy { get; set; }
}
