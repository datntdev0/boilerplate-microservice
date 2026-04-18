import { Injectable } from '@angular/core';

/**
 * Mock Logger Service for testing.
 * Suppresses all console logs during test execution.
 * Use this to replace the real LoggerService in spec files.
 */
@Injectable()
export class MockLoggerService {
  debug(message: string, object?: any): void {
    // Suppress logs in tests
  }

  info(message: string, object?: any): void {
    // Suppress logs in tests
  }

  warn(message: string, object?: any): void {
    // Suppress logs in tests
  }

  error(message: string, error?: any): void {
    // Suppress logs in tests
  }

  fatal(message: string, error?: any): void {
    // Suppress logs in tests
  }
}
