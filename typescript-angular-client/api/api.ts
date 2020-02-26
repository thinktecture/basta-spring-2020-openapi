export * from './article.service';
import { ArticleService } from './article.service';
export * from './health.service';
import { HealthService } from './health.service';
export * from './weatherForecast.service';
import { WeatherForecastService } from './weatherForecast.service';
export const APIS = [ArticleService, HealthService, WeatherForecastService];
