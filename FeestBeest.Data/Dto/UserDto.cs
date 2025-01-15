﻿using FeestBeest.Data.Models;

namespace FeestBeest.Data.Dto;  
public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public Rank Rank { get; set; }
}