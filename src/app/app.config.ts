import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import Aura from '@primeng/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { errorInterceptor } from './interceptor/error.interceptor';
import { loadingInterceptor } from './interceptor/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideAnimationsAsync(),
              providePrimeNG({
                  theme: {
                      preset: Aura,
                      options: {
                          darkModeSelector: false || 'none'
                        }
                  }
              }),provideZoneChangeDetection({ eventCoalescing: true }),
              provideRouter(routes),
              MessageService,
              provideHttpClient(),
              provideHttpClient(withInterceptors([errorInterceptor])),
              provideHttpClient(withInterceptors([loadingInterceptor])),
            ]
};
