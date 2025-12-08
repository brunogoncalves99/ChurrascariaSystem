using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Entities;
using ChurrascariaSystem.Domain.Enums;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class MesaService : IMesaService
    {
        private readonly IMesaRepository _mesaRepository;

        public MesaService(IMesaRepository mesaRepository)
        {
            _mesaRepository = mesaRepository;
        }

        public async Task<MesaDTO?> GetByIdAsync(int id)
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);
            return mesa == null ? null : MapToDTO(mesa);
        }

        public async Task<IEnumerable<MesaDTO>> GetAllAsync()
        {
            var mesas = await _mesaRepository.GetAllAsync();
            return mesas.Select(MapToDTO);
        }

        public async Task<IEnumerable<MesaDTO>> GetAllActiveAsync()
        {
            var mesas = await _mesaRepository.GetAllActiveAsync();
            return mesas.Select(MapToDTO);
        }

        public async Task<IEnumerable<MesaDTO>> GetMesasLivresAsync()
        {
            var mesas = await _mesaRepository.GetMesasLivresAsync();
            return mesas.Select(MapToDTO);
        }

        public async Task<MesaDTO> CreateAsync(MesaDTO mesaDto)
        {
            var mesa = new Mesa
            {
                Numero = mesaDto.Numero,
                Capacidade = mesaDto.Capacidade,
                Status = mesaDto.Status,
                Ativo = mesaDto.Ativo
            };

            await _mesaRepository.AddAsync(mesa);
            return MapToDTO(mesa);
        }

        public async Task UpdateAsync(MesaDTO mesaDto)
        {
            var mesa = await _mesaRepository.GetByIdAsync(mesaDto.idMesa);

            if (mesa == null) 
                throw new Exception("Mesa não encontrada");

            mesa.Numero = mesaDto.Numero;
            mesa.Capacidade = mesaDto.Capacidade;
            mesa.Status = mesaDto.Status;
            mesa.Ativo = mesaDto.Ativo;

            await _mesaRepository.UpdateAsync(mesa);
        }

        public async Task UpdateStatusAsync(int id, int status)
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);

            if (mesa == null) 
                throw new Exception("Mesa não encontrada");

            mesa.Status = (StatusMesa)status;
            await _mesaRepository.UpdateAsync(mesa);
        }

        public async Task DeleteAsync(int id)
        {
            await _mesaRepository.DeleteAsync(id);
        }

        private MesaDTO MapToDTO(Mesa mesa)
        {
            return new MesaDTO
            {
                idMesa = mesa.idMesa,
                Numero = mesa.Numero,
                Capacidade = mesa.Capacidade,
                Status = mesa.Status,
                StatusDescricao = mesa.Status.ToString(),
                Ativo = mesa.Ativo
            };
        }
    }
}
