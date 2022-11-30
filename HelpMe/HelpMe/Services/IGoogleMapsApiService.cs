using HelpMe.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpMe.Services
{
    public interface IGoogleMapsApiService
    {
        Task<GooglePlaceAutoCompleteResult> ApiPlaces(string text);

        Task<GooglePlace> ApiPlacesDetails(string placeId);
    }
}
