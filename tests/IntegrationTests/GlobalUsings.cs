global using System.Net;
global using System.Net.Http.Json;

global using Xunit;
global using Moq;
global using Moq.Protected;
global using AutoMapper;
global using FluentAssertions;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Caching.Memory;

global using LimbooCards.Domain.Entities;
global using LimbooCards.Domain.Events;
global using LimbooCards.Domain.Services;
global using LimbooCards.Domain.Repositories;

global using LimbooCards.Application.DTOs;
global using LimbooCards.Application.Services;

global using LimbooCards.Infra.DTOs;
global using LimbooCards.Infra.Repositories;
global using LimbooCards.Infra.Mappings;
global using LimbooCards.Infra.Settings;
global using LimbooCards.Infra.Persistence;
global using LimbooCards.Infra.Services;

global using LimbooCards.Domain.Shared;