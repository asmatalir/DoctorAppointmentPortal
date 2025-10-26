import { LocationService } from "../../services/location-service";
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AddressDropdown {
  states: any[] = [];
  districts: any[] = [];
  talukas: any[] = [];
  cities: any[] = [];

  stateId: number = 0;
  districtId: number = 0;
  talukaId: number = 0;
  cityId: number = 0;

  constructor(private locationService: LocationService) {}

  loadStates() {
    return this.locationService.getStates().subscribe(data => this.states = data);
  }

loadDistricts(model: any, selectedDistrictId?: number) {
  if (!model.StateId) return;
  this.locationService.getDistricts(model.StateId).subscribe(districts => {
    this.districts = districts;
    if (selectedDistrictId) {
      model.DistrictId = selectedDistrictId;
      this.loadTalukas(model, model.TalukaId);
    }
  });
}

loadTalukas(model: any, selectedTalukaId?: number) {
  if (!model.DistrictId) return;
  this.locationService.getTalukas(model.DistrictId).subscribe(talukas => {
    this.talukas = talukas;
    if (selectedTalukaId) {
      model.TalukaId = selectedTalukaId;
      this.loadCities(model, model.CityId);
    }
  });
}

loadCities(model: any, selectedCityId?: number) {
  if (!model.TalukaId) return;
  this.locationService.getCities(model.TalukaId).subscribe(cities => {
    this.cities = cities;
    if (selectedCityId) model.CityId = selectedCityId;
  });
}

onStateChange(model: any) {
  model.DistrictId = 0;
  model.TalukaId = 0;
  model.CityId = 0;
  this.districts = [];
  this.talukas = [];
  this.cities = [];

  if (model.StateId) this.loadDistricts(model);
}

onDistrictChange(model: any) {
  model.TalukaId = 0;
  model.CityId = 0;
  this.talukas = [];
  this.cities = [];

  if (model.DistrictId) this.loadTalukas(model);
}

onTalukaChange(model: any) {
  model.CityId = 0;
  this.cities = [];

  if (model.TalukaId) this.loadCities(model);
}

  // Optional: preload selections in edit mode
preload(model: any, stateId: number, districtId?: number, talukaId?: number, cityId?: number) {
  model.StateId = stateId;
  if (districtId) model.DistrictId = districtId;
  if (talukaId) model.TalukaId = talukaId;
  if (cityId) model.CityId = cityId;

  if (stateId) this.loadDistricts(model, districtId);
  if (districtId) this.loadTalukas(model, talukaId);
  if (talukaId) this.loadCities(model, cityId);
}

}
