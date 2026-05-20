global using System.Net;
global using Scalar.AspNetCore;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;

global using Aquasmart.Server.Source.Domain.Enums;
global using Aquasmart.Server.Source.Domain.Entities;
global using Aquasmart.Server.Source.Domain.Entities.Base;
global using Aquasmart.Server.Source.Domain.Shared.Exceptions;
global using Aquasmart.Server.Source.Domain.Shared.Validator;
global using Aquasmart.Server.Source.Domain.Abstractions.UoW;
global using Aquasmart.Server.Source.Domain.Abstractions.Services;
global using Aquasmart.Server.Source.Infrastructure.Data.Context;
global using Aquasmart.Server.Source.Infrastructure.Data.Connection;
global using Aquasmart.Server.Source.Infrastructure.Services;
global using Aquasmart.Server.Source.Infrastructure.UoW;
global using Aquasmart.Server.Source.Presentation.Common.Configs;
global using Aquasmart.Server.Source.Presentation.Common.Extensions;
global using Aquasmart.Server.Source.Presentation.Common.Pipelines;
global using Aquasmart.Server.Source.Presentation.Hubs;
global using Aquasmart.Server.Source.Presentation.Enpoints;
global using Aquasmart.Server.Source.Presentation.Enpoints.System;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.Commands;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.Queries;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.Alerta.DTOs;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.Commands;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.Queries;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.Comando.DTOs;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.Commands;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.Queries;
global using Aquasmart.Server.Source.Presentation.Enpoints.Business.LeituraSensor.DTOs;




