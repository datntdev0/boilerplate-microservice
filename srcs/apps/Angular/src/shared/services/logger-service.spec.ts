import { LoggerService } from './logger-service';
import { describe, it, beforeEach, afterEach, expect, vi } from 'vitest';

describe('Services.LoggerService', () => {
  let service: LoggerService;
  let originalConsole: Console;
  let consoleSpy = vi.fn(class {
    debug = vi.fn();
    info = vi.fn();
    warn = vi.fn();
    error = vi.fn();
  })

  beforeAll(() => {
    originalConsole = globalThis.console;
    globalThis.console = new consoleSpy() as unknown as Console;
    service = new LoggerService();
  });

  afterAll(() => {
    globalThis.console = originalConsole;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should log debug messages when log level is debug', () => {
    service['logLevel'] = 'debug';
    service.debug('Test debug message');
    expect(console.debug).toHaveBeenCalledWith('DEBUG: Test debug message');
  });

  it('should not log debug messages when log level is info', () => {
    service['logLevel'] = 'info';
    service.debug('Test debug message');
    expect(console.debug).not.toHaveBeenCalled();
  });

  it('should log info messages when log level is info', () => {
    service['logLevel'] = 'info';
    service.info('Test info message');
    expect(console.info).toHaveBeenCalledWith('INFO: Test info message');
  });

  it('should log warnings when log level is warn', () => {
    service['logLevel'] = 'warn';
    service.warn('Test warn message');
    expect(console.warn).toHaveBeenCalledWith('WARN: Test warn message');
  });

  it('should log errors when log level is error', () => {
    service['logLevel'] = 'error';
    service.error('Test error message');
    expect(console.error).toHaveBeenCalledWith('ERROR: Test error message');
  });

  it('should log fatal errors when log level is fatal', () => {
    service['logLevel'] = 'fatal';
    service.fatal('Test fatal message');
    expect(console.error).toHaveBeenCalledWith('FATAL: Test fatal message');
  });

  it('should not log info messages when log level is warn', () => {
    service['logLevel'] = 'warn';
    service.info('Test info message');
    expect(console.info).not.toHaveBeenCalled();
  });

  it('should log debug messages with extra object context', () => {
    service['logLevel'] = 'debug';
    const context = { key: 'value' };
    service.debug('Test debug message', context);
    expect(console.debug).toHaveBeenCalledWith('DEBUG: Test debug message', context);
  });

  it('should log info messages with extra object context', () => {
    service['logLevel'] = 'info';
    const context = { key: 'value' };
    service.info('Test info message', context);
    expect(console.info).toHaveBeenCalledWith('INFO: Test info message', context);
  });

  it('should log warnings with extra object context', () => {
    service['logLevel'] = 'warn';
    const context = { key: 'value' };
    service.warn('Test warn message', context);
    expect(console.warn).toHaveBeenCalledWith('WARN: Test warn message', context);
  });

  it('should log errors with extra object context', () => {
    service['logLevel'] = 'error';
    const context = { key: 'value' };
    service.error('Test error message', context);
    expect(console.error).toHaveBeenCalledWith('ERROR: Test error message', context);
  });

  it('should log fatal errors with extra object context', () => {
    service['logLevel'] = 'fatal';
    const context = { key: 'value' };
    service.fatal('Test fatal message', context);
    expect(console.error).toHaveBeenCalledWith('FATAL: Test fatal message', context);
  });
});