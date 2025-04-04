import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorService 
{

  formatError(err: HttpErrorResponse): string
  {
    return this.httpErrorFormatter(err);
  }

  private httpErrorFormatter(err: any): string
  {
    let errorMessage = '';

    if (err instanceof HttpErrorResponse)
    {
      var error = err as HttpErrorResponse;

      if (error.error && error.error)
      {
        errorMessage = error.error;
      }

      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;

    }
    else
    {
      errorMessage = `Server returned code: ${err.message}, error message is ${err.statusText}`
    }

    return errorMessage;
  }
}
