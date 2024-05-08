using MagicVilla_VillaAPi.Models.Dto;

namespace MagicVilla_VillaAPi.Data
{
    public static class VillaStore
    {
        public static new List<VillaDto> VillaList = new List<VillaDto> { 
                new VillaDto { Id = 1, Name =  "Beach Villa" },
                new VillaDto { Id = 2, Name = "Pool Villa" }
                };
    }
}
