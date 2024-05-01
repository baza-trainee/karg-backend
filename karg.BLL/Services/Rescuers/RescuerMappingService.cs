using karg.BLL.DTO.Rescuers;
using karg.BLL.Interfaces.Rescuers;
using karg.BLL.Interfaces.Utilities;
using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Services.Rescuers
{
    public class RescuerMappingService : IRescuerMappingService
    {
        private readonly IImageService _imageService;

        public RescuerMappingService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<List<AllRescuersDTO>> MapToAllRescuersDTO(List<Rescuer> rescuers)
        {
            try
            {
                var rescuersDto = new List<AllRescuersDTO>();

                foreach (var rescuer in rescuers)
                {
                    var rescuerImage = await _imageService.GetImageById(rescuer.ImageId);
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
                throw new ApplicationException("An error occurred while mapping rescuers to DTOs:", exception);
            }
        }
    }
}
