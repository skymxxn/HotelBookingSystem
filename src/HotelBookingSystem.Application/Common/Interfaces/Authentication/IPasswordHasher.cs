﻿namespace HotelBookingSystem.Application.Common.Interfaces.Authentication;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}