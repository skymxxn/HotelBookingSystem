﻿namespace HotelBookingSystem.Application.Common.DTOs;

public class HotelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsApproved { get; set; }
    public bool IsPublished { get; set; }
}