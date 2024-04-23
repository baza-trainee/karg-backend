using karg.BLL.DTO;
using karg.BLL.Interfaces;
using karg.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services
{
    public class RescuerService : IRescuerService
    {
        private readonly IRescuerRepository _rescuerRepository;
        private readonly IImageService _imageService;

        public RescuerService(IRescuerRepository rescuerRepository, IImageService imageService)
        {
            _rescuerRepository = rescuerRepository;
            _imageService = imageService;
        }

        public async Task<List<AllRescuersDTO>> GetRescuers()
        {
            try
            {
                var rescuers = await _rescuerRepository.GetRescuers();
                var rescuersDto = new List<AllRescuersDTO>();

                foreach (var rescuer in rescuers)
                {
                    var rescuerImage = await _imageService.GetRescuerImage(rescuer.ImageId);
                    var rescuerDto = new AllRescuersDTO
                    {
                        FullName = rescuer.FullName,
                        PhoneNumber = rescuer.PhoneNumber,
                        Image = rescuerImage.Uri,
                    };

                    rescuersDto.Add(rescuerDto);
                }

                return rescuersDto;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error retrieving list of rescuers.", exception);
            }
        }
    }
}
