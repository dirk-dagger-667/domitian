import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environments.prod';
import { z } from 'zod';

interface ValidateConfig<T extends z.ZodTypeAny> {
  dto: unknown;
  schema: T;
  schemaName: string;
}

@Injectable({
  providedIn: 'root',
})
export class TypeValidatorService {
  constructor() {}

  public validateSchema<T extends z.ZodTypeAny>(
    config: ValidateConfig<T>
  ): z.infer<T> {
    const { data, success, error } = config.schema.safeParse(config.dto);

    if (success) {
      return data;
    } else {
      this.captureError(`API Validation Error: ${config.schemaName}`, {
        dto: config.dto,
        error: error.message,
        issues: error.issues,
      });

      throw error;
    }
  }

  private captureError(message: string, extra = {}): void {
    if (!environment.production) {
      console.error(message, extra);
    } else {
      // TODO: report to Sentry/something else
    }
  }
}
