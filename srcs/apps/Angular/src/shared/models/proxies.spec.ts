import { describe, it, expect, beforeEach } from 'vitest';
import { ErrorResponse } from './proxies';

describe('Models.ErrorResponse', () => {
  let errorResponse: ErrorResponse;

  beforeEach(() => {
    errorResponse = new ErrorResponse();
  });

  it('should create an instance', () => {
    expect(errorResponse).toBeTruthy();
  });

  it('should initialize with no data', () => {
    expect(errorResponse.statusCode).toBeUndefined();
    expect(errorResponse.errorCode).toBeUndefined();
    expect(errorResponse.message).toBeUndefined();
    expect(errorResponse.details).toBeUndefined();
  });

  it('should initialize with provided data in constructor', () => {
    const data = {
      statusCode: 404,
      errorCode: 1001,
      message: 'Not Found',
      details: 'Resource not found'
    };

    const response = new ErrorResponse(data);

    expect(response.statusCode).toBe(404);
    expect(response.errorCode).toBe(1001);
    expect(response.message).toBe('Not Found');
    expect(response.details).toBe('Resource not found');
  });

  it('should copy all properties from provided data', () => {
    const data = {
      statusCode: 400,
      errorCode: 2000,
      message: 'Bad Request',
      details: 'Invalid input',
      customProperty: 'custom value'
    };

    const response = new ErrorResponse(data);

    expect(response.statusCode).toBe(400);
    expect(response.errorCode).toBe(2000);
    expect((response as any).customProperty).toBe('custom value');
  });

  it('should update properties via init method', () => {
    errorResponse.init({
      statusCode: 500,
      errorCode: 5000,
      message: 'Internal Server Error',
      details: 'Server error occurred'
    });

    expect(errorResponse.statusCode).toBe(500);
    expect(errorResponse.errorCode).toBe(5000);
    expect(errorResponse.message).toBe('Internal Server Error');
    expect(errorResponse.details).toBe('Server error occurred');
  });

  it('should handle init with undefined data', () => {
    errorResponse.init(undefined);

    expect(errorResponse.statusCode).toBeUndefined();
    expect(errorResponse.errorCode).toBeUndefined();
  });

  it('should create instance from JS object via fromJS', () => {
    const data = {
      statusCode: 401,
      errorCode: 3000,
      message: 'Unauthorized',
      details: 'Authentication required'
    };

    const response = ErrorResponse.fromJS(data);

    expect(response).toBeInstanceOf(ErrorResponse);
    expect(response.statusCode).toBe(401);
    expect(response.errorCode).toBe(3000);
    expect(response.message).toBe('Unauthorized');
    expect(response.details).toBe('Authentication required');
  });

  it('should handle fromJS with non-object data', () => {
    const response = ErrorResponse.fromJS(null);

    expect(response).toBeInstanceOf(ErrorResponse);
    expect(response.statusCode).toBeUndefined();
  });

  it('should convert to JSON', () => {
    errorResponse.statusCode = 403;
    errorResponse.errorCode = 4000;
    errorResponse.message = 'Forbidden';
    errorResponse.details = 'Access denied';

    const json = errorResponse.toJSON();

    expect(json.statusCode).toBe(403);
    expect(json.errorCode).toBe(4000);
    expect(json.message).toBe('Forbidden');
    expect(json.details).toBe('Access denied');
  });

  it('should handle toJSON with existing data object', () => {
    errorResponse.statusCode = 422;
    errorResponse.errorCode = 6000;
    errorResponse.message = 'Validation Error';

    const existingData = { additionalField: 'value' };
    const json = errorResponse.toJSON(existingData);

    expect(json.statusCode).toBe(422);
    expect(json.errorCode).toBe(6000);
    expect(json.message).toBe('Validation Error');
    expect(json.additionalField).toBe('value');
  });

  it('should support custom properties with bracket notation', () => {
    const response = new ErrorResponse();
    response['customKey'] = 'customValue';

    expect(response['customKey']).toBe('customValue');
  });

  it('should include custom properties in toJSON', () => {
    errorResponse.statusCode = 200;
    (errorResponse as any).customProp = 'custom';

    const json = errorResponse.toJSON();

    expect(json.statusCode).toBe(200);
    expect(json.customProp).toBe('custom');
  });

  it('should round-trip data through fromJS and toJSON', () => {
    const originalData = {
      statusCode: 201,
      errorCode: 7000,
      message: 'Created',
      details: 'Resource created successfully'
    };

    const response = ErrorResponse.fromJS(originalData);
    const roundTrippedData = response.toJSON();

    expect(roundTrippedData).toEqual({
      ...originalData
    });
  });
});
