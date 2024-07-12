﻿using tablero_bi.Application.Common;
using tablero_bi.Application.DTOs;

namespace tablero_bi.Application.Interfaces
{
    public interface ISucursalService
    {
        Task<Result<CreateSucursalDto>> CreateNewSucursalAsync(CreateSucursalDto sucursalDto);

    }
}
