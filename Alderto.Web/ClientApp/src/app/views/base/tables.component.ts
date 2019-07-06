import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  templateUrl: 'tables.component.html'
})

export class TablesComponent {
  forecasts: IWeatherForecast[];

  constructor(http: HttpClient) {
    http.get<IWeatherForecast[]>(window.location.origin + '/api/SampleData/WeatherForecasts').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface IWeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
