using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using SmartMass.Controller.Shared.Models;
using System.Runtime.CompilerServices;

namespace SmartMass.Controller.Client.Components
{
    public class SpoolBasePage : ComponentBase
    {
        [Inject]
        protected HttpClient httpClient { get; set; }

        private List<Manufacturer> manufacturers = new List<Manufacturer>();
        private List<Material> materials = new List<Material>();
        private bool loading = false;

        protected async Task LoadManufacturersData()
        {
            loading = true;
            StateHasChanged();

            var response = await httpClient.GetAsync($"api/Manufacturers");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Manufacturer>>();
                if (result != null)
                {
                    manufacturers.Clear();
                    manufacturers.AddRange(result);
                    StateHasChanged();
                }
            }

            loading = false;
            StateHasChanged();
        }

        private async Task LoadMaterialsData()
        {
            loading = true;
            StateHasChanged();

            var response = await httpClient.GetAsync($"api/Materials");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Material>>();
                if (result != null)
                {
                    materials.Clear();
                    materials.AddRange(result);
                    StateHasChanged();
                }
            }

            loading = false;
            StateHasChanged();
        }
    }
}
