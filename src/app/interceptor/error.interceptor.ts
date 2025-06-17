// error.interceptor.ts
import { inject, Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpHandlerFn } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AppMessageService } from '../services/message.service';

export function errorInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const messageService = inject(AppMessageService);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMsg = 'An unknown error occurred!';
      if (error.error?.errorMessage) {
        errorMsg = error.error.errorMessage;
      } else if (error.message) {
        errorMsg = error.message;
      }
      messageService.showError(errorMsg);
      return throwError(() => error);
    })
  );
}